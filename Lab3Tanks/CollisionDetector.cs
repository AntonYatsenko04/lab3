namespace Lab3Tanks;

using static System.Windows.Forms.Form;


public partial class MainForm
{
    public   void GetCollidableConstraints(ICollidable first,
            ICollidable second, out List<Direction> firstConstraints, out List<Direction> secondConstraints)
        {
            firstConstraints = new List<Direction>();
            secondConstraints = new List<Direction>();

            int closeInterval = Constants.CloseInterval;

            bool firstHigherThanSecond = first.UpperY >= second.LowerY;
            bool secondHigherThanFirst = second.UpperY >= first.LowerY;
            bool firstAndSecondHaveCollideVertically = !firstHigherThanSecond && !secondHigherThanFirst;

            bool firstCloseToSecondFromLeft =
                first.RightX >= second.LeftX - closeInterval && first.RightX <= second.LeftX + closeInterval;
            bool firstCloseToSecondFromRight =
                first.LeftX <= second.RightX + closeInterval && first.LeftX >= second.RightX - closeInterval;
            
            bool firstMoreLeftThanSecond = first.RightX <= second.LeftX;
            bool secondMoreLeftThanFirst = second.RightX <= first.LeftX;
            bool firstAndSecondCollideHorisontally = !firstMoreLeftThanSecond && !secondMoreLeftThanFirst;

            bool firstCloseToSecondFromTop = first.UpperY <= second.LowerY + closeInterval&&first.UpperY>=second.LowerY-closeInterval;
            bool firstCloseToSecondFromBottom = first.LowerY >= second.UpperY - closeInterval &&
                                                first.LowerY <= second.UpperY + closeInterval;


            if (firstAndSecondHaveCollideVertically)
            {
                if (firstCloseToSecondFromLeft)
                {
                    firstConstraints.Add(Direction.Right);
                    secondConstraints.Add(Direction.Left);
                }

                if (firstCloseToSecondFromRight)
                {
                    firstConstraints.Add(Direction.Left);
                    secondConstraints.Add(Direction.Right);
                }
            }


            if (firstAndSecondCollideHorisontally)
            {
                if (firstCloseToSecondFromBottom)
                {
                    firstConstraints.Add(Direction.Down);
                    secondConstraints.Add(Direction.Up);
                }
            
                if (firstCloseToSecondFromTop)
                {
                    firstConstraints.Add(Direction.Up);
                    secondConstraints.Add(Direction.Down);
                }
            }

            firstConstraints.AddRange(_getClientCollisionConstraints(first));
            secondConstraints.AddRange(_getClientCollisionConstraints(second));
        }

    private  List<Direction> _getClientCollisionConstraints(ICollidable obj)
    {
        var constraints = new List<Direction>();
        var clientSize = ClientRectangle.Size;
        bool cantMoveRight = obj.RightX >= ClientRectangle.Right;
        bool cantMoveUp = obj.UpperY <= 0;
        bool cantMoveLeft = obj.LeftX <= 0;
        bool cantMoveDown = obj.LowerY >= ClientRectangle.Bottom;

        if (cantMoveRight)
        {
            constraints.Add(Direction.Right);
        }
        if (cantMoveDown)
        {
            constraints.Add(Direction.Down);
        }
        if (cantMoveLeft)
        {
            constraints.Add(Direction.Left);
        }
        if (cantMoveUp)
        {
            constraints.Add(Direction.Up);
        }

        return constraints;
    }
}