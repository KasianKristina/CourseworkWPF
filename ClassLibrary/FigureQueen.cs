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
                BarrierPositionLeft = new Position(1, 2);
                BarrierPositionMiddle = new Position(1, 3);
                BarrierPositionRight = new Position(1, 4);
            }
            else
            {
                Id = -4;
                GameField[7, 3] = Id;
                this.StartOffset = new Position(7, 3);
                Offset = new Position(7, 3);
                EndingPosition = new Position(0, 3);
                RowConst = 5;
                BarrierPositionLeft = new Position(6, 2);
                BarrierPositionMiddle = new Position(6, 3);
                BarrierPositionRight = new Position(6, 4);
            }
            Color = color;
            LoserFlag = false;
        }

        /// <summary>
        /// Проверка: ферзь соперника преграждает путь королю
        /// </summary>
        /// <param name="king"></param>
        /// <param name="competitorQueen"></param>
        /// <returns>true - преграждает, false - нет</returns>
        private bool CheckPregradaCompetitorQueen(FigureKing king, FigureQueen competitorQueen)
        {
            int k;
            if (competitorQueen.Color == Color.Black)
                k = -1;
            else
                k = 1;

            if (king.Offset.Column < competitorQueen.Offset.Column)
            {
                // вправо
                for (int i = king.Offset.Column + 1; i < 8; i++)
                {
                    if (GameField.IsInside(king.Offset.Row - k, i))
                    {
                        if (GameField[king.Offset.Row - k, i] == competitorQueen.Id)
                            return true;
                        if (GameField[king.Offset.Row - k, i] != 0)
                            return false;
                    }
                }
            }
            if (king.Offset.Column > competitorQueen.Offset.Column)
            {
                // влево
                for (int i = king.Offset.Column - 1; i >= 0; i--)
                {
                    if (GameField.IsInside(king.Offset.Row - k, i))
                    {
                        if (GameField[king.Offset.Row - k, i] == competitorQueen.Id)
                            return true;
                        if (GameField[king.Offset.Row - k, i] != 0)
                            return false;
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// Проверка: ферзь не может сходить по причине блокировки
        /// </summary>
        /// <param name="competitorQueenId"></param>
        /// <param name="king"></param>
        /// <returns>true - ферзь не проиграл, ему преграждают путь, false - ферзь проиграл</returns>
        public bool CheckLoseGame(int competitorQueenId, Position king)
        {
            List<Position> list = new List<Position>() {
                            new Position(1, 0),
                            new Position(1, 1),
                            new Position(1, -1),
                            new Position(-1, 0),
                            new Position(-1, 1),
                            new Position(-1, -1),
                            new Position(0, 1),
                            new Position(0, -1),
                        };
            foreach (Position pos in list)
            {
                if (GameField.IsInside(Offset.Row + pos.Row, Offset.Column + pos.Column))
                {
                    if (GameField[Offset.Row + pos.Row, Offset.Column + pos.Column] == competitorQueenId ||
                        (GameField[Offset.Row + pos.Row, Offset.Column + pos.Column] != -5 && !CheckQueenAttack(new Position(Offset.Row + pos.Row, Offset.Column + pos.Column), king)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// Проверка на наличие преград перед стартовой позицией ферзя
        /// Если преграды присутсвуют, то ход на позицию с дальнейшей возможностью сделать не только ход по горизонтали
        /// </summary>
        /// <param name="history"></param>
        /// <param name="motion"></param>
        /// <param name="competitorKing"></param>
        /// <returns>true - обходной ход сделан, false - преград нет или пока нет возможности сделать обходной ход</returns>
        public bool CheckStartingBarriers(Dictionary<int, (int, Position)> history, int motion, Position competitorKing)
        {
            List<Position> horizontalPositions;
            int row;
            int barrierRow;
            if (Color == Color.White)
            {
                row = 0;
                barrierRow = 1;
            }
            else
            {
                row = 7;
                barrierRow = 6;
            }

            if (GameField[StartOffset.Row, StartOffset.Column] == Id &&
                GameField[BarrierPositionRight.Row, BarrierPositionRight.Column] == -5 &&
                GameField[BarrierPositionMiddle.Row, BarrierPositionMiddle.Column] == -5 &&
                GameField[BarrierPositionLeft.Row, BarrierPositionLeft.Column] == -5)
            {
                horizontalPositions = GetHorizontalPositions(competitorKing);
            }
            else return false;

            foreach (Position pos in horizontalPositions)
            {
                if (GameField[barrierRow, pos.Column] == 0)
                {
                    MoveFigure(row, pos.Column);
                    history.Add(motion, (Id, new Position(row, pos.Column)));
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Проверка на необходимость делать разблокирующий ход
        /// Если такая необходимость есть, то ферзь ходит на разблокирующую позицию
        /// </summary>
        /// <param name="competitorKing"></param>
        /// <param name="motion"></param>
        /// <param name="motionColor"></param>
        /// <param name="history"></param>
        /// <param name="king"></param>
        /// <param name="competitorQueen"></param>
        /// <returns>true - ход сделан, false - король не заблокирован или нет возможности его разблокировать</returns>
        public bool UnlockingMove(FigureKing competitorKing, int motion, int motionColor, Dictionary<int, (int, Position)> history, FigureKing king, FigureQueen competitorQueen)
        {
            if (!CheckPregradaCompetitorQueen(king, competitorQueen))
                return false;

            List<Position> listPregradi = GetUnlockingPositions(king.Offset.Column, competitorQueen.Offset);
            List<Position> listAll = GetAllPosition(motionColor, competitorKing);

            for (int i = 0; i < listPregradi.Count; i++)
            {
                for (int j = 0; j < listAll.Count; j++)
                {
                    if (listAll[j].Equals(listPregradi[i]))
                    {
                        MoveFigure(listPregradi[i].Row, listPregradi[i].Column);
                        history.Add(motion, (Id, new Position(listPregradi[i].Row, listPregradi[i].Column)));
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Сходить на случайную позицию
        /// </summary>
        /// <param name="competitorKing"></param>
        /// <param name="motion"></param>
        /// <param name="history"></param>
        /// <param name="motionQueen"></param>
        /// <returns>1 - если удалось сходить (не на ту же горизонталь), 0 - нет, 2 - ход на той же горизонтали</returns>
        public int RandomMove(FigureKing competitorKing, int motion, Dictionary<int, (int, Position)> history, int motionQueen)
        {
            int position;
            List<Position> list = GetAllPosition(motionQueen, competitorKing);
            Random random = new Random();

            if (list.Count == 0)
                return 0;
            position = random.Next(list.Count);

            if (list[position].Row == Offset.Row)
            {
                MoveFigure(list[position].Row, list[position].Column);
                history.Add(motion, (Id, new Position(list[position].Row, list[position].Column)));
                return 2;
            }

            MoveFigure(list[position].Row, list[position].Column);
            history.Add(motion, (Id, new Position(list[position].Row, list[position].Column)));
            return 1;
        }


        /// <summary>
        /// Проверка: ферзь уже блокирует короля
        /// </summary>
        /// <param name="kingPosition">позиция короля соперника</param>
        /// <returns>true - блокирует, false - не блокирует</returns>
        private bool IsQueenAlreadyBlockingKing(Position kingPosition)
        {
            int k;
            if (Color == Color.Black)
                k = -1;
            else
                k = 1;
            // вправо
            for (int i = kingPosition.Column + 1; i < 8; i++)
            {
                if (GameField.IsInside(kingPosition.Row - k, i))
                    if (GameField[kingPosition.Row - k, i] == Id)
                        return true;
            }
            // влево
            for (int i = kingPosition.Column - 1; i >= 0; i--)
            {
                if (GameField.IsInside(kingPosition.Row - k, i))
                    if (GameField[kingPosition.Row - k, i] == Id)
                        return true;
            }
            return false;
        }


        /// <summary>
        /// Преграждающий ход
        /// </summary>
        /// <param name="competitorKing"></param>
        /// <param name="motionColor"></param>
        /// <param name="history"></param>
        /// <param name="motion"></param>
        /// <returns></returns>
        public bool ObstacleMove(FigureKing competitorKing, int motionColor, Dictionary<int, (int, Position)> history, int motion)
        {
            List<Position> listObstacles = GetObstaclesPosition(competitorKing);
            List<Position> listAll = GetAllPosition(motionColor, competitorKing);
            for (int i = 0; i < listObstacles.Count; i++)
            {
                for (int j = 0; j < listAll.Count; j++)
                {
                    if (listAll[j].Equals(listObstacles[i]) &&
                        history.Count > 1 &&
                        listObstacles[i].Row != CheckPreviousPosition(history) &&
                        !IsQueenAlreadyBlockingKing(competitorKing.Offset))
                    {
                        MoveFigure(listObstacles[i].Row, listObstacles[i].Column);
                        history.Add(motion, (Id, new Position(listObstacles[i].Row, listObstacles[i].Column)));
                        return true;
                        
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// Возвращает строку предыдущего хода
        /// </summary>
        /// <param name="history"></param>
        /// <returns>строка предыдущего хода</returns>
        private int CheckPreviousPosition(Dictionary<int, (int, Position)> history)
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


        /// <summary>
        /// Если возможно, заблокировать короля, иначе - сходить недалеко от своей позиции
        /// </summary>
        /// <param name="competitorKing"></param>
        /// <param name="motionColor"></param>
        /// <param name="history"></param>
        /// <param name="motion"></param>
        /// <returns>true - ход сделан, false - ферзь не смог сходить</returns>
        public bool ObstacleOrNearbyMove(FigureKing competitorKing, int motionColor, Dictionary<int, (int, Position)> history, int motion)
        {
            if ((Color == Color.Black && competitorKing.Offset.Row >= RowConst) ||
                (Color == Color.White && competitorKing.Offset.Row <= RowConst))
            {
                bool check = ObstacleMove(competitorKing, motionColor, history, motion);
                return check;
            }
            else if (motionColor >= 6 &&
                ((Color == Color.Black && competitorKing.Offset.Row < RowConst) ||
                 (Color == Color.White && competitorKing.Offset.Row > RowConst)))
            {
                bool check = NearbyMove(competitorKing.Offset, motion, history, motionColor);
                return check;
            }
            else
                return false;
        }


        /// <summary>
        /// Получить все позиции по горизонтали
        /// </summary>
        /// <param name="kingPosition"></param>
        /// <returns>список позиций</returns>
        private List<Position> GetHorizontalPositions(Position kingPosition)
        {
            List<Position> list = new List<Position>();
            int x = Offset.Row;
            int y = Offset.Column;
            // иду вправо
            for (int i = y + 1; i < 8; i++)
            {
                if (GameField[x, i] == 0)
                {
                    if (CheckQueenAttack(new Position(x, i), kingPosition))
                        list.Add(new Position(x, i));
                }
                else break;
            }
            // иду влево
            for (int i = y - 1; i >= 0; i--)
            {
                if (GameField[x, i] == 0)
                {
                    if (CheckQueenAttack(new Position(x, i), kingPosition))
                        list.Add(new Position(x, i));
                }
                else break;
            }

            return list;
        }


        /// <summary>
        /// Получить все возможные позиции
        /// </summary>
        /// <param name="motionQueen"></param>
        /// <param name="competitorKing"></param>
        /// <returns></returns>
        public List<Position> GetAllPosition(int motionQueen, FigureKing competitorKing = null, Position pos = null)
        {
            int x = Offset.Row;
            int y = Offset.Column;
            Position competitorOffset;
            bool additionalVerification = true;

            if (competitorKing != null)
            {
                competitorOffset = competitorKing.Offset;
            }
            else
            {
                competitorOffset = pos;
                additionalVerification = false;
            }
            List<Position> list = new List<Position>();
            if (motionQueen < 5)
            {
                // иду вправо
                for (int i = y + 1; i < 8; i++)
                {
                    if (GameField[x, i] == 0)
                    {
                        if (CheckQueenAttack(new Position(x, i), competitorOffset, additionalVerification))
                            list.Add(new Position(x, i));
                    }
                    else break;
                }
                // иду влево
                for (int i = y - 1; i >= 0; i--)
                {
                    if (GameField[x, i] == 0)
                    {
                        if (CheckQueenAttack(new Position(x, i), competitorOffset, additionalVerification))
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
                    if (CheckQueenAttack(new Position(i, y), competitorOffset, additionalVerification))
                        list.Add(new Position(i, y));
                }
                else break;
            }
            // иду вверх
            for (int i = x - 1; i >= 0; i--)
            {
                if (GameField[i, y] == 0)
                {
                    if (CheckQueenAttack(new Position(i, y), competitorOffset, additionalVerification))
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
                    if (CheckQueenAttack(new Position(x + i * rowStep, y + i * columnStep), competitorOffset, additionalVerification))
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
                    if (CheckQueenAttack(new Position(x + i * rowStep, y + i * columnStep), competitorOffset, additionalVerification))
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
                    if (CheckQueenAttack(new Position(x + i * rowStep, y + i * columnStep), competitorOffset, additionalVerification))
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
                    if (CheckQueenAttack(new Position(x + i * rowStep, y + i * columnStep), competitorOffset, additionalVerification))
                        list.Add(new Position(x + i * rowStep, y + i * columnStep));
                }
                else break;
            }
            return list;
        }


        /// <summary>
        /// Сходить недалеко от своей позиции
        /// </summary>
        /// <param name="kingPosition"></param>
        /// <param name="motion"></param>
        /// <param name="history"></param>
        /// <returns>true - ход сделан, false - ферзь не смог сходить</returns>
        private bool NearbyMove(Position kingPosition, int motion, Dictionary<int, (int, Position)> history, int motionQueen)
        {
            List<Position> list = new List<Position>() {
                            new Position(1, 0),
                            new Position(1, 1),
                            new Position(1, -1),
                            new Position(-1, 0),
                            new Position(-1, 1),
                            new Position(-1, -1),
                        };
            if (motionQueen < 6)
            {
                list.Add(new Position(0, 1));
                list.Add(new Position(0, -1));
            }
            Random random = new Random();
            while (list.Any())
            {
                int position = random.Next(list.Count);
                int row = Offset.Row + list[position].Row;
                int col = Offset.Column + list[position].Column;
                if (GameField.IsEmpty(row, col)
                    && CheckQueenAttack(new Position(row, col), kingPosition))
                {
                    MoveFigure(row, col);
                    history.Add(motion, (Id, new Position(row, col)));
                    return true;
                }
                else list.RemoveAt(position);
            }
            return false;
        }


        /// <summary>
        /// Получить все позиции, преграждающие путь королю соперника
        /// </summary>
        /// <param name="competitorKing">король соперника</param>
        /// <returns>список позиций</returns>
        public List<Position> GetObstaclesPosition(FigureKing competitorKing = null, Position pos = null)
        {
            List<Position> listRight = new List<Position>();
            List<Position> listLeft = new List<Position>();
            List<Position> list = new List<Position>();
            int column;
            int row;

            if (competitorKing != null)
            {
                column = competitorKing.Offset.Column;
                row = competitorKing.Offset.Row;
            }
            else
            {
                column = pos.Column;
                row = pos.Row;
            }
            int k;
            if (Color == Color.Black)
                k = -1;
            else k = 1;
            // вправо
            for (int i = column + 1; i < 8; i++)
            {
                if (GameField.IsWall(row - k, i))
                    break;
                if (GameField.IsInside(row - k, i) && GameField[row - k, i] == 0)
                    listRight.Add(new Position(row - k, i));
            }
            // влево
            for (int i = column - 1; i >= 0; i--)
            {
                if (GameField.IsWall(row - k, i))
                    break;
                if (GameField.IsInside(row - k, i) && GameField[row - k, i] == 0)
                    listLeft.Add(new Position(row - k, i));
            }
            int limit = Math.Min(listRight.Count, listLeft.Count);
            for (int i = 0; i < limit; i++)
            {
                list.Add(listRight[i]);
                list.Add(listLeft[i]);
            }
            if (listLeft.Count < listRight.Count)
            {
                for (int i = listLeft.Count; i < listRight.Count; i++)
                {
                    list.Add(listRight[i]);
                }
            }
            if (listLeft.Count > listRight.Count)
            {
                for (int i = listRight.Count; i < listLeft.Count; i++)
                {
                    list.Add(listLeft[i]);
                }
            }
            return list;
        }


        /// <summary>
        /// Получить позиции между блокирующим ферзем соперника и королем
        /// </summary>
        /// <param name="kingCol"></param>
        /// <param name="queenCompetitorPosition"></param>
        /// <returns>список позиций</returns>
        private List<Position> GetUnlockingPositions(int kingCol, Position queenCompetitorPosition)
        {
            int queenCompetitorRow = queenCompetitorPosition.Row;
            int queenCompetitorCol = queenCompetitorPosition.Column;
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


        /// <summary>
        /// Проверка: ферзь не бьет короля соперника
        /// </summary>
        /// <param name="positionQueen">рассматриваемая дальнейшая позиция ферзя</param>
        /// <param name="positionKing">позиция короля соперника</param>
        /// <returns>true - не бьет, false - бьет</returns>
        public bool CheckQueenAttack(Position positionQueen, Position positionKing, bool additionalVerification = true)
        {
            int queenRow = positionQueen.Row;
            int queenCol = positionQueen.Column;
            int kingRow = positionKing.Row;
            int kingCol = positionKing.Column;

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
            // находится ли король на той же диагонали, что и ферзь
            int rowDiff = Math.Abs(queenRow - kingRow);
            int colDiff = Math.Abs(queenCol - kingCol);

            if (rowDiff == colDiff)
            {
                // проверить наличие препятствий
                if (queenRow > kingRow)
                {
                    int rowStep = -1;
                    int columnStep = 1;
                    if (queenCol > kingCol)
                        columnStep = -1;
                    for (int i = 1; i < rowDiff; i++)
                    {
                        if (additionalVerification && GameField[queenRow + i * rowStep, queenCol + i * columnStep] < 0 &&
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
                        if (additionalVerification && GameField[queenRow + i * rowStep, queenCol + i * columnStep] < 0 &&
                            GameField[queenRow + i * rowStep, queenCol + i * columnStep] >= -5)
                            return true;
                    }
                }
                return false;
            }
            return true;
        }


        public int DefenderMove(FigureKing king, int motion, Dictionary<int, (int, Position)> history, int motionQueen, List<Position> path)
        {
            int currPositionKingIndex = path.IndexOf(king.Offset);
            //List<Position> list = GetUnlockingPositions(path[currPositionKingIndex + 1].Column, );
            return 0;
        }
    }
}
