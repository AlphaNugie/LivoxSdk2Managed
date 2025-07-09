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

        #region 常量定义
        /// <summary>
        /// 每包包含的点数为96
        /// </summary>
        public const int POINTS_PER_PKG = 96;

        /// <summary>
        /// 每毫秒包含的包数为4
        /// </summary>
        public const int PKGS_PER_MILLISEC = 4;

        /// <summary>
        /// 每毫秒包含的点数
        /// </summary>
        public const int POINTS_PER_MILLISEC = POINTS_PER_PKG * PKGS_PER_MILLISEC;
        #endregion

        /// <summary>
        /// 同步锁对象（确保跨线程操作原子性）
        /// </summary>
#if NET9_0_OR_GREATER
        private static readonly Lock _syncRoot = new();
#elif NET45
        private static readonly object _syncRoot = new object();
#endif

        #region 属性
        /// <summary>
        /// 设备的三轴角度旋转与空间位移参数集
        /// </summary>
        public static CoordTransParamSet CoordTransParamSet { get; private set; } = new CoordTransParamSet();

        /// <summary>
        /// 双缓冲的基础信息
        /// </summary>
        public static string BufferHeader { get; private set; } = string.Empty;

        /// <summary>
        /// 以太网数据包结构体简略信息
        /// </summary>
        public static string PacketHeader { get; private set; } = string.Empty;

        /// <summary>
        /// 以太网数据包体结构体内的点云数据（16进制字符串）
        /// </summary>
        public static string PacketData { get; private set; } = string.Empty;

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
                PkgsPerFrame = PKGS_PER_MILLISEC * _frameTime;
            }
        }

        //private static int _pkgsPerFrame = PKGS_PER_MILLISEC * 100;
        private static int _pkgsPerFrame = PKGS_PER_MILLISEC * _frameTime;
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
                PointsPerFrame = POINTS_PER_PKG * _pkgsPerFrame;
            }
        }

        //private static int _ptsPerFrame = POINTS_PER_MILLISEC * 100;
        private static int _ptsPerFrame = POINTS_PER_MILLISEC * _frameTime;
        /// <summary>
        /// 每帧的点数，每包含96个点，因此每帧点数 = 96 * 每帧包数；当帧速率为1ms时，每帧包数为452K/1K/96=4，每帧点数为96 * 4 = 384
        /// </summary>
        //public static int PointsPerFrame { get; private set; } = POINTS_PER_MILLISEC * 100;
        public static int PointsPerFrame
        {
            get { return _ptsPerFrame; }
            private set
            {
                _ptsPerFrame = value;
//#if NET9_0_OR_GREATER
                BufferServiceHighRawPoints.WindowCapacity = BufferServiceLowRawPoints.WindowCapacity = BufferServiceSpherPoints.WindowCapacity = _ptsPerFrame;
//#endif
            }
        }

        /// <summary>
        /// 返回的点的数据类型
        /// </summary>
        public static LivoxLidarPointDataType DataType { get; private set; }

        #region 双缓冲
//#if NET9_0_OR_GREATER
//        /// <summary>
//        /// 双缓冲服务对象（笛卡尔坐标系高精度坐标点）
//        /// </summary>
//        public static DataBufferService<LivoxLidarCartesianHighRawPoint> BufferServiceHighRawPoints { get; private set; } = new DataBufferService<LivoxLidarCartesianHighRawPoint>() { WindowCapacity = _ptsPerFrame };

//        /// <summary>
//        /// 双缓冲服务对象（笛卡尔坐标系低精度坐标点）
//        /// </summary>
//        public static DataBufferService<LivoxLidarCartesianLowRawPoint> BufferServiceLowRawPoints { get; private set; } = new DataBufferService<LivoxLidarCartesianLowRawPoint>() { WindowCapacity = _ptsPerFrame };

//        /// <summary>
//        /// 双缓冲服务对象（球坐标系坐标点）
//        /// </summary>
//        public static DataBufferService<LivoxLidarSpherPoint> BufferServiceSpherPoints { get; private set; } = new DataBufferService<LivoxLidarSpherPoint>() { WindowCapacity = _ptsPerFrame };
//#elif NET45
//        /// <summary>
//        /// 笛卡尔坐标系高精度坐标点的缓存序列（1mm）
//        /// </summary>
//        public static List<LivoxLidarCartesianHighRawPoint> CartesianHighRawPoints { get; private set; } = new List<LivoxLidarCartesianHighRawPoint>();

//        /// <summary>
//        /// 笛卡尔坐标系低精度坐标点的序列（10mm）
//        /// </summary>
//        public static List<LivoxLidarCartesianLowRawPoint> CartesianLowRawPoints { get; private set; } = new List<LivoxLidarCartesianLowRawPoint>();

//        /// <summary>
//        /// 球坐标系坐标点的序列
//        /// </summary>
//        public static List<LivoxLidarSpherPoint> SpherPoints { get; private set; } = new List<LivoxLidarSpherPoint>();
//#endif
        /// <summary>
        /// 双缓冲服务对象（笛卡尔坐标系高精度坐标点）
        /// </summary>
        public static DataBufferService<LivoxLidarCartesianHighRawPoint> BufferServiceHighRawPoints { get; private set; } = new DataBufferService<LivoxLidarCartesianHighRawPoint>() { WindowCapacity = _ptsPerFrame };

        /// <summary>
        /// 双缓冲服务对象（笛卡尔坐标系低精度坐标点）
        /// </summary>
        public static DataBufferService<LivoxLidarCartesianLowRawPoint> BufferServiceLowRawPoints { get; private set; } = new DataBufferService<LivoxLidarCartesianLowRawPoint>() { WindowCapacity = _ptsPerFrame };

        /// <summary>
        /// 双缓冲服务对象（球坐标系坐标点）
        /// </summary>
        public static DataBufferService<LivoxLidarSpherPoint> BufferServiceSpherPoints { get; private set; } = new DataBufferService<LivoxLidarSpherPoint>() { WindowCapacity = _ptsPerFrame };
#if NET45
        ///// <summary>
        ///// 笛卡尔坐标系高精度坐标点的缓存序列（1mm）
        ///// </summary>
        //public static List<LivoxLidarCartesianHighRawPoint> CartesianHighRawPoints { get; private set; } = new List<LivoxLidarCartesianHighRawPoint>();

        ///// <summary>
        ///// 笛卡尔坐标系低精度坐标点的序列（10mm）
        ///// </summary>
        //public static List<LivoxLidarCartesianLowRawPoint> CartesianLowRawPoints { get; private set; } = new List<LivoxLidarCartesianLowRawPoint>();

        ///// <summary>
        ///// 球坐标系坐标点的序列
        ///// </summary>
        //public static List<LivoxLidarSpherPoint> SpherPoints { get; private set; } = new List<LivoxLidarSpherPoint>();
#endif
        #endregion

        #endregion

        // 添加这些静态变量来保持委托引用
#if NET9_0_OR_GREATER
        private static LivoxLidarPointCloudCallBack? _pointCloudCallback;
        private static LivoxLidarImuDataCallback? _imuDataCallback;
        private static LivoxLidarInfoCallback? _pushMsgCallback;
        private static LivoxLidarInfoChangeCallback? _infoChangeCallback;
#elif NET45
        private static LivoxLidarPointCloudCallBack _pointCloudCallback;
        private static LivoxLidarImuDataCallback _imuDataCallback;
        private static LivoxLidarInfoCallback _pushMsgCallback;
        private static LivoxLidarInfoChangeCallback _infoChangeCallback;
#endif

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
#if NET9_0_OR_GREATER
            var packet = Marshal.PtrToStructure<LivoxLidarEthernetPacket>(data);
#elif NET45
            var packet = (LivoxLidarEthernetPacket)Marshal.PtrToStructure(data, typeof(LivoxLidarEthernetPacket));
#endif
            PacketHeader = $"Point cloud handle: {handle}, udp_counter: {packet.udp_cnt}, data_num: {packet.dot_num}, data_type: {packet.data_type}, length: {packet.length}, frame_counter: {packet.frame_cnt}";
            //取280个字节的数据并转换为16进制字符串
            int datalen = 280;
            PacketData = packet.data.Take(datalen).Aggregate("", (current, b) => current + b.ToString("X2") + " ").Trim();

            //Console.WriteLine($"Point cloud handle: {handle}, udp_counter: {packet.udp_cnt}, data_num: {packet.dot_num}, data_type: {packet.data_type}, length: {packet.length}, frame_counter: {packet.frame_cnt}");
            Console.WriteLine(PacketHeader);
            //if (packet.data.Sum(b => b) > 0)
            //    ;

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
#if NET9_0_OR_GREATER
                    size = Marshal.SizeOf<LivoxLidarCartesianHighRawPoint>() * packet.dot_num;
#elif NET45
                    size = Marshal.SizeOf(typeof(LivoxLidarCartesianHighRawPoint)) * packet.dot_num;
#endif
                    break;
                case LivoxLidarPointDataType.kLivoxLidarCartesianCoordinateLowData:
                    points = new LivoxLidarCartesianLowRawPoint[packet.dot_num];
#if NET9_0_OR_GREATER
                    size = Marshal.SizeOf<LivoxLidarCartesianLowRawPoint>() * packet.dot_num;
#elif NET45
                    size = Marshal.SizeOf(typeof(LivoxLidarCartesianLowRawPoint)) * packet.dot_num;
#endif
                    break;
                case LivoxLidarPointDataType.kLivoxLidarSphericalCoordinateData:
                    points = new LivoxLidarSpherPoint[packet.dot_num];
#if NET9_0_OR_GREATER
                    size = Marshal.SizeOf<LivoxLidarSpherPoint>() * packet.dot_num;
#elif NET45
                    size = Marshal.SizeOf(typeof(LivoxLidarSpherPoint)) * packet.dot_num;
#endif
                    break;
                default:
                    return;
            }

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
                    var newHighPoints = (LivoxLidarCartesianHighRawPoint[])points;
                    //假如坐标转换参数集不为null，则进行坐标转换
                    //为增加性能，在将点云数据插入缓存前，先进行坐标转换
                    if (CoordTransParamSet != null)
                        newHighPoints = newHighPoints.TransformPoints(CoordTransParamSet);
//#if NET9_0_OR_GREATER
//                    BufferServiceHighRawPoints.AddDataChunk(newHighPoints);
//#elif NET45
//                    CartesianHighRawPoints.InsertRange(0, newHighPoints);
//#endif

                    BufferServiceHighRawPoints.AddDataChunk(newHighPoints);
#if NET45
                    //CartesianHighRawPoints.InsertRange(0, newHighPoints);
#endif
                    break;
                case LivoxLidarPointDataType.kLivoxLidarCartesianCoordinateLowData:
                    var newLowPoints = (LivoxLidarCartesianLowRawPoint[])points;
                    //假如坐标转换参数集不为null，则进行坐标转换
                    //为增加性能，在将点云数据插入缓存前，先进行坐标转换
                    if (CoordTransParamSet != null)
                        newLowPoints = newLowPoints.TransformPoints(CoordTransParamSet);
//#if NET9_0_OR_GREATER
//                    BufferServiceLowRawPoints.AddDataChunk(newLowPoints);
//#elif NET45
//                    CartesianLowRawPoints.InsertRange(0, newLowPoints);
//#endif

                    BufferServiceLowRawPoints.AddDataChunk(newLowPoints);
#if NET45
                    //CartesianLowRawPoints.InsertRange(0, newLowPoints);
#endif
                    break;
                case LivoxLidarPointDataType.kLivoxLidarSphericalCoordinateData:
//#if NET9_0_OR_GREATER
                    var newSpherPoints = (LivoxLidarSpherPoint[])points;
                    //球坐标系不能进行坐标转换，因此直接插入缓存
                    BufferServiceSpherPoints.AddDataChunk(newSpherPoints);
//#endif
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
#if NET9_0_OR_GREATER
            var packet = Marshal.PtrToStructure<LivoxLidarEthernetPacket>(data);
#elif NET45
            var packet = (LivoxLidarEthernetPacket)Marshal.PtrToStructure(data, typeof(LivoxLidarEthernetPacket));
#endif
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
#if NET9_0_OR_GREATER
            var result = Marshal.PtrToStructure<LivoxLidarAsyncControlResponse>(response);
#elif NET45
            var result = (LivoxLidarAsyncControlResponse)Marshal.PtrToStructure(response, typeof(LivoxLidarAsyncControlResponse));
#endif
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
            // var result = (LivoxLidarRebootResponse)Marshal.PtrToStructure(response, typeof(LivoxLidarRebootResponse));
#if NET9_0_OR_GREATER
            var result = Marshal.PtrToStructure<LivoxLidarRebootResponse>(response);
#elif NET45
            var result = (LivoxLidarRebootResponse)Marshal.PtrToStructure(response, typeof(LivoxLidarRebootResponse));
#endif
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
#if NET9_0_OR_GREATER
            var result = Marshal.PtrToStructure<LivoxLidarAsyncControlResponse>(response);
#elif NET45
            var result = (LivoxLidarAsyncControlResponse)Marshal.PtrToStructure(response, typeof(LivoxLidarAsyncControlResponse));
#endif
            Console.WriteLine($"LivoxLidarIpInfoCallback, status: {status}, handle: {handle}, ret_code: {result.ret_code}, error_key: {result.error_key}");

            if (result.ret_code == 0 && result.error_key == 0)
            {
                int rebootResult = LivoxLidarSdk.LivoxLidarRequestReboot(handle, RebootCallback, IntPtr.Zero);
                if (rebootResult != 0) // 检查 HRESULT 是否成功
                {
                    Console.WriteLine($"设备重启失败，错误代码: {rebootResult}");
                    // 在这里可以添加更多的错误处理逻辑，比如重试重启或通知用户
                }
            }
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
                //LivoxLidarSdk.QueryLivoxLidarInternalInfo(handle, QueryInternalInfoCallback, IntPtr.Zero);
                // 保存 HRESULT 返回值
                int queryResult = LivoxLidarSdk.QueryLivoxLidarInternalInfo(handle, QueryInternalInfoCallback, IntPtr.Zero);
                // 检查 HRESULT 返回值
                if (queryResult != 0) // HRESULT 为 0 通常表示成功
                {
                    Console.WriteLine($"查询雷达内部信息失败，错误代码：{queryResult}");
                }
                return;
            }

            if (response == IntPtr.Zero) return;

            // 使用非泛型版本的 Marshal.PtrToStructure
            // var result = (LivoxLidarDiagInternalInfoResponse)Marshal.PtrToStructure(response, typeof(LivoxLidarDiagInternalInfoResponse));
#if NET9_0_OR_GREATER
            var result = Marshal.PtrToStructure<LivoxLidarDiagInternalInfoResponse>(response);
#elif NET45
            var result = (LivoxLidarDiagInternalInfoResponse)Marshal.PtrToStructure(response, typeof(LivoxLidarDiagInternalInfoResponse));
#endif

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
#if NET9_0_OR_GREATER
            var lidarInfo = Marshal.PtrToStructure<LivoxLidarInfo>(info);
#elif NET45
            var lidarInfo = (LivoxLidarInfo)Marshal.PtrToStructure(info, typeof(LivoxLidarInfo));
#endif
            Console.WriteLine($"LidarInfoChangeCallback Lidar handle: {handle} SN: {lidarInfo.sn}");

            // 将工作模式设置为正常模式，即启动 LiDAR
            //LivoxLidarSdk.SetLivoxLidarWorkMode(handle, (int)LivoxLidarWorkMode.kLivoxLidarNormal, WorkModeCallback, IntPtr.Zero);
            //LivoxLidarSdk.QueryLivoxLidarInternalInfo(handle, QueryInternalInfoCallback, IntPtr.Zero);
            // 将工作模式设置为正常模式，即启动 LiDAR
            int result = LivoxLidarSdk.SetLivoxLidarWorkMode(handle, (int)LivoxLidarWorkMode.kLivoxLidarNormal, WorkModeCallback, IntPtr.Zero);
            if (result != 0) // 假设 0 表示成功，非 0 表示失败
            {
                Console.WriteLine($"SetLivoxLidarWorkMode failed with error code: {result}");
                // 在这里可以添加更多的错误处理逻辑，例如重试、记录日志等
            }
            // 检查 QueryLivoxLidarInternalInfo 的返回值
            int queryResult = LivoxLidarSdk.QueryLivoxLidarInternalInfo(handle, QueryInternalInfoCallback, IntPtr.Zero);
            if (queryResult != 0) // 假设 0 表示成功，非 0 表示失败
            {
                Console.WriteLine($"QueryLivoxLidarInternalInfo failed with error code: {queryResult}");
                // 在这里可以添加更多的错误处理逻辑，例如重试、记录日志等
            }
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

        #region 启动与停止
#if NET9_0_OR_GREATER
        private static CancellationTokenSource? _cancellationTokenSource;
#elif NET45
        private static CancellationTokenSource _cancellationTokenSource;
#endif

        /// <summary>
        /// 以配置文件启动并获取点云数据
        /// </summary>
        /// <param name="configFile">配置文件名称</param>
        /// <param name="coordTransParamSet">坐标转换参数集</param>
#if NET9_0_OR_GREATER
        public static void Start(string configFile, CoordTransParamSet? coordTransParamSet = null)
#elif NET45
        public static void Start(string configFile, CoordTransParamSet coordTransParamSet = null)
#endif
        {
            Start(configFile, out _, coordTransParamSet);
        }

        /// <summary>
        /// 以配置文件启动并获取点云数据
        /// </summary>
        /// <param name="configFile">配置文件名称</param>
        /// <param name="coordTransParamSet">坐标转换参数集</param>
        /// <param name="msg">输出的消息</param>
#if NET9_0_OR_GREATER
        public static void Start(string configFile, out string msg, CoordTransParamSet? coordTransParamSet = null)
#elif NET45
        public static void Start(string configFile, out string msg, CoordTransParamSet coordTransParamSet = null)
#endif
        {
            msg = string.Empty;
            if (string.IsNullOrWhiteSpace(configFile))
            {
                msg = "Config file Invalid, must input config file path.";
                goto ERROR;
                //Console.WriteLine("Config file Invalid, must input config file path.");
                //return;
            }
            string path = configFile.Contains(Path.VolumeSeparatorChar) ? configFile : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFile);
            if (!File.Exists(path))
            {
                msg = "Config file does not exist, check again.";
                goto ERROR;
                //Console.WriteLine("Config file does not exist, check again.");
                //return;
            }

            if (coordTransParamSet!= null)
                CoordTransParamSet = coordTransParamSet;
#if NET9_0_OR_GREATER
            LivoxLidarLoggerCfgInfo livoxLidarLoggerCfgInfo = new();
#elif NET45
            LivoxLidarLoggerCfgInfo livoxLidarLoggerCfgInfo = new LivoxLidarLoggerCfgInfo();
#endif
            // 初始化 Livox SDK
            if (!LivoxLidarSdk.LivoxLidarSdkInit(path, "", ref livoxLidarLoggerCfgInfo))
            {
                msg = "Livox Init Failed";
                //Console.WriteLine("Livox Init Failed");
                Console.WriteLine(msg);
                LivoxLidarSdk.LivoxLidarSdkUninit();
                return;
            }

            _pointCloudCallback = PointCloudCallback;
            _imuDataCallback = ImuDataCallback;
            _pushMsgCallback = LivoxLidarPushMsgCallback;
            _infoChangeCallback = LidarInfoChangeCallback;

            // 设置回调函数
            LivoxLidarSdk.SetLivoxLidarPointCloudCallBack(_pointCloudCallback, IntPtr.Zero);
            LivoxLidarSdk.SetLivoxLidarImuDataCallback(_imuDataCallback, IntPtr.Zero);
            LivoxLidarSdk.SetLivoxLidarInfoCallback(_pushMsgCallback, IntPtr.Zero);
            LivoxLidarSdk.SetLivoxLidarInfoChangeCallback(_infoChangeCallback, IntPtr.Zero);

            // 创建 CancellationTokenSource
            _cancellationTokenSource = new CancellationTokenSource();
            // 启动异步监控缓存
            // 使用下划线表示这个任务不需要被等待
            _ = MonitorAndTrimCacheAsync(_cancellationTokenSource.Token);

            msg = "Livox Quick Start Demo Start!";

            //// 保持程序运行
            //Thread.Sleep(300000);

            //Stop();

        ERROR:
            Console.WriteLine(msg);
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
        #endregion

        #region 获取数据快照

        /// <summary>
        /// 获取当前帧的数据快照（线程安全）（笛卡尔坐标系高精度坐标点，精度1mm）
        /// </summary>
        public static LivoxLidarCartesianHighRawPoint[] GetCurrentFrameOfHighRawPoints()
        {
//#if NET9_0_OR_GREATER
//            return BufferServiceHighRawPoints.SwapAndGetSnapshot();
//#elif NET45
//            return CartesianHighRawPoints.ToArray();
//#endif

            return BufferServiceHighRawPoints.SwapAndGetSnapshot();
        }

        /// <summary>
        /// 获取当前帧的数据快照（线程安全）（笛卡尔坐标系低精度坐标点，精度1mm）
        /// </summary>
        public static LivoxLidarCartesianLowRawPoint[] GetCurrentFrameOfLowRawPoints()
        {
//#if NET9_0_OR_GREATER
//            return BufferServiceLowRawPoints.SwapAndGetSnapshot();
//#elif NET45
//            return CartesianLowRawPoints.ToArray();
//#endif

            return BufferServiceLowRawPoints.SwapAndGetSnapshot();
        }

        /// <summary>
        /// 获取当前帧的数据快照（线程安全）（球坐标系坐标点）
        /// </summary>
        public static LivoxLidarSpherPoint[] GetCurrentFrameOfSpherPoints()
        {
//#if NET9_0_OR_GREATER
//            return BufferServiceSpherPoints.SwapAndGetSnapshot();
//#elif NET45
//            return SpherPoints.ToArray();
//#endif

            return BufferServiceSpherPoints.SwapAndGetSnapshot();
        }
        #endregion

        /// <summary>
        /// 检测缓存列表的长度，当超过PkgsPerFrame后，在缓存列表末尾移除超出的部分，并把原始列表内的所有元素替换为缓存列表内的所有剩余元素
        /// </summary>
        /// <returns></returns>
        public static async Task MonitorAndTrimCacheAsync(CancellationToken cancellationToken)
        {
            //检查取消请求
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1, cancellationToken); // 每毫秒检测一次

#if NET45
                //// 监测高精度列表，并移除超出缓存长度的部分
                //if (CartesianHighRawPoints.Count > PointsPerFrame)
                //    CartesianHighRawPoints.RemoveRange(PointsPerFrame, CartesianHighRawPoints.Count - PointsPerFrame);

                //// 监测低精度列表，并移除超出缓存长度的部分
                //if (CartesianLowRawPoints.Count > PointsPerFrame)
                //    CartesianLowRawPoints.RemoveRange(PointsPerFrame, CartesianLowRawPoints.Count - PointsPerFrame);

                //// 监测球坐标列表
                //if (SpherPoints.Count > PointsPerFrame)
                //    SpherPoints.RemoveRange(PointsPerFrame, SpherPoints.Count - PointsPerFrame);

                //BufferHeader = string.Format("Frame time(ms): {0}, points / Frame: {1}, actual points (high): {2}", FrameTime, PointsPerFrame, CartesianHighRawPoints.Count);
#elif NET9_0_OR_GREATER
                //BufferHeader = string.Format("Frame time(ms): {0}, points / Frame: {1}, actual points (high): {2}", FrameTime, PointsPerFrame, BufferServiceHighRawPoints.SwapAndGetSnapshot().Length);
#endif

                BufferHeader = string.Format("time: {0:yyyy-MM-dd HH:mm:ss.fff}, Frame time(ms): {1}, points / Frame: {2}, actual points (high): {3}", DateTime.Now, FrameTime, PointsPerFrame, BufferServiceHighRawPoints.SwapAndGetSnapshot().Length);

                //lock (_syncRoot)
                //{
                //    switch (DataType)
                //    {
                //        default:
                //            break;
                //    }
                //}

                //Console.WriteLine($"高精度点数量: {CartesianHighRawPoints.Count}, 首点XYZ坐标: [{(CartesianHighRawPoints.Count > 0 ? CartesianHighRawPoints[0].x : 0)}], [{(CartesianHighRawPoints.Count > 0 ? CartesianHighRawPoints[0].y : 0)}], [{(CartesianHighRawPoints.Count > 0 ? CartesianHighRawPoints[0].z : 0)}], 低精度点数量: {CartesianLowRawPoints.Count}, 球坐标点数量: {SpherPoints.Count}");
            }
        }
    }
}
