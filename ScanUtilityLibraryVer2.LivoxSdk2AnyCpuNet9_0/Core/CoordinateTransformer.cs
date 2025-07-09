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
        /// <param name="pointCloud">高精度坐标点列表，坐标单位为毫米</param>
        /// <param name="paramSet">空间旋转位移参数集，包含Roll、Pitch、Yaw、X、Y、Z，前三者单位为度、后三者单位为毫米</param>
        public static LivoxLidarCartesianHighRawPoint[] TransformPoints(this IEnumerable<LivoxLidarCartesianHighRawPoint> pointCloud, CoordTransParamSet paramSet)
        {
#if NET9_0_OR_GREATER
            if (pointCloud == null)
                return [];

            return [.. pointCloud.Select(p =>
            {
                TransformPoint(ref p, paramSet);
                return p;
            })];
#elif NET45
            if (pointCloud == null)
                return new LivoxLidarCartesianHighRawPoint[0];

            return pointCloud.Select(p =>
            {
                TransformPoint(ref p, paramSet);
                return p;
            }).ToArray();
#endif
        }

        /// <summary>
        /// 将点云坐标系中的多个笛卡尔坐标系低精度点批量变换到现实空间坐标系，并更新点云坐标
        /// </summary>
        /// <param name="pointCloud">低精度坐标点列表，坐标单位为厘米（10毫米）</param>
        /// <param name="paramSet">空间旋转位移参数集，包含Roll、Pitch、Yaw、X、Y、Z，前三者单位为度、后三者单位为毫米</param>
        public static LivoxLidarCartesianLowRawPoint[] TransformPoints(this IEnumerable<LivoxLidarCartesianLowRawPoint> pointCloud, CoordTransParamSet paramSet)
        {
#if NET9_0_OR_GREATER
            if (pointCloud == null)
                return [];

            return [.. pointCloud.Select(p =>
            {
                TransformPoint(ref p, paramSet);
                return p;
            })];
#elif NET45
            if (pointCloud == null)
                return new LivoxLidarCartesianLowRawPoint[0];

            return pointCloud.Select(p =>
            {
                TransformPoint(ref p, paramSet);
                return p;
            }).ToArray();
#endif
        }

        /// <summary>
        /// 将点云坐标系中的点变换到现实空间坐标系，并更新点云坐标
        /// </summary>
        /// <param name="highRawPoint">笛卡尔坐标系高精度点对象，坐标单位为毫米</param>
        /// <param name="paramSet">空间旋转位移参数集，包含Roll、Pitch、Yaw、X、Y、Z，前三者单位为度、后三者单位为毫米</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void TransformPoint(/*this */ref LivoxLidarCartesianHighRawPoint highRawPoint, CoordTransParamSet paramSet)
        {
            if (paramSet == null)
                throw new ArgumentNullException(nameof(paramSet), "空间旋转位移参数不能为空");
            var coord = TransformPoint(highRawPoint.x, highRawPoint.y, highRawPoint.z, paramSet);
            highRawPoint.x = (int)coord[0];
            highRawPoint.y = (int)coord[1];
            highRawPoint.z = (int)coord[2];
        }

        /// <summary>
        /// 将点云坐标系中的笛卡尔坐标系低精度点变换到现实空间坐标系，并更新点云坐标
        /// </summary>
        /// <param name="lowRawPoint">笛卡尔坐标系低精度点对象，坐标单位为厘米（10毫米）</param>
        /// <param name="paramSet">空间旋转位移参数集，包含Roll、Pitch、Yaw、X、Y、Z，前三者单位为度、后三者单位为毫米</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void TransformPoint(/*this */ref LivoxLidarCartesianLowRawPoint lowRawPoint, CoordTransParamSet paramSet)
        {
            if (paramSet == null)
                throw new ArgumentNullException(nameof(paramSet), "空间旋转位移参数不能为空");
            // 低精度点坐标单位为厘米，转换前需要乘以10以转化为毫米
            var coord = TransformPoint(lowRawPoint.x * 10, lowRawPoint.y * 10, lowRawPoint.z * 10, paramSet);
            // 转换结果单位为毫米，转换后需要将转换结果除以10以转化为厘米
            lowRawPoint.x = (short)(coord[0] / 10);
            lowRawPoint.y = (short)(coord[1] / 10);
            lowRawPoint.z = (short)(coord[2] / 10);
        }

        /// <summary>
        /// 将点云坐标系中的点变换到现实空间坐标系
        /// </summary>
        /// <param name="x">点云坐标系X坐标（单位：毫米）</param>
        /// <param name="y">点云坐标系Y坐标（单位：毫米）</param>
        /// <param name="z">点云坐标系Z坐标（单位：毫米）</param>
        /// <param name="paramSet">储存空间旋转位移参数的集合</param>
        /// <returns>变换后的现实空间坐标（单位：毫米），以数组方式返回，顺序为X、Y、Z</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static double[] TransformPoint(double x, double y, double z, CoordTransParamSet paramSet)
        {
            if (paramSet == null)
                throw new ArgumentNullException(nameof(paramSet), "空间旋转位移参数不能为空");
            return TransformPoint(x, y, z, paramSet.Roll, paramSet.Pitch, paramSet.Yaw, paramSet.X, paramSet.Y, paramSet.Z);
        }

        /// <summary>
        /// 将点云坐标系中的点变换到现实空间坐标系
        /// <para/>横滚角Roll绕X轴，俯仰角Pitch绕Y轴，回转角Yaw绕Z轴，每次都绕空间中的固定轴旋转（绕动轴旋转需修改变换矩阵相乘的顺序由Z→Y→X变为X→Y→Z）
        /// <para/>三轴的正旋转方向均符合右手法则，即绕此轴旋转时，使右手大拇指伸直指向此轴正向、其余四指握成拳状，则其余四指所指的方向则为正向
        /// </summary>
        /// <param name="x">点云坐标系X坐标（单位：毫米）</param>
        /// <param name="y">点云坐标系Y坐标（单位：毫米）</param>
        /// <param name="z">点云坐标系Z坐标（单位：毫米）</param>
        /// <param name="rollDeg">绕X旋转的横滚角（度）</param>
        /// <param name="pitchDeg">绕Y旋转的俯仰角（度）</param>
        /// <param name="yawDeg">绕Z旋转的偏航角（度）</param>
        /// <param name="xoffset">设备固定的X方向平移量（单位：毫米）</param>
        /// <param name="yoffset">设备固定的Y方向平移量（单位：毫米）</param>
        /// <param name="zoffset">设备固定的Z方向平移量（单位：毫米）</param>
        /// <returns>变换后的现实空间坐标（单位：毫米），以数组方式返回，顺序为X、Y、Z</returns>
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
#if NET9_0_OR_GREATER
            double[] originalPoint = [x, y, z];
#elif NET45
            double[] originalPoint = { x, y, z };
#endif
            double[] rotatedPoint = MathUtils.MultiplyMatrixVector(totalRotation, originalPoint);

            return
#if NET9_0_OR_GREATER
            [
                rotatedPoint[0] + xoffset,  // 变换后X坐标
                rotatedPoint[1] + yoffset,  // 变换后Y坐标
                rotatedPoint[2] + zoffset   // 变换后Z坐标
            ];
#elif NET45
                new double[]
                {
                    rotatedPoint[0] + xoffset,  // 变换后X坐标
                    rotatedPoint[1] + yoffset,  // 变换后Y坐标
                    rotatedPoint[2] + zoffset   // 变换后Z坐标
                };
#endif
        }
    }
}
