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

        public int check_delegate(StrategyDelegate strategy_first, PlayerDelegate strategy_second, Position pos, Figure figure)
        {
            if (IsGameOver())
                return 0;
            Console.WriteLine("Ход, {0} ", motion_with_player);
            strategy_first(motion_with_player);

            if (IsGameOver())
                return 0;
            Draw();
            strategy_second(motion_with_player, figure, pos);
            motion_with_player++;
            Draw();
            Console.WriteLine("Конец игры");
            return 1;
        }

        public int check_delegate(PlayerDelegate strategy_first, PlayerDelegate strategy_second, Position pos, Figure figure)
        {
            if (IsGameOver())
                return 0;
            Console.WriteLine("Ход, {0} ", motion_with_player);
            strategy_first(motion_with_player, figure, pos);

            if (IsGameOver())
                return 0;
            Draw();
            strategy_second(motion_with_player, figure, pos);
            motion_with_player++;
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

        public void WallsTest()
        {
            Field copy;

            copy = GameField.Copy();

            copy[0, 2] = -5;
            copy[0, 5] = -5;
            copy[0, 7] = -5;
            copy[1, 1] = -5;
            copy[1, 2] = -5;
            copy[1, 5] = -5;
            copy[1, 6] = -5;
            copy[2, 0] = -5;
            copy[2, 3] = -5;
            copy[2, 4] = -5;
            copy[2, 6] = -5;
            copy[2, 7] = -5;
            copy[3, 2] = -5;
            copy[3, 5] = -5;
            copy[3, 7] = -5;
            copy[4, 0] = -5;
            copy[4, 2] = -5;
            copy[4, 3] = -5;
            copy[4, 5] = -5;
            copy[4, 7] = -5;
            copy[5, 0] = -5;
            copy[5, 2] = -5;
            copy[5, 6] = -5;
            copy[6, 0] = -5;
            copy[6, 7] = -5;
            copy[7, 0] = -5;
            copy[7, 1] = -5;
            copy[7, 5] = -5;
            copy[7, 6] = -5;
            copy[7, 7] = -5;

            copy.Draw();
           
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
                field.Walls(30);
                field.check_delegate(field.player1.Strategy4, field.player2.Strategy4);
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

        // поиск позиции, на которую нужно идти королю
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
                    y = y + 1;
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

        public static Field CreateWave(int startX, int startY, int finishX, int finishY, Field field)
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
                }
            }

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
    }
}
