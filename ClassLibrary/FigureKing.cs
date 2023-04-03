using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class FigureKing : Figure
    {
        public FigureKing(ref Field GameField, Color color)
        {
            if (color != Color.Black && color != Color.White)
                throw new Exception("color black or white");
            this.GameField = GameField;
            if (color == Color.White)
            {
                Id = -1;
                GameField[0, 4] = Id;
                this.StartOffset = new Position(0, 4);
                Offset = new Position(0, 4);
                EndingPosition = new Position(7, 4);
                LeaveSquareFlag = false;
            }
            else
            {
                Id = -3;
                GameField[7, 4] = Id;
                this.StartOffset = new Position(7, 4);
                Offset = new Position(7, 4);
                EndingPosition = new Position(0, 4);
                LeaveSquareFlag = false;
            }
            Color = color;
        }

        // true - делаем ход else - нет
        public bool LeaveSquareCheck(int x, int y)
        {
            int Row = 0;
            int iterator = 1;
            if (Color == Color.Black)
            {
                Row += 7;
                iterator = -1;
            }

            if (((x, y) == (Row, 3) || (x, y) == (Row + iterator * 2, 3) || (x, y) == (Row + iterator * 3, 3) ||
                (x, y) == (Row, 4) || (x, y) == (Row + iterator * 2, 4) || (x, y) == (Row + iterator * 3, 4) ||
                (x, y) == (Row, 5) || (x, y) == (Row + iterator * 2, 5) || (x, y) == (Row + iterator * 3, 5)) && (LeaveSquareFlag))
                return false;
            if (!((x, y) == (Row, 3) || (x, y) == (Row + iterator * 2, 3) || (x, y) == (Row + iterator * 3, 3) ||
                 (x, y) == (Row, 4) || (x, y) == (Row + iterator * 2, 4) || (x, y) == (Row + iterator * 3, 4) ||
                 (x, y) == (Row, 5) || (x, y) == (Row + iterator * 2, 5) || (x, y) == (Row + iterator * 3, 5)))
                LeaveSquareFlag = true;
            return true;
        }
        // проверка: король должен покинуть квадрат за 16 ходов и больше не возвращаться в него
        // x, y - позиция короля, которую он займет при передвижении
        // true - в квадрате, false - не в квадрате
        public bool LeaveSquare(int x, int y, int motion)
        {
            if (motion <= 16)
                return false;
            else
            {
                int Row = 0;
                int iterator = 1;
                if (Color == Color.Black)
                {
                    Row += 7;
                    iterator = -1;
                }
                if ((x, y) == (Row, 3) || (x, y) == (Row + iterator * 2, 3) || (x, y) == (Row + iterator * 3, 3) ||
                    (x, y) == (Row, 4) || (x, y) == (Row + iterator * 2, 4) || (x, y) == (Row + iterator * 3, 4) ||
                    (x, y) == (Row, 5) || (x, y) == (Row + iterator * 2, 5) || (x, y) == (Row + iterator * 3, 5))
                    return true;
                return false;
            }
        }

        // проверка: король находится в смежной позиции с королем противника
        // x, y - координаты короля; xOpponent, yOpponent - координаты короля противника
        // возвращаем true, если позиция смежная
        public bool AdjacentPosition(int x, int y, int xOpponent, int yOpponent)
        {
            if ((x, y) == (xOpponent - 1, yOpponent) ||
               (x, y) == (xOpponent - 1, yOpponent - 1) ||
               (x, y) == (xOpponent, yOpponent - 1) ||
                (x, y) == (xOpponent + 1, yOpponent - 1) ||
                (x, y) == (xOpponent + 1, yOpponent) ||
                (x, y) == (xOpponent + 1, yOpponent + 1) ||
                (x, y) == (xOpponent, yOpponent + 1) ||
                (x, y) == (xOpponent - 1, yOpponent + 1))
            {
                return true;
            }
            else return false;
        }

        public Position RandomXodKing(List<Position> list)
        {
            int position;
            Random random = new Random();
            position = random.Next(list.Count);
            return list[position];
        }

        //public override List<Position> GetAllPosition(FigureQueen queen, FigureKing king, int motion)
        //{
        //    if (queen.CheckQueenAttack(queen.Offset.Row, queen.Offset.Column, king.Offset.Row, king.Offset.Column) ||
        //       king.AdjacentPosition(x, y, Сompetitor.king.Offset.Row, Сompetitor.king.Offset.Column) ||
        //       (!(motion <= 16 || (motion > 16 && king.LeaveSquareCheck(x, y))) ||
        //       !GameField.IsEmptyWave(x, y)))
        //        return false;
        //    else
        //        return true;
        //}

        public override List<Position> GetAllPosition(int x, int y, int kingRow, int kingCol, int motion)
        {
            List < Position > list = new List<Position>();
            list.Add(new Position(5, 5));
            return list;
        }
    }
}
