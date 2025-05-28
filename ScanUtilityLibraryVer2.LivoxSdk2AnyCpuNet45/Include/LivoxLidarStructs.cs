using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScanUtilityLibraryVer2.LivoxSdk2.Include
{
    #region 结构体(Struct)
    // Numeric version information struct
    /// <summary>
    /// 数字版本信息结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxLidarSdkVer
    {
        /// <summary>
        /// 主版本号
        /// </summary>
        public int major;

        /// <summary>
        /// 次版本号
        /// </summary>
        public int minor;

        /// <summary>
        /// 修订版本号
        /// </summary>
        public int patch;
    }

    /// <summary>
    /// LiDAR设备信息结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxLidarInfo
    {
        /// <summary>
        /// 设备类型
        /// </summary>
        public byte dev_type;

        /// <summary>
        /// 序列号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string sn;

        /// <summary>
        /// LiDAR IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string lidar_ip;
    }

    /// <summary>
    /// LiDAR以太网数据包结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxLidarEthernetPacket
    {
        /// <summary>
        /// 包协议版本：当前为0
        /// </summary>
        public byte version;

        /// <summary>
        /// 数据包长度：从version开始的整个UDP数据段长度
        /// </summary>
        public ushort length;

        /// <summary>
        /// 帧内点云采样时间(单位0.1us)
        /// <para/>这帧点云数据中最后一个点减去第一个点时间
        /// </summary>
        public ushort time_interval;

        /// <summary>
        /// 当前UDP包data字段包含点数目：
        /// <para/>点云数据为96，IMU数据为1
        /// </summary>
        public ushort dot_num;

        /// <summary>
        /// 点云UDP包计数，用来区分发送的顺序，从0开始，每个UDP包依次加1，最大值为ushort.MaxValue(65535)
        /// </summary>
        public ushort udp_cnt;

        /// <summary>
        /// 帧计数（HAP该字段为0）
        /// </summary>
        public byte frame_cnt;

        /// <summary>
        /// 数据类型：0-IMU Data；1-Point Cloud Data1；2-Point Cloud Data2
        /// </summary>
        //不能试使用枚举，会使字段专用空间变大（占4个字节）
        public LivoxLidarPointDataType data_type;

        /// <summary>
        /// 时间戳类型：
        /// <para/>0 无同步源，时间戳为雷达开机时间
        /// <para/>1 gPTP同步，时间戳为master时钟源时间
        /// </summary>
        public byte time_type;

        /// <summary>
        /// bit0-1: 功能安全safety_info
        /// 0x0：整包可信;
        /// 0x1：整包不可信;
        /// 0x2：非0点可信;
        /// <para/>bit2-3: tag_type, HAP 固定为0
        /// <para/>bit4-7: rsvd
        /// </summary>
        public byte pack_info;

        /// <summary>
        /// 预留
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
        public byte[] rsvd;

        /// <summary>
        /// CRC32校验码，校验timestamp+data段
        /// </summary>
        public uint crc32;

        /// <summary>
        /// 点云时间戳（微秒），类型由time_type决定
        /// <para/>每个数据包中的时间戳代表第一个点云的时间，每个数据包中有n个点云，这n个点云的时间是等间隔的，总间隔时间为time_interval的值
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] timestamp;

        /// <summary>
        /// 点云数据或IMU数据（由data_type决定）
        /// <para/>IMU：24字节 * 数量1
        /// <para/>Point Cloud Data1：14字节 * 数量96
        /// <para/>Point Cloud Data2：8字节  * 数量96
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1344)] //最大字节数为14*96=1344，将SizeConst设为此值，当data长度小于SizeConst时，后续的字节将由0xCD(205)填充
        public byte[] data;
    }

    /// <summary>
    /// LiDAR命令数据包结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxLidarCmdPacket
    {
        /// <summary>
        /// 帧起始标志
        /// </summary>
        public byte sof;

        /// <summary>
        /// 版本号
        /// </summary>
        public byte version;

        /// <summary>
        /// 数据包长度
        /// </summary>
        public ushort length;

        /// <summary>
        /// 序列号
        /// </summary>
        public uint seq_num;

        /// <summary>
        /// 命令ID
        /// </summary>
        public ushort cmd_id;

        /// <summary>
        /// 命令类型
        /// </summary>
        public byte cmd_type;

        /// <summary>
        /// 发送方类型
        /// </summary>
        public byte sender_type;

        /// <summary>
        /// 预留
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
        public string rsvd;

        /// <summary>
        /// 高位CRC16校验码
        /// </summary>
        public ushort crc16_h;

        /// <summary>
        /// 数据CRC32校验码
        /// </summary>
        public uint crc32_d;

        /// <summary>
        /// 数据
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] data;
    }

    /// <summary>
    /// LiDAR IMU原始点数据结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxLidarImuRawPoint
    {
        /// <summary>
        /// 陀螺仪X轴数据
        /// </summary>
        public float gyro_x;

        /// <summary>
        /// 陀螺仪Y轴数据
        /// </summary>
        public float gyro_y;

        /// <summary>
        /// 陀螺仪Z轴数据
        /// </summary>
        public float gyro_z;

        /// <summary>
        /// 加速度计X轴数据
        /// </summary>
        public float acc_x;

        /// <summary>
        /// 加速度计Y轴数据
        /// </summary>
        public float acc_y;

        /// <summary>
        /// 加速度计Z轴数据
        /// </summary>
        public float acc_z;
    }

    /// <summary>
    /// LiDAR高精度笛卡尔坐标系点数据结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxLidarCartesianHighRawPoint
    {
        /// <summary>
        /// X轴坐标，单位：毫米
        /// </summary>
        public int x;

        /// <summary>
        /// Y轴坐标，单位：毫米
        /// </summary>
        public int y;

        /// <summary>
        /// Z轴坐标，单位：毫米
        /// </summary>
        public int z;

        /// <summary>
        /// 反射率
        /// </summary>
        public byte reflectivity;

        /// <summary>
        /// 标签
        /// </summary>
        public byte tag;
    }

    /// <summary>
    /// LiDAR低精度笛卡尔坐标系点数据结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxLidarCartesianLowRawPoint
    {
        /// <summary>
        /// X轴坐标，单位：厘米
        /// </summary>
        public short x;

        /// <summary>
        /// Y轴坐标，单位：厘米
        /// </summary>
        public short y;

        /// <summary>
        /// Z轴坐标，单位：厘米
        /// </summary>
        public short z;

        /// <summary>
        /// 反射率
        /// </summary>
        public byte reflectivity;

        /// <summary>
        /// 标签
        /// </summary>
        public byte tag;
    }

    /// <summary>
    /// LiDAR球坐标系点数据结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxLidarSpherPoint
    {
        /// <summary>
        /// 深度
        /// </summary>
        public uint depth;

        /// <summary>
        /// 横向角度
        /// </summary>
        public ushort theta;

        /// <summary>
        /// 纵向角度
        /// </summary>
        public ushort phi;

        /// <summary>
        /// 反射率
        /// </summary>
        public byte reflectivity;

        /// <summary>
        /// 标签
        /// </summary>
        public byte tag;
    }

    /// <summary>
    /// LiDAR键值对参数结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxLidarKeyValueParam
    {
        /// <summary>
        /// 键名
        /// </summary>
        public ushort key;

        /// <summary>
        /// 值的长度
        /// </summary>
        public ushort length;

        /// <summary>
        /// 值
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] value;
    }

    /// <summary>
    /// LiDAR异步控制响应结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxLidarAsyncControlResponse
    {
        /// <summary>
        /// 返回代码
        /// </summary>
        public byte ret_code;

        /// <summary>
        /// 错误键
        /// </summary>
        public ushort error_key;
    }

    /// <summary>
    /// LiDAR信息响应结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxLidarInfoResponse
    {
        /// <summary>
        /// 返回代码
        /// </summary>
        public byte ret_code;

        /// <summary>
        /// LiDAR信息
        /// </summary>
        public IntPtr lidar_info;
    }

    /// <summary>
    /// LiDAR内部诊断信息响应结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxLidarDiagInternalInfoResponse
    {
        /// <summary>
        /// 返回代码
        /// </summary>
        public byte ret_code;

        /// <summary>
        /// 参数数量
        /// </summary>
        public ushort param_num;

        /// <summary>
        /// 数据
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] data;
    }

    /// <summary>
    /// LiDAR安装姿态结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxLidarInstallAttitude
    {
        /// <summary>
        /// 滚转角度，单位：度
        /// </summary>
        public float roll_deg;

        /// <summary>
        /// 俯仰角度，单位：度
        /// </summary>
        public float pitch_deg;

        /// <summary>
        /// 偏航角度，单位：度
        /// </summary>
        public float yaw_deg;

        /// <summary>
        /// X轴坐标，单位：毫米
        /// </summary>
        public int x;

        /// <summary>
        /// Y轴坐标，单位：毫米
        /// </summary>
        public int y;

        /// <summary>
        /// Z轴坐标，单位：毫米
        /// </summary>
        public int z;
    }

    /// <summary>
    /// 视场配置结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FovCfg
    {
        /// <summary>
        /// 横向视场起始角度，单位：度
        /// </summary>
        public int yaw_start;

        /// <summary>
        /// 横向视场终止角度，单位：度
        /// </summary>
        public int yaw_stop;

        /// <summary>
        /// 纵向视场起始角度，单位：度
        /// </summary>
        public int pitch_start;

        /// <summary>
        /// 纵向视场终止角度，单位：度
        /// </summary>
        public int pitch_stop;

        /// <summary>
        /// 保留字段
        /// </summary>
        public uint rsvd;
    }

    /// <summary>
    /// 功能IO配置结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FuncIOCfg
    {
        /// <summary>
        /// 输入0
        /// </summary>
        public byte in0;

        /// <summary>
        /// 输入1
        /// </summary>
        public byte int1;

        /// <summary>
        /// 输出0
        /// </summary>
        public byte out0;

        /// <summary>
        /// 输出1
        /// </summary>
        public byte out1;
    }

    /// <summary>
    /// LiDAR日志配置结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxLidarLoggerCfgInfo
    {
        /// <summary>
        /// 是否启用LiDAR日志
        /// </summary>
        public bool lidar_log_enable;

        /// <summary>
        /// LiDAR日志缓存大小
        /// </summary>
        public uint lidar_log_cache_size;

        /// <summary>
        /// LiDAR日志路径
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string lidar_log_path;
    }

    /// <summary>
    /// LiDAR IP信息结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxLidarIpInfo
    {
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string ip_addr;

        /// <summary>
        /// 子网掩码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string net_mask;

        /// <summary>
        /// 网关地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string gw_addr;
    }

    /// <summary>
    /// 主机状态信息IP配置结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HostStateInfoIpInfo
    {
        /// <summary>
        /// 主机IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string host_ip_addr;

        /// <summary>
        /// 主机状态信息端口
        /// </summary>
        public ushort host_state_info_port;

        /// <summary>
        /// LiDAR状态信息端口
        /// </summary>
        public ushort lidar_state_info_port;
    }

    /// <summary>
    /// 主机点云数据IP配置结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HostPointIPInfo
    {
        /// <summary>
        /// 主机IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string host_ip_addr;

        /// <summary>
        /// 主机点云数据端口
        /// </summary>
        public ushort host_point_data_port;

        /// <summary>
        /// LiDAR点云数据端口
        /// </summary>
        public ushort lidar_point_data_port;
    }

    /// <summary>
    /// 主机IMU数据IP配置结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HostImuDataIPInfo
    {
        /// <summary>
        /// 主机IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string host_ip_addr;

        /// <summary>
        /// 主机IMU数据端口（保留）
        /// </summary>
        public ushort host_imu_data_port;

        /// <summary>
        /// LiDAR IMU数据端口（保留）
        /// </summary>
        public ushort lidar_imu_data_port;
    }

    /// <summary>
    /// LiDAR IP配置结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxIpCfg
    {
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string ip_addr;

        /// <summary>
        /// 目标端口
        /// </summary>
        public ushort dst_port;

        /// <summary>
        /// 源端口
        /// </summary>
        public ushort src_port;
    }

    /// <summary>
    /// LiDAR状态信息结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxLidarStateInfo
    {
        /// <summary>
        /// 点云数据类型
        /// </summary>
        public byte pcl_data_type;

        /// <summary>
        /// 扫描模式
        /// </summary>
        public byte pattern_mode;

        /// <summary>
        /// 双发射使能
        /// </summary>
        public byte dual_emit_en;

        /// <summary>
        /// 点发送使能
        /// </summary>
        public byte point_send_en;

        /// <summary>
        /// LiDAR IP信息
        /// </summary>
        public LivoxLidarIpInfo lidar_ip_info;

        /// <summary>
        /// 主机点云数据IP信息
        /// </summary>
        public HostPointIPInfo host_point_ip_info;

        /// <summary>
        /// 主机IMU数据IP信息
        /// </summary>
        public HostImuDataIPInfo host_imu_ip_info;

        /// <summary>
        /// 安装姿态
        /// </summary>
        public LivoxLidarInstallAttitude install_attitude;

        /// <summary>
        /// 盲区设置
        /// </summary>
        public uint blind_spot_set;

        /// <summary>
        /// 工作模式
        /// </summary>
        public byte work_mode;

        /// <summary>
        /// 玻璃加热
        /// </summary>
        public byte glass_heat;

        /// <summary>
        /// IMU数据使能
        /// </summary>
        public byte imu_data_en;

        /// <summary>
        /// FUSA使能
        /// </summary>
        public byte fusa_en;

        /// <summary>
        /// 序列号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string sn;

        /// <summary>
        /// 产品信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string product_info;

        /// <summary>
        /// 应用版本
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] version_app;

        /// <summary>
        /// 加载器版本
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] version_loader;

        /// <summary>
        /// 硬件版本
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] version_hardware;

        /// <summary>
        /// MAC地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] mac;

        /// <summary>
        /// 当前工作状态
        /// </summary>
        public byte cur_work_state;

        /// <summary>
        /// 状态码
        /// </summary>
        public ulong status_code;
    }

    /// <summary>
    /// LiDAR直接状态信息结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DirectLidarStateInfo
    {
        /// <summary>
        /// 点云数据类型
        /// </summary>
        public byte pcl_data_type;

        /// <summary>
        /// 扫描模式
        /// </summary>
        public byte pattern_mode;

        /// <summary>
        /// 双发射使能
        /// </summary>
        public byte dual_emit_en;

        /// <summary>
        /// 点发送使能
        /// </summary>
        public byte point_send_en;

        /// <summary>
        /// LiDAR IP配置
        /// </summary>
        public LivoxLidarIpInfo lidar_ipcfg;

        /// <summary>
        /// 主机状态信息IP配置
        /// </summary>
        public HostStateInfoIpInfo host_state_info;

        /// <summary>
        /// 主机点云数据IP配置
        /// </summary>
        public HostPointIPInfo pointcloud_host_ipcfg;

        /// <summary>
        /// 主机IMU数据IP配置
        /// </summary>
        public HostImuDataIPInfo imu_host_ipcfg;

        /// <summary>
        /// 控制主机IP配置
        /// </summary>
        public LivoxIpCfg ctl_host_ipcfg;

        /// <summary>
        /// 日志主机IP配置
        /// </summary>
        public LivoxIpCfg log_host_ipcfg;

        /// <summary>
        /// 车辆速度
        /// </summary>
        public int vehicle_speed;

        /// <summary>
        /// 环境温度
        /// </summary>
        public int environment_temp;

        /// <summary>
        /// 安装姿态
        /// </summary>
        public LivoxLidarInstallAttitude install_attitude;

        /// <summary>
        /// 盲区设置
        /// </summary>
        public uint blind_spot_set;

        /// <summary>
        /// 帧率
        /// </summary>
        public byte frame_rate;

        /// <summary>
        /// 视场配置0
        /// </summary>
        public FovCfg fov_cfg0;

        /// <summary>
        /// 视场配置1
        /// </summary>
        public FovCfg fov_cfg1;

        /// <summary>
        /// 视场配置使能
        /// </summary>
        public byte fov_cfg_en;

        /// <summary>
        /// 检测模式
        /// </summary>
        public byte detect_mode;

        /// <summary>
        /// 功能IO配置
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] func_io_cfg;

        /// <summary>
        /// 工作目标模式
        /// </summary>
        public byte work_tgt_mode;

        /// <summary>
        /// 玻璃加热
        /// </summary>
        public byte glass_heat;

        /// <summary>
        /// IMU数据使能
        /// </summary>
        public byte imu_data_en;

        /// <summary>
        /// FUSA使能
        /// </summary>
        public byte fusa_en;

        /// <summary>
        /// 序列号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string sn;

        /// <summary>
        /// 产品信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string product_info;

        /// <summary>
        /// 应用版本
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] version_app;

        /// <summary>
        /// 加载器版本
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] version_loader;

        /// <summary>
        /// 硬件版本
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] version_hardware;

        /// <summary>
        /// MAC地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] mac;

        /// <summary>
        /// 当前工作状态
        /// </summary>
        public byte cur_work_state;

        /// <summary>
        /// 核心温度
        /// </summary>
        public int core_temp;

        /// <summary>
        /// 上电次数
        /// </summary>
        public uint powerup_cnt;

        /// <summary>
        /// 当前本地时间
        /// </summary>
        public ulong local_time_now;

        /// <summary>
        /// 上次同步时间
        /// </summary>
        public ulong last_sync_time;

        /// <summary>
        /// 时间偏移
        /// </summary>
        public long time_offset;

        /// <summary>
        /// 时间同步类型
        /// </summary>
        public byte time_sync_type;

        /// <summary>
        /// 状态码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] status_code;

        /// <summary>
        /// LiDAR诊断状态
        /// </summary>
        public ushort lidar_diag_status;

        /// <summary>
        /// LiDAR闪存状态
        /// </summary>
        public byte lidar_flash_status;

        /// <summary>
        /// 固件类型
        /// </summary>
        public byte fw_type;

        /// <summary>
        /// HMS代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public uint[] hms_code;

        /// <summary>
        /// ROI模式
        /// </summary>
        public byte ROI_Mode;
    }

    /// <summary>
    /// LiDAR查询内部信息响应结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxLidarQueryInternalInfoResponse
    {
        /// <summary>
        /// 返回代码
        /// </summary>
        public byte ret_code;

        /// <summary>
        /// LiDAR状态信息
        /// </summary>
        public LivoxLidarStateInfo livox_lidar_state_info;
    }

    /// <summary>
    /// LiDAR日志参数结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxLidarLogParam
    {
        /// <summary>
        /// 日志发送方法
        /// </summary>
        public byte log_send_method;

        /// <summary>
        /// 日志ID
        /// </summary>
        public ushort log_id;

        /// <summary>
        /// 日志频率
        /// </summary>
        public ushort log_frequency;

        /// <summary>
        /// 是否保存设置
        /// </summary>
        public byte is_save_setting;

        /// <summary>
        /// 校验码
        /// </summary>
        public byte check_code;
    }

    /// <summary>
    /// LiDAR日志响应结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxLidarLoggerResponse
    {
        /// <summary>
        /// 返回代码
        /// </summary>
        public byte ret_code;
    }

    /// <summary>
    /// LiDAR重启请求结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxLidarRebootRequest
    {
        /// <summary>
        /// 重启延迟时间
        /// </summary>
        public ushort timeout;
    }

    /// <summary>
    /// LiDAR重启响应结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxLidarRebootResponse
    {
        /// <summary>
        /// 返回代码
        /// </summary>
        public byte ret_code;
    }

    /// <summary>
    /// LiDAR复位请求结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxLidarResetRequest
    {
        /// <summary>
        /// 数据
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] data;
    }

    /// <summary>
    /// LiDAR复位响应结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxLidarResetResponse
    {
        /// <summary>
        /// 返回代码
        /// </summary>
        public byte ret_code;
    }

    /// <summary>
    /// LiDAR升级状态结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxLidarUpgradeState
    {
        /// <summary>
        /// 状态机事件
        /// </summary>
        public LivoxLidarFsmEvent state;

        /// <summary>
        /// 升级进度
        /// </summary>
        public byte progress;
    }

    /// <summary>
    /// LiDAR RMC同步时间响应结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LivoxLidarRmcSyncTimeResponse
    {
        /// <summary>
        /// 成功：0，失败：1
        /// </summary>
        public byte ret;
    }
    #endregion
}
