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
}