// See https://aka.ms/new-console-template for more information
using ScanUtilityLibraryVer2.LivoxSdk2;
using ScanUtilityLibraryVer2.LivoxSdk2.Model;
using ScanUtilityLibraryVer2.LivoxSdk2.Samples;
using ScanUtilityLibraryVer2.LivoxSdk2.Test;
using System.Diagnostics;

Console.WriteLine("Hello, World!");
////双缓冲区压力测试
//BufferManagerPerformanceTest.RunTest();

DllLoader.ConfigureDllPath();
LivoxLidarQuickStart.FrameTime = 500;
//转换到现有码头坐标系：走行增大的方向为前方，X轴正向前，Y轴正向左，Z轴正向上
CoordTransParamSet coordTransParamSet = new(-90, 45, -90);
LivoxLidarQuickStart.Start("hap_config.json", coordTransParamSet);
Stopwatch stopwatch = new();
stopwatch.Start();
while (stopwatch.ElapsedMilliseconds <= 300000)
{
    Thread.Sleep(10000);
    //LivoxLidarQuickStart.BufferManagerHighRawPoints.TryRead(out var highRawPointsArray, out var size);
    var rawPointsCopy = LivoxLidarQuickStart.GetCurrentFrameOfHighRawPoints();
    //var rawPointsCopy = LivoxLidarQuickStart.CartesianHighRawPoints.ToList();

    //List<PlyDotObject> plyDots = rawPointsCopy
    //    .Where(rawPoint => rawPoint.x != 0 || rawPoint.y != 0 || rawPoint.z != 0)
    //    .Select(rawPoint => new PlyDotObject(rawPoint.x, rawPoint.y, rawPoint.z, colorSmoother.GetColor(rawPoint.reflectivity))
    //    {
    //        CustomProperties = new List<object> { rawPoint.reflectivity }
    //    })
    //    .ToList();
    //plyFile.SaveVertexes(plyDots);
}
//Thread.Sleep(300000); // 保持程序运行
LivoxLidarQuickStart.Stop();
