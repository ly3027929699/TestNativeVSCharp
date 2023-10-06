using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

[HideColumns("Error", "StdDev", "Median", "RatioSD")]
[SimpleJob(RuntimeMoniker.Net80)]
public unsafe class Test2
{
    private RectangleF constantInfos;
    private RectangleF renderInfos;
    private int** len_pp;
    private int textLength;
    private nint vertexPtrTemp;
    private VertexPositionNormalTexture** vertexBufferPointer;
    private string text;
    
    private nint lengthPtr;
    private nint vertexPosition11;
    private VertexPositionNormalTexture[] array1;

    [Params(100, 500, 1000)] public int num;
    public Test2()
    {
        
        this.constantInfos = new RectangleF(8, 16, 256, 64);
        this.renderInfos = new RectangleF(1, 1, 1920, 1080);
        this.text = "AAa地方aaaa%^*(0_$%^aaaaA";
        this.textLength = this.text.Length;
        var asPointer = (nint)Unsafe.AsPointer(ref this.textLength);
        this.lengthPtr = asPointer;
        var textPtrLength = 4*this.textLength;
        this.array1 = new VertexPositionNormalTexture[textPtrLength];
        var vertexPosition1 = (VertexPositionNormalTexture*)Unsafe.AsPointer(ref this.array1[0]);
        this.vertexPosition11 = (nint)vertexPosition1;
    }

    [Benchmark]
    public void NativeMethod()
    {
        var lengthPtr = this.lengthPtr;
        var vertexPosition11 = this.vertexPosition11;
        for (var i = 0; i < num; i++)
        {
             lengthPtr = this.lengthPtr;
             vertexPosition11 = this.vertexPosition11;
            NativeInvoke.xnGraphicsFastTextRendererGenerateVertices(constantInfos, renderInfos, text, out lengthPtr, out vertexPosition11);
        }
    }

    [Benchmark]
    public void CSharpMethod()
    {
        var lengthPtr = (int*)this.lengthPtr;
        var vertexPosition11 = (VertexPositionNormalTexture*)this.vertexPosition11;
        for (var i = 0; i < this.num; i++)
        {
            lengthPtr = (int*)this.lengthPtr;
            vertexPosition11 = (VertexPositionNormalTexture*)this.vertexPosition11;
            CSharp.xnGraphicsFastTextRendererGenerateVertices(constantInfos, renderInfos, text, ref lengthPtr,  ref vertexPosition11);
        }
    }
}

static class CSharp
{
    private static VertexPositionNormalTexture[] BaseVertexBufferData = new VertexPositionNormalTexture[4]
    {
        new VertexPositionNormalTexture() { Position = new v3(-1, 1, 0), Normal = new v3(0, 0, 1), TextureCoordinate = new Vector2(0, 0) },
        new VertexPositionNormalTexture() { Position = new v3(1, 1, 0), Normal = new v3(0, 0, 1), TextureCoordinate = new Vector2(1, 0) },
        new VertexPositionNormalTexture() { Position = new v3(-1, -1, 0), Normal = new v3(0, 0, 1), TextureCoordinate = new Vector2(0, 1) },
        new VertexPositionNormalTexture() { Position = new v3(1, -1, 0), Normal = new v3(0, 0, 1), TextureCoordinate = new Vector2(1, 1) },
    };

    public static unsafe void xnGraphicsFastTextRendererGenerateVertices(RectangleF constantInfos, RectangleF renderInfos, string textPointer, ref int* textLength, ref VertexPositionNormalTexture* vertexBufferPointer)
    {
        float fX = renderInfos.X / renderInfos.Width;
        float fY = renderInfos.Y / renderInfos.Height;
        float fW = constantInfos.X / renderInfos.Width;
        float fH = constantInfos.Y / renderInfos.Height;

        RectangleF destination = new RectangleF(fX, fY, fW, fH);
        RectangleF source = new RectangleF(0.0f, 0.0f, constantInfos.X, constantInfos.Y);

        // Copy the array length (since it may change during an iteration)
        int textCharCount = *textLength;

        float scaledDestinationX = 0.0f;
        float scaledDestinationY = -(destination.Y * 2.0f - 1.0f);

        float invertedWidth = 1.0f / constantInfos.Width;
        float invertedHeight = 1.0f / constantInfos.Height;

        for (int i = 0; i < textCharCount; i++)
        {
            char currentChar = textPointer[i];

            if (currentChar == 11)
            {
                // Tabulation
                destination.X += 8 * fX;
                --*textLength;
                continue;
            }
            else if (currentChar >= 10 && currentChar <= 13)
            {
                // New Line
                destination.X = fX;
                destination.Y += fH;
                scaledDestinationY = -(destination.Y * 2.0f - 1.0f);
                --*textLength;
                continue;
            }
            else if (currentChar < 32 || currentChar > 126)
            {
                currentChar = (char)32;
            }

            source.X = ((float)(currentChar % 32)) * constantInfos.X;
            source.Y = ((float)((currentChar / 32) % 4)) * constantInfos.Y;

            scaledDestinationX = (destination.X * 2.0f - 1.0f);

            // 0
            (vertexBufferPointer)->Position.X = scaledDestinationX + BaseVertexBufferData[0].Position.X * destination.Width;
            (vertexBufferPointer)->Position.Y = scaledDestinationY + BaseVertexBufferData[0].Position.Y * destination.Height;

            (vertexBufferPointer)->TextureCoordinate.X = (source.X + BaseVertexBufferData[0].TextureCoordinate.X * source.Width) * invertedWidth;
            (vertexBufferPointer)->TextureCoordinate.Y = (source.Y + BaseVertexBufferData[0].TextureCoordinate.Y * source.Height) * invertedHeight;

            ++(vertexBufferPointer);

            // 1
            (vertexBufferPointer)->Position.X = scaledDestinationX + BaseVertexBufferData[1].Position.X * destination.Width;
            (vertexBufferPointer)->Position.Y = scaledDestinationY + BaseVertexBufferData[1].Position.Y * destination.Height;

            (vertexBufferPointer)->TextureCoordinate.X = (source.X + BaseVertexBufferData[1].TextureCoordinate.X * source.Width) * invertedWidth;
            (vertexBufferPointer)->TextureCoordinate.Y = (source.Y + BaseVertexBufferData[1].TextureCoordinate.Y * source.Height) * invertedHeight;

            ++(vertexBufferPointer);

            // 2
            (vertexBufferPointer)->Position.X = scaledDestinationX + BaseVertexBufferData[2].Position.X * destination.Width;
            (vertexBufferPointer)->Position.Y = scaledDestinationY + BaseVertexBufferData[2].Position.Y * destination.Height;

            (vertexBufferPointer)->TextureCoordinate.X = (source.X + BaseVertexBufferData[2].TextureCoordinate.X * source.Width) * invertedWidth;
            (vertexBufferPointer)->TextureCoordinate.Y = (source.Y + BaseVertexBufferData[2].TextureCoordinate.Y * source.Height) * invertedHeight;

            ++(vertexBufferPointer);

            // 3
            (vertexBufferPointer)->Position.X = scaledDestinationX + BaseVertexBufferData[3].Position.X * destination.Width;
            (vertexBufferPointer)->Position.Y = scaledDestinationY + BaseVertexBufferData[3].Position.Y * destination.Height;

            (vertexBufferPointer)->TextureCoordinate.X = (source.X + BaseVertexBufferData[3].TextureCoordinate.X * source.Width) * invertedWidth;
            (vertexBufferPointer)->TextureCoordinate.Y = (source.Y + BaseVertexBufferData[3].TextureCoordinate.Y * source.Height) * invertedHeight;

            ++(vertexBufferPointer);

            destination.X += destination.Width;
        }
    }
}