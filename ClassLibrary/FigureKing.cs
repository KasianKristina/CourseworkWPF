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
        public override void MoveFigure(int x, int y)
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
        private bool LeaveSquareCheck(int x, int y)
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
        private void ChangeFlag(int x, int y)
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
        private bool AdjacentPosition(int x, int y, int xOpponent, int yOpponent)
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
        /// Выбор позиции, для которой не будет преград на следующем ходе
        /// </summary>
        /// <param name="list">список возможных позиций</param>
        /// <returns>выгодную позицию или (-10, -10), если такой позиции не существует</returns>
        public List<Position> ChooseNonPregradaPosition(FigureQueen competitorQueen, FigureKing competitorKing, int motionQueen, FigureQueen queen)
        {
            List<Position> listPositions = new List<Position>();
            List<Position> listPositionsAround;
            if (motionQueen >= 5 &&
                queen.GetAllPosition(motionQueen, competitorKing).Count != 0)
                return listPositions;
            if (this.Color == Color.White)
                listPositionsAround = new List<Position>() {
                            new Position(1, 0),
                            new Position(1, 1),
                            new Position(1, -1),
                        };
            else listPositionsAround = new List<Position>() {
                            new Position(-1, -1),
                            new Position(-1, 0),
                            new Position(-1, 1),
                        };

            for (int j = 0; j < listPositionsAround.Count; j++)
            {
                if (OpportunityToMakeMove(this.Offset.Row + listPositionsAround[j].Row, this.Offset.Column + listPositionsAround[j].Column, competitorQueen, competitorKing))
                {
                    List<Position> checkPregradaPositions = competitorQueen.GetObstaclesPosition(null, new Position(this.Offset.Row + listPositionsAround[j].Row, this.Offset.Column + listPositionsAround[j].Column));
                    List<Position> checkAllPositions = competitorQueen.GetAllPosition(motionQueen, null, new Position(this.Offset.Row + listPositionsAround[j].Row, this.Offset.Column + listPositionsAround[j].Column));
                    if (checkPregradaPositions.Count == 0 || !CheckElementsMatch(checkPregradaPositions, checkAllPositions))
                    {
                        listPositions.Add(new Position(this.Offset.Row + listPositionsAround[j].Row, this.Offset.Column + listPositionsAround[j].Column));
                    }
                }
            }
            return listPositions;
        }

        private bool CheckElementsMatch(List<Position> firstList, List<Position> secondList)
        {
            for (int i = 0; i < firstList.Count; i++)
            {
                for (int j = 0; j < secondList.Count; j++)
                {
                    if (firstList[i].Equals(secondList[j]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// Выбор случайной позиции
        /// </summary>
        /// <param name="list">список возможных позиций</param>
        /// <returns>случайную позицию</returns>
        private Position ChooseRandomPosition(List<Position> list)
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
        /// <returns>true - король может сделать ход, false - не может</returns>
        private bool OpportunityToMakeMove(int x, int y, FigureQueen competitorQueen, FigureKing competitorKing)
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
        /// <param name="motion">текущий ход</param>
        /// <param name="motionQueen">сколько ходов ферзь оставался на одной горизонтали</param>
        /// <param name="competitorQueen"></param>
        /// <param name="competitorKing"></param>
        /// <param name="queen"></param>
        /// <returns>список всех возможных позиций</returns>
        public List<Position> GetAllPosition(int motion, int motionQueen, FigureQueen competitorQueen, FigureKing competitorKing, FigureQueen queen)
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

        private void ClearGameFieldAfterKingsMove()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (GameField[i, j] == -7)
                    {
                        GameField[i, j] = 0;
                    }
                }
            }
        }

        public int OptimalMove(int motion, Position posEnd, FigureKing competitorKing, FigureQueen competitorQueen, Dictionary<int, (int, Position)> history, int motionColor, FigureQueen queen, bool isNonPregradaWay)
        {
            int result, fx, fy;
            while (true)
            {
                Field cMap = DynamicField.CreateWave(Offset.Row, Offset.Column, posEnd.Row, posEnd.Column, GameField);
                result = cMap[posEnd.Row, posEnd.Column];

                (fx, fy) = DynamicField.Search(posEnd.Row, posEnd.Column, result, ref cMap, false);

                if (fx != -100 &&
                    OpportunityToMakeMove(fx, fy, competitorQueen, competitorKing))
                {
                    MoveFigure(fx, fy);
                    history.Add(motion, (Id, new Position(fx, fy)));
                    break;
                }
                else
                {
                    if (fx == -100 || (fx, fy) == (posEnd.Row, posEnd.Column))
                    {
                        List<Position> allPositions;
                        if (isNonPregradaWay)
                        {
                            allPositions = ChooseNonPregradaPosition(competitorQueen, competitorKing, motionColor, queen);
                        }
                        else
                        {
                            allPositions = GetAllPosition(motion, motionColor, competitorQueen, competitorKing, queen);
                        }
                        if (allPositions.Count != 0)
                        {
                            Position position = ChooseRandomPosition(allPositions);
                            MoveFigure(position.Row, position.Column);
                            history.Add(motion, (Id, new Position(position.Row, position.Column)));
                            fx = position.Row;
                            break;
                        }
                        else
                        {
                            fx = -100;
                            break;
                        }
                    }
                    GameField[fx, fy] = -7;
                }
                cMap.Draw();
            }
            ClearGameFieldAfterKingsMove();
            return fx;
        }

        // TODO удалить дублирование
        public int OptimalMoveWith(int motion, Position posEnd, FigureKing competitorKing, FigureQueen competitorQueen, Dictionary<int, (int, Position)> history, int motionColor, FigureQueen queen)
        {
            int result, fx, fy;
            while (true)
            {
                Field cMap = DynamicField.CreateWave(Offset.Row, Offset.Column, posEnd.Row, posEnd.Column, GameField);
                result = cMap[posEnd.Row, posEnd.Column];

                (fx, fy) = DynamicField.Search(posEnd.Row, posEnd.Column, result, ref cMap, false);

                if (fx != -100 &&
                    OpportunityToMakeMove(fx, fy, competitorQueen, competitorKing))
                {
                    MoveFigure(fx, fy);
                    history.Add(motion, (Id, new Position(fx, fy)));
                    break;
                }
                else
                {
                    if (fx == -100 || (fx, fy) == (posEnd.Row, posEnd.Column))
                    {
                        List<Position> allPositions = GetAllPosition(motion, motionColor, competitorQueen, competitorKing, queen);
                        if (allPositions.Count != 0)
                        {
                            Position position = ChooseRandomPosition(allPositions);
                            MoveFigure(position.Row, position.Column);
                            history.Add(motion, (Id, new Position(position.Row, position.Column)));
                            fx = position.Row;
                            break;
                        }
                        else
                        {
                            fx = -100;
                            break;
                        }
                    }
                    GameField[fx, fy] = -7;
                }
                cMap.Draw();
            }
            ClearGameFieldAfterKingsMove();
            return fx;
        }

        public List<Position> SameWayMoveGetPath(Position posEnd)
        {
            int result;

            Field cMap = DynamicField.CreateWave(Offset.Row, Offset.Column, posEnd.Row, posEnd.Column, GameField);
            result = cMap[posEnd.Row, posEnd.Column];

            List<Position> path = DynamicField.FindKingPath(posEnd.Row, posEnd.Column, result, ref cMap, false);
            path.Reverse();
            path.Add(posEnd);
            return path;
        }

        public int RandomMove(int motion, FigureKing competitorKing, FigureQueen competitorQueen, Dictionary<int, (int, Position)> history, int motionColor, FigureQueen queen)
        {
            int fx;
            List<Position> allPositions = GetAllPosition(motion, motionColor, competitorQueen, competitorKing, queen);
            if (allPositions.Count != 0)
            {
                Position position = ChooseRandomPosition(allPositions);
                MoveFigure(position.Row, position.Column);
                history.Add(motion, (Id, new Position(position.Row, position.Column)));
                fx = position.Row;
            }
            else
                fx = -100;
            return fx;
        }

        public int SameWayMove(int motion, FigureKing competitorKing, FigureQueen competitorQueen, Dictionary<int, (int, Position)> history, List<Position> path)
        {
            int fx = 0;
            int pathLength = path.Count();
            while (true)
            {
                if (pathLength != 0 &&
                   OpportunityToMakeMove(path[0].Row, path[0].Column, competitorQueen, competitorKing))
                {
                    MoveFigure(path[0].Row, path[0].Column);
                    history.Add(motion, (Id, new Position(path[0].Row, path[0].Column)));
                    // TODO удалить первый элемент массива path
                    path.RemoveAt(0);
                    break;
                }
                // TODO проеврить работу
                //else
                //{
                //if (pathLength == 0 || (path[0].Row, path[0].Column) == (posEnd.Row, posEnd.Column))
                //{
                //List<Position> allPositions = GetAllPosition(motion, motionColor, competitorQueen, competitorKing, queen);
                //if (allPositions.Count != 0)
                //{
                // Position position = ChooseRandomPosition(allPositions);
                // MoveFigure(position.Row, position.Column);
                // history.Add(motion, (Id, new Position(position.Row, position.Column)));
                // fx = position.Row;
                // break;
                //}
                else
                {
                    fx = -100;
                    break;
                }
                //}
                //GameField[path[0].Row, path[0].Column] = -7;
                //}
                //cMap.Draw();
            }
            ClearGameFieldAfterKingsMove();
            return fx;
        }

        public int NextPositionMove(int motion, FigureKing competitorKing, FigureQueen competitorQueen, Dictionary<int, (int, Position)> history)
        {
            List<Position> defaultPath = SameWayMoveGetPath(EndingPosition);
            int index = defaultPath.IndexOf(Offset);
            int result;
            if (index + 1 != defaultPath.Count && OpportunityToMakeMove(defaultPath[index + 1].Row, defaultPath[index + 1].Column, competitorQueen, competitorKing)) // если это не последний элемент в массиве
            {
                MoveFigure(defaultPath[index + 1].Row, defaultPath[index + 1].Column); // сдвиг фигуры на следующую позицию
                history.Add(motion, (Id, new Position(defaultPath[index + 1].Row, defaultPath[index + 1].Column)));
                result = 1;
            }
            else
            {
                result = 0 ;
            }
            return result;
        }
    }
}
