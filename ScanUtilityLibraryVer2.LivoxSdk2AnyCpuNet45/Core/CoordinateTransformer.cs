using ScanUtilityLibraryVer2.LivoxSdk2.Include;
using ScanUtilityLibraryVer2.LivoxSdk2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanUtilityLibraryVer2.LivoxSdk2.Core
{
    /// <summary>
    /// 坐标变换工具类，用于处理Livox HAP激光雷达的旋转和平移变换
    /// </summary>
    public static class CoordinateTransformer
    {
        /// <summary>
        /// 将点云坐标系中的多个点批量变换到现实空间坐标系，并更新点云坐标
        /// </summary>
        /// <param name="pointCloud"></param>
        /// <param name="paramSet"></param>
        public static LivoxLidarCartesianHighRawPoint[] TransformPoints(this IEnumerable<LivoxLidarCartesianHighRawPoint> pointCloud, CoordTransParamSet paramSet)
        {
            if (pointCloud == null)
                return new LivoxLidarCartesianHighRawPoint[0];

            var list = pointCloud.ToList();
            return pointCloud.Select(p =>
            {
                TransformPoint(ref p, paramSet);
                return p;
            }).ToArray();
        }

        /// <summary>
        /// 将点云坐标系中的点变换到现实空间坐标系，并更新点云坐标
        /// </summary>
        /// <param name="highRawPoint"></param>
        /// <param name="paramSet"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void TransformPoint(/*this */ref LivoxLidarCartesianHighRawPoint highRawPoint, CoordTransParamSet paramSet)
        {
            if (paramSet == null)
                throw new ArgumentNullException("paramSet", "空间旋转位移参数不能为空");
            var coord = TransformPoint(highRawPoint.x, highRawPoint.y, highRawPoint.z, paramSet);
            highRawPoint.x = (int)coord[0];
            highRawPoint.y = (int)coord[1];
            highRawPoint.z = (int)coord[2];
        }

        /// <summary>
        /// 将点云坐标系中的点变换到现实空间坐标系
        /// </summary>
        /// <param name="x">点云坐标系X坐标（单位自定）</param>
        /// <param name="y">点云坐标系Y坐标（单位自定）</param>
        /// <param name="z">点云坐标系Z坐标（单位自定）</param>
        /// <param name="paramSet">储存空间旋转位移参数的集合</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static double[] TransformPoint(double x, double y, double z, CoordTransParamSet paramSet)
        {
            if (paramSet == null)
                throw new ArgumentNullException("paramSet", "空间旋转位移参数不能为空");
            return TransformPoint(x, y, z, paramSet.Roll, paramSet.Pitch, paramSet.Yaw, paramSet.X, paramSet.Y, paramSet.Z);
        }

        /// <summary>
        /// 将点云坐标系中的点变换到现实空间坐标系
        /// <para/>横滚角Roll绕X轴，俯仰角Pitch绕Y轴，回转角Yaw绕Z轴，每次都绕空间中的固定轴旋转（绕动轴旋转需修改变换矩阵相乘的顺序由Z→Y→X变为X→Y→Z）
        /// <para/>三轴的正旋转方向均符合右手法则，即绕此轴旋转时，使右手大拇指伸直指向此轴正向、其余四指握成拳状，则其余四指所指的方向则为正向
        /// </summary>
        /// <param name="x">点云坐标系X坐标（单位自定）</param>
        /// <param name="y">点云坐标系Y坐标（单位自定）</param>
        /// <param name="z">点云坐标系Z坐标（单位自定）</param>
        /// <param name="rollDeg">绕X旋转的横滚角（度）</param>
        /// <param name="pitchDeg">绕Y旋转的俯仰角（度）</param>
        /// <param name="yawDeg">绕Z旋转的偏航角（度）</param>
        /// <param name="xoffset">设备固定的X方向平移量（单位自定）</param>
        /// <param name="yoffset">设备固定的Y方向平移量（单位自定）</param>
        /// <param name="zoffset">设备固定的Z方向平移量（单位自定）</param>
        /// <returns>变换后的现实空间坐标（单位自定）</returns>
        /// <remarks>
        /// 变换顺序：先绕X轴旋转(Roll)，再绕Y轴旋转(Pitch)，最后绕Z轴旋转(Yaw)，最后应用平移
        /// </remarks>
        public static double[] TransformPoint(double x, double y, double z,
                                            double rollDeg, double pitchDeg, double yawDeg, double xoffset, double yoffset, double zoffset)
        {
            // 角度转换为弧度（三角函数计算需要弧度值）
            double roll = rollDeg.DegreeToRadian();
            double pitch = pitchDeg.DegreeToRadian();
            double yaw = yawDeg.DegreeToRadian();

            // 生成绕各轴的旋转矩阵，旋转正向符合右手法则
            double[,] rx = roll.CreateRotationX();   // 绕X旋转矩阵
            double[,] ry = pitch.CreateRotationY();  // 绕Y旋转矩阵
            double[,] rz = yaw.CreateRotationZ();    // 绕Z旋转矩阵

            //// 绕动轴的组合旋转矩阵：R_total = Rx * Ry * Rz（矩阵乘法顺序对应旋转顺序）
            //double[,] totalRotation = MultiplyMatrices(MultiplyMatrices(rx, ry), rz);
            // 绕固定轴的组合旋转矩阵：R_total = Rz * Ry * Rz（矩阵乘法顺序对应旋转顺序）
            double[,] totalRotation = MathUtils.MultiplyMatrices(rz, MathUtils.MultiplyMatrices(ry, rx));

            // 应用旋转到原始点坐标
            double[] originalPoint = { x, y, z };
            double[] rotatedPoint = MathUtils.MultiplyMatrixVector(totalRotation, originalPoint);

            return new double[]
            {
                rotatedPoint[0] + xoffset,  // 变换后X坐标
                rotatedPoint[1] + yoffset,  // 变换后Y坐标
                rotatedPoint[2] + zoffset   // 变换后Z坐标
            };
        }
    }
}
