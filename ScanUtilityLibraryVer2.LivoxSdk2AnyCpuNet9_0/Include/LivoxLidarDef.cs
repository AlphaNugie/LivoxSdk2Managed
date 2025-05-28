using System;
using System.Runtime.InteropServices;

namespace ScanUtilityLibraryVer2.LivoxSdk2.Include
{
    /// <summary>
    /// 定义了一些常量、枚举和基础数据结构，用于描述LiDAR设备的各种参数和状态，并在SDK的其他部分中广泛使用
    /// <para/>例如，定义了设备的状态码、数据类型、错误码等。这些定义有助于确保SDK中的参数和状态的一致性
    /// <para/>这里定义的数据结构和回调函数类型与C++相同，以便在C#中使用
    /// </summary>
    public static class LivoxLidarDef
    {
        #region 常量(const)（已注释，调用api时不需要）
        //// Constants
        ///// <summary>
        ///// 最大LiDAR数量
        ///// </summary>
        //public const int kMaxLidarCount = 32;

        //// SDK Version
        ///// <summary>
        ///// SDK主版本号
        ///// </summary>
        //public const int LIVOX_LIDAR_SDK_MAJOR_VERSION = 1;

        ///// <summary>
        ///// SDK次版本号
        ///// </summary>
        //public const int LIVOX_LIDAR_SDK_MINOR_VERSION = 2;

        ///// <summary>
        ///// SDK修订版本号
        ///// </summary>
        //public const int LIVOX_LIDAR_SDK_PATCH_VERSION = 5;

        ///// <summary>
        ///// 广播码大小
        ///// </summary>
        //public const int kBroadcastCodeSize = 16;
        #endregion

        // Function return value definition
        /// <summary>
        /// 函数返回值定义
        /// <para/>参见https://github.com/Livox-SDK/Livox-SDK2/wiki/Livox-SDK-Communication-Protocol-HAP(English)#return-code
        /// </summary>
        public delegate int livox_status();
    }

    //#region 结构体(Struct)
    //// Numeric version information struct
    ///// <summary>
    ///// 数字版本信息结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct LivoxLidarSdkVer
    //{
    //    /// <summary>
    //    /// 主版本号
    //    /// </summary>
    //    public int major;

    //    /// <summary>
    //    /// 次版本号
    //    /// </summary>
    //    public int minor;

    //    /// <summary>
    //    /// 修订版本号
    //    /// </summary>
    //    public int patch;
    //}

    ///// <summary>
    ///// LiDAR设备信息结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct LivoxLidarInfo
    //{
    //    /// <summary>
    //    /// 设备类型
    //    /// </summary>
    //    public byte dev_type;

    //    /// <summary>
    //    /// 序列号
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    //    public string sn;

    //    /// <summary>
    //    /// LiDAR IP地址
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    //    public string lidar_ip;
    //}

    ///// <summary>
    ///// LiDAR以太网数据包结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct LivoxLidarEthernetPacket
    //{
    //    /// <summary>
    //    /// 包协议版本：当前为0
    //    /// </summary>
    //    public byte version;

    //    /// <summary>
    //    /// 数据包长度：从version开始的整个UDP数据段长度
    //    /// </summary>
    //    public ushort length;

    //    /// <summary>
    //    /// 帧内点云采样时间(单位0.1us)
    //    /// <para/>这帧点云数据中最后一个点减去第一个点时间
    //    /// </summary>
    //    public ushort time_interval;

    //    /// <summary>
    //    /// 当前UDP包data字段包含点数目：
    //    /// <para/>点云数据为96，IMU数据为1
    //    /// </summary>
    //    public ushort dot_num;

    //    /// <summary>
    //    /// 点云UDP包计数，每个UDP包依次加1，点云帧起始包清0
    //    /// </summary>
    //    public ushort udp_cnt;

    //    /// <summary>
    //    /// 帧计数（HAP该字段为0）
    //    /// </summary>
    //    public byte frame_cnt;

    //    /// <summary>
    //    /// 数据类型：0-IMU Data；1-Point Cloud Data1；2-Point Cloud Data2
    //    /// </summary>
    //    //不能试使用枚举，会使字段专用空间变大（占4个字节）
    //    public byte data_type;

    //    /// <summary>
    //    /// 时间戳类型：
    //    /// <para/>0 无同步源，时间戳为雷达开机时间
    //    /// <para/>1 gPTP同步，时间戳为master时钟源时间
    //    /// </summary>
    //    public byte time_type;

    //    /// <summary>
    //    /// bit0-1: 功能安全safety_info
    //    /// 0x0：整包可信;
    //    /// 0x1：整包不可信;
    //    /// 0x2：非0点可信;
    //    /// <para/>bit2-3: tag_type, HAP 固定为0
    //    /// <para/>bit4-7: rsvd
    //    /// </summary>
    //    public byte pack_info;

    //    /// <summary>
    //    /// 预留
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
    //    public byte[] rsvd;

    //    /// <summary>
    //    /// CRC32校验码，校验timestamp+data段
    //    /// </summary>
    //    public uint crc32;

    //    /// <summary>
    //    /// 点云时间戳（微秒），类型由time_type决定
    //    /// <para/>每个数据包中的时间戳代表第一个点云的时间，每个数据包中有n个点云，这n个点云的时间是等间隔的，总间隔时间为time_interval的值
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    //    public byte[] timestamp;

    //    /// <summary>
    //    /// 点云数据或IMU数据（由data_type决定）
    //    /// <para/>IMU：24字节 * 数量1
    //    /// <para/>Point Cloud Data1：14字节 * 数量96
    //    /// <para/>Point Cloud Data2：8字节  * 数量96
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1344)] //最大字节数为14*96=1344，将SizeConst设为此值，当data长度小于SizeConst时，后续的字节将由0xCD(205)填充
    //    public byte[] data;
    //}

    ///// <summary>
    ///// LiDAR命令数据包结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct LivoxLidarCmdPacket
    //{
    //    /// <summary>
    //    /// 帧起始标志
    //    /// </summary>
    //    public byte sof;

    //    /// <summary>
    //    /// 版本号
    //    /// </summary>
    //    public byte version;

    //    /// <summary>
    //    /// 数据包长度
    //    /// </summary>
    //    public ushort length;

    //    /// <summary>
    //    /// 序列号
    //    /// </summary>
    //    public uint seq_num;

    //    /// <summary>
    //    /// 命令ID
    //    /// </summary>
    //    public ushort cmd_id;

    //    /// <summary>
    //    /// 命令类型
    //    /// </summary>
    //    public byte cmd_type;

    //    /// <summary>
    //    /// 发送方类型
    //    /// </summary>
    //    public byte sender_type;

    //    /// <summary>
    //    /// 预留
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
    //    public string rsvd;

    //    /// <summary>
    //    /// 高位CRC16校验码
    //    /// </summary>
    //    public ushort crc16_h;

    //    /// <summary>
    //    /// 数据CRC32校验码
    //    /// </summary>
    //    public uint crc32_d;

    //    /// <summary>
    //    /// 数据
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
    //    public byte[] data;
    //}

    ///// <summary>
    ///// LiDAR IMU原始点数据结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct LivoxLidarImuRawPoint
    //{
    //    /// <summary>
    //    /// 陀螺仪X轴数据
    //    /// </summary>
    //    public float gyro_x;

    //    /// <summary>
    //    /// 陀螺仪Y轴数据
    //    /// </summary>
    //    public float gyro_y;

    //    /// <summary>
    //    /// 陀螺仪Z轴数据
    //    /// </summary>
    //    public float gyro_z;

    //    /// <summary>
    //    /// 加速度计X轴数据
    //    /// </summary>
    //    public float acc_x;

    //    /// <summary>
    //    /// 加速度计Y轴数据
    //    /// </summary>
    //    public float acc_y;

    //    /// <summary>
    //    /// 加速度计Z轴数据
    //    /// </summary>
    //    public float acc_z;
    //}

    ///// <summary>
    ///// LiDAR高精度笛卡尔坐标系点数据结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct LivoxLidarCartesianHighRawPoint
    //{
    //    /// <summary>
    //    /// X轴坐标，单位：毫米
    //    /// </summary>
    //    public int x;

    //    /// <summary>
    //    /// Y轴坐标，单位：毫米
    //    /// </summary>
    //    public int y;

    //    /// <summary>
    //    /// Z轴坐标，单位：毫米
    //    /// </summary>
    //    public int z;

    //    /// <summary>
    //    /// 反射率
    //    /// </summary>
    //    public byte reflectivity;

    //    /// <summary>
    //    /// 标签
    //    /// </summary>
    //    public byte tag;
    //}

    ///// <summary>
    ///// LiDAR低精度笛卡尔坐标系点数据结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct LivoxLidarCartesianLowRawPoint
    //{
    //    /// <summary>
    //    /// X轴坐标，单位：厘米
    //    /// </summary>
    //    public short x;

    //    /// <summary>
    //    /// Y轴坐标，单位：厘米
    //    /// </summary>
    //    public short y;

    //    /// <summary>
    //    /// Z轴坐标，单位：厘米
    //    /// </summary>
    //    public short z;

    //    /// <summary>
    //    /// 反射率
    //    /// </summary>
    //    public byte reflectivity;

    //    /// <summary>
    //    /// 标签
    //    /// </summary>
    //    public byte tag;
    //}

    ///// <summary>
    ///// LiDAR球坐标系点数据结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct LivoxLidarSpherPoint
    //{
    //    /// <summary>
    //    /// 深度
    //    /// </summary>
    //    public uint depth;

    //    /// <summary>
    //    /// 横向角度
    //    /// </summary>
    //    public ushort theta;

    //    /// <summary>
    //    /// 纵向角度
    //    /// </summary>
    //    public ushort phi;

    //    /// <summary>
    //    /// 反射率
    //    /// </summary>
    //    public byte reflectivity;

    //    /// <summary>
    //    /// 标签
    //    /// </summary>
    //    public byte tag;
    //}

    ///// <summary>
    ///// LiDAR键值对参数结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct LivoxLidarKeyValueParam
    //{
    //    /// <summary>
    //    /// 键名
    //    /// </summary>
    //    public ushort key;

    //    /// <summary>
    //    /// 值的长度
    //    /// </summary>
    //    public ushort length;

    //    /// <summary>
    //    /// 值
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
    //    public byte[] value;
    //}

    ///// <summary>
    ///// LiDAR异步控制响应结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct LivoxLidarAsyncControlResponse
    //{
    //    /// <summary>
    //    /// 返回代码
    //    /// </summary>
    //    public byte ret_code;

    //    /// <summary>
    //    /// 错误键
    //    /// </summary>
    //    public ushort error_key;
    //}

    ///// <summary>
    ///// LiDAR信息响应结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct LivoxLidarInfoResponse
    //{
    //    /// <summary>
    //    /// 返回代码
    //    /// </summary>
    //    public byte ret_code;

    //    /// <summary>
    //    /// LiDAR信息
    //    /// </summary>
    //    public IntPtr lidar_info;
    //}

    ///// <summary>
    ///// LiDAR内部诊断信息响应结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct LivoxLidarDiagInternalInfoResponse
    //{
    //    /// <summary>
    //    /// 返回代码
    //    /// </summary>
    //    public byte ret_code;

    //    /// <summary>
    //    /// 参数数量
    //    /// </summary>
    //    public ushort param_num;

    //    /// <summary>
    //    /// 数据
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
    //    public byte[] data;
    //}

    ///// <summary>
    ///// LiDAR安装姿态结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct LivoxLidarInstallAttitude
    //{
    //    /// <summary>
    //    /// 滚转角度，单位：度
    //    /// </summary>
    //    public float roll_deg;

    //    /// <summary>
    //    /// 俯仰角度，单位：度
    //    /// </summary>
    //    public float pitch_deg;

    //    /// <summary>
    //    /// 偏航角度，单位：度
    //    /// </summary>
    //    public float yaw_deg;

    //    /// <summary>
    //    /// X轴坐标，单位：毫米
    //    /// </summary>
    //    public int x;

    //    /// <summary>
    //    /// Y轴坐标，单位：毫米
    //    /// </summary>
    //    public int y;

    //    /// <summary>
    //    /// Z轴坐标，单位：毫米
    //    /// </summary>
    //    public int z;
    //}

    ///// <summary>
    ///// 视场配置结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct FovCfg
    //{
    //    /// <summary>
    //    /// 横向视场起始角度，单位：度
    //    /// </summary>
    //    public int yaw_start;

    //    /// <summary>
    //    /// 横向视场终止角度，单位：度
    //    /// </summary>
    //    public int yaw_stop;

    //    /// <summary>
    //    /// 纵向视场起始角度，单位：度
    //    /// </summary>
    //    public int pitch_start;

    //    /// <summary>
    //    /// 纵向视场终止角度，单位：度
    //    /// </summary>
    //    public int pitch_stop;

    //    /// <summary>
    //    /// 保留字段
    //    /// </summary>
    //    public uint rsvd;
    //}

    ///// <summary>
    ///// 功能IO配置结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct FuncIOCfg
    //{
    //    /// <summary>
    //    /// 输入0
    //    /// </summary>
    //    public byte in0;

    //    /// <summary>
    //    /// 输入1
    //    /// </summary>
    //    public byte int1;

    //    /// <summary>
    //    /// 输出0
    //    /// </summary>
    //    public byte out0;

    //    /// <summary>
    //    /// 输出1
    //    /// </summary>
    //    public byte out1;
    //}

    ///// <summary>
    ///// LiDAR日志配置结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct LivoxLidarLoggerCfgInfo
    //{
    //    /// <summary>
    //    /// 是否启用LiDAR日志
    //    /// </summary>
    //    public bool lidar_log_enable;

    //    /// <summary>
    //    /// LiDAR日志缓存大小
    //    /// </summary>
    //    public uint lidar_log_cache_size;

    //    /// <summary>
    //    /// LiDAR日志路径
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
    //    public string lidar_log_path;
    //}

    ///// <summary>
    ///// LiDAR IP信息结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct LivoxLidarIpInfo
    //{
    //    /// <summary>
    //    /// IP地址
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    //    public string ip_addr;

    //    /// <summary>
    //    /// 子网掩码
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    //    public string net_mask;

    //    /// <summary>
    //    /// 网关地址
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    //    public string gw_addr;
    //}

    ///// <summary>
    ///// 主机状态信息IP配置结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct HostStateInfoIpInfo
    //{
    //    /// <summary>
    //    /// 主机IP地址
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    //    public string host_ip_addr;

    //    /// <summary>
    //    /// 主机状态信息端口
    //    /// </summary>
    //    public ushort host_state_info_port;

    //    /// <summary>
    //    /// LiDAR状态信息端口
    //    /// </summary>
    //    public ushort lidar_state_info_port;
    //}

    ///// <summary>
    ///// 主机点云数据IP配置结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct HostPointIPInfo
    //{
    //    /// <summary>
    //    /// 主机IP地址
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    //    public string host_ip_addr;

    //    /// <summary>
    //    /// 主机点云数据端口
    //    /// </summary>
    //    public ushort host_point_data_port;

    //    /// <summary>
    //    /// LiDAR点云数据端口
    //    /// </summary>
    //    public ushort lidar_point_data_port;
    //}

    ///// <summary>
    ///// 主机IMU数据IP配置结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct HostImuDataIPInfo
    //{
    //    /// <summary>
    //    /// 主机IP地址
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    //    public string host_ip_addr;

    //    /// <summary>
    //    /// 主机IMU数据端口（保留）
    //    /// </summary>
    //    public ushort host_imu_data_port;

    //    /// <summary>
    //    /// LiDAR IMU数据端口（保留）
    //    /// </summary>
    //    public ushort lidar_imu_data_port;
    //}

    ///// <summary>
    ///// LiDAR IP配置结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct LivoxIpCfg
    //{
    //    /// <summary>
    //    /// IP地址
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    //    public string ip_addr;

    //    /// <summary>
    //    /// 目标端口
    //    /// </summary>
    //    public ushort dst_port;

    //    /// <summary>
    //    /// 源端口
    //    /// </summary>
    //    public ushort src_port;
    //}

    ///// <summary>
    ///// LiDAR状态信息结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct LivoxLidarStateInfo
    //{
    //    /// <summary>
    //    /// 点云数据类型
    //    /// </summary>
    //    public byte pcl_data_type;

    //    /// <summary>
    //    /// 扫描模式
    //    /// </summary>
    //    public byte pattern_mode;

    //    /// <summary>
    //    /// 双发射使能
    //    /// </summary>
    //    public byte dual_emit_en;

    //    /// <summary>
    //    /// 点发送使能
    //    /// </summary>
    //    public byte point_send_en;

    //    /// <summary>
    //    /// LiDAR IP信息
    //    /// </summary>
    //    public LivoxLidarIpInfo lidar_ip_info;

    //    /// <summary>
    //    /// 主机点云数据IP信息
    //    /// </summary>
    //    public HostPointIPInfo host_point_ip_info;

    //    /// <summary>
    //    /// 主机IMU数据IP信息
    //    /// </summary>
    //    public HostImuDataIPInfo host_imu_ip_info;

    //    /// <summary>
    //    /// 安装姿态
    //    /// </summary>
    //    public LivoxLidarInstallAttitude install_attitude;

    //    /// <summary>
    //    /// 盲区设置
    //    /// </summary>
    //    public uint blind_spot_set;

    //    /// <summary>
    //    /// 工作模式
    //    /// </summary>
    //    public byte work_mode;

    //    /// <summary>
    //    /// 玻璃加热
    //    /// </summary>
    //    public byte glass_heat;

    //    /// <summary>
    //    /// IMU数据使能
    //    /// </summary>
    //    public byte imu_data_en;

    //    /// <summary>
    //    /// FUSA使能
    //    /// </summary>
    //    public byte fusa_en;

    //    /// <summary>
    //    /// 序列号
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    //    public string sn;

    //    /// <summary>
    //    /// 产品信息
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    //    public string product_info;

    //    /// <summary>
    //    /// 应用版本
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    //    public byte[] version_app;

    //    /// <summary>
    //    /// 加载器版本
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    //    public byte[] version_loader;

    //    /// <summary>
    //    /// 硬件版本
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    //    public byte[] version_hardware;

    //    /// <summary>
    //    /// MAC地址
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    //    public byte[] mac;

    //    /// <summary>
    //    /// 当前工作状态
    //    /// </summary>
    //    public byte cur_work_state;

    //    /// <summary>
    //    /// 状态码
    //    /// </summary>
    //    public ulong status_code;
    //}

    ///// <summary>
    ///// LiDAR直接状态信息结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct DirectLidarStateInfo
    //{
    //    /// <summary>
    //    /// 点云数据类型
    //    /// </summary>
    //    public byte pcl_data_type;

    //    /// <summary>
    //    /// 扫描模式
    //    /// </summary>
    //    public byte pattern_mode;

    //    /// <summary>
    //    /// 双发射使能
    //    /// </summary>
    //    public byte dual_emit_en;

    //    /// <summary>
    //    /// 点发送使能
    //    /// </summary>
    //    public byte point_send_en;

    //    /// <summary>
    //    /// LiDAR IP配置
    //    /// </summary>
    //    public LivoxLidarIpInfo lidar_ipcfg;

    //    /// <summary>
    //    /// 主机状态信息IP配置
    //    /// </summary>
    //    public HostStateInfoIpInfo host_state_info;

    //    /// <summary>
    //    /// 主机点云数据IP配置
    //    /// </summary>
    //    public HostPointIPInfo pointcloud_host_ipcfg;

    //    /// <summary>
    //    /// 主机IMU数据IP配置
    //    /// </summary>
    //    public HostImuDataIPInfo imu_host_ipcfg;

    //    /// <summary>
    //    /// 控制主机IP配置
    //    /// </summary>
    //    public LivoxIpCfg ctl_host_ipcfg;

    //    /// <summary>
    //    /// 日志主机IP配置
    //    /// </summary>
    //    public LivoxIpCfg log_host_ipcfg;

    //    /// <summary>
    //    /// 车辆速度
    //    /// </summary>
    //    public int vehicle_speed;

    //    /// <summary>
    //    /// 环境温度
    //    /// </summary>
    //    public int environment_temp;

    //    /// <summary>
    //    /// 安装姿态
    //    /// </summary>
    //    public LivoxLidarInstallAttitude install_attitude;

    //    /// <summary>
    //    /// 盲区设置
    //    /// </summary>
    //    public uint blind_spot_set;

    //    /// <summary>
    //    /// 帧率
    //    /// </summary>
    //    public byte frame_rate;

    //    /// <summary>
    //    /// 视场配置0
    //    /// </summary>
    //    public FovCfg fov_cfg0;

    //    /// <summary>
    //    /// 视场配置1
    //    /// </summary>
    //    public FovCfg fov_cfg1;

    //    /// <summary>
    //    /// 视场配置使能
    //    /// </summary>
    //    public byte fov_cfg_en;

    //    /// <summary>
    //    /// 检测模式
    //    /// </summary>
    //    public byte detect_mode;

    //    /// <summary>
    //    /// 功能IO配置
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    //    public byte[] func_io_cfg;

    //    /// <summary>
    //    /// 工作目标模式
    //    /// </summary>
    //    public byte work_tgt_mode;

    //    /// <summary>
    //    /// 玻璃加热
    //    /// </summary>
    //    public byte glass_heat;

    //    /// <summary>
    //    /// IMU数据使能
    //    /// </summary>
    //    public byte imu_data_en;

    //    /// <summary>
    //    /// FUSA使能
    //    /// </summary>
    //    public byte fusa_en;

    //    /// <summary>
    //    /// 序列号
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    //    public string sn;

    //    /// <summary>
    //    /// 产品信息
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    //    public string product_info;

    //    /// <summary>
    //    /// 应用版本
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    //    public byte[] version_app;

    //    /// <summary>
    //    /// 加载器版本
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    //    public byte[] version_loader;

    //    /// <summary>
    //    /// 硬件版本
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    //    public byte[] version_hardware;

    //    /// <summary>
    //    /// MAC地址
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    //    public byte[] mac;

    //    /// <summary>
    //    /// 当前工作状态
    //    /// </summary>
    //    public byte cur_work_state;

    //    /// <summary>
    //    /// 核心温度
    //    /// </summary>
    //    public int core_temp;

    //    /// <summary>
    //    /// 上电次数
    //    /// </summary>
    //    public uint powerup_cnt;

    //    /// <summary>
    //    /// 当前本地时间
    //    /// </summary>
    //    public ulong local_time_now;

    //    /// <summary>
    //    /// 上次同步时间
    //    /// </summary>
    //    public ulong last_sync_time;

    //    /// <summary>
    //    /// 时间偏移
    //    /// </summary>
    //    public long time_offset;

    //    /// <summary>
    //    /// 时间同步类型
    //    /// </summary>
    //    public byte time_sync_type;

    //    /// <summary>
    //    /// 状态码
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    //    public byte[] status_code;

    //    /// <summary>
    //    /// LiDAR诊断状态
    //    /// </summary>
    //    public ushort lidar_diag_status;

    //    /// <summary>
    //    /// LiDAR闪存状态
    //    /// </summary>
    //    public byte lidar_flash_status;

    //    /// <summary>
    //    /// 固件类型
    //    /// </summary>
    //    public byte fw_type;

    //    /// <summary>
    //    /// HMS代码
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    //    public uint[] hms_code;

    //    /// <summary>
    //    /// ROI模式
    //    /// </summary>
    //    public byte ROI_Mode;
    //}

    ///// <summary>
    ///// LiDAR查询内部信息响应结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct LivoxLidarQueryInternalInfoResponse
    //{
    //    /// <summary>
    //    /// 返回代码
    //    /// </summary>
    //    public byte ret_code;

    //    /// <summary>
    //    /// LiDAR状态信息
    //    /// </summary>
    //    public LivoxLidarStateInfo livox_lidar_state_info;
    //}

    ///// <summary>
    ///// LiDAR日志参数结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct LivoxLidarLogParam
    //{
    //    /// <summary>
    //    /// 日志发送方法
    //    /// </summary>
    //    public byte log_send_method;

    //    /// <summary>
    //    /// 日志ID
    //    /// </summary>
    //    public ushort log_id;

    //    /// <summary>
    //    /// 日志频率
    //    /// </summary>
    //    public ushort log_frequency;

    //    /// <summary>
    //    /// 是否保存设置
    //    /// </summary>
    //    public byte is_save_setting;

    //    /// <summary>
    //    /// 校验码
    //    /// </summary>
    //    public byte check_code;
    //}

    ///// <summary>
    ///// LiDAR日志响应结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct LivoxLidarLoggerResponse
    //{
    //    /// <summary>
    //    /// 返回代码
    //    /// </summary>
    //    public byte ret_code;
    //}

    ///// <summary>
    ///// LiDAR重启请求结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct LivoxLidarRebootRequest
    //{
    //    /// <summary>
    //    /// 重启延迟时间
    //    /// </summary>
    //    public ushort timeout;
    //}

    ///// <summary>
    ///// LiDAR重启响应结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct LivoxLidarRebootResponse
    //{
    //    /// <summary>
    //    /// 返回代码
    //    /// </summary>
    //    public byte ret_code;
    //}

    ///// <summary>
    ///// LiDAR复位请求结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct LivoxLidarResetRequest
    //{
    //    /// <summary>
    //    /// 数据
    //    /// </summary>
    //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    //    public byte[] data;
    //}

    ///// <summary>
    ///// LiDAR复位响应结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct LivoxLidarResetResponse
    //{
    //    /// <summary>
    //    /// 返回代码
    //    /// </summary>
    //    public byte ret_code;
    //}

    ///// <summary>
    ///// LiDAR升级状态结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct LivoxLidarUpgradeState
    //{
    //    /// <summary>
    //    /// 状态机事件
    //    /// </summary>
    //    public LivoxLidarFsmEvent state;

    //    /// <summary>
    //    /// 升级进度
    //    /// </summary>
    //    public byte progress;
    //}

    ///// <summary>
    ///// LiDAR RMC同步时间响应结构体
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public struct LivoxLidarRmcSyncTimeResponse
    //{
    //    /// <summary>
    //    /// 成功：0，失败：1
    //    /// </summary>
    //    public byte ret;
    //}
    //#endregion

    //#region 枚举(enum)
    ///// <summary>
    ///// 向设备发送各项指令的返回码
    ///// <para/>参见https://github.com/Livox-SDK/Livox-SDK2/wiki/Livox-SDK-Communication-Protocol-HAP(English)#return-code
    ///// </summary>
    //public enum ReturnCode
    //{
    //    /// <summary>
    //    /// 执行成功
    //    /// Execution succeed
    //    /// </summary>
    //    LVX_RET_SUCCESS = 0x00,

    //    /// <summary>
    //    /// 执行失败
    //    /// Execution failed
    //    /// </summary>
    //    LVX_RET_FAILURE = 0x01,

    //    /// <summary>
    //    /// 当前状态不支持
    //    /// Current state does not support
    //    /// </summary>
    //    LVX_RET_NOT_PERMIT_NOW = 0x02,

    //    /// <summary>
    //    /// 设置值超出范围
    //    /// Setting value out of range
    //    /// </summary>
    //    LVX_RET_OUT_OF_RANGE = 0x03,

    //    /// <summary>
    //    /// 参数不支持
    //    /// The parameter is not supported
    //    /// </summary>
    //    LVX_RET_PARAM_NOTSUPPORT = 0x20,

    //    /// <summary>
    //    /// 参数需要重启才能生效
    //    /// Parameters need to reboot to take effect
    //    /// </summary>
    //    LVX_RET_PARAM_REBOOT_EFFECT = 0x21,

    //    /// <summary>
    //    /// 参数是只读的，不能写入
    //    /// The parameter is read-only and cannot be written
    //    /// </summary>
    //    LVX_RET_PARAM_RD_ONLY = 0x22,

    //    /// <summary>
    //    /// 请求参数长度错误，或者确认包超过最大长度
    //    /// The request parameter length is wrong, or the ack packet exceeds the maximum length
    //    /// </summary>
    //    LVX_RET_PARAM_INVALID_LEN = 0x23,

    //    /// <summary>
    //    /// 参数键数量和键列表不匹配
    //    /// Parameter key number and key list mismatch
    //    /// </summary>
    //    LVX_RET_PARAM_KEY_NUM_ERR = 0x24,

    //    /// <summary>
    //    /// 公钥签名验证错误
    //    /// Public key signature verification error
    //    /// </summary>
    //    LVX_RET_UPGRADE_PUB_KEY_ERROR = 0x30,

    //    /// <summary>
    //    /// 摘要检查错误
    //    /// Digest check error
    //    /// </summary>
    //    LVX_RET_UPGRADE_DIGEST_ERROR = 0x31,

    //    /// <summary>
    //    /// 固件类型不匹配
    //    /// Firmware type mismatch
    //    /// </summary>
    //    LVX_RET_UPGRADE_FW_TYPE_ERROR = 0x32,

    //    /// <summary>
    //    /// 固件长度超出范围
    //    /// Firmware length out of range
    //    /// </summary>
    //    LVX_RET_UPGRADE_FW_OUT_OF_RANGE = 0x33
    //}

    //// Device type enum
    ///// <summary>
    ///// 设备类型枚举
    ///// </summary>
    //public enum LivoxLidarDeviceType : byte
    //{
    //    /// <summary>
    //    /// Hub设备
    //    /// </summary>
    //    kLivoxLidarTypeHub = 0,

    //    /// <summary>
    //    /// Mid40设备
    //    /// </summary>
    //    kLivoxLidarTypeMid40 = 1,

    //    /// <summary>
    //    /// Tele设备
    //    /// </summary>
    //    kLivoxLidarTypeTele = 2,

    //    /// <summary>
    //    /// Horizon设备
    //    /// </summary>
    //    kLivoxLidarTypeHorizon = 3,

    //    /// <summary>
    //    /// Mid70设备
    //    /// </summary>
    //    kLivoxLidarTypeMid70 = 6,

    //    /// <summary>
    //    /// Avia设备
    //    /// </summary>
    //    kLivoxLidarTypeAvia = 7,

    //    /// <summary>
    //    /// Mid360设备
    //    /// </summary>
    //    kLivoxLidarTypeMid360 = 9,

    //    /// <summary>
    //    /// 工业HAP设备
    //    /// </summary>
    //    kLivoxLidarTypeIndustrialHAP = 10,

    //    /// <summary>
    //    /// HAP设备
    //    /// </summary>
    //    kLivoxLidarTypeHAP = 15,

    //    /// <summary>
    //    /// PA设备
    //    /// </summary>
    //    kLivoxLidarTypePA = 16,
    //}

    //// Parameter key names enum
    ///// <summary>
    ///// 参数键名枚举
    ///// </summary>
    //public enum ParamKeyName
    //{
    //    /// <summary>
    //    /// 点云数据类型键
    //    /// </summary>
    //    kKeyPclDataType = 0x0000,

    //    /// <summary>
    //    /// 模式键
    //    /// </summary>
    //    kKeyPatternMode = 0x0001,

    //    /// <summary>
    //    /// 双发射使能键
    //    /// </summary>
    //    kKeyDualEmitEn = 0x0002,

    //    /// <summary>
    //    /// 点发送使能键
    //    /// </summary>
    //    kKeyPointSendEn = 0x0003,

    //    /// <summary>
    //    /// LiDAR IP配置键
    //    /// </summary>
    //    kKeyLidarIpCfg = 0x0004,

    //    /// <summary>
    //    /// 状态信息主机IP配置键
    //    /// </summary>
    //    kKeyStateInfoHostIpCfg = 0x0005,

    //    /// <summary>
    //    /// 点云数据主机IP配置键
    //    /// </summary>
    //    kKeyLidarPointDataHostIpCfg = 0x0006,

    //    /// <summary>
    //    /// IMU数据主机IP配置键
    //    /// </summary>
    //    kKeyLidarImuHostIpCfg = 0x0007,

    //    /// <summary>
    //    /// 控制主机IP配置键
    //    /// </summary>
    //    kKeyCtlHostIpCfg = 0x0008,

    //    /// <summary>
    //    /// 日志主机IP配置键
    //    /// </summary>
    //    kKeyLogHostIpCfg = 0x0009,

    //    /// <summary>
    //    /// 车辆速度键
    //    /// </summary>
    //    kKeyVehicleSpeed = 0x0010,

    //    /// <summary>
    //    /// 环境温度键
    //    /// </summary>
    //    kKeyEnvironmentTemp = 0x0011,

    //    /// <summary>
    //    /// 安装姿态键
    //    /// </summary>
    //    kKeyInstallAttitude = 0x0012,

    //    /// <summary>
    //    /// 盲区设置键
    //    /// </summary>
    //    kKeyBlindSpotSet = 0x0013,

    //    /// <summary>
    //    /// 帧率键
    //    /// </summary>
    //    kKeyFrameRate = 0x0014,

    //    /// <summary>
    //    /// 视场配置0键
    //    /// </summary>
    //    kKeyFovCfg0 = 0x0015,

    //    /// <summary>
    //    /// 视场配置1键
    //    /// </summary>
    //    kKeyFovCfg1 = 0x0016,

    //    /// <summary>
    //    /// 视场配置使能键
    //    /// </summary>
    //    kKeyFovCfgEn = 0x0017,

    //    /// <summary>
    //    /// 检测模式键
    //    /// </summary>
    //    kKeyDetectMode = 0x0018,

    //    /// <summary>
    //    /// 功能IO配置键
    //    /// </summary>
    //    kKeyFuncIoCfg = 0x0019,

    //    /// <summary>
    //    /// 启动后工作模式键
    //    /// </summary>
    //    kKeyWorkModeAfterBoot = 0x0020,

    //    /// <summary>
    //    /// 工作模式键
    //    /// </summary>
    //    kKeyWorkMode = 0x001A,

    //    /// <summary>
    //    /// 玻璃加热键
    //    /// </summary>
    //    kKeyGlassHeat = 0x001B,

    //    /// <summary>
    //    /// IMU数据使能键
    //    /// </summary>
    //    kKeyImuDataEn = 0x001C,

    //    /// <summary>
    //    /// FUSA使能键
    //    /// </summary>
    //    kKeyFusaEn = 0x001D,

    //    /// <summary>
    //    /// 强制加热使能键
    //    /// </summary>
    //    kKeyForceHeatEn = 0x001E,

    //    /// <summary>
    //    /// 日志参数设置键
    //    /// </summary>
    //    kKeyLogParamSet = 0x7FFF,

    //    /// <summary>
    //    /// 序列号键
    //    /// </summary>
    //    kKeySn = 0x8000,

    //    /// <summary>
    //    /// 产品信息键
    //    /// </summary>
    //    kKeyProductInfo = 0x8001,

    //    /// <summary>
    //    /// 应用版本键
    //    /// </summary>
    //    kKeyVersionApp = 0x8002,

    //    /// <summary>
    //    /// 加载器版本键
    //    /// </summary>
    //    kKeyVersionLoader = 0x8003,

    //    /// <summary>
    //    /// 硬件版本键
    //    /// </summary>
    //    kKeyVersionHardware = 0x8004,

    //    /// <summary>
    //    /// MAC地址键
    //    /// </summary>
    //    kKeyMac = 0x8005,

    //    /// <summary>
    //    /// 当前工作状态键
    //    /// </summary>
    //    kKeyCurWorkState = 0x8006,

    //    /// <summary>
    //    /// 核心温度键
    //    /// </summary>
    //    kKeyCoreTemp = 0x8007,

    //    /// <summary>
    //    /// 上电次数键
    //    /// </summary>
    //    kKeyPowerUpCnt = 0x8008,

    //    /// <summary>
    //    /// 当前本地时间键
    //    /// </summary>
    //    kKeyLocalTimeNow = 0x8009,

    //    /// <summary>
    //    /// 上次同步时间键
    //    /// </summary>
    //    kKeyLastSyncTime = 0x800A,

    //    /// <summary>
    //    /// 时间偏移键
    //    /// </summary>
    //    kKeyTimeOffset = 0x800B,

    //    /// <summary>
    //    /// 时间同步类型键
    //    /// </summary>
    //    kKeyTimeSyncType = 0x800C,

    //    /// <summary>
    //    /// 状态码键
    //    /// </summary>
    //    kKeyStatusCode = 0x800D,

    //    /// <summary>
    //    /// LiDAR诊断状态键
    //    /// </summary>
    //    kKeyLidarDiagStatus = 0x800E,

    //    /// <summary>
    //    /// LiDAR闪存状态键
    //    /// </summary>
    //    kKeyLidarFlashStatus = 0x800F,

    //    /// <summary>
    //    /// 固件类型键
    //    /// </summary>
    //    kKeyFwType = 0x8010,

    //    /// <summary>
    //    /// HMS代码键
    //    /// </summary>
    //    kKeyHmsCode = 0x8011,

    //    /// <summary>
    //    /// 当前玻璃加热状态键
    //    /// </summary>
    //    kKeyCurGlassHeatState = 0x8012,

    //    /// <summary>
    //    /// ROI模式键
    //    /// </summary>
    //    kKeyRoiMode = 0xFFFE,

    //    /// <summary>
    //    /// LiDAR诊断信息查询键
    //    /// </summary>
    //    kKeyLidarDiagInfoQuery = 0xFFFF,
    //}

    //// Point data type enum
    ///// <summary>
    ///// 点数据类型枚举
    ///// </summary>
    //public enum LivoxLidarPointDataType
    //{
    //    /// <summary>
    //    /// IMU数据类型
    //    /// </summary>
    //    kLivoxLidarImuData = 0,

    //    /// <summary>
    //    /// 高精度笛卡尔坐标数据类型
    //    /// </summary>
    //    kLivoxLidarCartesianCoordinateHighData = 0x01,

    //    /// <summary>
    //    /// 低精度笛卡尔坐标数据类型
    //    /// </summary>
    //    kLivoxLidarCartesianCoordinateLowData = 0x02,

    //    /// <summary>
    //    /// 球坐标数据类型
    //    /// </summary>
    //    kLivoxLidarSphericalCoordinateData = 0x03
    //}

    //// Log type enum
    ///// <summary>
    ///// 日志类型枚举
    ///// </summary>
    //public enum LivoxLidarLogType
    //{
    //    /// <summary>
    //    /// 实时日志类型
    //    /// </summary>
    //    kLivoxLidarRealTimeLog = 0,

    //    /// <summary>
    //    /// 异常日志类型
    //    /// </summary>
    //    kLivoxLidarExceptionLog = 0x01
    //}

    //// Status enum
    ///// <summary>
    ///// 状态枚举
    ///// </summary>
    //public enum LivoxLidarStatus
    //{
    //    /// <summary>
    //    /// 命令发送失败
    //    /// </summary>
    //    kLivoxLidarStatusSendFailed = -9,

    //    /// <summary>
    //    /// 处理程序实现不存在
    //    /// </summary>
    //    kLivoxLidarStatusHandlerImplNotExist = -8,

    //    /// <summary>
    //    /// 设备句柄无效
    //    /// </summary>
    //    kLivoxLidarStatusInvalidHandle = -7,

    //    /// <summary>
    //    /// 命令通道不存在
    //    /// </summary>
    //    kLivoxLidarStatusChannelNotExist = -6,

    //    /// <summary>
    //    /// 内存不足
    //    /// </summary>
    //    kLivoxLidarStatusNotEnoughMemory = -5,

    //    /// <summary>
    //    /// 操作超时
    //    /// </summary>
    //    kLivoxLidarStatusTimeout = -4,

    //    /// <summary>
    //    /// 设备不支持的操作
    //    /// </summary>
    //    kLivoxLidarStatusNotSupported = -3,

    //    /// <summary>
    //    /// 请求的设备未连接
    //    /// </summary>
    //    kLivoxLidarStatusNotConnected = -2,

    //    /// <summary>
    //    /// 失败
    //    /// </summary>
    //    kLivoxLidarStatusFailure = -1,

    //    /// <summary>
    //    /// 成功
    //    /// </summary>
    //    kLivoxLidarStatusSuccess = 0
    //}

    //// Scan pattern enum
    ///// <summary>
    ///// 扫描模式枚举
    ///// </summary>
    //public enum LivoxLidarScanPattern
    //{
    //    /// <summary>
    //    /// 非重复扫描模式
    //    /// </summary>
    //    kLivoxLidarScanPatternNoneRepetive = 0x00,

    //    /// <summary>
    //    /// 重复扫描模式
    //    /// </summary>
    //    kLivoxLidarScanPatternRepetive = 0x01,

    //    /// <summary>
    //    /// 低帧率重复扫描模式
    //    /// </summary>
    //    kLivoxLidarScanPatternRepetiveLowFrameRate = 0x02
    //}

    //// Frame rate enum
    ///// <summary>
    ///// 帧率枚举
    ///// </summary>
    //public enum LivoxLidarPointFrameRate
    //{
    //    /// <summary>
    //    /// 10Hz帧率
    //    /// </summary>
    //    kLivoxLidarFrameRate10Hz = 0x00,

    //    /// <summary>
    //    /// 15Hz帧率
    //    /// </summary>
    //    kLivoxLidarFrameRate15Hz = 0x01,

    //    /// <summary>
    //    /// 20Hz帧率
    //    /// </summary>
    //    kLivoxLidarFrameRate20Hz = 0x02,

    //    /// <summary>
    //    /// 25Hz帧率
    //    /// </summary>
    //    kLivoxLidarFrameRate25Hz = 0x03,
    //}

    //// Work mode enum
    ///// <summary>
    ///// 工作模式枚举
    ///// </summary>
    //public enum LivoxLidarWorkMode
    //{
    //    /// <summary>
    //    /// 正常模式
    //    /// </summary>
    //    kLivoxLidarNormal = 0x01,

    //    /// <summary>
    //    /// 唤醒模式
    //    /// </summary>
    //    kLivoxLidarWakeUp = 0x02,

    //    /// <summary>
    //    /// 休眠模式
    //    /// </summary>
    //    kLivoxLidarSleep = 0x03,

    //    /// <summary>
    //    /// 错误模式
    //    /// </summary>
    //    kLivoxLidarError = 0x04,

    //    /// <summary>
    //    /// 上电自检模式
    //    /// </summary>
    //    kLivoxLidarPowerOnSelfTest = 0x05,

    //    /// <summary>
    //    /// 电机启动模式
    //    /// </summary>
    //    kLivoxLidarMotorStarting = 0x06,

    //    /// <summary>
    //    /// 电机停止模式
    //    /// </summary>
    //    kLivoxLidarMotorStoping = 0x07,

    //    /// <summary>
    //    /// 升级模式
    //    /// </summary>
    //    kLivoxLidarUpgrade = 0x08
    //}

    //// Work mode after boot enum
    ///// <summary>
    ///// 启动后的工作模式枚举
    ///// </summary>
    //public enum LivoxLidarWorkModeAfterBoot
    //{
    //    /// <summary>
    //    /// 默认工作模式
    //    /// </summary>
    //    kLivoxLidarWorkModeAfterBootDefault = 0x00,

    //    /// <summary>
    //    /// 正常工作模式
    //    /// </summary>
    //    kLivoxLidarWorkModeAfterBootNormal = 0x01,

    //    /// <summary>
    //    /// 唤醒工作模式
    //    /// </summary>
    //    kLivoxLidarWorkModeAfterBootWakeUp = 0x02
    //}

    //// Detect mode enum
    ///// <summary>
    ///// 检测模式枚举
    ///// </summary>
    //public enum LivoxLidarDetectMode
    //{
    //    /// <summary>
    //    /// 检测模式：正常
    //    /// </summary>
    //    kLivoxLidarDetectNormal = 0x00,

    //    /// <summary>
    //    /// 检测模式：敏感
    //    /// </summary>
    //    kLivoxLidarDetectSensitive = 0x01
    //}

    //// Glass heat enum
    ///// <summary>
    ///// 玻璃加热枚举
    ///// </summary>
    //public enum LivoxLidarGlassHeat
    //{
    //    /// <summary>
    //    /// 停止加热或诊断加热
    //    /// </summary>
    //    kLivoxLidarStopPowerOnHeatingOrDiagnosticHeating = 0x00,

    //    /// <summary>
    //    /// 开启加热
    //    /// </summary>
    //    kLivoxLidarTurnOnHeating = 0x01,

    //    /// <summary>
    //    /// 诊断加热
    //    /// </summary>
    //    kLivoxLidarDiagnosticHeating = 0x02,

    //    /// <summary>
    //    /// 停止自加热
    //    /// </summary>
    //    kLivoxLidarStopSelfHeating = 0x03
    //}

    //// Upgrade related data struct
    ///// <summary>
    ///// LiDAR状态机状态枚举
    ///// </summary>
    //public enum LivoxLidarFsmState
    //{
    //    /// <summary>
    //    /// 升级空闲状态
    //    /// </summary>
    //    kLivoxLidarUpgradeIdle = 0,

    //    /// <summary>
    //    /// 升级请求状态
    //    /// </summary>
    //    kLivoxLidarUpgradeRequest = 1,

    //    /// <summary>
    //    /// 传输固件状态
    //    /// </summary>
    //    kLivoxLidarUpgradeXferFirmware = 2,

    //    /// <summary>
    //    /// 完成固件传输状态
    //    /// </summary>
    //    kLivoxLidarUpgradeCompleteXferFirmware = 3,

    //    /// <summary>
    //    /// 获取升级进度状态
    //    /// </summary>
    //    kLivoxLidarUpgradeGetUpgradeProgress = 4,

    //    /// <summary>
    //    /// 升级完成状态
    //    /// </summary>
    //    kLivoxLidarUpgradeComplete = 5,

    //    /// <summary>
    //    /// 升级超时状态
    //    /// </summary>
    //    kLivoxLidarUpgradeTimeout = 6,

    //    /// <summary>
    //    /// 升级错误状态
    //    /// </summary>
    //    kLivoxLidarUpgradeErr = 7,

    //    /// <summary>
    //    /// 未定义状态
    //    /// </summary>
    //    kLivoxLidarUpgradeUndef = 8
    //}

    ///// <summary>
    ///// LiDAR状态机事件枚举
    ///// </summary>
    //public enum LivoxLidarFsmEvent
    //{
    //    /// <summary>
    //    /// 请求升级事件
    //    /// </summary>
    //    kLivoxLidarEventRequestUpgrade = 0,

    //    /// <summary>
    //    /// 传输固件事件
    //    /// </summary>
    //    kLivoxLidarEventXferFirmware = 1,

    //    /// <summary>
    //    /// 完成固件传输事件
    //    /// </summary>
    //    kLivoxLidarEventCompleteXferFirmware = 2,

    //    /// <summary>
    //    /// 获取升级进度事件
    //    /// </summary>
    //    kLivoxLidarEventGetUpgradeProgress = 3,

    //    /// <summary>
    //    /// 升级完成事件
    //    /// </summary>
    //    kLivoxLidarEventComplete = 4,

    //    /// <summary>
    //    /// 重新初始化事件
    //    /// </summary>
    //    kLivoxLidarEventReinit = 5,

    //    /// <summary>
    //    /// 超时事件
    //    /// </summary>
    //    kLivoxLidarEventTimeout = 6,

    //    /// <summary>
    //    /// 错误事件
    //    /// </summary>
    //    kLivoxLidarEventErr = 7,

    //    /// <summary>
    //    /// 未定义事件
    //    /// </summary>
    //    kLivoxLidarEventUndef = 8
    //}
    //#endregion

    //#region 回调函数(Callback)
    //// Callback function for receiving point cloud data
    ///// <summary>
    ///// 接收点云数据的回调函数
    ///// </summary>
    ///// <param name="handle">设备句柄。</param>
    ///// <param name="dev_type">设备类型。</param>
    ///// <param name="data">点云数据指针。</param>
    ///// <param name="client_data">用户数据。</param>
    //public delegate void LivoxLidarPointCloudCallBack(uint handle, byte dev_type, IntPtr data, IntPtr client_data);

    //// Callback function for receiving command data
    ///// <summary>
    ///// 接收命令数据的回调函数
    ///// </summary>
    ///// <param name="handle">设备句柄。</param>
    ///// <param name="data">命令数据指针。</param>
    ///// <param name="client_data">用户数据。</param>
    //public delegate void LivoxLidarCmdObserverCallBack(uint handle, IntPtr data, IntPtr client_data);

    //// Callback function for point cloud observer
    ///// <summary>
    ///// 点云观察者回调函数
    ///// </summary>
    ///// <param name="handle">设备句柄。</param>
    ///// <param name="dev_type">设备类型。</param>
    ///// <param name="data">点云数据指针。</param>
    ///// <param name="client_data">用户数据。</param>
    //public delegate void LivoxLidarPointCloudObserver(uint handle, byte dev_type, IntPtr data, IntPtr client_data);

    //// Callback function for receiving IMU data
    ///// <summary>
    ///// 接收IMU数据的回调函数
    ///// </summary>
    ///// <param name="handle">设备句柄。</param>
    ///// <param name="dev_type">设备类型。</param>
    ///// <param name="data">IMU数据指针。</param>
    ///// <param name="client_data">用户数据。</param>
    //public delegate void LivoxLidarImuDataCallback(uint handle, byte dev_type, IntPtr data, IntPtr client_data);

    //// Callback function for receiving status info
    ///// <summary>
    ///// 接收状态信息的回调函数
    ///// </summary>
    ///// <param name="handle">设备句柄。</param>
    ///// <param name="dev_type">设备类型。</param>
    ///// <param name="info">状态信息字符串。</param>
    ///// <param name="client_data">用户数据。</param>
    //public delegate void LivoxLidarInfoCallback(uint handle, byte dev_type, string info, IntPtr client_data);

    //// Callback function for receiving status info change
    ///// <summary>
    ///// 接收状态信息变化的回调函数
    ///// </summary>
    ///// <param name="handle">设备句柄。</param>
    ///// <param name="info">状态信息指针。</param>
    ///// <param name="client_data">用户数据。</param>
    //public delegate void LivoxLidarInfoChangeCallback(uint handle, IntPtr info, IntPtr client_data);

    ///// <summary>
    ///// 查询LiDAR内部信息的回调函数
    ///// </summary>
    ///// <param name="status">操作状态。</param>
    ///// <param name="handle">设备句柄。</param>
    ///// <param name="response">响应数据指针。</param>
    ///// <param name="client_data">用户数据。</param>
    //public delegate void QueryLivoxLidarInternalInfoCallback(LivoxLidarStatus status, uint handle, IntPtr response, IntPtr client_data);

    //// Callback function for receiving async control response
    ///// <summary>
    ///// 接收异步控制响应的回调函数
    ///// </summary>
    ///// <param name="status">操作状态。</param>
    ///// <param name="handle">设备句柄。</param>
    ///// <param name="response">响应数据指针。</param>
    ///// <param name="client_data">用户数据。</param>
    //public delegate void LivoxLidarAsyncControlCallback(LivoxLidarStatus status, uint handle, IntPtr response, IntPtr client_data);

    //// Callback function for receiving reset setting response
    ///// <summary>
    ///// 接收重置设置响应的回调函数
    ///// </summary>
    ///// <param name="status">操作状态。</param>
    ///// <param name="handle">设备句柄。</param>
    ///// <param name="response">响应数据指针。</param>
    ///// <param name="client_data">用户数据。</param>
    //public delegate void LivoxLidarResetCallback(LivoxLidarStatus status, uint handle, IntPtr response, IntPtr client_data);

    //// Callback function for receiving logger setting response
    ///// <summary>
    ///// 接收日志设置响应的回调函数
    ///// </summary>
    ///// <param name="status">操作状态。</param>
    ///// <param name="handle">设备句柄。</param>
    ///// <param name="response">响应数据指针。</param>
    ///// <param name="client_data">用户数据。</param>
    //public delegate void LivoxLidarLoggerCallback(LivoxLidarStatus status, uint handle, IntPtr response, IntPtr client_data);

    //// Callback function for receiving reboot setting response
    ///// <summary>
    ///// 接收重启设置响应的回调函数
    ///// </summary>
    ///// <param name="status">操作状态。</param>
    ///// <param name="handle">设备句柄。</param>
    ///// <param name="response">响应数据指针。</param>
    ///// <param name="client_data">用户数据。</param>
    //public delegate void LivoxLidarRebootCallback(LivoxLidarStatus status, uint handle, IntPtr response, IntPtr client_data);

    ///// <summary>
    ///// 接收LiDAR升级进度的回调函数
    ///// </summary>
    ///// <param name="handle">设备句柄。</param>
    ///// <param name="state">升级状态。</param>
    ///// <param name="client_data">用户数据。</param>
    //public delegate void OnLivoxLidarUpgradeProgressCallback(uint handle, LivoxLidarUpgradeState state, IntPtr client_data);

    //// Callback function for receiving RMC sync time response
    ///// <summary>
    ///// 接收RMC时间同步响应的回调函数
    ///// </summary>
    ///// <param name="status">操作状态。</param>
    ///// <param name="handle">设备句柄。</param>
    ///// <param name="data">同步时间数据指针。</param>
    ///// <param name="client_data">用户数据。</param>
    //public delegate void LivoxLidarRmcSyncTimeCallBack(LivoxLidarStatus status, uint handle, IntPtr data, IntPtr client_data);
    //#endregion
}