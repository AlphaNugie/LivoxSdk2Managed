using ScanUtilityLibraryVer2.LivoxSdk2.Core;
using ScanUtilityLibraryVer2.LivoxSdk2.Include;
using ScanUtilityLibraryVer2.LivoxSdk2.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScanUtilityLibraryVer2.LivoxSdk2.Samples
{
    /// <summary>
    /// Livox激光扫描仪QuickStart测试
    /// </summary>
    public class LivoxLidarQuickStart
    {
        //private static readonly System.Timers.Timer _timer = new System.Timers.Timer(100);

        //// 笛卡尔坐标系高精度坐标点的序列缓存（1mm）
        //private static List<LivoxLidarCartesianHighRawPoint> _cartesianHighRawPointsCache = new List<LivoxLidarCartesianHighRawPoint>();
        //// 笛卡尔坐标系低精度坐标点的序列缓存（10mm）
        //private static List<LivoxLidarCartesianLowRawPoint> _cartesianLowRawPointsCache = new List<LivoxLidarCartesianLowRawPoint>();
        //// 球坐标系坐标点的序列缓存
        //private static List<LivoxLidarSpherPoint> _spherPointsCache = new List<LivoxLidarSpherPoint>();

        // 初始化双缓冲区（容量384,000点，对应192,000点/500ms × 2的规格）
        private readonly BufferManager<LivoxLidarCartesianHighRawPoint> _bufferManagerHighRawPoints = new BufferManager<LivoxLidarCartesianHighRawPoint>(384000);
        private readonly BufferManager<LivoxLidarCartesianLowRawPoint> _bufferManagerLowRawPoints = new BufferManager<LivoxLidarCartesianLowRawPoint>(384000);
        private readonly BufferManager<LivoxLidarSpherPoint> _bufferManagerSpherPoints = new BufferManager<LivoxLidarSpherPoint>(384000);

        #region 属性
        /// <summary>
        /// 设备的三轴角度旋转与空间位移参数集
        /// </summary>
        public static CoordTransParamSet CoordTransParamSet { get; private set; } = null;

        private static int _frameTime = 100;
        /// <summary>
        /// 帧速率（毫秒），因为HAP雷达使用非重复扫描，因此帧速率时间越长，扫描图像细节越丰富
        /// </summary>
        public static int FrameTime
        {
            get { return _frameTime; }
            set
            {
                if (value < 0) return;
                _frameTime = value;
                PkgsPerFrame = 4 * _frameTime;
            }
        }

        private static int _pkgsPerFrame = 400;
        /// <summary>
        /// 每帧的包数，HAP雷达使用非重复扫描，因此包数越多，扫描细节越丰富
        /// <para/>每包含96个点，HAP雷达点发送速率为452KHZ，因此当帧速率为1ms时，每帧包数为452K/1K/96=4，当帧速率为1000ms时，每帧包数为4000
        /// </summary>
        public static int PkgsPerFrame
        {
            get { return _pkgsPerFrame; }
            private set
            {
                _pkgsPerFrame = value;
                PointsPerFrame = 96 * _pkgsPerFrame;
            }
        }

        /// <summary>
        /// 每帧的点数，每包含96个点，因此每帧点数 = 96 * 每帧包数
        /// </summary>
        public static int PointsPerFrame { get; private set; } = 38400;

        /// <summary>
        /// 返回的点的数据类型
        /// </summary>
        public static LivoxLidarPointDataType DataType { get; private set; }

        /// <summary>
        /// 笛卡尔坐标系高精度坐标点的缓存序列（1mm）
        /// </summary>
        public static List<LivoxLidarCartesianHighRawPoint> CartesianHighRawPoints { get; private set; } = new List<LivoxLidarCartesianHighRawPoint>();

        ///// <summary>
        ///// 笛卡尔坐标系高精度坐标点的序列（1mm）
        ///// </summary>
        //private static ConcurrentQueue<LivoxLidarCartesianHighRawPoint> CartesianHighRawPoints { get;/* private set;*/ } = new ConcurrentQueue<LivoxLidarCartesianHighRawPoint>();

        /// <summary>
        /// 笛卡尔坐标系低精度坐标点的序列（10mm）
        /// </summary>
        public static List<LivoxLidarCartesianLowRawPoint> CartesianLowRawPoints { get; private set; } = new List<LivoxLidarCartesianLowRawPoint>();

        /// <summary>
        /// 球坐标系坐标点的序列
        /// </summary>
        public static List<LivoxLidarSpherPoint> SpherPoints { get; private set; } = new List<LivoxLidarSpherPoint>();
        #endregion

        // 添加这些静态变量来保持委托引用
        private static LivoxLidarPointCloudCallBack _pointCloudCallback;
        private static LivoxLidarImuDataCallback _imuDataCallback;
        private static LivoxLidarInfoCallback _pushMsgCallback;
        private static LivoxLidarInfoChangeCallback _infoChangeCallback;

        #region 回调函数
        /// <summary>
        /// 点云数据回调函数
        /// </summary>
        /// <param name="handle">LiDAR 设备句柄</param>
        /// <param name="dev_type">设备类型</param>
        /// <param name="data">点云数据指针</param>
        /// <param name="client_data">客户端数据指针</param>
        public static void PointCloudCallback(uint handle, byte dev_type, IntPtr data, IntPtr client_data)
        {
            if (data == IntPtr.Zero) return;

            // 使用非泛型版本的 Marshal.PtrToStructure，并显式地传递类型参数 typeof(T)
            var packet = (LivoxLidarEthernetPacket)Marshal.PtrToStructure(data, typeof(LivoxLidarEthernetPacket));

            Console.WriteLine($"Point cloud handle: {handle}, udp_counter: {packet.udp_cnt}, data_num: {packet.dot_num}, data_type: {packet.data_type}, length: {packet.length}, frame_counter: {packet.frame_cnt}");
            if (packet.data.Sum(b => b) > 0)
                ;

            #region backup
            //if (packet.data_type == (byte)LivoxLidarPointDataType.kLivoxLidarCartesianCoordinateHighData)
            //{
            //    //创建一个与点云数据数量具有相应长度的数组来存储点云数据
            //    var points = new LivoxLidarCartesianHighRawPoint[packet.dot_num];
            //    //计算整个点云数据的字节大小（先获取单个 LivoxLidarCartesianHighRawPoint 结构体的字节大小）
            //    int size = Marshal.SizeOf(typeof(LivoxLidarCartesianHighRawPoint)) * packet.dot_num;
            //    //将 points 数组固定在内存中，以防止垃圾回收器移动它。
            //    //GCHandle.Alloc 方法分配一个句柄，使得垃圾回收器不会移动 points 数组。GCHandleType.Pinned 表示该句柄为固定句柄
            //    GCHandle handleData = GCHandle.Alloc(points, GCHandleType.Pinned);
            //    //获取固定数组的指针：用handleData.AddrOfPinnedObject() 方法返回 points 数组的内存地址（指针）
            //    IntPtr destinationPtr = handleData.AddrOfPinnedObject();
            //    // 将 byte[] 数据复制到 LivoxLidarCartesianHighRawPoint 数组
            //    Marshal.Copy(packet.data, 0, destinationPtr, size);
            //    //释放对 points 数组的固定：用handleData.Free() 方法释放之前分配的固定句柄，使得垃圾回收器可以再次移动 points 数组
            //    handleData.Free();
            //    // 处理点云数据
            //}
            //else if (packet.data_type == (byte)LivoxLidarPointDataType.kLivoxLidarCartesianCoordinateLowData)
            //{
            //    var points = new LivoxLidarCartesianLowRawPoint[packet.dot_num];
            //    int size = Marshal.SizeOf(typeof(LivoxLidarCartesianLowRawPoint)) * packet.dot_num;
            //    GCHandle handleData = GCHandle.Alloc(points, GCHandleType.Pinned);
            //    IntPtr destinationPtr = handleData.AddrOfPinnedObject();
            //    Marshal.Copy(packet.data, 0, destinationPtr, size);
            //    handleData.Free();
            //}
            //else if (packet.data_type == (byte)LivoxLidarPointDataType.kLivoxLidarSphericalCoordinateData)
            //{
            //    var points = new LivoxLidarSpherPoint[packet.dot_num];
            //    int size = Marshal.SizeOf(typeof(LivoxLidarSpherPoint)) * packet.dot_num;
            //    GCHandle handleData = GCHandle.Alloc(points, GCHandleType.Pinned);
            //    IntPtr destinationPtr = handleData.AddrOfPinnedObject();
            //    Marshal.Copy(packet.data, 0, destinationPtr, size);
            //    handleData.Free();
            //}
            #endregion
            object points;
            int size; //点云数据的字节大小
            DataType = packet.data_type;
            switch (DataType)
            {
                case LivoxLidarPointDataType.kLivoxLidarCartesianCoordinateHighData:
                    //创建一个与点云数据数量具有相应长度的数组来存储点云数据
                    points = new LivoxLidarCartesianHighRawPoint[packet.dot_num];
                    //计算整个点云数据的字节大小（先获取单个 LivoxLidarCartesianHighRawPoint 结构体的字节大小）
                    size = Marshal.SizeOf(typeof(LivoxLidarCartesianHighRawPoint)) * packet.dot_num;
                    break;
                case LivoxLidarPointDataType.kLivoxLidarCartesianCoordinateLowData:
                    points = new LivoxLidarCartesianLowRawPoint[packet.dot_num];
                    size = Marshal.SizeOf(typeof(LivoxLidarCartesianLowRawPoint)) * packet.dot_num;
                    break;
                case LivoxLidarPointDataType.kLivoxLidarSphericalCoordinateData:
                    points = new LivoxLidarSpherPoint[packet.dot_num];
                    size = Marshal.SizeOf(typeof(LivoxLidarSpherPoint)) * packet.dot_num;
                    break;
                default:
                    return;
            }
            //if (packet.data_type == LivoxLidarPointDataType.kLivoxLidarCartesianCoordinateHighData)
            //{
            //    //创建一个与点云数据数量具有相应长度的数组来存储点云数据
            //    points = new LivoxLidarCartesianHighRawPoint[packet.dot_num];
            //    //计算整个点云数据的字节大小（先获取单个 LivoxLidarCartesianHighRawPoint 结构体的字节大小）
            //    size = Marshal.SizeOf(typeof(LivoxLidarCartesianHighRawPoint)) * packet.dot_num;
            //}
            //else if (packet.data_type == LivoxLidarPointDataType.kLivoxLidarCartesianCoordinateLowData)
            //{
            //    points = new LivoxLidarCartesianLowRawPoint[packet.dot_num];
            //    size = Marshal.SizeOf(typeof(LivoxLidarCartesianLowRawPoint)) * packet.dot_num;
            //}
            //else if (packet.data_type == LivoxLidarPointDataType.kLivoxLidarSphericalCoordinateData)
            //{
            //    points = new LivoxLidarSpherPoint[packet.dot_num];
            //    size = Marshal.SizeOf(typeof(LivoxLidarSpherPoint)) * packet.dot_num;
            //}
            //else return;

            //将 points 数组固定在内存中，以防止垃圾回收器移动它。
            //GCHandle.Alloc 方法分配一个句柄，使得垃圾回收器不会移动 points 数组。GCHandleType.Pinned 表示该句柄为固定句柄
            GCHandle handleData = GCHandle.Alloc(points, GCHandleType.Pinned);
            //获取固定数组的指针：用handleData.AddrOfPinnedObject() 方法返回 points 数组的内存地址（指针）
            IntPtr destinationPtr = handleData.AddrOfPinnedObject();
            // 将 byte[] 数据复制到 LivoxLidarCartesianHighRawPoint 数组
            Marshal.Copy(packet.data, 0, destinationPtr, size);
            //释放对 points 数组的固定：用handleData.Free() 方法释放之前分配的固定句柄，使得垃圾回收器可以再次移动 points 数组
            handleData.Free();
            //根据数据类型将当前package的点云插入最前侧
            switch (DataType)
            {
                case LivoxLidarPointDataType.kLivoxLidarCartesianCoordinateHighData:
                    CartesianHighRawPoints.InsertRange(0, (LivoxLidarCartesianHighRawPoint[])points);
                    //var highPoints = (LivoxLidarCartesianHighRawPoint[])points;
                    //if (CoordTransParamSet != null)
                    //    highPoints = highPoints.TransformPoints(CoordTransParamSet);
                    ////CartesianHighRawPoints.InsertRange(0, highPoints);
                    //foreach (var p in highPoints)
                    //{
                    //    while (CartesianHighRawPoints.Count >= PointsPerFrame)
                    //        CartesianHighRawPoints.TryDequeue(out _); // 丢弃旧数据
                    //    CartesianHighRawPoints.Enqueue(p);
                    //}
                    break;
                case LivoxLidarPointDataType.kLivoxLidarCartesianCoordinateLowData:
                    CartesianLowRawPoints.InsertRange(0, (LivoxLidarCartesianLowRawPoint[])points);
                    break;
                case LivoxLidarPointDataType.kLivoxLidarSphericalCoordinateData:
                    SpherPoints.InsertRange(0, (LivoxLidarSpherPoint[])points);
                    break;
            }
        }

        /// <summary>
        /// IMU 数据回调函数
        /// </summary>
        /// <param name="handle">LiDAR 设备句柄</param>
        /// <param name="dev_type">设备类型</param>
        /// <param name="data">IMU 数据指针</param>
        /// <param name="client_data">客户端数据指针</param>
        public static void ImuDataCallback(uint handle, byte dev_type, IntPtr data, IntPtr client_data)
        {
            if (data == IntPtr.Zero) return;

            // 使用非泛型版本的 Marshal.PtrToStructure
            var packet = (LivoxLidarEthernetPacket)Marshal.PtrToStructure(data, typeof(LivoxLidarEthernetPacket));
            Console.WriteLine($"IMU data callback handle: {handle}, data_num: {packet.dot_num}, data_type: {packet.data_type}, length: {packet.length}, frame_counter: {packet.frame_cnt}");
        }

        /// <summary>
        /// 工作模式回调函数
        /// </summary>
        /// <param name="status">操作状态</param>
        /// <param name="handle">LiDAR 设备句柄</param>
        /// <param name="response">响应数据指针</param>
        /// <param name="client_data">客户端数据指针</param>
        public static void WorkModeCallback(LivoxLidarStatus status, uint handle, IntPtr response, IntPtr client_data)
        {
            if (response == IntPtr.Zero) return;

            // 使用非泛型版本的 Marshal.PtrToStructure
            var result = (LivoxLidarAsyncControlResponse)Marshal.PtrToStructure(response, typeof(LivoxLidarAsyncControlResponse));
            Console.WriteLine($"WorkModeCallback, status: {status}, handle: {handle}, ret_code: {result.ret_code}, error_key: {result.error_key}");
        }

        /// <summary>
        /// 重启回调函数
        /// </summary>
        /// <param name="status">操作状态</param>
        /// <param name="handle">LiDAR 设备句柄</param>
        /// <param name="response">响应数据指针</param>
        /// <param name="client_data">客户端数据指针</param>
        public static void RebootCallback(LivoxLidarStatus status, uint handle, IntPtr response, IntPtr client_data)
        {
            if (response == IntPtr.Zero) return;

            // 使用非泛型版本的 Marshal.PtrToStructure
            var result = (LivoxLidarRebootResponse)Marshal.PtrToStructure(response, typeof(LivoxLidarRebootResponse));
            Console.WriteLine($"RebootCallback, status: {status}, handle: {handle}, ret_code: {result.ret_code}");
        }

        /// <summary>
        /// 设置 IP 信息回调函数
        /// </summary>
        /// <param name="status">操作状态</param>
        /// <param name="handle">LiDAR 设备句柄</param>
        /// <param name="response">响应数据指针</param>
        /// <param name="client_data">客户端数据指针</param>
        public static void SetIpInfoCallback(LivoxLidarStatus status, uint handle, IntPtr response, IntPtr client_data)
        {
            if (response == IntPtr.Zero) return;

            // 使用非泛型版本的 Marshal.PtrToStructure
            var result = (LivoxLidarAsyncControlResponse)Marshal.PtrToStructure(response, typeof(LivoxLidarAsyncControlResponse));
            Console.WriteLine($"LivoxLidarIpInfoCallback, status: {status}, handle: {handle}, ret_code: {result.ret_code}, error_key: {result.error_key}");

            if (result.ret_code == 0 && result.error_key == 0)
                LivoxLidarSdk.LivoxLidarRequestReboot(handle, RebootCallback, IntPtr.Zero);
        }

        /// <summary>
        /// 查询内部信息回调函数
        /// </summary>
        /// <param name="status">操作状态</param>
        /// <param name="handle">LiDAR 设备句柄</param>
        /// <param name="response">响应数据指针</param>
        /// <param name="client_data">客户端数据指针</param>
        public static void QueryInternalInfoCallback(LivoxLidarStatus status, uint handle, IntPtr response, IntPtr client_data)
        {
            if (status != LivoxLidarStatus.kLivoxLidarStatusSuccess)
            {
                Console.WriteLine("Query lidar internal info failed.");
                LivoxLidarSdk.QueryLivoxLidarInternalInfo(handle, QueryInternalInfoCallback, IntPtr.Zero);
                return;
            }

            if (response == IntPtr.Zero) return;

            // 使用非泛型版本的 Marshal.PtrToStructure
            var result = (LivoxLidarDiagInternalInfoResponse)Marshal.PtrToStructure(response, typeof(LivoxLidarDiagInternalInfoResponse));

            // 处理内部信息
            Console.WriteLine("Query internal info callback.");
        }

        /// <summary>
        /// LiDAR 信息变化回调函数
        /// </summary>
        /// <param name="handle">LiDAR 设备句柄</param>
        /// <param name="info">设备信息指针</param>
        /// <param name="client_data">客户端数据指针</param>
        public static void LidarInfoChangeCallback(uint handle, IntPtr info, IntPtr client_data)
        {
            if (info == IntPtr.Zero)
            {
                Console.WriteLine("Lidar info change callback failed, the info is nullptr.");
                return;
            }

            // 使用非泛型版本的 Marshal.PtrToStructure
            var lidarInfo = (LivoxLidarInfo)Marshal.PtrToStructure(info, typeof(LivoxLidarInfo));
            Console.WriteLine($"LidarInfoChangeCallback Lidar handle: {handle} SN: {lidarInfo.sn}");

            // 将工作模式设置为正常模式，即启动 LiDAR
            LivoxLidarSdk.SetLivoxLidarWorkMode(handle, (int)LivoxLidarWorkMode.kLivoxLidarNormal, WorkModeCallback, IntPtr.Zero);

            LivoxLidarSdk.QueryLivoxLidarInternalInfo(handle, QueryInternalInfoCallback, IntPtr.Zero);
        }

        /// <summary>
        /// LiDAR 推送消息回调函数
        /// </summary>
        /// <param name="handle">LiDAR 设备句柄</param>
        /// <param name="dev_type">设备类型</param>
        /// <param name="info">消息信息</param>
        /// <param name="client_data">客户端数据指针</param>
        public static void LivoxLidarPushMsgCallback(uint handle, byte dev_type, string info, IntPtr client_data)
        {
            var tmp_addr = new System.Net.IPAddress(handle);
            Console.WriteLine($"handle: {handle}, ip: {tmp_addr}, push msg info: {info}");
        }
        #endregion

        private static CancellationTokenSource _cancellationTokenSource;
        /// <summary>
        /// 以配置文件启动并获取点云数据
        /// </summary>
        /// <param name="configFile">配置文件名称</param>
        /// <param name="coordTransParamSet">坐标转换参数集</param>
        ///// <param name="args">命令行参数</param>
        public static void Start(string configFile, CoordTransParamSet coordTransParamSet = null)
        {
            if (string.IsNullOrWhiteSpace(configFile))
            {
                Console.WriteLine("Config file Invalid, must input config file path.");
                return;
            }
            string path = configFile.Contains(Path.VolumeSeparatorChar) ? configFile : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFile);
            if (!File.Exists(path))
            {
                Console.WriteLine("Config file does not exist, check again.");
                return;
            }

            CoordTransParamSet = coordTransParamSet;
            LivoxLidarLoggerCfgInfo livoxLidarLoggerCfgInfo = new LivoxLidarLoggerCfgInfo();
            // 初始化 Livox SDK
            if (!LivoxLidarSdk.LivoxLidarSdkInit(path, "", ref livoxLidarLoggerCfgInfo))
            {
                Console.WriteLine("Livox Init Failed");
                LivoxLidarSdk.LivoxLidarSdkUninit();
                return;
            }

            _pointCloudCallback = PointCloudCallback;
            _imuDataCallback = ImuDataCallback;
            _pushMsgCallback = LivoxLidarPushMsgCallback;
            _infoChangeCallback = LidarInfoChangeCallback;

            // 设置回调函数
            //LivoxLidarSdk.SetLivoxLidarPointCloudCallBack(PointCloudCallback, IntPtr.Zero);
            //LivoxLidarSdk.SetLivoxLidarImuDataCallback(ImuDataCallback, IntPtr.Zero);
            //LivoxLidarSdk.SetLivoxLidarInfoCallback(LivoxLidarPushMsgCallback, IntPtr.Zero);
            //LivoxLidarSdk.SetLivoxLidarInfoChangeCallback(LidarInfoChangeCallback, IntPtr.Zero);
            LivoxLidarSdk.SetLivoxLidarPointCloudCallBack(_pointCloudCallback, IntPtr.Zero);
            LivoxLidarSdk.SetLivoxLidarImuDataCallback(_imuDataCallback, IntPtr.Zero);
            LivoxLidarSdk.SetLivoxLidarInfoCallback(_pushMsgCallback, IntPtr.Zero);
            LivoxLidarSdk.SetLivoxLidarInfoChangeCallback(_infoChangeCallback, IntPtr.Zero);

            // 创建 CancellationTokenSource
            _cancellationTokenSource = new CancellationTokenSource();
            // 启动异步监控缓存
            // 使用下划线表示这个任务不需要被等待
            _ = MonitorAndTrimCacheAsync(_cancellationTokenSource.Token);

            //// 保持程序运行
            //Thread.Sleep(300000);

            //Stop();
        }

        /// <summary>
        /// 结束并停止获取点云
        /// </summary>
        public static void Stop()
        {
            // 取消 MonitorAndTrimCacheAsync
            _cancellationTokenSource?.Cancel();

            // 移除回调前需要保持引用
            LivoxLidarSdk.SetLivoxLidarPointCloudCallBack(null, IntPtr.Zero);
            LivoxLidarSdk.SetLivoxLidarImuDataCallback(null, IntPtr.Zero);
            LivoxLidarSdk.SetLivoxLidarInfoCallback(null, IntPtr.Zero);
            LivoxLidarSdk.SetLivoxLidarInfoChangeCallback(null, IntPtr.Zero);

            LivoxLidarSdk.LivoxLidarSdkUninit();

            // 清理静态委托引用
            _pointCloudCallback = null;
            _imuDataCallback = null;
            _pushMsgCallback = null;
            _infoChangeCallback = null;

            Console.WriteLine("Livox Quick Start Demo End!");
        }

        /// <summary>
        /// 获取当前帧的数据快照（线程安全）（笛卡尔坐标系高精度坐标点，精度1mm）
        /// </summary>
        public LivoxLidarCartesianHighRawPoint[] GetCurrentFrameOfHighRawPoints()
        {
            return CartesianHighRawPoints.ToArray();
        }

        /// <summary>
        /// 检测缓存列表的长度，当超过PkgsPerFrame后，在缓存列表末尾移除超出的部分，并把原始列表内的所有元素替换为缓存列表内的所有剩余元素
        /// </summary>
        /// <returns></returns>
        public static async Task MonitorAndTrimCacheAsync(CancellationToken cancellationToken)
        {
            //检查取消请求
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1); // 每毫秒检测一次

                // 监测高精度列表，并移除超出缓存长度的部分
                if (CartesianHighRawPoints.Count > PointsPerFrame)
                    CartesianHighRawPoints.RemoveRange(PointsPerFrame, CartesianHighRawPoints.Count - PointsPerFrame);

                // 监测低精度列表，并移除超出缓存长度的部分
                if (CartesianLowRawPoints.Count > PointsPerFrame)
                    CartesianLowRawPoints.RemoveRange(PointsPerFrame, CartesianLowRawPoints.Count - PointsPerFrame);

                // 监测球坐标列表
                if (SpherPoints.Count > PointsPerFrame)
                    SpherPoints.RemoveRange(PointsPerFrame, SpherPoints.Count - PointsPerFrame);

                //Console.WriteLine($"高精度点数量: {CartesianHighRawPoints.Count}, 首点XYZ坐标: [{(CartesianHighRawPoints.Count > 0 ? CartesianHighRawPoints[0].x : 0)}], [{(CartesianHighRawPoints.Count > 0 ? CartesianHighRawPoints[0].y : 0)}], [{(CartesianHighRawPoints.Count > 0 ? CartesianHighRawPoints[0].z : 0)}], 低精度点数量: {CartesianLowRawPoints.Count}, 球坐标点数量: {SpherPoints.Count}");
            }
        }

    }
}
