using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanUtilityLibraryVer2.LivoxSdk2.Include
{
    #region 回调函数(Callback)
    // Callback function for receiving point cloud data
    /// <summary>
    /// 接收点云数据的回调函数
    /// </summary>
    /// <param name="handle">设备句柄。</param>
    /// <param name="dev_type">设备类型。</param>
    /// <param name="data">点云数据指针。</param>
    /// <param name="client_data">用户数据。</param>
    public delegate void LivoxLidarPointCloudCallBack(uint handle, byte dev_type, IntPtr data, IntPtr client_data);

    // Callback function for receiving command data
    /// <summary>
    /// 接收命令数据的回调函数
    /// </summary>
    /// <param name="handle">设备句柄。</param>
    /// <param name="data">命令数据指针。</param>
    /// <param name="client_data">用户数据。</param>
    public delegate void LivoxLidarCmdObserverCallBack(uint handle, IntPtr data, IntPtr client_data);

    // Callback function for point cloud observer
    /// <summary>
    /// 点云观察者回调函数
    /// </summary>
    /// <param name="handle">设备句柄。</param>
    /// <param name="dev_type">设备类型。</param>
    /// <param name="data">点云数据指针。</param>
    /// <param name="client_data">用户数据。</param>
    public delegate void LivoxLidarPointCloudObserver(uint handle, byte dev_type, IntPtr data, IntPtr client_data);

    // Callback function for receiving IMU data
    /// <summary>
    /// 接收IMU数据的回调函数
    /// </summary>
    /// <param name="handle">设备句柄。</param>
    /// <param name="dev_type">设备类型。</param>
    /// <param name="data">IMU数据指针。</param>
    /// <param name="client_data">用户数据。</param>
    public delegate void LivoxLidarImuDataCallback(uint handle, byte dev_type, IntPtr data, IntPtr client_data);

    // Callback function for receiving status info
    /// <summary>
    /// 接收状态信息的回调函数
    /// </summary>
    /// <param name="handle">设备句柄。</param>
    /// <param name="dev_type">设备类型。</param>
    /// <param name="info">状态信息字符串。</param>
    /// <param name="client_data">用户数据。</param>
    public delegate void LivoxLidarInfoCallback(uint handle, byte dev_type, string info, IntPtr client_data);

    // Callback function for receiving status info change
    /// <summary>
    /// 接收状态信息变化的回调函数
    /// </summary>
    /// <param name="handle">设备句柄。</param>
    /// <param name="info">状态信息指针。</param>
    /// <param name="client_data">用户数据。</param>
    public delegate void LivoxLidarInfoChangeCallback(uint handle, IntPtr info, IntPtr client_data);

    /// <summary>
    /// 查询LiDAR内部信息的回调函数
    /// </summary>
    /// <param name="status">操作状态。</param>
    /// <param name="handle">设备句柄。</param>
    /// <param name="response">响应数据指针。</param>
    /// <param name="client_data">用户数据。</param>
    public delegate void QueryLivoxLidarInternalInfoCallback(LivoxLidarStatus status, uint handle, IntPtr response, IntPtr client_data);

    // Callback function for receiving async control response
    /// <summary>
    /// 接收异步控制响应的回调函数
    /// </summary>
    /// <param name="status">操作状态。</param>
    /// <param name="handle">设备句柄。</param>
    /// <param name="response">响应数据指针。</param>
    /// <param name="client_data">用户数据。</param>
    public delegate void LivoxLidarAsyncControlCallback(LivoxLidarStatus status, uint handle, IntPtr response, IntPtr client_data);

    // Callback function for receiving reset setting response
    /// <summary>
    /// 接收重置设置响应的回调函数
    /// </summary>
    /// <param name="status">操作状态。</param>
    /// <param name="handle">设备句柄。</param>
    /// <param name="response">响应数据指针。</param>
    /// <param name="client_data">用户数据。</param>
    public delegate void LivoxLidarResetCallback(LivoxLidarStatus status, uint handle, IntPtr response, IntPtr client_data);

    // Callback function for receiving logger setting response
    /// <summary>
    /// 接收日志设置响应的回调函数
    /// </summary>
    /// <param name="status">操作状态。</param>
    /// <param name="handle">设备句柄。</param>
    /// <param name="response">响应数据指针。</param>
    /// <param name="client_data">用户数据。</param>
    public delegate void LivoxLidarLoggerCallback(LivoxLidarStatus status, uint handle, IntPtr response, IntPtr client_data);

    // Callback function for receiving reboot setting response
    /// <summary>
    /// 接收重启设置响应的回调函数
    /// </summary>
    /// <param name="status">操作状态。</param>
    /// <param name="handle">设备句柄。</param>
    /// <param name="response">响应数据指针。</param>
    /// <param name="client_data">用户数据。</param>
    public delegate void LivoxLidarRebootCallback(LivoxLidarStatus status, uint handle, IntPtr response, IntPtr client_data);

    /// <summary>
    /// 接收LiDAR升级进度的回调函数
    /// </summary>
    /// <param name="handle">设备句柄。</param>
    /// <param name="state">升级状态。</param>
    /// <param name="client_data">用户数据。</param>
    public delegate void OnLivoxLidarUpgradeProgressCallback(uint handle, LivoxLidarUpgradeState state, IntPtr client_data);

    // Callback function for receiving RMC sync time response
    /// <summary>
    /// 接收RMC时间同步响应的回调函数
    /// </summary>
    /// <param name="status">操作状态。</param>
    /// <param name="handle">设备句柄。</param>
    /// <param name="data">同步时间数据指针。</param>
    /// <param name="client_data">用户数据。</param>
    public delegate void LivoxLidarRmcSyncTimeCallBack(LivoxLidarStatus status, uint handle, IntPtr data, IntPtr client_data);
    #endregion
}
