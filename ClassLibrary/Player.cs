﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Player
    {
        public FigureKing king;
        public FigureQueen queen;
        public Color Color;
        public Field GameField;
        public Player Сompetitor { get; set; }
        public bool Pat = false;
        public bool Lose = false;
        public int motionColor = 0;
        public Position posEnd;
        public Dictionary<int, (int, Position)> history;

        public Player(Color color, ref Field GameField)
        {
            Color = color;
            this.GameField = GameField;
            king = new FigureKing(ref GameField, color);
            queen = new FigureQueen(ref GameField, color);
            posEnd = king.EndingPosition;
            history = new Dictionary<int, (int, Position)>();
        }

        public int Wave(FigureKing king, int motion)
        {
            int result, fx, fy;
            while (true)
            {
                Field cMap = DynamicField.CreateWave(king.Offset.Row, king.Offset.Column, posEnd.Row, posEnd.Column, GameField);
                result = cMap[posEnd.Row, posEnd.Column];

                (fx, fy) = DynamicField.Search(posEnd.Row, posEnd.Column, result, ref cMap, false);

                if (fx != -100 &&
                    king.OpportunityToMakeMove(fx, fy, Сompetitor.queen, Сompetitor.king))
                {
                    king.MoveBlock(fx, fy);
                    history.Add(motion, (king.Id, new Position(fx, fy)));
                    break;
                }
                else
                {
                    if (fx == -100 || (fx, fy) == (posEnd.Row, posEnd.Column))
                    {
                        List<Position> allPositions = king.GetAllPosition(motion, motionColor, Сompetitor.queen, Сompetitor.king, queen);
                        if (allPositions.Count != 0)
                        {
                            Position position = king.ChooseRandomPosition(allPositions);
                            king.MoveBlock(position.Row, position.Column);
                            history.Add(motion, (king.Id, new Position(position.Row, position.Column)));
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
            ClearGameField();
            return fx;
        }

        private void ClearGameField()
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
        public void StrategySimple(int motion)
        {
            motionColor++;
            Console.WriteLine("Ходит {0} ", Color);
            if (motion < 5)
            {
                if (queen.CheckStartingBarriers(history, motion, Сompetitor.king.Offset))
                    return;
            }
            if (motionColor >= 6)
            {
                int check = queen.RandomMove(Сompetitor.king, motion, history, motionColor);
                if (check == 1)
                {
                    motionColor = 0;
                    return;
                }
                if (check == 2)
                {
                    // проверка
                    return;
                }
                if (check == 0) { 
                    if (queen.CheckLoseGame(Сompetitor.queen.Id, Сompetitor.king.Offset))
                    {
                        if (-100 == Wave(king, motion))
                        {
                            Pat = true;
                            return;
                        }
                    }
                    else
                    {
                        Lose = true;
                        return;
                    }
                }
            }
            else
            {
                if (-100 == Wave(king, motion))
                {
                    int check = queen.RandomMove(Сompetitor.king, motion, history, motionColor);
                    if (check == 1)
                    {
                        motionColor = 0;
                        return;
                    }
                    if (check == 0)
                    {
                        if (queen.CheckLoseGame(Сompetitor.queen.Id, Сompetitor.king.Offset))
                        {
                            Pat = true;
                            return;
                        }
                        else
                        {
                            Lose = true;
                            return;
                        }
                    }
                }
            }
        }

        public void Strategy2(int motion)
        {
            motionColor++;
            Console.WriteLine("Ходит {0} ", Color);
            if (motion < 5)
            {
                if (queen.CheckStartingBarriers(history, motion, Сompetitor.king.Offset))
                    return;
            }
            if (motionColor >= 6)
            {
                if (queen.ObstacleMove(Сompetitor.king, motionColor, history, motion))
                {
                    motionColor = 0;
                    return;
                }
                int check = queen.RandomMove(Сompetitor.king, motion, history, motion);
                if (check == 1)
                {
                    motionColor = 0;
                    return;
                }
                if (check == 0)
                {
                    if (queen.CheckLoseGame(Сompetitor.queen.Id, Сompetitor.king.Offset))
                    {
                        if (-100 == Wave(king, motion))
                        {
                            Pat = true;
                            return;
                        }
                    }
                    else
                    {
                        Lose = true;
                        return;
                    };
                }
            }
            else
            {
                bool check = queen.ObstacleMove(Сompetitor.king, motionColor, history, motion);
                if (check)
                {
                    motionColor = 0;
                    return;
                }
                if (-100 == Wave(king, motion))
                {
                    int checkRandomMove = queen.RandomMove(Сompetitor.king, motion, history, motion);
                    if (checkRandomMove == 1)
                    {
                        motionColor = 0;
                        return;
                    }
                    if (checkRandomMove == 0)
                    {
                        if (queen.CheckLoseGame(Сompetitor.queen.Id, Сompetitor.king.Offset))
                        {
                            Pat = true;
                            return;
                        }
                        else
                        {
                            Lose = true;
                            return;
                        }
                    }
                }
            }
        }

        public void StrategySecurity(int motion)
        {
            motionColor++;
            Console.WriteLine("Ходит {0} ", Color);
            if (motion < 5)
            {
                if (queen.CheckStartingBarriers(history, motion, Сompetitor.king.Offset))
                    return;
            }
            if (motionColor >= 6)
            {
                if (queen.ObstacleOrNearbyMove(Сompetitor.king, motionColor, history, motion))
                {
                    motionColor = 0;
                    return;
                }
                int check = queen.RandomMove(Сompetitor.king, motion, history, motionColor);
                if (check == 1)
                {
                    motionColor = 0;
                    return;
                }
                if (check == 0)
                {
                    if (queen.CheckLoseGame(Сompetitor.queen.Id, Сompetitor.king.Offset))
                    {
                        if (-100 == Wave(king, motion))
                        {
                            Pat = true;
                            return;
                        }
                    }
                    else
                    {
                        Lose = true;
                        return;
                    }
                }
            }
            else
            {
                if (queen.ObstacleOrNearbyMove(Сompetitor.king, motionColor, history, motion))
                {
                    motionColor = 0;
                    return;
                }
                if (-100 == Wave(king, motion))
                {
                    int check = queen.RandomMove(Сompetitor.king, motion, history, motionColor);
                    if (check == 1)
                    {
                        motionColor = 0;
                        return;
                    }
                    if (check == 0)
                    {
                        if (queen.CheckLoseGame(Сompetitor.queen.Id, Сompetitor.king.Offset))
                        {
                            Pat = true;
                            return;
                        }
                        else
                        {
                            Lose = true;
                            return;
                        }
                    }
                }
            }
        }

        public void CheckPat(int motion)
        {
            List<Position> listPositionsQueen = queen.GetAllPosition(motionColor, Сompetitor.king);
            List<Position> listPositionsKing = king.GetAllPosition(motion, motionColor, Сompetitor.queen, Сompetitor.king, queen);
            if (listPositionsKing.Count == 0 && listPositionsQueen.Count == 0)
                Pat = true;
        }

        public void StrategyUser(int motion, Figure figure, Position pos)
        {
            motionColor++;
            Console.WriteLine("Ходит {0} ", Color);
            if ((figure.Id == -4 || figure.Id == -2) && pos.Row != figure.Offset.Row)
                motionColor = 0;
            figure.MoveBlock(pos.Row, pos.Column);
            history.Add(motion, (figure.Id, pos));
            CheckPat(motion);
            if (motionColor > 5)
            {
                if (!queen.CheckLoseGame(Сompetitor.queen.Id, Сompetitor.king.Offset))
                {
                    Lose = true;
                    return;
                };
            }
        }

        public void Strategy4(int motion)
        {
            motionColor++;
            Console.WriteLine("Ходит {0} ", Color);
            if (motion < 5)
            {
                if (queen.CheckStartingBarriers(history, motion, Сompetitor.king.Offset))
                    return;
            }

            if (motionColor >= 6)
            {
                if (queen.CheckUnlockingMove(Сompetitor.king, motion, motionColor, history, king, Сompetitor.queen))
                {
                    motionColor = 0;
                    return;
                }
                int check = queen.RandomMove(Сompetitor.king, motion, history, motionColor);
                if (check == 1)
                {
                    motionColor = 0;
                    return;
                }
                if (check == 0)
                {
                    if (queen.CheckLoseGame(Сompetitor.queen.Id, Сompetitor.king.Offset))
                    {
                        if (-100 == Wave(king, motion))
                        {
                            Pat = true;
                            return;
                        }
                    }
                    else
                    {
                        Lose = true;
                        return;
                    }
                }
            }
            else
            {
                if (queen.CheckUnlockingMove(Сompetitor.king, motion, motionColor, history, king, Сompetitor.queen))
                {
                    motionColor = 0;
                    return;
                }
                if (-100 == Wave(king, motion))
                {
                    int check = queen.RandomMove(Сompetitor.king, motion, history, motionColor);
                    if (check == 1)
                    {
                        motionColor = 0;
                        return;
                    }
                    if (check == 0)
                    {
                        if (queen.CheckLoseGame(Сompetitor.queen.Id, Сompetitor.king.Offset))
                        {
                            Pat = true;
                            return;
                        }
                        else
                        {
                            Lose = true;
                            return;
                        }
                    }
                }
            }
        }
    }
}
