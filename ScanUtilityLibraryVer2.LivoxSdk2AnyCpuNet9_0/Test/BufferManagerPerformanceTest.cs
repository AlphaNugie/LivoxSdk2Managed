using ScanUtilityLibraryVer2.LivoxSdk2.Core;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ScanUtilityLibraryVer2.LivoxSdk2.Test
{
    /// <summary>
    /// BufferManager 压力测试类
    /// 模拟高并发场景下的生产者-消费者模型，验证双缓冲区的性能和稳定性
    /// <para/>使用方法：在程序入口调用 BufferManagerPerformanceTest.RunTest()
    /// </summary>
    public class BufferManagerPerformanceTest
    {
        // 测试持续时间（秒）
        private const int TestDurationSeconds = 60;

        // 线程安全的随机数生成器（解决多线程竞争问题）
        private static readonly Random _random = new();

        /// <summary>
        /// 模拟LiDAR点数据结构体
        /// 包含三维坐标、反射率和标签字段（模拟真实点云数据结构）
        /// </summary>
        public struct MockPoint
        {
            /// <summary>
            /// X轴坐标
            /// </summary>
            public int X;

            /// <summary>
            /// Y轴坐标
            /// </summary>
            public int Y;

            /// <summary>
            /// Z轴坐标
            /// </summary>
            public int Z;

            /// <summary>
            /// 反射强度（0-255）
            /// </summary>
            public byte Reflectivity;

            /// <summary>
            /// 点标签（分类标识）
            /// </summary>
            public byte Tag;
        }

        /// <summary>
        /// 生成随机点数据（线程安全）
        /// </summary>
        /// <returns>随机生成的MockPoint结构体</returns>
        private static MockPoint GenerateRandomPoint()
        {
            return new MockPoint
            {
                X = _random.Next(-10000, 10000),
                Y = _random.Next(-10000, 10000),
                Z = _random.Next(-10000, 10000),
                Reflectivity = (byte)_random.Next(0, 256), // 生成0-255的反射率
                Tag = (byte)_random.Next(0, 16)           // 生成0-15的分类标签
            };
        }

        /// <summary>
        /// 运行性能测试并输出实时统计
        /// 测试流程：
        /// 1. 初始化双缓冲区
        /// 2. 启动生产者线程（模拟雷达数据输入）
        /// 3. 启动消费者线程（模拟数据处理）
        /// 4. 启动监控线程（实时显示统计数据）
        /// 5. 运行指定时长后停止测试
        /// 6. 输出最终测试报告
        /// </summary>
        public static void RunTest()
        {
            // 初始化双缓冲区（容量384,000点，对应192,000点/500ms × 2的规格）
            var buffer = new BufferManager<MockPoint>(384000);

            // 取消令牌源（用于协调线程终止）
            var cts = new CancellationTokenSource();

            // 高精度计时器
            var stopwatch = new Stopwatch();

            // 测试统计数据
            long totalWritten = 0;     // 总写入点数
            long totalRead = 0;        // 总读取点数
            int dataLossCount = 0;     // 数据丢失次数计数器（使用原子操作保证线程安全）

            // 生产者任务（模拟雷达数据输入）
            var producerTask = Task.Run(() =>
            {
                stopwatch.Start(); // 在生产者线程启动计时器（避免主线程计时误差）

                while (!cts.Token.IsCancellationRequested)
                {
                    // 生成模拟数据批次（每批2000个点，模拟典型雷达数据包）
                    var points = new MockPoint[2000];
                    for (int i = 0; i < points.Length; i++)
                    {
                        points[i] = GenerateRandomPoint();
                    }

                    // 尝试写入缓冲区（失败时记录数据丢失）
                    if (!buffer.TryWrite(points))
                    {
                        Interlocked.Increment(ref dataLossCount); // 原子递增操作
                    }

                    // 累加总写入点数（原子操作保证线程安全）
                    Interlocked.Add(ref totalWritten, points.Length);
                }
            });

            // 消费者任务（模拟数据处理流程）
            var consumerTask = Task.Run(() =>
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    // 尝试读取可用数据
                    if (buffer.TryRead(out var data, out var count))
                    {
                        // 累加总读取点数（原子操作保证线程安全）
                        Interlocked.Add(ref totalRead, count);
                    }
                    Thread.Sleep(15); // 模拟66Hz刷新率（约15ms/帧）
                }
            });

            // 实时监控任务（控制台输出统计信息）
            var monitorTask = Task.Run(() =>
            {
                // 绘制控制台表格头部
                Console.WriteLine("┌────────────────────┬──────────────┬──────────────┬────────────┐");
                Console.WriteLine("│ 耗时(s) │ 写入速率(k/s) │ 读取速率(k/s) │ 缓冲区状态 │");
                Console.WriteLine("├────────────────────┼──────────────┼──────────────┼────────────┤");

                while (!cts.Token.IsCancellationRequested)
                {
                    // 计算实时性能指标
                    var elapsed = stopwatch.Elapsed.TotalSeconds;
                    var writeRate = totalWritten / elapsed / 1000;  // 千点/秒
                    var readRate = totalRead / elapsed / 1000;      // 千点/秒

                    // 格式化输出性能数据
                    Console.WriteLine($"│ {elapsed,8:F2} │ {writeRate,12:F1} │ {readRate,12:F1} │ " +
                                    $"W:{buffer.WriteBufferUsage:P0} R:{buffer.ReadBufferRemaining:P0} │");

                    Thread.Sleep(1000); // 每秒更新显示
                }
            });

            // 主线程等待测试时长
            Thread.Sleep(TestDurationSeconds * 1000);

            // 发出终止信号
            cts.Cancel();

            // 等待所有任务完成
            Task.WaitAll(producerTask, consumerTask, monitorTask);
            stopwatch.Stop();

            // 输出最终测试报告
            Console.WriteLine("└────────────────────┴──────────────┴──────────────┴────────────┘");
            Console.WriteLine("\n测试结果汇总：");
            Console.WriteLine($"- 总运行时间: {stopwatch.Elapsed.TotalSeconds:F2}秒");
            Console.WriteLine($"- 总写入点数: {totalWritten:N0}");
            Console.WriteLine($"- 总读取点数: {totalRead:N0}");
            Console.WriteLine($"- 数据丢失次数: {dataLossCount}");
            Console.WriteLine($"- 平均写入速率: {totalWritten / stopwatch.Elapsed.TotalSeconds / 1000:F1}千点/秒");
            Console.WriteLine($"- 平均读取速率: {totalRead / stopwatch.Elapsed.TotalSeconds / 1000:F1}千点/秒");
        }
    }

    // 使用方法：
    // 在程序入口调用 BufferManagerPerformanceTest.RunTest();
}
