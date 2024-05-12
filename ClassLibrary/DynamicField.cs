using System;
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
            List<string> dsa = new List<string>();

            int checkwinwhite = 0;
            int checkwinblack = 0;
            for (int i = 0; i < 100; i++)
            {
                DynamicField field = new DynamicField();
                field.Walls(10);
                field.check_delegate(field.player1.StrategySimpleSameWay, field.player2.StrategySimple);
                checkwinwhite += field.countWinWhite;
                checkwinblack += field.countWinBlack;
            }
            dsa.Add($"{checkwinwhite}/{checkwinblack}/{100 - checkwinwhite - checkwinblack}");
            checkwinwhite = 0;
            checkwinblack = 0;
            for (int i = 0; i < 100; i++)
            {
                DynamicField field = new DynamicField();
                field.Walls(20);
                field.check_delegate(field.player1.StrategySimpleSameWay, field.player2.StrategySimple);
                checkwinwhite += field.countWinWhite;
                checkwinblack += field.countWinBlack;
            }
            dsa.Add($"{checkwinwhite}/{checkwinblack}/{100 - checkwinwhite - checkwinblack}");

            checkwinwhite = 0;
            checkwinblack = 0;
            for (int i = 0; i < 100; i++)
            {
                DynamicField field = new DynamicField();
                field.Walls(30);
                field.check_delegate(field.player1.StrategySimpleSameWay, field.player2.StrategySimple);
                checkwinwhite += field.countWinWhite;
                checkwinblack += field.countWinBlack;
            }
            dsa.Add($"{checkwinwhite}/{checkwinblack}/{100 - checkwinwhite - checkwinblack}");

            foreach (var name in dsa)
            {
                Console.WriteLine($"{name}");
            }
            
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

        public static int MinMax(
            Player player,
            int level,
            Position whiteKingStart,
            Position whiteQueenStart,
            Position blackKingStart,
            Position blackQueenStart,
            int alpha,
            int beta,
            int maxDepth,
            //Dictionary<int, (int, Position)> history,
            int motion,
            Field field
            //int motionColor
            )
        {
            int MIN_VALUE = 400;
            int MAX_VALUE = -400;
            int MinMax = player.Color == Color.White ? MIN_VALUE : MAX_VALUE;
            if (level == 0)
                return GetResultMinMax(player);
            bool isWhitePlayer = (player.Color == Color.White);
            Field virtualField = field.Copy();

            Position whiteQueenPosition = new Position(whiteQueenStart.Row, whiteQueenStart.Column);
            Position whiteKingPosition = new Position(whiteKingStart.Row, whiteKingStart.Column);
            Position blackQueenPosition = new Position(blackQueenStart.Row, blackQueenStart.Column);
            Position blackKingPosition = new Position(blackKingStart.Row, blackKingStart.Column);

            int bestMove = -1;
            bool isKingBestMove = true;

            List<Position> allPositionsQueen = player.queen.GetAllPosition(player.motionColor, player.Сompetitor.king);
            List<Position> allPositionsKing = player.motionColor < 6 || (player.motionColor >= 6 && player.queen.CheckLoseGame(player.Сompetitor.queen.Id, player.king.Offset)) ?
                player.king.GetAllPosition(1, player.motionColor, player.Сompetitor.queen, player.Сompetitor.king, player.queen) :
                new List<Position>();

            for (int i = 0; i < allPositionsKing.Count; i++)
            {
                player.king.MoveFigureVirtualField(allPositionsKing[i].Row, allPositionsKing[i].Column, virtualField);
                int test = DynamicField.MinMax(
                    player.Сompetitor,
                    level - 1,
                    whiteKingStart,
                    whiteQueenStart,
                    blackKingStart,
                    blackQueenStart,
                    alpha,
                    beta,
                    maxDepth,
                    //history,
                    motion,
                    field
                    //motionColor
                    );

                //if (isWhitePlayer)
                //    alpha = Math.Max(alpha, test);
                //else
                //    beta = Math.Min(beta, test);
                //if (beta <= alpha)
                //{
                //    if (player.Color == Color.White)
                //        player.king.MoveFigureVirtualField(whiteKingPosition.Row, whiteKingPosition.Column, virtualField);
                //    else
                //        player.king.MoveFigureVirtualField(blackKingPosition.Row, blackKingPosition.Column, virtualField);
                //    break; // Альфа-бета отсечение
                //}

                if ((test < MinMax && isWhitePlayer) || (test > MinMax && !isWhitePlayer))
                {
                    MinMax = test;
                    bestMove = i;
                    isKingBestMove = true;
                }
                if (player.Color == Color.White)
                    player.king.MoveFigureVirtualField(whiteKingPosition.Row, whiteKingPosition.Column, virtualField);
                else
                    player.king.MoveFigureVirtualField(blackKingPosition.Row, blackKingPosition.Column, virtualField);
            }

            alpha = int.MinValue;
            beta = int.MaxValue;

            for (int i = 0; i < allPositionsQueen.Count; i++)
            {
                player.queen.MoveFigureVirtualField(allPositionsQueen[i].Row, allPositionsQueen[i].Column, virtualField);
                int test = DynamicField.MinMax(
                    player.Сompetitor,
                    level - 1,
                    whiteKingStart,
                    whiteQueenPosition,
                    blackKingStart,
                    blackQueenStart,
                    alpha,
                    beta,
                    maxDepth,
                    //history,
                    motion,
                    field
                    //motionColor
                    );

                //if (isWhitePlayer)
                //    alpha = Math.Max(alpha, test);
                //else
                //    beta = Math.Min(beta, test);
                //if (beta <= alpha)
                //{
                //    if (player.Color == Color.White)
                //        player.queen.MoveFigureVirtualField(whiteQueenPosition.Row, whiteQueenPosition.Column, virtualField);
                //    else
                //        player.queen.MoveFigureVirtualField(blackQueenPosition.Row, blackQueenPosition.Column, virtualField);
                //    break; // Альфа-бета отсечение
                //}

                if ((test < MinMax && isWhitePlayer) || (test > MinMax && !isWhitePlayer))
                {
                    MinMax = test;
                    bestMove = i;
                    isKingBestMove = false;
                }
                if (player.Color == Color.White)
                    player.queen.MoveFigureVirtualField(whiteQueenPosition.Row, whiteQueenPosition.Column, virtualField);
                else
                    player.queen.MoveFigureVirtualField(blackQueenPosition.Row, blackQueenPosition.Column, virtualField);

            }

            if (bestMove == -1)
                //return getResult(player);
                return 1000;
            if (level == maxDepth)
                if (isKingBestMove)
                {
                    player.king.MoveFigure(allPositionsKing[bestMove].Row, allPositionsKing[bestMove].Column);
                    player.history.Add(motion, (player.king.Id, new Position(allPositionsKing[bestMove].Row, allPositionsKing[bestMove].Column)));
                    return 1;
                }

                else
                {
                    player.queen.MoveFigure(allPositionsQueen[bestMove].Row, allPositionsQueen[bestMove].Column);
                    player.history.Add(motion, (player.queen.Id, new Position(allPositionsQueen[bestMove].Row, allPositionsQueen[bestMove].Column)));
                    if (allPositionsQueen[bestMove].Row == whiteQueenStart.Row && player.Color == Color.White ||
                        allPositionsQueen[bestMove].Row == blackQueenStart.Row && player.Color == Color.Black)
                        return 2; // та же диагональ   TODO: сделать другие возвращаемые значения
                    else return 3; // смена диагонали 
                }

            return MinMax;
        }

        // получение результата для minMax
        public static int GetResultMinMax(Player player)
        {
            Field cMap = CreateWave(player.king.Offset.Row, player.king.Offset.Column,
                player.Сompetitor.king.StartOffset.Row, player.Сompetitor.king.StartOffset.Column, player.GameField, player);
            int minimumNumberOfMoves = cMap[player.Сompetitor.king.StartOffset.Row, player.Сompetitor.king.StartOffset.Column];

            Field cMap2 = CreateWave(player.Сompetitor.king.Offset.Row, player.Сompetitor.king.Offset.Column,
                player.king.StartOffset.Row, player.king.StartOffset.Column, player.GameField, player.Сompetitor);
            int minimumNumberOfCompetitorMoves = cMap2[player.king.StartOffset.Row, player.king.StartOffset.Column];

            if (minimumNumberOfMoves == -6) // перекрывают путь
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
