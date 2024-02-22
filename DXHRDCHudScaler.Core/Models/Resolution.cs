namespace DXHRDCHudScaler.Core.Models;

public class Resolution(long id, uint width, uint height) : IResolution, IEquatable<Resolution>
{
    public long Id { get; } = id;
    public uint Width { get; } = width;
    public uint Height { get; } = height;

    public Resolution(uint width, uint height)
        : this(0, width, height) { }

    public bool Equals(Resolution? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Width == other.Width;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Width);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Resolution);
    }
}
