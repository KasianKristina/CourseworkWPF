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
            posEnd = king.endingPosition;
            history = new Dictionary<int, (int, Position)>();
        }

        public int Wave(FigureKing figure, int motion)
        {
            int result, fx, fy;
            while (true)
            {
                Field cMap = DynamicField.CreateWave(figure.offset.Row, figure.offset.Column, posEnd.Row, posEnd.Column, false, GameField);
                result = cMap[posEnd.Row, posEnd.Column];

                (fx, fy) = DynamicField.Search(posEnd.Row, posEnd.Column, result, ref cMap, false);

                if (fx != -100 && checkXodKing(fx, fy, motion))
                {
                    figure.MoveBlock(fx, fy);
                    history.Add(motion, (figure.Id, new Position(fx, fy)));
                    break;
                }
                else
                {
                    if (fx == -100 || (fx, fy) == (posEnd.Row, posEnd.Column))
                    {
                        List<Position> list = new List<Position>() {
                            new Position(0, 1),
                            new Position(0, -1),
                            new Position(1, 0),
                            new Position(1, 1),
                            new Position(1, -1),
                            new Position(-1, 0),
                            new Position(-1, 1),
                            new Position(-1, -1),
                        };
                        while (list.Any())
                        {
                            Position pos = king.RandomXodKing(list);
                            if (GameField.IsInside(king.offset.Row + pos.Row, king.offset.Column + pos.Column) &&
                                checkXodKing(king.offset.Row + pos.Row, king.offset.Column + pos.Column, motion))
                            {
                                figure.MoveBlock(king.offset.Row + pos.Row, king.offset.Column + pos.Column);
                                history.Add(motion, (king.Id, new Position(king.offset.Row + pos.Row, king.offset.Column + pos.Column)));
                                fx = pos.Row;
                                break;
                            }
                            else
                                list.Remove(pos);
                        }
                        if (list.Count == 0)
                            fx = -100;
                        break;
                    }
                    GameField[fx, fy] = -7;
                    //queen.ObstaclesQueenCheck();
                }

                cMap.Draw();
            }
            clearGameField();
            return fx;
        }
        private void clearGameField()
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

        public bool checkXodKing(int x, int y, int motion)
        {
            if (!Сompetitor.queen.CheckQueenAttack(Сompetitor.queen.offset.Row, Сompetitor.queen.offset.Column, x, y) ||
               king.AdjacentPosition(x, y, Сompetitor.king.offset.Row, Сompetitor.king.offset.Column) ||
               king.LeaveSquare(x, y, motion) ||
               !GameField.IsEmptyWave(x, y))
                return false;
            else
                return true;
        }

        public void StrategySimple(int motion)
        {
            Console.WriteLine("Ходит {0} ", Color);
            if (motion % 6 == 0)
            {
                bool check = queen.RandomMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column, motion, history, 7);
                if (!check)
                {
                    if (king.offset.Row == posEnd.Row && king.offset.Column == posEnd.Column)
                        Console.WriteLine("Finish. Win is {0} ", Color);
                    else
                    {
                        if (-100 == Wave(king, motion))
                        {
                            check = queen.HorizontalMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column, history, motion);
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
                if (king.offset.Row == posEnd.Row && king.offset.Column == posEnd.Column)
                    Console.WriteLine("Finish. {0} is win", Color);
                else
                {
                    if (-100 == Wave(king, motion))
                    {
                        bool check = queen.RandomMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column, motion, history, 0);
                        if (check == false)
                        {
                            Console.WriteLine("пат");
                            Pat = true;
                        }
                    }
                }
            }
        }

        public void PrintHistory()
        {
            if (history is null)
                Console.WriteLine("Hisory null");
            else
                foreach (var stat in history)
                {
                    Console.WriteLine("Hisory {0}, {1}", stat.Value.Item2.Row, stat.Value.Item2.Column);
                }
        }

        public void Str(int motion)
        {
            motionColor++;
            Console.WriteLine("Ходит {0} ", Color);

            if (motionColor >= 6)
            {
                bool check = queen.ObstacleMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column, Color, motionColor, history, motion);
                if (check)
                {
                    motionColor = 0;
                    return;
                }
                check = queen.RandomMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column, motion, history, motion);
                if (check == false)
                {
                    if (-100 == Wave(king, motion))
                    {
                        check = queen.HorizontalMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column, history, motion);
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
                bool check = queen.ObstacleMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column, Color, motionColor, history, motion);
                if (check)
                {
                    motionColor = 0;
                    return;
                }
                if (-100 == Wave(king, motion))
                {
                    check = queen.RandomMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column, motion, history, motion);
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

                if (queen.ObstacleOrNearbyMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column, Color, motionColor, history, motion))
                {
                    motionColor = 0;
                    return;
                }
                if (queen.RandomMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column, motion, history, motionColor))
                {
                    motionColor = 0;
                    return;
                }

                if (-100 == Wave(king, motion))
                {
                    if (!queen.HorizontalMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column, history, motion))
                    {
                        Console.WriteLine("пат");
                        Pat = true;
                        return;
                    }
                }
            }
            else
            {
                bool check = queen.ObstacleOrNearbyMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column, Color, motionColor, history, motion);
                if (check)
                {
                    //TODO  УБРАТЬ  в функцию и уьрать тогда чек
                    motionColor = 0;
                    return;
                }
                if (-100 == Wave(king, motion))
                {
                    //TOOD УБРАТЬ -100 на что-то другое
                    check = queen.RandomMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column, motion, history, motionColor);
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

        public void Strategy4(int motion)
        {
            motionColor++;
            Console.WriteLine("Ходит {0} ", Color);

            if (motionColor >= 6)
            {
                bool check = queen.CheckPregradaMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column, Сompetitor.queen.offset.Row, Сompetitor.queen.offset.Row, motion, Color, motionColor, history);
                if (check)
                {
                    motionColor = 0;
                    return;
                }

                check = queen.RandomMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column, motion, history, motionColor);
                if (check)
                {
                    motionColor = 0;
                    return;
                }

                if (-100 == Wave(king, motion))
                {
                    check = queen.HorizontalMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column, history, motion);
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
                bool check = queen.CheckPregradaMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column, Сompetitor.queen.offset.Row, Сompetitor.queen.offset.Row, motion, Color, motionColor, history);
                if (check)
                {
                    motionColor = 0;
                    return;
                }
                if (-100 == Wave(king, motion))
                {
                    if (!check)
                    {
                        check = queen.RandomMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column, motion, history, motionColor);
                        if (check)
                        {
                            motionColor = 0;
                            return;
                        }
                        check = queen.HorizontalMove(Сompetitor.king.offset.Row, Сompetitor.king.offset.Column, history, motion);
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
