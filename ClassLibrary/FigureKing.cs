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

        /// <summary>
        /// Проверка, находится ли переданная позиция внутри квадрата
        /// </summary>
        /// <param name="x">строка</param>
        /// <param name="y">столбец</param>
        /// <param name="row">начало отсчета: 0 - белые, 7 - черные</param>
        /// <param name="iterator"></param>
        /// <returns>true - позиция в квадрате, false - не в квадрате</returns>
        private bool CheckInSquare(int x, int y, int row, int iterator)
        {
            if ((x, y) == (row, 3) || (x, y) == (row + iterator * 1, 3) || (x, y) == (row + iterator * 2, 3) ||
                 (x, y) == (row, 4) || (x, y) == (row + iterator * 1, 4) || (x, y) == (row + iterator * 2, 4) ||
                 (x, y) == (row, 5) || (x, y) == (row + iterator * 1, 5) || (x, y) == (row + iterator * 2, 5))
                return true;
            else return false;
        }


        /// <summary>
        /// Метод для перемещения короля на указанную позицию. Изменение флага выхода из квадрата по необходимости.
        /// </summary>
        /// <param name="x">строка</param>
        /// <param name="y">столбец</param>
        public override void MoveBlock(int x, int y)
        {
            if (GameField[x, y] >= 0)
            {
                GameField[Offset.Row, Offset.Column] = 0;
                Offset.Row = x;
                Offset.Column = y;
                GameField[Offset.Row, Offset.Column] = Id;
                ChangeFlag(x, y);
            }
        }
        

        /// <summary>
        /// Метод для проверки, не является ли переданная позиция невозможной для хода по причине выхода короля из квдрата
        /// </summary>
        /// <param name="x">возможная строка</param>
        /// <param name="y">возможный столбец</param>
        /// <returns>true - можно сходить на эту позицию, false - нельзя</returns>
        public bool LeaveSquareCheck(int x, int y)
        {
            int Row = 0;
            int iterator = 1;
            if (Color == Color.Black)
            {
                Row += 7;
                iterator = -1;
            }

            if ((CheckInSquare(x, y, Row, iterator)) && LeaveSquareFlag)
                return false;
            return true;
        }


        /// <summary>
        /// Смена флага при выходе за границы квадрата
        /// </summary>
        /// <param name="x">строка</param>
        /// <param name="y">столбец</param>
        public void ChangeFlag(int x, int y)
        {
            int row = 0;
            int iterator = 1;
            if (Color == Color.Black)
            {
                row += 7;
                iterator = -1;
            }
            if (!CheckInSquare(x, y, row, iterator))
                LeaveSquareFlag = true;
        }


        /// <summary>
        /// Проверка, что король находится в смежной позиции с королем соперника
        /// </summary>
        /// <param name="x">строка короля игрока</param>
        /// <param name="y">столбец короля игрока</param>
        /// <param name="xOpponent">строка короля соперника</param>
        /// <param name="yOpponent">столбец короля соперника</param>
        /// <returns>true - позиция смежная, false - иначе</returns>
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

        /// <summary>
        /// Выбор случайной позиции
        /// </summary>
        /// <param name="list">список возможных позиций</param>
        /// <returns>случайную позицию</returns>
        public Position ChooseRandomPosition(List<Position> list)
        {
            int position;
            Random random = new Random();
            position = random.Next(list.Count);
            return list[position];
        }


        /// <summary>
        /// Проверка, что король может сделать ход
        /// </summary>
        /// <param name="x">строка короля</param>
        /// <param name="y">столбец короля</param>
        /// <param name="competitorQueen">ферзь соперника</param>
        /// <param name="competitorKing">король соперника</param>
        /// <param name="motionQueen">количество ходов, когда ферзь находится на одной горизонтали</param>
        /// <returns>true - король может сделать ход, false - не может</returns>
        public bool OpportunityToMakeMove(int x, int y, FigureQueen competitorQueen, FigureKing competitorKing)
        {
            if (GameField.IsEmpty(x, y) && 
               competitorQueen.CheckQueenAttack(competitorQueen.Offset, new Position(x, y)) &&
               !AdjacentPosition(x, y, competitorKing.Offset.Row, competitorKing.Offset.Column) &&
               LeaveSquareCheck(x, y))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Получение всех возможных позиций короля
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="motion">текущий ход</param>
        /// <param name="motionQueen">сколько ходов ферзь оставался на одной горизонтали</param>
        /// <param name="competitorQueen"></param>
        /// <param name="competitorKing"></param>
        /// <param name="queen"></param>
        /// <returns>список всех возможных позиций</returns>
        public override List<Position> GetAllPosition(int motion, int motionQueen, FigureQueen competitorQueen, FigureKing competitorKing, FigureQueen queen)
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
            
            if (motionQueen >= 5 &&
                queen.GetAllPosition(motionQueen, competitorKing).Count != 0)
                return list;

            foreach (Position pos in listCheck)
            {
                if (OpportunityToMakeMove(Offset.Row + pos.Row, Offset.Column + pos.Column, competitorQueen, competitorKing))
                    list.Add(new Position(Offset.Row + pos.Row, Offset.Column + pos.Column));
            }
            return list;
        }
    }
}
