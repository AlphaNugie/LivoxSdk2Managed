using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanUtilityLibraryVer2.LivoxSdk2.Model
{
    /// <summary>
    /// 空间旋转与位移的参数集
    /// </summary>
    public class CoordTransParamSet
    {
        /// <summary>
        /// 横滚角，绕X轴旋转的角度（单位：度）
        /// </summary>
        public double Roll { get; set; }

        /// <summary>
        /// 俯仰角，绕Y轴旋转的角度（单位：度）
        /// </summary>
        public double Pitch { get; set; }

        /// <summary>
        /// 偏航角，绕Z轴旋转的角度（单位：度）
        /// </summary>
        public double Yaw { get; set; }

        /// <summary>
        /// X坐标偏移量（单位：自定）
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y坐标偏移量（单位：自定）
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Z坐标偏移量（单位：自定）
        /// </summary>
        public double Z { get; set; }

        /// <summary>
        /// 默认构造函数，三轴角度和坐标偏移量初始化为0
        /// </summary>
        public CoordTransParamSet() { }

        /// <summary>
        /// 使用给定的三轴角度和坐标偏移量构造函数
        /// </summary>
        /// <param name="roll">横滚角，绕X轴旋转的角度（单位：度）</param>
        /// <param name="pitch">俯仰角，绕Y轴旋转的角度（单位：度）</param>
        /// <param name="yaw">偏航角，绕Z轴旋转的角度（单位：度）</param>
        /// <param name="x">X坐标偏移量（单位：自定）</param>
        /// <param name="y">Y坐标偏移量（单位：自定）</param>
        /// <param name="z">Z坐标偏移量（单位：自定）</param>
        public CoordTransParamSet(double roll, double pitch, double yaw, double x = 0, double y = 0, double z = 0)
        {
            Roll = roll;
            Pitch = pitch;
            Yaw = yaw;
            X = x;
            Y = y;
            Z = z;
        }
    }
}
