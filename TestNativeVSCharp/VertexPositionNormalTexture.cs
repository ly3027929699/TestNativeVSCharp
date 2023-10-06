using System.Numerics;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct VertexPositionNormalTexture : IEquatable<VertexPositionNormalTexture>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VertexPositionNormalTexture"/> struct.
    /// </summary>
    /// <param name="position">The position of this vertex.</param>
    /// <param name="normal">The vertex normal.</param>
    /// <param name="textureCoordinate">UV texture coordinates.</param>
    public VertexPositionNormalTexture(v3 position, v3 normal, Vector2 textureCoordinate) : this()
    {
        this.Position = position;
        this.Normal = normal;
        this.TextureCoordinate = textureCoordinate;
    }

    /// <summary>
    /// XYZ position.
    /// </summary>
    public Vector3 Position;

    /// <summary>
    /// The vertex normal.
    /// </summary>
    public Vector3 Normal;

    /// <summary>
    /// UV texture coordinates.
    /// </summary>
    public Vector2 TextureCoordinate;

    /// <summary>
    /// Defines structure byte size.
    /// </summary>
    public static readonly int Size = 32;


    public bool Equals(VertexPositionNormalTexture other)
    {
        return this.Position.Equals(other.Position) && this.Normal.Equals(other.Normal) && this.TextureCoordinate.Equals(other.TextureCoordinate);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is VertexPositionNormalTexture && this.Equals((VertexPositionNormalTexture)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = this.Position.GetHashCode();
            hashCode = (hashCode * 397) ^ this.Normal.GetHashCode();
            hashCode = (hashCode * 397) ^ this.TextureCoordinate.GetHashCode();
            return hashCode;
        }
    }

    public void FlipWinding()
    {
        this.TextureCoordinate.X = (1.0f - this.TextureCoordinate.X);
    }

    public static bool operator ==(VertexPositionNormalTexture left, VertexPositionNormalTexture right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(VertexPositionNormalTexture left, VertexPositionNormalTexture right)
    {
        return !left.Equals(right);
    }
}