using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanUtilityLibraryVer2.LivoxSdk2.Include
{
    #region 枚举(enum)
    /// <summary>
    /// 向设备发送各项指令的返回码
    /// <para/>参见https://github.com/Livox-SDK/Livox-SDK2/wiki/Livox-SDK-Communication-Protocol-HAP(English)#return-code
    /// </summary>
    public enum ReturnCode
    {
        /// <summary>
        /// 执行成功
        /// Execution succeed
        /// </summary>
        LVX_RET_SUCCESS = 0x00,

        /// <summary>
        /// 执行失败
        /// Execution failed
        /// </summary>
        LVX_RET_FAILURE = 0x01,

        /// <summary>
        /// 当前状态不支持
        /// Current state does not support
        /// </summary>
        LVX_RET_NOT_PERMIT_NOW = 0x02,

        /// <summary>
        /// 设置值超出范围
        /// Setting value out of range
        /// </summary>
        LVX_RET_OUT_OF_RANGE = 0x03,

        /// <summary>
        /// 参数不支持
        /// The parameter is not supported
        /// </summary>
        LVX_RET_PARAM_NOTSUPPORT = 0x20,

        /// <summary>
        /// 参数需要重启才能生效
        /// Parameters need to reboot to take effect
        /// </summary>
        LVX_RET_PARAM_REBOOT_EFFECT = 0x21,

        /// <summary>
        /// 参数是只读的，不能写入
        /// The parameter is read-only and cannot be written
        /// </summary>
        LVX_RET_PARAM_RD_ONLY = 0x22,

        /// <summary>
        /// 请求参数长度错误，或者确认包超过最大长度
        /// The request parameter length is wrong, or the ack packet exceeds the maximum length
        /// </summary>
        LVX_RET_PARAM_INVALID_LEN = 0x23,

        /// <summary>
        /// 参数键数量和键列表不匹配
        /// Parameter key number and key list mismatch
        /// </summary>
        LVX_RET_PARAM_KEY_NUM_ERR = 0x24,

        /// <summary>
        /// 公钥签名验证错误
        /// Public key signature verification error
        /// </summary>
        LVX_RET_UPGRADE_PUB_KEY_ERROR = 0x30,

        /// <summary>
        /// 摘要检查错误
        /// Digest check error
        /// </summary>
        LVX_RET_UPGRADE_DIGEST_ERROR = 0x31,

        /// <summary>
        /// 固件类型不匹配
        /// Firmware type mismatch
        /// </summary>
        LVX_RET_UPGRADE_FW_TYPE_ERROR = 0x32,

        /// <summary>
        /// 固件长度超出范围
        /// Firmware length out of range
        /// </summary>
        LVX_RET_UPGRADE_FW_OUT_OF_RANGE = 0x33
    }

    // Device type enum
    /// <summary>
    /// 设备类型枚举
    /// </summary>
    public enum LivoxLidarDeviceType
    {
        /// <summary>
        /// Hub设备
        /// </summary>
        kLivoxLidarTypeHub = 0,

        /// <summary>
        /// Mid40设备
        /// </summary>
        kLivoxLidarTypeMid40 = 1,

        /// <summary>
        /// Tele设备
        /// </summary>
        kLivoxLidarTypeTele = 2,

        /// <summary>
        /// Horizon设备
        /// </summary>
        kLivoxLidarTypeHorizon = 3,

        /// <summary>
        /// Mid70设备
        /// </summary>
        kLivoxLidarTypeMid70 = 6,

        /// <summary>
        /// Avia设备
        /// </summary>
        kLivoxLidarTypeAvia = 7,

        /// <summary>
        /// Mid360设备
        /// </summary>
        kLivoxLidarTypeMid360 = 9,

        /// <summary>
        /// 工业HAP设备
        /// </summary>
        kLivoxLidarTypeIndustrialHAP = 10,

        /// <summary>
        /// HAP设备
        /// </summary>
        kLivoxLidarTypeHAP = 15,

        /// <summary>
        /// PA设备
        /// </summary>
        kLivoxLidarTypePA = 16,
    }

    // Parameter key names enum
    /// <summary>
    /// 参数键名枚举
    /// </summary>
    public enum ParamKeyName
    {
        /// <summary>
        /// 点云数据类型键
        /// </summary>
        kKeyPclDataType = 0x0000,

        /// <summary>
        /// 模式键
        /// </summary>
        kKeyPatternMode = 0x0001,

        /// <summary>
        /// 双发射使能键
        /// </summary>
        kKeyDualEmitEn = 0x0002,

        /// <summary>
        /// 点发送使能键
        /// </summary>
        kKeyPointSendEn = 0x0003,

        /// <summary>
        /// LiDAR IP配置键
        /// </summary>
        kKeyLidarIpCfg = 0x0004,

        /// <summary>
        /// 状态信息主机IP配置键
        /// </summary>
        kKeyStateInfoHostIpCfg = 0x0005,

        /// <summary>
        /// 点云数据主机IP配置键
        /// </summary>
        kKeyLidarPointDataHostIpCfg = 0x0006,

        /// <summary>
        /// IMU数据主机IP配置键
        /// </summary>
        kKeyLidarImuHostIpCfg = 0x0007,

        /// <summary>
        /// 控制主机IP配置键
        /// </summary>
        kKeyCtlHostIpCfg = 0x0008,

        /// <summary>
        /// 日志主机IP配置键
        /// </summary>
        kKeyLogHostIpCfg = 0x0009,

        /// <summary>
        /// 车辆速度键
        /// </summary>
        kKeyVehicleSpeed = 0x0010,

        /// <summary>
        /// 环境温度键
        /// </summary>
        kKeyEnvironmentTemp = 0x0011,

        /// <summary>
        /// 安装姿态键
        /// </summary>
        kKeyInstallAttitude = 0x0012,

        /// <summary>
        /// 盲区设置键
        /// </summary>
        kKeyBlindSpotSet = 0x0013,

        /// <summary>
        /// 帧率键
        /// </summary>
        kKeyFrameRate = 0x0014,

        /// <summary>
        /// 视场配置0键
        /// </summary>
        kKeyFovCfg0 = 0x0015,

        /// <summary>
        /// 视场配置1键
        /// </summary>
        kKeyFovCfg1 = 0x0016,

        /// <summary>
        /// 视场配置使能键
        /// </summary>
        kKeyFovCfgEn = 0x0017,

        /// <summary>
        /// 检测模式键
        /// </summary>
        kKeyDetectMode = 0x0018,

        /// <summary>
        /// 功能IO配置键
        /// </summary>
        kKeyFuncIoCfg = 0x0019,

        /// <summary>
        /// 启动后工作模式键
        /// </summary>
        kKeyWorkModeAfterBoot = 0x0020,

        /// <summary>
        /// 工作模式键
        /// </summary>
        kKeyWorkMode = 0x001A,

        /// <summary>
        /// 玻璃加热键
        /// </summary>
        kKeyGlassHeat = 0x001B,

        /// <summary>
        /// IMU数据使能键
        /// </summary>
        kKeyImuDataEn = 0x001C,

        /// <summary>
        /// FUSA使能键
        /// </summary>
        kKeyFusaEn = 0x001D,

        /// <summary>
        /// 强制加热使能键
        /// </summary>
        kKeyForceHeatEn = 0x001E,

        /// <summary>
        /// 日志参数设置键
        /// </summary>
        kKeyLogParamSet = 0x7FFF,

        /// <summary>
        /// 序列号键
        /// </summary>
        kKeySn = 0x8000,

        /// <summary>
        /// 产品信息键
        /// </summary>
        kKeyProductInfo = 0x8001,

        /// <summary>
        /// 应用版本键
        /// </summary>
        kKeyVersionApp = 0x8002,

        /// <summary>
        /// 加载器版本键
        /// </summary>
        kKeyVersionLoader = 0x8003,

        /// <summary>
        /// 硬件版本键
        /// </summary>
        kKeyVersionHardware = 0x8004,

        /// <summary>
        /// MAC地址键
        /// </summary>
        kKeyMac = 0x8005,

        /// <summary>
        /// 当前工作状态键
        /// </summary>
        kKeyCurWorkState = 0x8006,

        /// <summary>
        /// 核心温度键
        /// </summary>
        kKeyCoreTemp = 0x8007,

        /// <summary>
        /// 上电次数键
        /// </summary>
        kKeyPowerUpCnt = 0x8008,

        /// <summary>
        /// 当前本地时间键
        /// </summary>
        kKeyLocalTimeNow = 0x8009,

        /// <summary>
        /// 上次同步时间键
        /// </summary>
        kKeyLastSyncTime = 0x800A,

        /// <summary>
        /// 时间偏移键
        /// </summary>
        kKeyTimeOffset = 0x800B,

        /// <summary>
        /// 时间同步类型键
        /// </summary>
        kKeyTimeSyncType = 0x800C,

        /// <summary>
        /// 状态码键
        /// </summary>
        kKeyStatusCode = 0x800D,

        /// <summary>
        /// LiDAR诊断状态键
        /// </summary>
        kKeyLidarDiagStatus = 0x800E,

        /// <summary>
        /// LiDAR闪存状态键
        /// </summary>
        kKeyLidarFlashStatus = 0x800F,

        /// <summary>
        /// 固件类型键
        /// </summary>
        kKeyFwType = 0x8010,

        /// <summary>
        /// HMS代码键
        /// </summary>
        kKeyHmsCode = 0x8011,

        /// <summary>
        /// 当前玻璃加热状态键
        /// </summary>
        kKeyCurGlassHeatState = 0x8012,

        /// <summary>
        /// ROI模式键
        /// </summary>
        kKeyRoiMode = 0xFFFE,

        /// <summary>
        /// LiDAR诊断信息查询键
        /// </summary>
        kKeyLidarDiagInfoQuery = 0xFFFF,
    }

    // Point data type enum
    /// <summary>
    /// 点数据类型枚举
    /// </summary>
    public enum LivoxLidarPointDataType : byte
    {
        /// <summary>
        /// IMU数据类型
        /// </summary>
        kLivoxLidarImuData = 0,

        /// <summary>
        /// 高精度笛卡尔坐标数据类型
        /// </summary>
        kLivoxLidarCartesianCoordinateHighData = 0x01,

        /// <summary>
        /// 低精度笛卡尔坐标数据类型
        /// </summary>
        kLivoxLidarCartesianCoordinateLowData = 0x02,

        /// <summary>
        /// 球坐标数据类型
        /// </summary>
        kLivoxLidarSphericalCoordinateData = 0x03
    }

    // Log type enum
    /// <summary>
    /// 日志类型枚举
    /// </summary>
    public enum LivoxLidarLogType
    {
        /// <summary>
        /// 实时日志类型
        /// </summary>
        kLivoxLidarRealTimeLog = 0,

        /// <summary>
        /// 异常日志类型
        /// </summary>
        kLivoxLidarExceptionLog = 0x01
    }

    // Status enum
    /// <summary>
    /// 状态枚举
    /// </summary>
    public enum LivoxLidarStatus
    {
        /// <summary>
        /// 命令发送失败
        /// </summary>
        kLivoxLidarStatusSendFailed = -9,

        /// <summary>
        /// 处理程序实现不存在
        /// </summary>
        kLivoxLidarStatusHandlerImplNotExist = -8,

        /// <summary>
        /// 设备句柄无效
        /// </summary>
        kLivoxLidarStatusInvalidHandle = -7,

        /// <summary>
        /// 命令通道不存在
        /// </summary>
        kLivoxLidarStatusChannelNotExist = -6,

        /// <summary>
        /// 内存不足
        /// </summary>
        kLivoxLidarStatusNotEnoughMemory = -5,

        /// <summary>
        /// 操作超时
        /// </summary>
        kLivoxLidarStatusTimeout = -4,

        /// <summary>
        /// 设备不支持的操作
        /// </summary>
        kLivoxLidarStatusNotSupported = -3,

        /// <summary>
        /// 请求的设备未连接
        /// </summary>
        kLivoxLidarStatusNotConnected = -2,

        /// <summary>
        /// 失败
        /// </summary>
        kLivoxLidarStatusFailure = -1,

        /// <summary>
        /// 成功
        /// </summary>
        kLivoxLidarStatusSuccess = 0
    }

    // Scan pattern enum
    /// <summary>
    /// 扫描模式枚举
    /// </summary>
    public enum LivoxLidarScanPattern
    {
        /// <summary>
        /// 非重复扫描模式
        /// </summary>
        kLivoxLidarScanPatternNoneRepetive = 0x00,

        /// <summary>
        /// 重复扫描模式
        /// </summary>
        kLivoxLidarScanPatternRepetive = 0x01,

        /// <summary>
        /// 低帧率重复扫描模式
        /// </summary>
        kLivoxLidarScanPatternRepetiveLowFrameRate = 0x02
    }

    // Frame rate enum
    /// <summary>
    /// 帧率枚举
    /// </summary>
    public enum LivoxLidarPointFrameRate
    {
        /// <summary>
        /// 10Hz帧率
        /// </summary>
        kLivoxLidarFrameRate10Hz = 0x00,

        /// <summary>
        /// 15Hz帧率
        /// </summary>
        kLivoxLidarFrameRate15Hz = 0x01,

        /// <summary>
        /// 20Hz帧率
        /// </summary>
        kLivoxLidarFrameRate20Hz = 0x02,

        /// <summary>
        /// 25Hz帧率
        /// </summary>
        kLivoxLidarFrameRate25Hz = 0x03,
    }

    // Work mode enum
    /// <summary>
    /// 工作模式枚举
    /// </summary>
    public enum LivoxLidarWorkMode
    {
        /// <summary>
        /// 正常模式
        /// </summary>
        kLivoxLidarNormal = 0x01,

        /// <summary>
        /// 唤醒模式
        /// </summary>
        kLivoxLidarWakeUp = 0x02,

        /// <summary>
        /// 休眠模式
        /// </summary>
        kLivoxLidarSleep = 0x03,

        /// <summary>
        /// 错误模式
        /// </summary>
        kLivoxLidarError = 0x04,

        /// <summary>
        /// 上电自检模式
        /// </summary>
        kLivoxLidarPowerOnSelfTest = 0x05,

        /// <summary>
        /// 电机启动模式
        /// </summary>
        kLivoxLidarMotorStarting = 0x06,

        /// <summary>
        /// 电机停止模式
        /// </summary>
        kLivoxLidarMotorStoping = 0x07,

        /// <summary>
        /// 升级模式
        /// </summary>
        kLivoxLidarUpgrade = 0x08
    }

    // Work mode after boot enum
    /// <summary>
    /// 启动后的工作模式枚举
    /// </summary>
    public enum LivoxLidarWorkModeAfterBoot
    {
        /// <summary>
        /// 默认工作模式
        /// </summary>
        kLivoxLidarWorkModeAfterBootDefault = 0x00,

        /// <summary>
        /// 正常工作模式
        /// </summary>
        kLivoxLidarWorkModeAfterBootNormal = 0x01,

        /// <summary>
        /// 唤醒工作模式
        /// </summary>
        kLivoxLidarWorkModeAfterBootWakeUp = 0x02
    }

    // Detect mode enum
    /// <summary>
    /// 检测模式枚举
    /// </summary>
    public enum LivoxLidarDetectMode
    {
        /// <summary>
        /// 检测模式：正常
        /// </summary>
        kLivoxLidarDetectNormal = 0x00,

        /// <summary>
        /// 检测模式：敏感
        /// </summary>
        kLivoxLidarDetectSensitive = 0x01
    }

    // Glass heat enum
    /// <summary>
    /// 玻璃加热枚举
    /// </summary>
    public enum LivoxLidarGlassHeat
    {
        /// <summary>
        /// 停止加热或诊断加热
        /// </summary>
        kLivoxLidarStopPowerOnHeatingOrDiagnosticHeating = 0x00,

        /// <summary>
        /// 开启加热
        /// </summary>
        kLivoxLidarTurnOnHeating = 0x01,

        /// <summary>
        /// 诊断加热
        /// </summary>
        kLivoxLidarDiagnosticHeating = 0x02,

        /// <summary>
        /// 停止自加热
        /// </summary>
        kLivoxLidarStopSelfHeating = 0x03
    }

    // Upgrade related data struct
    /// <summary>
    /// LiDAR状态机状态枚举
    /// </summary>
    public enum LivoxLidarFsmState
    {
        /// <summary>
        /// 升级空闲状态
        /// </summary>
        kLivoxLidarUpgradeIdle = 0,

        /// <summary>
        /// 升级请求状态
        /// </summary>
        kLivoxLidarUpgradeRequest = 1,

        /// <summary>
        /// 传输固件状态
        /// </summary>
        kLivoxLidarUpgradeXferFirmware = 2,

        /// <summary>
        /// 完成固件传输状态
        /// </summary>
        kLivoxLidarUpgradeCompleteXferFirmware = 3,

        /// <summary>
        /// 获取升级进度状态
        /// </summary>
        kLivoxLidarUpgradeGetUpgradeProgress = 4,

        /// <summary>
        /// 升级完成状态
        /// </summary>
        kLivoxLidarUpgradeComplete = 5,

        /// <summary>
        /// 升级超时状态
        /// </summary>
        kLivoxLidarUpgradeTimeout = 6,

        /// <summary>
        /// 升级错误状态
        /// </summary>
        kLivoxLidarUpgradeErr = 7,

        /// <summary>
        /// 未定义状态
        /// </summary>
        kLivoxLidarUpgradeUndef = 8
    }

    /// <summary>
    /// LiDAR状态机事件枚举
    /// </summary>
    public enum LivoxLidarFsmEvent
    {
        /// <summary>
        /// 请求升级事件
        /// </summary>
        kLivoxLidarEventRequestUpgrade = 0,

        /// <summary>
        /// 传输固件事件
        /// </summary>
        kLivoxLidarEventXferFirmware = 1,

        /// <summary>
        /// 完成固件传输事件
        /// </summary>
        kLivoxLidarEventCompleteXferFirmware = 2,

        /// <summary>
        /// 获取升级进度事件
        /// </summary>
        kLivoxLidarEventGetUpgradeProgress = 3,

        /// <summary>
        /// 升级完成事件
        /// </summary>
        kLivoxLidarEventComplete = 4,

        /// <summary>
        /// 重新初始化事件
        /// </summary>
        kLivoxLidarEventReinit = 5,

        /// <summary>
        /// 超时事件
        /// </summary>
        kLivoxLidarEventTimeout = 6,

        /// <summary>
        /// 错误事件
        /// </summary>
        kLivoxLidarEventErr = 7,

        /// <summary>
        /// 未定义事件
        /// </summary>
        kLivoxLidarEventUndef = 8
    }
    #endregion
}
