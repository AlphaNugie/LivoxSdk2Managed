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
        /// 横滚角，绕X轴旋转的角度（单位：度），向前看时顺时针旋转为正
        /// </summary>
        public double Roll { get; set; }

        /// <summary>
        /// 俯仰角，绕Y轴旋转的角度（单位：度），向下转为正
        /// </summary>
        public double Pitch { get; set; }

        /// <summary>
        /// 偏航角，绕Z轴旋转的角度（单位：度），俯视时向左为正
        /// </summary>
        public double Yaw { get; set; }

        /// <summary>
        /// X坐标偏移量（单位：毫米）
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y坐标偏移量（单位：毫米）
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Z坐标偏移量（单位：毫米）
        /// </summary>
        public double Z { get; set; }

        /// <summary>
        /// 默认构造函数，三轴角度和坐标偏移量初始化为0
        /// </summary>
        public CoordTransParamSet() { }

        /// <summary>
        /// 使用给定的三轴角度和坐标偏移量构造函数
        /// </summary>
        /// <param name="roll">横滚角，绕X轴旋转的角度（单位：度），绕X轴旋转的角度（单位：度），向前看时顺时针旋转为正</param>
        /// <param name="pitch">俯仰角，绕Y轴旋转的角度（单位：度），绕Y轴旋转的角度（单位：度），向下转为正</param>
        /// <param name="yaw">偏航角，绕Z轴旋转的角度（单位：度），绕Z轴旋转的角度（单位：度），俯视时向左为正</param>
        /// <param name="x">X坐标偏移量（单位：毫米）</param>
        /// <param name="y">Y坐标偏移量（单位：毫米）</param>
        /// <param name="z">Z坐标偏移量（单位：毫米）</param>
        public CoordTransParamSet(double roll, double pitch, double yaw, double x = 0, double y = 0, double z = 0)
        {
            Roll = roll;
            Pitch = pitch;
            Yaw = yaw;
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// 更新横滚角
        /// </summary>
        /// <param name="roll">设备的横滚角，绕X轴旋转的角度（单位：度），向前看时顺时针旋转为正</param>
        public void UpdateRoll(double roll)
        {
            Roll = roll;
        }

        /// <summary>
        /// 更新俯仰角
        /// </summary>
        /// <param name="pitch">设备的俯仰角，绕Y轴旋转的角度（单位：度），向下转为正</param>
        /// <param name="machinePitch">单机的俯仰角，向上转为正</param>
        public void UpdatePitch(double pitch, double machinePitch = 0)
        {
            Pitch = pitch - machinePitch;
        }

        /// <summary>
        /// 更新回转角
        /// </summary>
        /// <param name="yaw">设备的回转角，绕Z轴旋转的角度（单位：度），俯视时向左为正</param>
        /// <param name="machineYaw">单机的回转角，向右转为正</param>
        public void UpdateYaw(double yaw, double machineYaw = 0)
        {
            Yaw = yaw - machineYaw;
        }

        /// <summary>
        /// 更新X坐标偏移量
        /// </summary>
        /// <param name="x"></param>
        public void UpdateXOffset(double x) { X = x; }

        /// <summary>
        /// 更新Y坐标偏移量
        /// </summary>
        /// <param name="y"></param>
        public void UpdateYOffset(double y) { Y = y; }

        /// <summary>
        /// 更新Z坐标偏移量
        /// </summary>
        /// <param name="z"></param>
        public void UpdateZOffset(double z) { Z = z; }
    }
}
