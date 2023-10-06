
global using i32 = System.Int32;
global using v3 = System.Numerics.Vector3;
global using q4 = System.Numerics.Quaternion;
global using f32 = System.Single;

using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

public class Program
{
    public static unsafe void Main()
    {
        var summary = BenchmarkRunner.Run<Test2>();
        NativeInvoke.Dispose();
    }
}