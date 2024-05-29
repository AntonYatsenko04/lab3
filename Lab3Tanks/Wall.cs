namespace Lab3Tanks;

public class Wall:ICollidable
{
    public Wall(int leftX, int upperY, int width, int height)
    {
        LeftX = leftX;
        UpperY = upperY;
        Width = width;
        Height = height;
    }

    public int LeftX { get; }
    public int UpperY { get; }
    public int Width { get; }
    public int Height { get; }
    public int RightX => LeftX + Width;
    public int LowerY => UpperY + Height;
}