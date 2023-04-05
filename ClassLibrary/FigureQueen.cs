using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class FigureQueen : Figure
    {
        public FigureQueen(ref Field GameField, Color color)
        {
            if (color != Color.Black && color != Color.White)
                throw new Exception("color black or white");
            this.GameField = GameField;
            if (color == Color.White)
            {
                Id = -2;
                GameField[0, 3] = Id;
                this.StartOffset = new Position(0, 3);
                Offset = new Position(0, 3);
                EndingPosition = new Position(7, 3);
                // строка с которой ферзь начинает охранять короля
                RowConst = 2;
            }
            else
            {
                Id = -4;
                GameField[7, 3] = Id;
                this.StartOffset = new Position(7, 3);
                Offset = new Position(7, 3);
                EndingPosition = new Position(0, 3);
                RowConst = 5;
            }
            Color = color;
        }

        public bool CheckPregradaCompetitorQueen(FigureKing king, FigureQueen competitorQueen)
        {
            int k;
            if (competitorQueen.Color == Color.Black)
                k = -1;
            else
                k = 1;
            // вправо
            for (int i = king.Offset.Column + 1; i < 8; i++)
            {
                if (GameField.IsInside(king.Offset.Row - k, i))
                    if (GameField[king.Offset.Row - k, i] == competitorQueen.Id)
                        return true;
            }
            // влево
            for (int i = king.Offset.Column - 1; i >= 0; i--)
            {
                if (GameField.IsInside(king.Offset.Row - k, i))
                    if (GameField[king.Offset.Row - k, i] == competitorQueen.Id)
                        return true;
            }

            return false;
        }

        

        public bool CheckPregradaMove(FigureKing competitorKing, int motion, Color color, int motionColor, Dictionary<int, (int, Position)> history, FigureKing king, FigureQueen competitorQueen)
        {
            if (!CheckPregradaCompetitorQueen(king, competitorQueen) && !GetPosFerz(king.Offset.Row, king.Offset.Column, color))
                return false;

            List<Position> listPregradi = GetBlocksPositions(king.Offset.Row, competitorQueen.Offset.Row, competitorQueen.Offset.Column);
            List<Position> listAll = GetAllPosition(Offset.Row, Offset.Column, motionColor, competitorKing);

            for (int i = 0; i < listPregradi.Count; i++)
            {
                for (int j = 0; j < listAll.Count; j++)
                {

                    if (listAll[j].Equals(listPregradi[i]))
                    {
                        MoveBlock(listPregradi[i].Row, listPregradi[i].Column);
                        history.Add(motion, (Id, new Position(listPregradi[i].Row, listPregradi[i].Column)));
                        Console.WriteLine("pregrada q {0}, {1}", listPregradi[i].Row, listPregradi[i].Column);
                        return true;
                    }
                }
            }
            return false;
        }
        public bool RandomMove(FigureKing competitorKing, int motion, Dictionary<int, (int, Position)> history, int motionColor)
        {
            int position;
            List<Position> list = GetAllPosition(Offset.Row, Offset.Column, motionColor, competitorKing);
            Random random = new Random();

            if (list.Count == 0)
                return false;
            position = random.Next(list.Count);

            MoveBlock(list[position].Row, list[position].Column);
            history.Add(motion, (Id, new Position(list[position].Row, list[position].Column)));
            Console.WriteLine("random q {0}, {1}", list[position].Row, list[position].Column);
            return true;
        }
        // ферзь уже блокирует короля
        public bool GetPosFerz(int kingRow, int kingCol, Color color)
        {
            int k;
            if (color == Color.Black)
                k = -1;
            else
                k = 1;
            // вправо
            for (int i = kingCol + 1; i < 8; i++)
            {
                if (GameField.IsInside(kingRow - k, i))
                    if (GameField[kingRow - k, i] == Id)
                        return true;
            }
            // влево
            for (int i = kingCol - 1; i >= 0; i--)
            {
                if (GameField.IsInside(kingRow - k, i))
                    if (GameField[kingRow - k, i] == Id)
                        return true;
            }

            return false;
        }


        // TODO: проверить, что CheckPreviousPosition работает правильно
        public bool ObstacleMove(FigureKing competitorKing, Color color, int motionColor, Dictionary<int, (int, Position)> history, int motion)
        {
            List<Position> listObstacles = GetObstaclesPosition(competitorKing, color);
            List<Position> listAll = GetAllPosition(Offset.Row, Offset.Column, motionColor, competitorKing);
            for (int i = 0; i < listObstacles.Count; i++)
            {
                for (int j = 0; j < listAll.Count; j++)
                {
                    if (listAll[j].Equals(listObstacles[i]) &&
                        history.Count > 1 &&
                        listObstacles[i].Row != CheckPreviousPosition(history) &&
                        !GetPosFerz(competitorKing.Offset.Row, competitorKing.Offset.Column, color))
                    {
                        MoveBlock(listObstacles[i].Row, listObstacles[i].Column);
                        history.Add(motion, (Id, new Position(listObstacles[i].Row, listObstacles[i].Column)));
                        Console.WriteLine("obstacle q {0}, {1}", listObstacles[i].Row, listObstacles[i].Column);
                        return true;
                    }
                }
            }
            return false;
        }

        // проверка, что ферзь сходил на предыдущую строку
        // возвращает предыдущую строку
        public int CheckPreviousPosition(Dictionary<int, (int, Position)> history)
        {
            int[] rows = { -10, -10 };
            foreach (var value in history)
            {
                if (value.Value.Item1 == Id)
                {
                    rows[0] = rows[1];
                    rows[1] = value.Value.Item2.Row;
                }
            }
            return rows[0];
        }

        public bool ObstacleOrNearbyMove(FigureKing competitorKing, Color color, int motionColor, Dictionary<int, (int, Position)> history, int motion)
        {
            if ((color == Color.Black && competitorKing.Offset.Row >= RowConst) || (color == Color.White && competitorKing.Offset.Row <= RowConst))
            {
                bool check = ObstacleMove(competitorKing, color, motionColor, history, motion);
                return check;
            }
            else if (motionColor >= 6 && ((color == Color.Black && competitorKing.Offset.Row < RowConst) || (color == Color.White && competitorKing.Offset.Row > RowConst)))
            {
                bool check = NearbyMove(competitorKing.Offset.Row, competitorKing.Offset.Column, motion, history);
                return check;
            }
            else
                return false;
        }

        public bool HorizontalMove(int kingRow, int kingCol, Dictionary<int, (int, Position)> history, int motion)
        {
            int position;
            List<Position> list = GetHorizontalPositions(Offset.Row, Offset.Column, kingRow, kingCol);
            Random random = new Random();

            if (list.Count == 0)
                return false;
            position = random.Next(list.Count);

            MoveBlock(list[position].Row, list[position].Column);
            history.Add(motion, (Id, new Position(list[position].Row, list[position].Column)));
            Console.WriteLine("horizontal q {0}, {1}", list[position].Row, list[position].Column);
            return true;
        }

        public List<Position> GetHorizontalPositions(int x, int y, int kingRow, int kingCol)
        {
            List<Position> list = new List<Position>();

            // иду вправо
            for (int i = y + 1; i < 8; i++)
            {
                if (GameField[x, i] == 0)
                {
                    if (CheckQueenAttack(x, i, kingRow, kingCol))
                        list.Add(new Position(x, i));
                }
                else break;
            }
            // иду влево
            for (int i = y - 1; i >= 0; i--)
            {
                if (GameField[x, i] == 0)
                {
                    if (CheckQueenAttack(x, i, kingRow, kingCol))
                        list.Add(new Position(x, i));
                }
                else break;
            }

            return list;
        }

        // все возможные позиции королевы
        public override List<Position> GetAllPosition(int x, int y, int motion, int motionQueen, FigureQueen competitorQueen, FigureKing competitorKing, FigureQueen queen)
        {
            List<Position> list = new List<Position>();
            if (motionQueen < 6)
            {
                // иду вправо
                for (int i = y + 1; i < 8; i++)
                {
                    if (GameField[x, i] == 0)
                    {
                        if (CheckQueenAttack(x, i, competitorKing.Offset.Row, competitorKing.Offset.Column))
                            list.Add(new Position(x, i));
                    }
                    else break;
                }
                // иду влево
                for (int i = y - 1; i >= 0; i--)
                {
                    if (GameField[x, i] == 0)
                    {
                        if (CheckQueenAttack(x, i, competitorKing.Offset.Row, competitorKing.Offset.Column))
                            list.Add(new Position(x, i));
                    }
                    else break;
                }
            }
            // иду вниз
            for (int i = x + 1; i < 8; i++)
            {
                if (GameField[i, y] == 0)
                {
                    if (CheckQueenAttack(i, y, competitorKing.Offset.Row, competitorKing.Offset.Column))
                        list.Add(new Position(i, y));
                }
                else break;
            }
            // иду вверх
            for (int i = x - 1; i >= 0; i--)
            {
                if (GameField[i, y] == 0)
                {
                    if (CheckQueenAttack(i, y, competitorKing.Offset.Row, competitorKing.Offset.Column))
                        list.Add(new Position(i, y));
                }
                else break;
            }
            // иду в правый нижний угол
            int rowStep = 1;
            int columnStep = 1;
            for (int i = 1; i < 8; i++)
            {
                if (GameField.IsInside(x + i * rowStep, y + i * columnStep) && GameField[x + i * rowStep, y + i * columnStep] == 0)
                {
                    if (CheckQueenAttack(x + i * rowStep, y + i * columnStep, competitorKing.Offset.Row, competitorKing.Offset.Column))
                        list.Add(new Position(x + i * rowStep, y + i * columnStep));
                }
                else break;
            }
            // иду в левый нижний угол
            rowStep = 1;
            columnStep = -1;
            for (int i = 1; i < 8; i++)
            {
                if (GameField.IsInside(x + i * rowStep, y + i * columnStep) && GameField[x + i * rowStep, y + i * columnStep] == 0)
                {
                    if (CheckQueenAttack(x + i * rowStep, y + i * columnStep, competitorKing.Offset.Row, competitorKing.Offset.Column))
                        list.Add(new Position(x + i * rowStep, y + i * columnStep));
                }
                else break;
            }
            // иду в правый верхний угол
            rowStep = -1;
            columnStep = 1;
            for (int i = 1; i < 8; i++)
            {
                if (GameField.IsInside(x + i * rowStep, y + i * columnStep) && GameField[x + i * rowStep, y + i * columnStep] == 0)
                {
                    if (CheckQueenAttack(x + i * rowStep, y + i * columnStep, competitorKing.Offset.Row, competitorKing.Offset.Column))
                        list.Add(new Position(x + i * rowStep, y + i * columnStep));
                }
                else break;
            }
            // иду в левый верхний угол
            rowStep = -1;
            columnStep = -1;
            for (int i = 1; i < 8; i++)
            {
                if (GameField.IsInside(x + i * rowStep, y + i * columnStep) && GameField[x + i * rowStep, y + i * columnStep] == 0)
                {
                    if (CheckQueenAttack(x + i * rowStep, y + i * columnStep, competitorKing.Offset.Row, competitorKing.Offset.Column))
                        list.Add(new Position(x + i * rowStep, y + i * columnStep));
                }
                else break;
            }
            return list;
        }

        public List<Position> GetAllPosition(int x, int y, int motionQueen, FigureKing competitorKing)
        {
            List<Position> list = new List<Position>();
            if (motionQueen < 6)
            {
                // иду вправо
                for (int i = y + 1; i < 8; i++)
                {
                    if (GameField[x, i] == 0)
                    {
                        if (CheckQueenAttack(x, i, competitorKing.Offset.Row, competitorKing.Offset.Column))
                            list.Add(new Position(x, i));
                    }
                    else break;
                }
                // иду влево
                for (int i = y - 1; i >= 0; i--)
                {
                    if (GameField[x, i] == 0)
                    {
                        if (CheckQueenAttack(x, i, competitorKing.Offset.Row, competitorKing.Offset.Column))
                            list.Add(new Position(x, i));
                    }
                    else break;
                }
            }
            // иду вниз
            for (int i = x + 1; i < 8; i++)
            {
                if (GameField[i, y] == 0)
                {
                    if (CheckQueenAttack(i, y, competitorKing.Offset.Row, competitorKing.Offset.Column))
                        list.Add(new Position(i, y));
                }
                else break;
            }
            // иду вверх
            for (int i = x - 1; i >= 0; i--)
            {
                if (GameField[i, y] == 0)
                {
                    if (CheckQueenAttack(i, y, competitorKing.Offset.Row, competitorKing.Offset.Column))
                        list.Add(new Position(i, y));
                }
                else break;
            }
            // иду в правый нижний угол
            int rowStep = 1;
            int columnStep = 1;
            for (int i = 1; i < 8; i++)
            {
                if (GameField.IsInside(x + i * rowStep, y + i * columnStep) && GameField[x + i * rowStep, y + i * columnStep] == 0)
                {
                    if (CheckQueenAttack(x + i * rowStep, y + i * columnStep, competitorKing.Offset.Row, competitorKing.Offset.Column))
                        list.Add(new Position(x + i * rowStep, y + i * columnStep));
                }
                else break;
            }
            // иду в левый нижний угол
            rowStep = 1;
            columnStep = -1;
            for (int i = 1; i < 8; i++)
            {
                if (GameField.IsInside(x + i * rowStep, y + i * columnStep) && GameField[x + i * rowStep, y + i * columnStep] == 0)
                {
                    if (CheckQueenAttack(x + i * rowStep, y + i * columnStep, competitorKing.Offset.Row, competitorKing.Offset.Column))
                        list.Add(new Position(x + i * rowStep, y + i * columnStep));
                }
                else break;
            }
            // иду в правый верхний угол
            rowStep = -1;
            columnStep = 1;
            for (int i = 1; i < 8; i++)
            {
                if (GameField.IsInside(x + i * rowStep, y + i * columnStep) && GameField[x + i * rowStep, y + i * columnStep] == 0)
                {
                    if (CheckQueenAttack(x + i * rowStep, y + i * columnStep, competitorKing.Offset.Row, competitorKing.Offset.Column))
                        list.Add(new Position(x + i * rowStep, y + i * columnStep));
                }
                else break;
            }
            // иду в левый верхний угол
            rowStep = -1;
            columnStep = -1;
            for (int i = 1; i < 8; i++)
            {
                if (GameField.IsInside(x + i * rowStep, y + i * columnStep) && GameField[x + i * rowStep, y + i * columnStep] == 0)
                {
                    if (CheckQueenAttack(x + i * rowStep, y + i * columnStep, competitorKing.Offset.Row, competitorKing.Offset.Column))
                        list.Add(new Position(x + i * rowStep, y + i * columnStep));
                }
                else break;
            }
            return list;
        }


        public bool NearbyMove(int kingRow, int kingCol, int motion, Dictionary<int, (int, Position)> history)
        {
            List<Position> list = new List<Position>() {
                            new Position(1, 0),
                            new Position(1, 1),
                            new Position(1, -1),
                            new Position(-1, 0),
                            new Position(-1, 1),
                            new Position(-1, -1),
                        };
            Random random = new Random();
            while (list.Any())
            {
                int position = random.Next(list.Count);
                int row = Offset.Row + list[position].Row;
                int col = Offset.Column + list[position].Column;
                if (GameField.IsEmpty(row, col)
                    && CheckQueenAttack(row, col, kingRow, kingCol))
                {
                    MoveBlock(row, col);
                    history.Add(motion, (Id, new Position(row, col)));
                    Console.WriteLine("nearby q {0}, {1}", row, col);
                    return true;
                }
                else list.RemoveAt(position);
            }
            return false;
        }

        // все позиции для блокировки короля соперника
        public List<Position> GetObstaclesPosition(FigureKing competitorKing, Color color)
        {
            List<Position> list = new List<Position>();
            int k;
            if (color == Color.Black)
                k = -1;
            else k = 1;
            // вправо
            for (int i = competitorKing.Offset.Column + 1; i < 8; i++)
            {
                if (GameField.IsWall(competitorKing.Offset.Row - k, i))
                    break;
                if (GameField.IsInside(competitorKing.Offset.Row - k, i) && GameField[competitorKing.Offset.Row - k, i] == 0)
                    list.Add(new Position(competitorKing.Offset.Row - k, i));
            }
            // влево
            for (int i = competitorKing.Offset.Column - 1; i >= 0; i--)
            {
                if (GameField.IsWall(competitorKing.Offset.Row - k, i))
                    break;
                if (GameField.IsInside(competitorKing.Offset.Row - k, i) && GameField[competitorKing.Offset.Row - k, i] == 0)
                    list.Add(new Position(competitorKing.Offset.Row - k, i));
            }
            return list;
        }

        // позиции между блокирующим ферзем соперника и королем
        public List<Position> GetBlocksPositions(int kingCol, int queenCompetitorRow, int queenCompetitorCol)
        {
            List<Position> list = new List<Position>();
            if (queenCompetitorCol > kingCol)
            {
                // вправо
                for (int i = kingCol + 1; i < queenCompetitorCol; i++)
                    list.Add(new Position(queenCompetitorRow, i));
            }
            else
            {
                // влево
                for (int i = kingCol - 1; i > queenCompetitorCol; i--)
                    list.Add(new Position(queenCompetitorRow, i));
            }
            return list;
        }

        // true - не бьет, false - бьет
        public bool CheckQueenAttack(int queenRow, int queenCol, int kingRow, int kingCol)
        {
            if (queenRow == kingRow)
                if (queenCol > kingCol)
                {
                    for (int i = queenCol - 1; i > kingCol; i--)
                    {
                        if (GameField[queenRow, i] < 0 & GameField[queenRow, i] >= -5)
                            return true;
                    }
                    return false;
                }
                else
                {
                    for (int i = queenCol + 1; i < kingCol; i++)
                    {
                        if (GameField[queenRow, i] < 0 & GameField[queenRow, i] >= -5)
                            return true;
                    }
                    return false;
                }


            if (queenCol == kingCol)
                if (queenRow > kingRow)
                {
                    for (int i = queenRow - 1; i > kingRow; i--)
                    {
                        if (GameField[i, queenCol] < 0 & GameField[i, queenCol] >= -5)
                            return true;
                    }
                    return false;
                }
                else
                {
                    for (int i = queenRow + 1; i < kingRow; i++)
                    {
                        if (GameField[i, queenCol] < 0 & GameField[i, queenCol] >= -5)
                            return true;
                    }
                    return false;
                }
            // Check if king is in the same diagonal as queen
            int rowDiff = Math.Abs(queenRow - kingRow);
            int colDiff = Math.Abs(queenCol - kingCol);

            if (rowDiff == colDiff)
            {
                // Check for obstacles
                if (queenRow > kingRow)
                {
                    int rowStep = -1;
                    int columnStep = 1;
                    if (queenCol > kingCol)
                        columnStep = -1;
                    for (int i = 1; i < rowDiff; i++)
                    {
                        if (GameField[queenRow + i * rowStep, queenCol + i * columnStep] < 0 &
                            GameField[queenRow + i * rowStep, queenCol + i * columnStep] >= -5)
                            return true;
                    }
                }
                else
                {
                    int rowStep = 1;
                    int columnStep = 1;
                    if (queenCol > kingCol)
                        columnStep = -1;
                    for (int i = 1; i < rowDiff; i++)
                    {
                        if (GameField[queenRow + i * rowStep, queenCol + i * columnStep] < 0 &
                            GameField[queenRow + i * rowStep, queenCol + i * columnStep] >= -5)
                            return true;
                    }
                }
                return false;
            }
            return true;
        }
    }
}
