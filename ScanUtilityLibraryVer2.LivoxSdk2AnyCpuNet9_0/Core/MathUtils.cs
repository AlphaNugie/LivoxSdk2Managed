using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanUtilityLibraryVer2.LivoxSdk2.Core
{
    /// <summary>
    /// 数学计算工具类，包含矩阵运算和角度转换等方法
    /// </summary>
    public static class MathUtils
    {
        /// <summary>
        /// 将角度值转换为弧度值
        /// </summary>
        /// <param name="degree">角度值（0-360度）</param>
        /// <returns>对应的弧度值</returns>
        /// <example>180度 → π ≈ 3.14159</example>
        public static double DegreeToRadian(this double degree)
        {
            return degree * Math.PI / 180.0;
        }

        /// <summary>
        /// 创建绕X轴的旋转矩阵（右手坐标系，动轴旋转）
        /// </summary>
        /// <param name="angle">旋转弧度值</param>
        /// <returns>3x3旋转矩阵</returns>
        /// <remarks>
        /// 矩阵形式：
        /// [1  0      0     ]
        /// [0  cosθ  -sinθ ]
        /// [0  sinθ   cosθ ]
        /// </remarks>
        public static double[,] CreateRotationX(this double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            return new double[,]
            {
            { 1, 0,    0   },
            { 0, cos, -sin },
            { 0, sin,  cos }
            };
        }

        /// <summary>
        /// 创建绕Y轴的旋转矩阵（右手坐标系，动轴旋转）
        /// </summary>
        /// <param name="angle">旋转弧度值</param>
        /// <returns>3x3旋转矩阵</returns>
        /// <remarks>
        /// 矩阵形式：
        /// [cosθ  0  sinθ ]
        /// [0     1  0    ]
        /// [-sinθ 0  cosθ]
        /// </remarks>
        public static double[,] CreateRotationY(this double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            return new double[,]
            {
            { cos, 0, sin },
            { 0,   1, 0   },
            { -sin, 0, cos }
            };
        }

        /// <summary>
        /// 创建绕Z轴的旋转矩阵（右手坐标系，动轴旋转）
        /// </summary>
        /// <param name="angle">旋转弧度值</param>
        /// <returns>3x3旋转矩阵</returns>
        /// <remarks>
        /// 矩阵形式：
        /// [cosθ  -sinθ  0]
        /// [sinθ   cosθ  0]
        /// [0       0    1]
        /// </remarks>
        public static double[,] CreateRotationZ(this double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            return new double[,]
            {
            { cos, -sin, 0 },
            { sin,  cos, 0 },
            { 0,    0,   1 }
            };
        }

        /// <summary>
        /// 通用矩阵乘法（支持任意维度，只要符合矩阵相乘条件）
        /// </summary>
        /// <param name="a">左侧矩阵（m×n）</param>
        /// <param name="b">右侧矩阵（n×p）</param>
        /// <returns>相乘结果矩阵（m×p）</returns>
        /// <exception cref="ArgumentException">当矩阵维度不匹配时抛出异常</exception>
        public static double[,] MultiplyMatrices(double[,] a, double[,] b)
        {
            int aRows = a.GetLength(0);
            int aCols = a.GetLength(1);
            int bCols = b.GetLength(1);

            // 验证矩阵维度是否匹配
            if (aCols != b.GetLength(0))
                throw new ArgumentException("矩阵A的列数必须等于矩阵B的行数");

            double[,] result = new double[aRows, bCols];

            for (int i = 0; i < aRows; i++)
            {
                for (int j = 0; j < bCols; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < aCols; k++)
                        sum += a[i, k] * b[k, j]; // 行×列累加
                    result[i, j] = sum;
                }
            }
            return result;
        }

        /// <summary>
        /// 将3x3矩阵与3D向量相乘
        /// </summary>
        /// <param name="m">3x3变换矩阵</param>
        /// <param name="v">三维向量[x,y,z]</param>
        /// <returns>变换后的三维向量</returns>
        /// <exception cref="ArgumentException">当矩阵维度不是3x3时抛出</exception>
        public static double[] MultiplyMatrixVector(double[,] m, double[] v)
        {
            // 验证输入维度
            if (m.GetLength(0) != 3 || m.GetLength(1) != 3)
                throw new ArgumentException("矩阵必须是3x3维度");
            if (v.Length != 3)
                throw new ArgumentException("向量必须是三维");

            double[] result = new double[3];
            for (int i = 0; i < 3; i++)
                // 矩阵第i行与向量点积
                result[i] = m[i, 0] * v[0] + m[i, 1] * v[1] + m[i, 2] * v[2];
            return result;
        }
    }
}
