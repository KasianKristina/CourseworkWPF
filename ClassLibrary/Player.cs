using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                Field cMap = DynamicField.CreateWave(king.Offset.Row, king.Offset.Column, posEnd.Row, posEnd.Column, false, GameField);
                result = cMap[posEnd.Row, posEnd.Column];

                (fx, fy) = DynamicField.Search(posEnd.Row, posEnd.Column, result, ref cMap, false);

                if (fx != -100 &&
                    king.OpportunityToMakeMove(fx, fy, Сompetitor.queen, Сompetitor.king))
                {
                    king.MoveBlock(fx, fy);
                    history.Add(motion, (king.Id, new Position(fx, fy)));
                    Console.WriteLine("king morm");
                    break;
                }
                else
                {
                    if (fx == -100 || (fx, fy) == (posEnd.Row, posEnd.Column))
                    {
                        List<Position> allPositions = king.GetAllPosition(motion, motionColor, Сompetitor.queen, Сompetitor.king, queen);
                        Position position = king.ChooseRandomPosition(allPositions);
                        king.MoveBlock(position.Row, position.Column);
                        history.Add(motion, (king.Id, new Position(position.Row, position.Column)));
                        fx = position.Row;
                        break;
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
            Console.WriteLine("Ходит {0} ", Color);
            if (motion % 6 == 0)
            {
                bool check = queen.RandomMove(Сompetitor.king, motion, history, 7);
                if (!check)
                {
                    if (king.Offset.Row == posEnd.Row && king.Offset.Column == posEnd.Column)
                        Console.WriteLine("Finish. Win is {0} ", Color);
                    else
                    {
                        if (-100 == Wave(king, motion))
                        {
                            check = queen.HorizontalMove(Сompetitor.king.Offset, history, motion);
                            if (check == false)
                            {
                                Console.WriteLine("пат");
                                Pat = true;
                            }
                        }
                    }
                }
            }
            else
            {
                if (king.Offset.Row == posEnd.Row && king.Offset.Column == posEnd.Column)
                    Console.WriteLine("Finish. {0} is win", Color);
                else
                {
                    if (-100 == Wave(king, motion))
                    {
                        bool check = queen.RandomMove(Сompetitor.king, motion, history, 0);
                        if (check == false)
                        {
                            Console.WriteLine("пат");
                            Pat = true;
                        }
                    }
                }
            }
        }

        public void Strategy2(int motion)
        {
            motionColor++;
            Console.WriteLine("Ходит {0} ", Color);

            if (motionColor >= 6)
            {
                bool check = queen.ObstacleMove(Сompetitor.king, motionColor, history, motion);
                if (check)
                {
                    motionColor = 0;
                    return;
                }
                check = queen.RandomMove(Сompetitor.king, motion, history, motion);
                if (check == false)
                {
                    if (-100 == Wave(king, motion))
                    {
                        check = queen.HorizontalMove(Сompetitor.king.Offset, history, motion);
                        if (check == false)
                        {
                            Console.WriteLine("пат");
                            Pat = true;
                            return;
                        }
                    }
                }
                else
                    motionColor = 0;
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
                    check = queen.RandomMove(Сompetitor.king, motion, history, motion);
                    if (check == false)
                    {
                        Console.WriteLine("пат");
                        Pat = true;
                        return;
                    }
                    else
                        motionColor = 0;
                }
            }
        }

        public void StrategySecurity(int motion)
        {
            motionColor++;
            Console.WriteLine("Ходит {0} ", Color);

            if (motionColor >= 6)
            {
                if (queen.ObstacleOrNearbyMove(Сompetitor.king, motionColor, history, motion))
                {
                    motionColor = 0;
                    return;
                }
                if (queen.RandomMove(Сompetitor.king, motion, history, motionColor))
                {
                    motionColor = 0;
                    return;
                }

                if (-100 == Wave(king, motion))
                {
                    if (!queen.HorizontalMove(Сompetitor.king.Offset, history, motion))
                    {
                        Console.WriteLine("пат");
                        Pat = true;
                        return;
                    }
                }
            }
            else
            {
                bool check = queen.ObstacleOrNearbyMove(Сompetitor.king, motionColor, history, motion);
                if (check)
                {
                    motionColor = 0;
                    return;
                }
                if (-100 == Wave(king, motion))
                {
                    check = queen.RandomMove(Сompetitor.king, motion, history, motionColor);
                    if (!check)
                    {
                        Console.WriteLine("пат");
                        Pat = true;
                        return;
                    }
                    else
                        motionColor = 0;
                }
            }
        }

        public void StrategyUser(int motion, Figure figure, Position pos)
        {
            // TODO добавить проверку на пат
            motionColor++;
            Console.WriteLine("Ходит {0} ", Color);
            if (figure.Id == -2 && pos.Row != figure.Offset.Row)
                motionColor = 0;
            figure.MoveBlock(pos.Row, pos.Column);
            history.Add(motion, (figure.Id, pos));
        }

        public void Strategy4(int motion)
        {
            motionColor++;
            Console.WriteLine("Ходит {0} ", Color);

            if (motionColor >= 6)
            {
                bool check = queen.CheckPregradaMove(Сompetitor.king, motion, motionColor, history, king, Сompetitor.queen);
                if (check)
                {
                    motionColor = 0;
                    return;
                }

                check = queen.RandomMove(Сompetitor.king, motion, history, motionColor);
                if (check)
                {
                    motionColor = 0;
                    return;
                }

                if (-100 == Wave(king, motion))
                {
                    check = queen.HorizontalMove(Сompetitor.king.Offset, history, motion);
                    if (!check)
                    {
                        Console.WriteLine("пат");
                        Pat = true;
                        return;
                    }
                }
            }
            else
            {
                bool check = queen.CheckPregradaMove(Сompetitor.king, motion, motionColor, history, king, Сompetitor.queen);
                if (check)
                {
                    motionColor = 0;
                    return;
                }
                if (-100 == Wave(king, motion))
                {
                    if (!check)
                    {
                        check = queen.RandomMove(Сompetitor.king, motion, history, motionColor);
                        if (check)
                        {
                            motionColor = 0;
                            return;
                        }
                        check = queen.HorizontalMove(Сompetitor.king.Offset, history, motion);
                        if (!check)
                        {
                            Console.WriteLine("пат");
                            Pat = true;
                            return;
                        }
                    }
                }
            }
        }
    }
}
