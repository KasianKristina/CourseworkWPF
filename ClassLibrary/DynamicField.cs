﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class DynamicField
    {
        private Figure currentDetail;

        public Figure CurrentDetail
        {
            get
            {
                return currentDetail;
            }
            set
            {
                currentDetail = value;
            }
        }

        public Field GameField;
        public Player player1;
        public Player player2;
        public string win = "";
        public int countWinWhite = 0;
        public int countWinBlack = 0;
        public int countPat = 0;
        public int countWin = 0;

        public DynamicField()
        {
            GameField = new Field(8, 8);
            player1 = new Player(Color.White, ref GameField);
            player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
        }

        public delegate void StrategyDelegate(int motion);
        public delegate void PlayerDelegate(int motion, Figure figure, Position pos);

        public void GameStrategy(StrategyDelegate strategy_first, StrategyDelegate strategy_second)
        {
            int motion = 1;
            while (!IsGameOver())
            {
                Console.WriteLine("Ход, {0} ", motion);
                strategy_first(motion);
                if (IsGameOver())
                    break;
                Draw();
                strategy_second(motion);
                motion++;
                Draw();
            }
            Console.WriteLine("Конец игры");
        }

        public void check_delegate(StrategyDelegate strategy_first, StrategyDelegate strategy_second)
        {
            GameStrategy(strategy_first, strategy_second);
        }

        private int motion_with_player = 1;
        public int check_delegate(PlayerDelegate strategy_first, StrategyDelegate strategy_second, Position pos, Figure figure)
        {
            if (IsGameOver())
                return 0;
            Console.WriteLine("Ход, {0} ", motion_with_player);
            strategy_first(motion_with_player, figure, pos);
            if (IsGameOver())
                return 0;
            Draw();
            strategy_second(motion_with_player);
            motion_with_player++;
            Draw();
            Console.WriteLine("Конец игры");
            return 1;
        }

        // компьютер против игрока
        public int check_delegate(StrategyDelegate strategy_first)
        {

            Console.WriteLine("Ход, {0} ", motion_with_player);
            strategy_first(motion_with_player);
            Draw();
            if (IsGameOver())
                return 0;
            return 1;
        }

        // cюда отправляется второй игрок
        // Делегат хода игрока
        public int check_delegate(PlayerDelegate strategy_first, Position pos, Figure figure, bool flaghoda)
        {
            if (IsGameOver())
                return 0;
            Console.WriteLine("Ход, {0} ", motion_with_player);
            strategy_first(motion_with_player, figure, pos);
            if (flaghoda)
                motion_with_player++;

            if (IsGameOver())
                return 0;

            Draw();
            Console.WriteLine("Конец игры");
            return 1;
        }

        public bool IsGameOver()
        {
            if ((GameField[0, 4] == -3) || (GameField[7, 4] == -1))
            {
                if (GameField[0, 4] == -3)
                {
                    win = "Черные фигуры";
                    Count(-3);
                }

                else
                {
                    win = "Белые фигуры";
                    Count(-1);
                }
                return true;
            }
            if (player1.Lose || (player1.king.LeaveSquareFlag == false && motion_with_player > 16))
            {
                win = "Черные фигуры";
                Count(-3);
                return true;
            }
            if (player2.Lose || (player2.king.LeaveSquareFlag == false && motion_with_player > 16))
            {
                win = "Белые фигуры";
                Count(-1);
                return true;
            }
            if (player1.Pat || player2.Pat)
            {
                win = "Ничья";
                return true;
            }
            return false;
        }

        public void Draw()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Console.Write(Math.Abs(GameField[i, j]));
                }
                Console.WriteLine();
            }
            Console.WriteLine("********");
        }

        public void Walls(int quantity)
        {
            Field copy;
            Random random = new Random();
            while (true)
            {
                copy = GameField.Copy();
                int i = 1;
                copy.Draw();
                while (i <= quantity)
                {
                    int x = random.Next(0, 8);
                    int y = random.Next(0, 8);
                    if (copy[x, y] == 0)
                    {
                        copy[x, y] = -5;
                        i++;
                    }
                }
                copy.Draw();
                if (TwoWave(copy))
                    break;
            }
            GameField.Clone(copy);
        }

        public void Count(int flag)
        {
            if (flag == -1)
                countWinWhite++;
            if (flag == -3)
                countWinBlack++;
            countPat = 100 - countWinWhite - countWinBlack;
        }

        public static void Result()
        {
            int checkwinwhite = 0;
            int checkwinblack = 0;
            for (int i = 0; i < 100; i++)
            {
                DynamicField field = new DynamicField();
                field.Walls(10);
                field.check_delegate(field.player1.StrategySecurity, field.player2.StrategyAttackNoTurningBack);
                checkwinwhite += field.countWinWhite;
                checkwinblack += field.countWinBlack;
            }
            Console.WriteLine("Количество побед белых {0}", checkwinwhite);
            Console.WriteLine("Количество побед черных {0}", checkwinblack);
            Console.WriteLine("Пат {0}", 100 - checkwinwhite - checkwinblack);
            Console.ReadKey();
        }


        // поиск двух непересекающихся путей
        // первый найденный путь отмечаем -5 (стены)
        public bool TwoWave(Field copy)
        {
            Field cMap = CreateWave(0, 4, 7, 4, copy);
            Console.WriteLine("Первый непересекающийся путь");
            int result = cMap[7, 4];
            cMap.Draw();
            if (result == -6)
                return false;

            Search(7, 4, result, ref cMap, true);
            cMap.Draw();
            cMap = CreateWave(0, 4, 7, 4, cMap);

            Console.WriteLine("Второй непересекающийся путь");
            cMap.Draw();
            result = cMap[7, 4];
            if (result == -6)
                return false;
            return true;
        }

        // нахождение всего оптимального пути до стартовой позиции короля соперника
        public static List<Position> FindKingPath(int x, int y, int result, ref Field cMap, bool wall)
        {
            List<Position> path = new List<Position>();
            while (result != 1)
            {
                if (cMap.IsEmptyWave(x, y - 1) && cMap[x, y - 1] == result - 1)
                {
                    result = cMap[x, y - 1];
                    if (wall)
                        cMap[x, y] = -5;
                    path.Add(new Position(x, y - 1));
                    y -= 1;
                }
                else if (cMap.IsEmptyWave(x, y + 1) && cMap[x, y + 1] == result - 1)
                {
                    result = cMap[x, y + 1];
                    if (wall)
                        cMap[x, y] = -5;
                    path.Add(new Position(x, y + 1));
                    y += 1;
                }
                else if (cMap.IsEmptyWave(x + 1, y) && cMap[x + 1, y] == result - 1)
                {
                    result = cMap[x + 1, y];
                    if (wall)
                        cMap[x, y] = -5;
                    path.Add(new Position(x + 1, y));
                    x += 1;
                }
                else if (cMap.IsEmptyWave(x - 1, y) && cMap[x - 1, y] == result - 1)
                {
                    result = cMap[x - 1, y];
                    //x = x - 1;
                    if (wall)
                        cMap[x, y] = -5;
                    path.Add(new Position(x - 1, y));
                    x -= 1;
                }
                else if (cMap.IsEmptyWave(x + 1, y + 1) && cMap[x + 1, y + 1] == result - 1)
                {
                    result = cMap[x + 1, y + 1];
                    if (wall)
                        cMap[x, y] = -5;
                    path.Add(new Position(x + 1, y + 1));
                    x += 1;
                    y += 1;
                }
                else if (cMap.IsEmptyWave(x + 1, y - 1) && cMap[x + 1, y - 1] == result - 1)
                {
                    result = cMap[x + 1, y - 1];
                    if (wall)
                        cMap[x, y] = -5;
                    path.Add(new Position(x + 1, y - 1));
                    x += 1;
                    y -= 1;
                }
                else if (cMap.IsEmptyWave(x - 1, y - 1) && cMap[x - 1, y - 1] == result - 1)
                {
                    result = cMap[x - 1, y - 1];
                    if (wall)
                        cMap[x, y] = -5;
                    path.Add(new Position(x - 1, y - 1));
                    x -= 1;
                    y -= 1;
                }
                else if (cMap.IsEmptyWave(x - 1, y + 1) && cMap[x - 1, y + 1] == result - 1)
                {
                    result = cMap[x - 1, y + 1];
                    if (wall)
                        cMap[x, y] = -5;
                    path.Add(new Position(x - 1, y + 1));
                    x -= 1;
                    y += 1;
                }
                else
                {
                    Console.WriteLine("оптимальный путь не найден");
                    return (new List<Position>());
                }
            }
            return path;
        }

        // восстановление пути
        public static (int fx, int fy) Search(int x, int y, int result, ref Field cMap, bool wall)
        {
            while (result != 1)
            {
                if (cMap.IsEmptyWave(x, y - 1) && cMap[x, y - 1] == result - 1)
                {
                    result = cMap[x, y - 1];
                    y = y - 1;
                    if (wall)
                        cMap[x, y] = -5;
                }
                else if (cMap.IsEmptyWave(x, y + 1) && cMap[x, y + 1] == result - 1)
                {
                    result = cMap[x, y + 1];
                    y += 1;
                    if (wall)
                        cMap[x, y] = -5;
                }
                else if (cMap.IsEmptyWave(x + 1, y) && cMap[x + 1, y] == result - 1)
                {
                    result = cMap[x + 1, y];
                    x = x + 1;
                    if (wall)
                        cMap[x, y] = -5;
                }
                else if (cMap.IsEmptyWave(x - 1, y) && cMap[x - 1, y] == result - 1)
                {
                    result = cMap[x - 1, y];
                    x = x - 1;
                    if (wall)
                        cMap[x, y] = -5;
                }
                else if (cMap.IsEmptyWave(x + 1, y + 1) && cMap[x + 1, y + 1] == result - 1)
                {
                    result = cMap[x + 1, y + 1];
                    x = x + 1;
                    y = y + 1;
                    if (wall)
                        cMap[x, y] = -5;
                }
                else if (cMap.IsEmptyWave(x + 1, y - 1) && cMap[x + 1, y - 1] == result - 1)
                {
                    result = cMap[x + 1, y - 1];
                    x = x + 1;
                    y = y - 1;
                    if (wall)
                        cMap[x, y] = -5;
                }
                else if (cMap.IsEmptyWave(x - 1, y - 1) && cMap[x - 1, y - 1] == result - 1)
                {
                    result = cMap[x - 1, y - 1];
                    x = x - 1;
                    y = y - 1;
                    if (wall)
                        cMap[x, y] = -5;
                }
                else if (cMap.IsEmptyWave(x - 1, y + 1) && cMap[x - 1, y + 1] == result - 1)
                {
                    result = cMap[x - 1, y + 1];
                    x = x - 1;
                    y = y + 1;
                    if (wall)
                        cMap[x, y] = -5;
                }
                else
                {
                    Console.WriteLine("оптимальный путь не найден");
                    return (-100, -100);
                }
            }
            return (x, y);
        }

        // распространение волны
        public static Field CreateWave(
            int startX,
            int startY,
            int finishX,
            int finishY,
            Field field,
            Player player = null
            )
        {
            bool add = true;
            int MapX = 8;
            int MapY = 8;
            Field cMap = new Field(8, 8);
            int x, y, step = 0, count = 0;

            for (x = 0; x < MapX; x++)
            {
                for (y = 0; y < MapY; y++)
                {
                    if (field[x, y] != -6 && field[x, y] < 0)
                        cMap[x, y] = -5; // индикатор стены
                    else
                        cMap[x, y] = -6; // индикатор: еще не были здесь

                    if (player != null && !player.king.OpportunityToMakeMove(x, y, player.Сompetitor.queen, player.Сompetitor.king))
                        cMap[x, y] = -5;
                }
            }

            if (player == null)
                cMap[finishX, finishY] = -6;

            cMap[startX, startY] = 0;
            while (add == true)
            {
                count++;
                for (x = 0; x < MapX; x++)
                {
                    for (y = 0; y < MapY; y++)
                    {
                        if (cMap[x, y] == step)
                        {
                            //Ставим значение шага+1 в соседние ячейки (если они проходимы)
                            if (y - 1 >= 0 && cMap[x, y - 1] == -6)
                                cMap[x, y - 1] = step + 1;
                            if (x - 1 >= 0 && cMap[x - 1, y] == -6)
                                cMap[x - 1, y] = step + 1;
                            if (y + 1 < MapX && cMap[x, y + 1] == -6)
                                cMap[x, y + 1] = step + 1;
                            if (x + 1 < MapY && cMap[x + 1, y] == -6)
                                cMap[x + 1, y] = step + 1;
                            if (x + 1 < MapY && y + 1 < MapY && cMap[x + 1, y + 1] == -6)
                                cMap[x + 1, y + 1] = step + 1;
                            if (x + 1 < MapY && y - 1 >= 0 && cMap[x + 1, y - 1] == -6)
                                cMap[x + 1, y - 1] = step + 1;
                            if (x - 1 >= 0 && y - 1 >= 0 && cMap[x - 1, y - 1] == -6)
                                cMap[x - 1, y - 1] = step + 1;
                            if (x - 1 >= 0 && y + 1 < MapY && cMap[x - 1, y + 1] == -6)
                                cMap[x - 1, y + 1] = step + 1;
                        }

                        if (cMap[finishX, finishY] != -6 && cMap[finishX, finishY] != -5)//решение найдено
                        {
                            add = false;
                            break;
                        }
                    }
                }
                step++;
                if (count == 164)
                    add = false;
            }
            return cMap;
        }

        public static int minMax(Player player, int level, Position whiteKingStart, Position whiteQueenStart, Position blackKingStart, Position blackQueenStart, int alpha, int beta, int maxDepth)
        {
            int MIN_VALUE = 400;// проеврить
            int MAX_VALUE = -400;
            int MinMax = player.Color == Color.White ? MIN_VALUE : MAX_VALUE;
            if (level == 0)
                return getResult(player);
            bool isWhitePlayer = (player.Color == Color.White);

            Position whiteQueenPosition = new Position(whiteQueenStart.Row, whiteQueenStart.Column);
            Position whiteKingPosition = new Position(whiteKingStart.Row, whiteKingStart.Column);
            Position blackQueenPosition = new Position(blackQueenStart.Row, blackQueenStart.Column);
            Position blackKingPosition = new Position(blackKingStart.Row, blackKingStart.Column);

            int bestMove = -1;
            bool isKingBestMove = true;

            List<Position> allPositionsQueen = player.queen.GetAllPosition(player.motionColor, player.Сompetitor.king);
            List<Position> allPositionsKing = player.king.GetAllPosition(1, player.motionColor, player.Сompetitor.queen, player.Сompetitor.king, player.queen);

            for (int i = 0; i < allPositionsQueen.Count; i++)
            {
                player.queen.MoveFigure(allPositionsQueen[i].Row, allPositionsQueen[i].Column);
                int test = minMax(player.Сompetitor, level - 1, whiteKingStart, whiteQueenPosition, blackKingStart, blackQueenStart, alpha, beta, maxDepth); 
                // test - разница положения между фигурами соперника и своими фигурами. Чем меньше test, тем меньше выгода у соперника => тем выше выгода игрока
                if (isWhitePlayer)
                    alpha = Math.Max(alpha, test);
                else
                    beta = Math.Min(beta, test);
                if (beta <= alpha)
                    break; // Альфа-бета отсечение
                if ((test < MinMax && isWhitePlayer) || (test > MinMax && !isWhitePlayer)) // каждый игрок пытается максимизировать свою выгоду
                {
                    MinMax = test;
                    bestMove = i;
                    isKingBestMove = false;
                }
                if (player.Color == Color.White)
                    player.queen.MoveFigure(whiteQueenPosition.Row, whiteQueenPosition.Column);
                else player.queen.MoveFigure(blackQueenPosition.Row, blackQueenPosition.Column);
            }

            alpha = int.MinValue;
            beta = int.MaxValue;

            for (int i = 0; i < allPositionsKing.Count; i++)
            {
                player.king.MoveFigure(allPositionsKing[i].Row, allPositionsKing[i].Column);
                int test = minMax(player.Сompetitor, level - 1, whiteKingStart, whiteQueenStart, blackKingStart, blackQueenStart, alpha, beta, maxDepth);
                if (isWhitePlayer)
                    alpha = Math.Max(alpha, test);
                else
                    beta = Math.Min(beta, test);
                if (beta <= alpha)
                    break; // Альфа-бета отсечение
                if ((test < MinMax && isWhitePlayer) || (test > MinMax && !isWhitePlayer)) // белые - максимизируют, черные - минимизируют
                {
                    MinMax = test;
                    bestMove = i;
                    isKingBestMove = true;
                }
                if (player.Color == Color.White)
                    player.king.MoveFigure(whiteKingPosition.Row, whiteKingPosition.Column);
                else player.king.MoveFigure(blackKingPosition.Row, blackKingPosition.Column);
            }

            if (bestMove == -1)
                return getResult(player);
            if (level == maxDepth) // написать другое условие if(level == 1 || level == 2)
                if (isKingBestMove)
                    player.king.MoveFigure(allPositionsKing[bestMove].Row, allPositionsKing[bestMove].Column);
                else player.queen.MoveFigure(allPositionsQueen[bestMove].Row, allPositionsQueen[bestMove].Column);

            return MinMax;
        }

        public int minMax2(Player player, int depth, int alpha, int beta, bool maximizingPlayer)
        {
            if (depth == 0 || IsGameOver())
            {
                return getResult(player);
            }

            if (maximizingPlayer)
            {
                int maxEval = int.MinValue;
                foreach (var move in player.king.GetAllPosition(player.motionColor, player.motionColor, player.Сompetitor.queen, player.Сompetitor.king, player.queen))
                {
                    player.king.MoveFigure(move.Row, move.Column);
                    int eval = minMax2(player.Сompetitor, depth - 1, alpha, beta, false);
                    //player.king.UndoMove(move);
                    maxEval = Math.Max(maxEval, eval);
                    alpha = Math.Max(alpha, eval);
                    if (beta <= alpha)
                        break; // Альфа-бета отсечение
                }
                return maxEval;
            }
            else
            {
                int minEval = int.MaxValue;
                foreach (var move in player.king.GetAllPosition(player.motionColor, player.motionColor, player.Сompetitor.queen, player.Сompetitor.king, player.queen))
                {
                    player.king.MoveFigure(move.Row, move.Column);
                    int eval = minMax2(player.Сompetitor, depth - 1, alpha, beta, true);
                    //player.king.UndoMove(move);
                    minEval = Math.Min(minEval, eval);
                    beta = Math.Min(beta, eval);
                    if (beta <= alpha)
                        break; // Альфа-бета отсечение
                }
                return minEval;
            }
        }

        // получение результата для minMax
        public static int getResult(Player player)
        {
            Field cMap = CreateWave(player.king.Offset.Row, player.king.Offset.Column,
                player.Сompetitor.king.StartOffset.Row, player.Сompetitor.king.StartOffset.Column, player.GameField, player);
            int minimumNumberOfMoves = cMap[player.Сompetitor.king.StartOffset.Row, player.Сompetitor.king.StartOffset.Column];

            Field cMap2 = CreateWave(player.Сompetitor.king.Offset.Row, player.Сompetitor.king.Offset.Column,
                player.king.StartOffset.Row, player.king.StartOffset.Column, player.GameField, player.Сompetitor);
            int minimumNumberOfCompetitorMoves = cMap2[player.king.StartOffset.Row, player.king.StartOffset.Column];

            if (minimumNumberOfMoves == -6)
                minimumNumberOfMoves = 100;
            else if (minimumNumberOfMoves == 1)
                minimumNumberOfMoves = -200;
            else if (minimumNumberOfMoves == 2)
                minimumNumberOfMoves = -100;
            else if (minimumNumberOfMoves == 0)
                minimumNumberOfMoves = -300;

            if (minimumNumberOfCompetitorMoves == -6)
                minimumNumberOfCompetitorMoves = 100;
            else if (minimumNumberOfCompetitorMoves == 1)
                minimumNumberOfCompetitorMoves = -200;
            else if (minimumNumberOfCompetitorMoves == 2)
                minimumNumberOfCompetitorMoves = -100;
            else if (minimumNumberOfCompetitorMoves == 0)
                minimumNumberOfCompetitorMoves = -300;

            if (player.Color == Color.White)
                return minimumNumberOfMoves - minimumNumberOfCompetitorMoves; // белые - черные
            else return minimumNumberOfCompetitorMoves - minimumNumberOfMoves; // черные.competitor (белые) - черные
        }
    }
}
