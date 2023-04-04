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

            if (((x, y) == (Row, 3) || (x, y) == (Row + iterator * 1, 3) || (x, y) == (Row + iterator * 2, 3) ||
                (x, y) == (Row, 4) || (x, y) == (Row + iterator * 1, 4) || (x, y) == (Row + iterator * 2, 4) ||
                (x, y) == (Row, 5) || (x, y) == (Row + iterator * 1, 5) || (x, y) == (Row + iterator * 2, 5)) && (LeaveSquareFlag))
                return false;
            if (!((x, y) == (Row, 3) || (x, y) == (Row + iterator * 1, 3) || (x, y) == (Row + iterator * 2, 3) ||
                 (x, y) == (Row, 4) || (x, y) == (Row + iterator * 1, 4) || (x, y) == (Row + iterator * 2, 4) ||
                 (x, y) == (Row, 5) || (x, y) == (Row + iterator * 1, 5) || (x, y) == (Row + iterator * 2, 5)))
                 LeaveSquareFlag = true;
            return true;
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

        // true - король может сделать ход, false - не может
        public bool CheckXodKing(int x, int y, FigureQueen competitorQueen, FigureKing competitorKing, int motionQueen)
        {
            if (GameField.IsEmpty(x, y) && 
               competitorQueen.CheckQueenAttack(competitorQueen.Offset.Row, competitorQueen.Offset.Column, x, y) &&
               !AdjacentPosition(x, y, competitorKing.Offset.Row, competitorKing.Offset.Column) &&
               LeaveSquareCheck(x, y) &&
               motionQueen < 6)
                return true;
            else
                return false;
        }

        public override List<Position> GetAllPosition(int x, int y, int motion, int motionQueen, FigureQueen competitorQueen, FigureKing competitorKing)
        {
            List<Position> list = new List<Position>();
            List<Position> listCheck = new List<Position>() {
                            new Position(0, 1),
                            new Position(0, -1),
                            new Position(1, 0),
                            new Position(1, 1),
                            new Position(1, -1),
                            new Position(-1, 0),
                            new Position(-1, 1),
                            new Position(-1, -1),
                        };
            if (motionQueen >= 6)
                return list;

            foreach (Position pos in listCheck)
            {
                if (GameField.IsInside(Offset.Row + pos.Row, Offset.Column + pos.Column) &&
                    CheckXodKing(Offset.Row + pos.Row, Offset.Column + pos.Column, competitorQueen, competitorKing, motionQueen))
                    list.Add(new Position(Offset.Row + pos.Row, Offset.Column + pos.Column));
            }
            return list;
        }
    }
}
