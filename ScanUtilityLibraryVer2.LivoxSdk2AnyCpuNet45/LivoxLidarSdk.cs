#define WINDOWS
//TODO 为Windows操作系统定义条件编译符号，为其它平台生成时注释掉

using ScanUtilityLibraryVer2.LivoxSdk2.Include;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScanUtilityLibraryVer2.LivoxSdk2
{
    /// <summary>
    /// SDK提供的功能函数（从非托管DLL中导出的静态方法）
    /// </summary>
    public class LivoxLidarSdk
    {
        // 平台调用（P/Invoke）：允许托管代码调用非托管的DLL函数
        // 使用[DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]声明DLL中的方法，确保调用约定与C++代码一致。

#if WINDOWS
        //在 Windows 上，DllImport 通常需要指定 DLL 文件的扩展名
        private const string dllName = "livox_lidar_sdk_shared.dll";
#else
        //TODO 在 Linux 或 macOS 上，通常不需要指定扩展名（不带 .so）；如果动态库的命名不规范（例如不以 lib 开头），可能需要显式指定扩展名
        private const string dllName = "liblivox_lidar_sdk_shared";
        //private const string dllName = "livox_lidar_sdk_shared.so";
#endif

        #region P/Invoke声明
        /// <summary>
        /// 获取SDK的版本信息
        /// </summary>
        /// <param name="version">用于存储版本信息的结构体</param>
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetLivoxLidarSdkVer(ref LivoxLidarSdkVer version);

        /// <summary>
        /// 初始化SDK
        /// </summary>
        /// <param name="path">配置文件路径</param>
        /// <param name="host_ip">主机IP地址</param>
        /// <param name="log_cfg_info">日志配置信息</param>
        /// <returns>返回true表示初始化成功，否则返回false</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LivoxLidarSdkInit(string path, string host_ip, ref LivoxLidarLoggerCfgInfo log_cfg_info);

        /// <summary>
        /// 启动SDK
        /// </summary>
        /// <returns>返回true表示启动成功，否则返回false</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LivoxLidarSdkStart();

        /// <summary>
        /// 反初始化SDK
        /// </summary>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LivoxLidarSdkUninit();

        /// <summary>
        /// 设置接收点云数据的回调函数
        /// </summary>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetLivoxLidarPointCloudCallBack(LivoxLidarPointCloudCallBack cb, IntPtr client_data);

        /// <summary>
        /// 添加LiDAR命令数据观察者
        /// </summary>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LivoxLidarAddCmdObserver(LivoxLidarCmdObserverCallBack cb, IntPtr client_data);

        /// <summary>
        /// 移除LiDAR命令数据观察者
        /// </summary>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LivoxLidarRemoveCmdObserver();

        /// <summary>
        /// 添加点云观察者
        /// </summary>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回观察者ID</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort LivoxLidarAddPointCloudObserver(LivoxLidarPointCloudObserver cb, IntPtr client_data);

        /// <summary>
        /// 移除点云观察者
        /// </summary>
        /// <param name="id">观察者ID</param>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LivoxLidarRemovePointCloudObserver(ushort id);

        /// <summary>
        /// 设置接收IMU数据的回调函数
        /// </summary>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetLivoxLidarImuDataCallback(LivoxLidarImuDataCallback cb, IntPtr client_data);

        /// <summary>
        /// 设置接收状态信息的回调函数
        /// </summary>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetLivoxLidarInfoCallback(LivoxLidarInfoCallback cb, IntPtr client_data);

        /// <summary>
        /// 禁用SDK控制台日志输出
        /// </summary>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DisableLivoxSdkConsoleLogger();

        /// <summary>
        /// 保存日志文件
        /// </summary>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SaveLivoxLidarSdkLoggerFile();

        /// <summary>
        /// 设置接收状态信息变化的回调函数
        /// </summary>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetLivoxLidarInfoChangeCallback(LivoxLidarInfoChangeCallback cb, IntPtr client_data);

        /// <summary>
        /// 查询LiDAR内部信息
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int QueryLivoxLidarInternalInfo(uint handle, QueryLivoxLidarInternalInfoCallback cb, IntPtr client_data);

        /// <summary>
        /// 查询LiDAR固件类型
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int QueryLivoxLidarFwType(uint handle, QueryLivoxLidarInternalInfoCallback cb, IntPtr client_data);

        /// <summary>
        /// 查询LiDAR固件版本
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int QueryLivoxLidarFirmwareVer(uint handle, QueryLivoxLidarInternalInfoCallback cb, IntPtr client_data);

        /// <summary>
        /// 设置LiDAR点云数据类型
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="data_type">点云数据类型</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetLivoxLidarPclDataType(uint handle, int data_type, LivoxLidarAsyncControlCallback cb, IntPtr client_data);

        /// <summary>
        /// 设置LiDAR扫描模式
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="scan_pattern">扫描模式</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetLivoxLidarScanPattern(uint handle, int scan_pattern, LivoxLidarAsyncControlCallback cb, IntPtr client_data);

        /// <summary>
        /// 设置LiDAR双发射模式
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="enable">是否启用双发射模式</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetLivoxLidarDualEmit(uint handle, bool enable, LivoxLidarAsyncControlCallback cb, IntPtr client_data);

        /// <summary>
        /// 启用LiDAR点发送
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int EnableLivoxLidarPointSend(uint handle, LivoxLidarAsyncControlCallback cb, IntPtr client_data);

        /// <summary>
        /// 禁用LiDAR点发送
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DisableLivoxLidarPointSend(uint handle, LivoxLidarAsyncControlCallback cb, IntPtr client_data);

        /// <summary>
        /// 设置LiDAR IP信息
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="ip_config">IP配置信息</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetLivoxLidarIp(uint handle, ref LivoxLidarIpInfo ip_config, LivoxLidarAsyncControlCallback cb, IntPtr client_data);

        /// <summary>
        /// 设置LiDAR状态信息主机IP配置
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="host_state_info_ipcfg">状态信息主机IP配置</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetLivoxLidarStateInfoHostIPCfg(uint handle, ref HostStateInfoIpInfo host_state_info_ipcfg, LivoxLidarAsyncControlCallback cb, IntPtr client_data);

        /// <summary>
        /// 设置LiDAR点云数据主机IP配置
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="host_point_ipcfg">点云数据主机IP配置</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetLivoxLidarPointDataHostIPCfg(uint handle, ref HostPointIPInfo host_point_ipcfg, LivoxLidarAsyncControlCallback cb, IntPtr client_data);

        /// <summary>
        /// 设置LiDAR IMU数据主机IP配置
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="host_imu_ipcfg">IMU数据主机IP配置</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetLivoxLidarImuDataHostIPCfg(uint handle, ref HostImuDataIPInfo host_imu_ipcfg, LivoxLidarAsyncControlCallback cb, IntPtr client_data);

        /// <summary>
        /// 设置LiDAR安装姿态
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="install_attitude">安装姿态</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetLivoxLidarInstallAttitude(uint handle, ref LivoxLidarInstallAttitude install_attitude, LivoxLidarAsyncControlCallback cb, IntPtr client_data);

        /// <summary>
        /// 设置LiDAR视场配置0
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="fov_cfg0">视场配置0</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetLivoxLidarFovCfg0(uint handle, ref FovCfg fov_cfg0, LivoxLidarAsyncControlCallback cb, IntPtr client_data);

        /// <summary>
        /// 设置LiDAR视场配置1
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="fov_cfg1">视场配置1</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetLivoxLidarFovCfg1(uint handle, ref FovCfg fov_cfg1, LivoxLidarAsyncControlCallback cb, IntPtr client_data);

        /// <summary>
        /// 启用LiDAR视场
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="fov_en">是否启用视场</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int EnableLivoxLidarFov(uint handle, byte fov_en, LivoxLidarAsyncControlCallback cb, IntPtr client_data);

        /// <summary>
        /// 禁用LiDAR视场
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DisableLivoxLidarFov(uint handle, LivoxLidarAsyncControlCallback cb, IntPtr client_data);

        /// <summary>
        /// 设置LiDAR检测模式
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="mode">检测模式</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetLivoxLidarDetectMode(uint handle, int mode, LivoxLidarAsyncControlCallback cb, IntPtr client_data);

        /// <summary>
        /// 设置LiDAR功能IO配置
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="func_io_cfg">功能IO配置</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetLivoxLidarFuncIOCfg(uint handle, ref FuncIOCfg func_io_cfg, LivoxLidarAsyncControlCallback cb, IntPtr client_data);

        /// <summary>
        /// 设置LiDAR盲区配置
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="blind_spot">盲区配置</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetLivoxLidarBlindSpot(uint handle, uint blind_spot, LivoxLidarAsyncControlCallback cb, IntPtr client_data);

        /// <summary>
        /// 设置LiDAR工作模式
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="work_mode">工作模式</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetLivoxLidarWorkMode(uint handle, int work_mode, LivoxLidarAsyncControlCallback cb, IntPtr client_data);

        /// <summary>
        /// 启用LiDAR玻璃加热功能
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int EnableLivoxLidarGlassHeat(uint handle, LivoxLidarAsyncControlCallback cb, IntPtr client_data);

        /// <summary>
        /// 禁用LiDAR玻璃加热功能
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DisableLivoxLidarGlassHeat(uint handle, LivoxLidarAsyncControlCallback cb, IntPtr client_data);

        /// <summary>
        /// 启用LiDAR强制加热功能
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int StartForcedHeating(uint handle, LivoxLidarAsyncControlCallback cb, IntPtr client_data);

        /// <summary>
        /// 禁用LiDAR强制加热功能
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int StopForcedHeating(uint handle, LivoxLidarAsyncControlCallback cb, IntPtr client_data);

        /// <summary>
        /// 启用LiDAR IMU数据
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int EnableLivoxLidarImuData(uint handle, LivoxLidarAsyncControlCallback cb, IntPtr client_data);

        /// <summary>
        /// 禁用LiDAR IMU数据
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DisableLivoxLidarImuData(uint handle, LivoxLidarAsyncControlCallback cb, IntPtr client_data);

        /// <summary>
        /// 启用LiDAR FUSA功能
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int EnableLivoxLidarFusaFunciont(uint handle, LivoxLidarAsyncControlCallback cb, IntPtr client_data);

        /// <summary>
        /// 禁用LiDAR FUSA功能
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int DisableLivoxLidarFusaFunciont(uint handle, LivoxLidarAsyncControlCallback cb, IntPtr client_data);

        /// <summary>
        /// 请求重置LiDAR
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int LivoxLidarRequestReset(uint handle, LivoxLidarResetCallback cb, IntPtr client_data);

        /// <summary>
        /// 启动日志记录当前仅支持设置实时日志
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="log_type">日志类型</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int LivoxLidarStartLogger(uint handle, int log_type, LivoxLidarLoggerCallback cb, IntPtr client_data);

        /// <summary>
        /// 停止日志记录当前仅支持设置实时日志
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="log_type">日志类型</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int LivoxLidarStopLogger(uint handle, int log_type, LivoxLidarLoggerCallback cb, IntPtr client_data);

        /// <summary>
        /// 设置LiDAR调试点云开关
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="enable">是否启用调试点云</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetLivoxLidarDebugPointCloud(uint handle, bool enable, LivoxLidarLoggerCallback cb, IntPtr client_data);

        /// <summary>
        /// 设置LiDAR GPS "GPRMC" 字符串同步时间
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="rmc">GPS "GPRMC" 字符串</param>
        /// <param name="rmc_length">GPS "GPRMC" 字符串长度</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetLivoxLidarRmcSyncTime(uint handle, string rmc, ushort rmc_length, LivoxLidarRmcSyncTimeCallBack cb, IntPtr client_data);

        /// <summary>
        /// 设置LiDAR启动后的工作模式
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="work_mode">启动后的工作模式</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SetLivoxLidarWorkModeAfterBoot(uint handle, int work_mode, LivoxLidarAsyncControlCallback cb, IntPtr client_data);

        /// <summary>
        /// 请求重启LiDAR
        /// </summary>
        /// <param name="handle">设备句柄</param>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        /// <returns>返回状态码</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int LivoxLidarRequestReboot(uint handle, LivoxLidarRebootCallback cb, IntPtr client_data);

        /// <summary>
        /// 设置LiDAR升级固件路径
        /// </summary>
        /// <param name="firmware_path">固件路径</param>
        /// <returns>返回true表示成功，否则返回false</returns>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SetLivoxLidarUpgradeFirmwarePath(string firmware_path);

        /// <summary>
        /// 设置升级进度回调函数
        /// </summary>
        /// <param name="cb">回调函数</param>
        /// <param name="client_data">用户数据</param>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetLivoxLidarUpgradeProgressCallback(OnLivoxLidarUpgradeProgressCallback cb, IntPtr client_data);

        /// <summary>
        /// 升级多个LiDAR设备
        /// </summary>
        /// <param name="handle">设备句柄数组指针</param>
        /// <param name="lidar_num">设备数量</param>
        [DllImport("livox_lidar_sdk_shared.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UpgradeLivoxLidars(IntPtr handle, byte lidar_num);
        #endregion
    }
}
