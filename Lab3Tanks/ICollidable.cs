namespace Lab3Tanks;

public interface ICollidable
{
    public int LeftX { get; }
    public int UpperY { get; }

    public int Width { get; }
    public int Height{ get; }

    public int RightX{ get; }
    public int LowerY{ get; }
}