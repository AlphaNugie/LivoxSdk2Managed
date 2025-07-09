using ScanUtilityLibraryVer2.LivoxSdk2.Include;
using ScanUtilityLibraryVer2.LivoxSdk2.Samples;
using ScanUtilityLibraryVer2.LivoxSdk2.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanUtilityLibraryVer2.LivoxSdk2.Core
{
    /// <summary>
    /// 支持动态容量调整的滑动窗口双缓冲服务
    /// 核心特性：
    /// 1. 可运行时修改窗口容量（总点数）
    /// 2. 自动根据当前容量淘汰旧数据
    /// 3. 线程安全的数据访问机制
    /// </summary>
    public class DataBufferService<T>
    {
        /// <summary>
        /// 同步锁对象
        /// </summary>
#if NET9_0_OR_GREATER
        private readonly Lock _bufferLock = new();
#elif NET45
        private readonly object _bufferLock = new object();
#endif

        /// <summary>
        /// 双缓冲队列（存储数据块）
        /// </summary>
#if NET9_0_OR_GREATER
        private readonly Queue<DataChunk<T>> _activeQueue = new();
#elif NET45
        private readonly Queue<DataChunk<T>> _activeQueue = new Queue<DataChunk<T>>();
#endif

        //// 内存池（优化频繁分配）
        //private readonly ObjectPool<DataChunk> _chunkPool =
        //    new DefaultObjectPool<DataChunk>(new DataChunkPoolPolicy(PointsPerMillisecond));
        /// <summary>
        /// 内存池（优化频繁分配）（按每次回调返回的包为单位分配）
        /// </summary>
        private readonly ObjectPool<DataChunk<T>> _chunkPool =
            //new(() => new DataChunk<T>(LivoxLidarQuickStart.POINTS_PER_PKG));
#if NET9_0_OR_GREATER
            new(() => new DataChunk<T>());
#elif NET45
            new ObjectPool<DataChunk<T>>(() => new DataChunk<T>());
#endif

        //// 创建对象池策略
        //var poolPolicy = new DefaultPooledObjectPolicy<DataChunk>(() =>
        //    new DataChunk(PointsPerMillisecond));

        //// 初始化对象池
        //_chunkPool = new DefaultObjectPool<DataChunk>(poolPolicy);

        /// <summary>
        /// 动态容量字段（volatile保证多线程可见性）
        /// </summary>
#if NET9_0_OR_GREATER
        private volatile int _windowCapacity = LivoxLidarQuickStart.POINTS_PER_PKG * LivoxLidarQuickStart.PKGS_PER_MILLISEC * 100; // 默认100ms窗口
#elif NET45
        private volatile int _windowCapacity = LivoxLidarQuickStart.POINTS_PER_PKG * LivoxLidarQuickStart.PKGS_PER_MILLISEC * 100; // 默认100ms窗口
#endif

        /// <summary>
        /// 当前窗口容量（可动态设置）
        /// </summary>
        public int WindowCapacity
        {
            get => _windowCapacity;
            set
            {
                lock (_bufferLock)
                {
                    //if (value < PointsPerMillisecond)
                    //    throw new ArgumentException("容量不能小于单次更新量");
                    _windowCapacity = value;
                    TrimActiveQueue(); // 立即应用新容量
                }
            }
        }

        /// <summary>
        /// 添加新数据块
        /// </summary>
        //public void AddDataChunk(List<LivoxLidarCartesianHighRawPoint> newPoints)
        public void AddDataChunk(IEnumerable<T> newPoints)
        {
#if NET9_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(newPoints);
#elif NET45
            if (newPoints == null) throw new ArgumentNullException(nameof(newPoints));
#endif

            //if (newPoints.Count() != PointsPerMillisecond)
            //    throw new ArgumentException($"必须提供{PointsPerMillisecond}个点");

            if (newPoints == null || !newPoints.Any())
                return;

            var chunk = _chunkPool.Get();
            chunk.Points.Clear();
            chunk.Points.AddRange(newPoints);

            lock (_bufferLock)
            {
                _activeQueue.Enqueue(chunk);
                TrimActiveQueue(); // 根据当前容量淘汰旧数据
            }
        }

        ///// <summary>
        ///// 交换缓冲区并获取当前数据快照
        ///// </summary>
        //public IEnumerable<T> SwapAndGetSnapshot()
        //{
        //    lock (_bufferLock)
        //    {
        //        // 交换队列引用
        //        (_backQueue, _activeQueue) = (_activeQueue, _backQueue);

        //        // 返回扁平化数据
        //        return _backQueue.SelectMany(chunk => chunk.Points);
        //    }
        //}

        /// <summary>
        /// 交换缓冲区并获取当前数据快照
        /// </summary>
        public T[] SwapAndGetSnapshot()
        {
            lock (_bufferLock)
            {
                //// 交换队列引用
                //(_backQueue, _activeQueue) = (_activeQueue, _backQueue);

                //// 返回扁平化数据
                //return _backQueue.SelectMany(chunk => chunk.Points);

                // 返回扁平化数据
#if NET9_0_OR_GREATER
                return [.. _activeQueue.SelectMany(chunk => chunk.Points)];
#elif NET45
                return _activeQueue.SelectMany(chunk => chunk.Points).ToArray();
#endif
            }
        }

        /// <summary>
        /// 根据当前容量修剪活动队列
        /// </summary>
        private void TrimActiveQueue()
        {
            // 计算当前队列总点数
            int totalPoints = _activeQueue.Sum(chunk => chunk.Points.Count);

            // 淘汰旧数据直到符合容量要求
            while (totalPoints > _windowCapacity)
            {
                var oldestChunk = _activeQueue.Dequeue();
                totalPoints -= oldestChunk.Points.Count;
                _chunkPool.Return(oldestChunk);
            }
        }
    }

#if NET9_0_OR_GREATER
    /// <summary>
    /// 内部数据块定义，用于存储单次回调返回的点云数据
    /// </summary>
    /// <typeparam name="T">点云数据类型</typeparam>
    /// <param name="capacity">数据块的容量</param>
    internal class DataChunk<T>(int capacity = 0)
#elif NET45
    /// <summary>
    /// 内部数据块定义，用于存储单次回调返回的点云数据
    /// </summary>
    /// <typeparam name="T">点云数据类型</typeparam>
    internal class DataChunk<T>
#endif
    {
#if NET9_0_OR_GREATER
        public List<T> Points { get; } = new List<T>(capacity); // capacity为0时，等效于new Lit<T>()
#elif NET45
        public List<T> Points { get; } // capacity为0时，等效于new Lit<T>()

        /// <summary>
        /// 使用给定的容量初始化数据块
        /// </summary>
        /// <param name="capacity">数据块的容量</param>
        public DataChunk(int capacity = 0)
        {
            Points = new List<T>(capacity);
        }
#endif
    }

    ///// <summary>
    ///// 支持动态容量调整的滑动窗口双缓冲服务
    ///// 核心特性：
    ///// 1. 可运行时修改窗口容量（总点数）
    ///// 2. 自动根据当前容量淘汰旧数据
    ///// 3. 线程安全的数据访问机制
    ///// </summary>
    //public class DataBufferService
    //{
    //    //// 常量
    //    //private const int PointsPerMillisecond = 384; // 每毫秒固定更新点数

    //    // 同步锁对象
    //    private readonly Lock _bufferLock = new();

    //    // 双缓冲队列（存储数据块）
    //    private Queue<DataChunk> _activeQueue = new();
    //    private Queue<DataChunk> _backQueue = new();

    //    //// 内存池（优化频繁分配）
    //    //private readonly ObjectPool<DataChunk> _chunkPool =
    //    //    new DefaultObjectPool<DataChunk>(new DataChunkPoolPolicy(PointsPerMillisecond));
    //    // 内存池（优化频繁分配）（按每次回调返回的包为单位分配）
    //    private readonly ObjectPool<DataChunk> _chunkPool =
    //        new(() => new DataChunk(LivoxLidarQuickStart.POINTS_PER_PKG));

    //    //// 创建对象池策略
    //    //var poolPolicy = new DefaultPooledObjectPolicy<DataChunk>(() =>
    //    //    new DataChunk(PointsPerMillisecond));

    //    //// 初始化对象池
    //    //_chunkPool = new DefaultObjectPool<DataChunk>(poolPolicy);

    //    // 动态容量字段（volatile保证多线程可见性）
    //    private volatile int _windowCapacity = LivoxLidarQuickStart.POINTS_PER_PKG * LivoxLidarQuickStart.PKGS_PER_MILLISEC * 100; // 默认100ms窗口

    //    /// <summary>
    //    /// 当前窗口容量（可动态设置）
    //    /// </summary>
    //    public int WindowCapacity
    //    {
    //        get => _windowCapacity;
    //        set
    //        {
    //            lock (_bufferLock)
    //            {
    //                //if (value < PointsPerMillisecond)
    //                //    throw new ArgumentException("容量不能小于单次更新量");
    //                _windowCapacity = value;
    //                TrimActiveQueue(); // 立即应用新容量
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// 添加新数据块（每毫秒调用一次）
    //    /// </summary>
    //    //public void AddDataChunk(List<LivoxLidarCartesianHighRawPoint> newPoints)
    //    public void AddDataChunk(IEnumerable<LivoxLidarCartesianHighRawPoint> newPoints)
    //    {
    //        ArgumentNullException.ThrowIfNull(newPoints);

    //        //if (newPoints.Count() != PointsPerMillisecond)
    //        //    throw new ArgumentException($"必须提供{PointsPerMillisecond}个点");

    //        if (newPoints == null || !newPoints.Any())
    //            return;

    //        var chunk = _chunkPool.Get();
    //        chunk.Points.Clear();
    //        chunk.Points.AddRange(newPoints);

    //        lock (_bufferLock)
    //        {
    //            _activeQueue.Enqueue(chunk);
    //            TrimActiveQueue(); // 根据当前容量淘汰旧数据
    //        }
    //    }

    //    /// <summary>
    //    /// 交换缓冲区并获取当前数据快照
    //    /// </summary>
    //    public IEnumerable<LivoxLidarCartesianHighRawPoint> SwapAndGetSnapshot()
    //    {
    //        lock (_bufferLock)
    //        {
    //            // 交换队列引用
    //            //var temp = _activeQueue;
    //            //_activeQueue = _backQueue;
    //            //_backQueue = temp;
    //            (_backQueue, _activeQueue) = (_activeQueue, _backQueue);

    //            // 返回扁平化数据
    //            return _backQueue.SelectMany(chunk => chunk.Points);
    //        }
    //    }

    //    /// <summary>
    //    /// 根据当前容量修剪活动队列
    //    /// </summary>
    //    private void TrimActiveQueue()
    //    {
    //        // 计算当前队列总点数
    //        int totalPoints = _activeQueue.Sum(chunk => chunk.Points.Count);

    //        // 淘汰旧数据直到符合容量要求
    //        while (totalPoints > _windowCapacity)
    //        {
    //            var oldestChunk = _activeQueue.Dequeue();
    //            totalPoints -= oldestChunk.Points.Count;
    //            _chunkPool.Return(oldestChunk);
    //        }
    //    }
    //}

    ///// <summary>
    ///// 内部数据块定义
    ///// </summary>
    //internal class DataChunk(int capacity)
    //{
    //    public List<LivoxLidarCartesianHighRawPoint> Points { get; } = new List<LivoxLidarCartesianHighRawPoint>(capacity);
    //}

    /////// <summary>
    /////// 自定义对象池策略，用于优化内存分配（更精细控制对象生命周期）
    /////// </summary>
    ////internal class DataChunkPoolPolicy(int capacity) : IPooledObjectPolicy<DataChunk>
    ////{
    ////    private readonly int _capacity = capacity;

    ////    public DataChunk Create() => new(_capacity);

    ////    public bool Return(DataChunk obj)
    ////    {
    ////        obj.Points.Clear();
    ////        return true; // 表示对象可重用
    ////    }
    ////}
}
