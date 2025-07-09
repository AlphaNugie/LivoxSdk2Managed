using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanUtilityLibraryVer2.LivoxSdk2.Core
{
    //**************
    // 设计目标：在雷达数据采集(400K+/秒)和渲染处理之间建立安全缓冲区
    // 高频数据吞吐：处理每秒超 384,000 点（192,000点/500ms）的实时流
    // 跨线程安全：协调雷达数据采集线程、数据处理线程、渲染线程之间的数据传递
    // 内存效率优化：避免因频繁内存分配导致的 GC 压力
    //**************
#if NET9_0_OR_GREATER
    /// <summary>
    /// 高并发双缓冲数据管道（支持无锁读/写）
    /// </summary>
    /// <typeparam name="T">点云数据类型（需为结构体以优化内存）</typeparam>
    /// <remarks>
    /// 初始化双缓冲管理器
    /// </remarks>
    /// <param name="capacity">单个缓冲区容量（建议为最大单帧点数的2倍）</param>
    public sealed class BufferManager<T>(int capacity = 384000) where T : struct
#elif NET45
    /// <summary>
    /// 高并发双缓冲数据管道（支持无锁读/写）
    /// </summary>
    /// <typeparam name="T">点云数据类型（需为结构体以优化内存）</typeparam>
    public sealed class BufferManager<T> where T : struct
#endif
    {
        #region 嵌套类型
        /// <summary>
        /// 单个缓冲区的状态跟踪
        /// </summary>
#if NET9_0_OR_GREATER
        private sealed class BufferState(int capacity)
#elif NET45
        private sealed class BufferState
#endif
        {
            /// <summary>
            /// 实际存储数据的数组
            /// 预分配内存以避免GC开销
            /// </summary>
#if NET9_0_OR_GREATER
            public T[] DataArray { get; } = new T[capacity];
#elif NET45
            public T[] DataArray { get; }
#endif

            /// <summary>
            /// 当前写入位置（下一个可写入的索引）
            /// 由生产者线程更新
            /// </summary>
            public int WritePosition { get; set; } = 0;

            /// <summary>
            /// 当前读取位置（下一个可读取的索引）
            /// 由消费者线程更新
            /// </summary>
            public int ReadPosition { get; set; } = 0;

            /// <summary>
            /// 缓冲区是否已满（写入位置达到容量）
            /// </summary>
            public bool IsFull => WritePosition >= DataArray.Length - 1; // 容量-1是为了避免数组越界

            /// <summary>
            /// 缓冲区是否有待读取数据
            /// </summary>
            public bool HasData => ReadPosition < WritePosition;

#if NET45
            /// <summary>
            /// 用指定容量初始化缓冲区状态
            /// </summary>
            /// <param name="capacity"></param>
            public BufferState(int capacity)
            {
                DataArray = new T[capacity];
            }
#endif
        }
        #endregion

        #region 字段
        /// <summary>
        /// 容量调整操作锁（防止并发修改）
        /// 使用独立锁对象以避免与常规操作的锁竞争
        /// </summary>
#if NET9_0_OR_GREATER
        private readonly Lock _resizeLock = new();
#elif NET45
        private readonly object _resizeLock = new object();
#endif

        /// <summary>
        /// 容量调整标记（当值为true时禁止写入）
        /// 使用 volatile 确保多线程可见性
        /// </summary>
        private volatile bool _isResizing;

        /// <summary>
        /// 同步锁对象（确保跨线程操作原子性）
        /// </summary>
#if NET9_0_OR_GREATER
        private readonly Lock _syncRoot = new();
#elif NET45
        private readonly object _syncRoot = new object();
#endif

        /// <summary>
        /// 双缓冲状态集合（环形缓冲区）
        /// 索引0: 当前写入缓冲区
        /// 索引1: 预备写入缓冲区
        /// </summary>
#if NET9_0_OR_GREATER
        private readonly BufferState[] _buffers =
            [
                new BufferState(capacity),
                new BufferState(capacity)
            ];
#elif NET45
        private readonly BufferState[] _buffers;
#endif

        //通过 _activeWriteIndex 和 _activeReadIndex 控制缓冲区角色，确保生产者和消费者永远不会同时操作同一缓冲区。

        /// <summary>
        /// 当前活跃的写入缓冲区索引
        /// 取值范围：0 或 1（环形切换）
        /// </summary>
        private int _activeWriteIndex = 0;

        /// <summary>
        /// 当前可读取的缓冲区索引
        /// 由消费者线程控制切换
        /// </summary>
        private int _activeReadIndex = 1;

        /// <summary>
        /// 单个缓冲区的容量（元素数量）
        /// 根据雷达最大帧率×2设计，例如：192,000点/帧 × 2 = 384,000
        /// </summary>
#if NET9_0_OR_GREATER
        private int _bufferCapacity = capacity;
#elif NET45
        private int _bufferCapacity;
#endif
        #endregion

        #region 属性
        /// <summary>
        /// 当前配置的单个缓冲区容量（只读）
        /// </summary>
        public int BufferCapacity => _bufferCapacity;

        /// <summary>
        /// 是否正在执行容量调整操作（只读）
        /// </summary>
        public bool IsResizing => _isResizing;

        #endregion

        #region 构造方法
#if NET45
        /// <summary>
        /// 初始化双缓冲管理器
        /// </summary>
        /// <param name="capacity">单个缓冲区容量（建议为最大单帧点数的2倍）</param>
        public BufferManager(int capacity = 384000)
        {
            _bufferCapacity = capacity;
            _buffers = new[]
            {
                new BufferState(capacity),
                new BufferState(capacity)
            };
        }
#endif
        #endregion

        #region 公共方法
        /// <summary>
        /// 尝试写入数据到缓冲区（生产者线程调用）
        /// </summary>
        /// <param name="data">待写入的数据序列</param>
        /// <returns>
        /// true - 数据全部成功写入
        /// false - 缓冲区已满或正在调整容量，部分数据可能丢失
        /// </returns>
        /// <exception cref="ArgumentNullException">输入数据为空</exception>
        public bool TryWrite(IEnumerable<T> data)
        {
#if NET9_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(data, nameof(data));
#elif NET45
            if (data == null) throw new ArgumentNullException(nameof(data));
#endif
            if (_isResizing) // ▶ 新增调整状态检查
                //throw new InvalidOperationException("缓冲区正在调整容量，禁止写入");
                return false;

            // 所有状态修改操作在 lock (_syncRoot) 块内完成
            lock (_syncRoot)
            {
                var currentBuffer = _buffers[_activeWriteIndex];

                foreach (var item in data)
                {
                    // 缓冲区已满时尝试切换
                    if (currentBuffer.IsFull)
                    {
                        if (!SwitchWriteBuffer())
                        {
                            // 所有缓冲区均不可用，数据丢失
                            return false;
                        }
                        currentBuffer = _buffers[_activeWriteIndex];
                    }

                    // 写入数据并移动指针
                    currentBuffer.DataArray[currentBuffer.WritePosition++] = item;
                }

                return true;
            }
        }

        /// <summary>
        /// 尝试从缓冲区读取数据（消费者线程调用）
        /// </summary>
        /// <param name="data">输出参数，接收数据的数组</param>
        /// <param name="count">实际读取到的元素数量</param>
        /// <returns>是否成功读取到数据</returns>
        public bool TryRead(out T[] data, out int count)
        {
            // 所有状态修改操作在 lock (_syncRoot) 块内完成
            lock (_syncRoot)
            {
                var readBuffer = _buffers[_activeReadIndex];

                if (!readBuffer.HasData)
                {
                    //data = null;
#if NET9_0_OR_GREATER
                    data = [];
#elif NET45
                    data = new T[0];
#endif
                    count = 0;
                    return false;
                }

                // 计算有效数据量
                count = readBuffer.WritePosition - readBuffer.ReadPosition;
                data = new T[count];

                // 复制数据到输出数组
                Array.Copy(
                    sourceArray: readBuffer.DataArray,
                    sourceIndex: readBuffer.ReadPosition,
                    destinationArray: data,
                    destinationIndex: 0,
                    length: count
                );

                // 更新读取位置
                readBuffer.ReadPosition += count;

                // 当缓冲区读取完毕时重置
                if (readBuffer.ReadPosition >= readBuffer.WritePosition)
                {
                    ResetBuffer(_activeReadIndex);
                    SwitchReadBuffer();
                }

                return true;
            }
        }

        #endregion

        #region 容量调整方法

        /// <summary>
        /// 动态调整缓冲区容量（线程安全）
        /// </summary>
        /// <param name="newCapacity">新容量（必须大于当前最大未读数据量）</param>
        /// <param name="force">是否强制调整（可能导致数据丢失）</param>
        /// <returns>
        /// true - 调整成功
        /// false - 调整失败（通常因新容量不足）
        /// </returns>
        public bool ResizeBuffers(int newCapacity, bool force = false)
        {
            // 参数校验
            if (newCapacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(newCapacity), "容量必须大于零");

            lock (_resizeLock)
            {
                // 设置调整标记（阻止新写入）
                _isResizing = true;

                try
                {
                    // 阶段1：等待所有进行中的操作完成
                    lock (_syncRoot)
                    {
                        // 计算当前所有未读数据总量
                        int pendingData = CalculatePendingData();

                        // 容量校验（除非强制调整）
                        if (!force && pendingData > newCapacity)
                        {
                            return false; // 新容量不足以容纳现有数据
                        }

                        // 阶段2：创建新缓冲区
                        var newBuffers = new[]
                        {
                        new BufferState(newCapacity),
                        new BufferState(newCapacity)
                    };

                        // 阶段3：迁移数据（保留未消费数据）
                        if (!force)
                        {
                            MigrateData(newBuffers/*, pendingData*/);
                        }

                        // 阶段4：切换缓冲区引用
                        _buffers[0] = newBuffers[0];
                        _buffers[1] = newBuffers[1];
                        _bufferCapacity = newCapacity;

                        // 阶段5：重置指针
                        _activeWriteIndex = 0;
                        _activeReadIndex = 1;
                    }

                    return true;
                }
                finally
                {
                    _isResizing = false; // 清除调整标记
                }
            }
        }

        /// <summary>
        /// 计算当前所有未读数据总量
        /// </summary>
        private int CalculatePendingData()
        {
            int total = 0;
            for (int i = 0; i < _buffers.Length; i++)
            {
                // 仅统计未被消费者完全处理的缓冲区
                if (i == _activeReadIndex) continue;
                total += _buffers[i].WritePosition - _buffers[i].ReadPosition;
            }
            return total;
        }

        /// <summary>
        /// 迁移现有数据到新缓冲区（保留未消费数据）
        /// </summary>
        private void MigrateData(BufferState[] newBuffers)
        {
            int newIndex = 0;
            int offset = 0;
            int totalMigrated = 0;

            // 遍历旧缓冲区
            for (int i = 0; i < _buffers.Length; i++)
            {
                var buffer = _buffers[i];
                int available = buffer.WritePosition - buffer.ReadPosition;

                // 跳过无数据的缓冲区
                if (available <= 0) continue;

                // 复制到新缓冲区
                while (available > 0)
                {
                    int copyLength = Math.Min(available, newBuffers[newIndex].DataArray.Length - offset);

                    Array.Copy(
                        sourceArray: buffer.DataArray,
                        sourceIndex: buffer.ReadPosition,
                        destinationArray: newBuffers[newIndex].DataArray,
                        destinationIndex: offset,
                        length: copyLength
                    );

                    // 更新状态
                    newBuffers[newIndex].WritePosition = offset + copyLength;
                    offset += copyLength;
                    totalMigrated += copyLength;
                    available -= copyLength;

                    // 环形切换
                    if (offset >= newBuffers[newIndex].DataArray.Length)
                    {
                        newIndex = (newIndex + 1) % 2;
                        offset = 0;
                    }
                }
            }

            // 完整性校验
            int expected = CalculatePendingData();
            bool successed = totalMigrated == expected;
            // 完整性断言
            Debug.Assert(successed, "数据迁移完整性校验失败");
            if (!successed)
            {
                throw new InvalidOperationException($"迁移数据量不符，预期:{expected} 实际:{totalMigrated}");
            }
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 切换写入缓冲区（当当前缓冲区写满时调用）
        /// </summary>
        /// <returns>是否切换成功</returns>
        private bool SwitchWriteBuffer()
        {
            // 计算下一个缓冲区索引
            int nextIndex = (_activeWriteIndex + 1) % 2;

            // 检查下一个缓冲区是否可写入（未被消费者占用）
            if (nextIndex == _activeReadIndex && _buffers[nextIndex].HasData)
            {
                return false; // 缓冲区仍被消费者使用
            }

            _activeWriteIndex = nextIndex;
            return true;
        }

        /// <summary>
        /// 切换读取缓冲区（当前缓冲区读取完毕后调用）
        /// </summary>
        private void SwitchReadBuffer()
        {
            _activeReadIndex = (_activeReadIndex + 1) % 2;
        }

        /// <summary>
        /// 重置指定缓冲区的状态
        /// </summary>
        /// <param name="index">缓冲区索引</param>
        private void ResetBuffer(int index)
        {
            var buffer = _buffers[index];
            buffer.WritePosition = 0;
            buffer.ReadPosition = 0;
        }

        #endregion

        #region 状态监测属性

        /// <summary>
        /// 当前写入缓冲区的使用率（0.0~1.0）
        /// </summary>
        public float WriteBufferUsage
        {
            get
            {
                lock (_syncRoot)
                {
                    return (float)_buffers[_activeWriteIndex].WritePosition / _bufferCapacity;
                }
            }
        }

        /// <summary>
        /// 当前读取缓冲区的数据剩余率（0.0~1.0）
        /// </summary>
        public float ReadBufferRemaining
        {
            get
            {
                lock (_syncRoot)
                {
                    var buffer = _buffers[_activeReadIndex];
                    return (float)(buffer.WritePosition - buffer.ReadPosition) / _bufferCapacity;
                }
            }
        }

        #endregion
    }
}
