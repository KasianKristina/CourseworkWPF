using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Field
    {
        private int[,] field;
        public int Rows { get; set; }
        public int Columns { get; set; }
        public Dictionary<int, Position> Positions { get; set; }

        public int this[int row, int column]
        {
            get
            {
                return field[row, column];
            }
            set
            {
                field[row, column] = value;
            }
        }

        public Field(int row, int column)
        {
            Rows = row;
            Columns = column;
            field = new int[row, column];
            Positions = new Dictionary<int, Position>();
        }

        // находится ли клетка внутри поля
        public bool IsInside(int row, int column)
        {
            if (row >= 0 && row < Rows
                    && column >= 0 && column < Columns)
                return true;
            else return false;
        }

        // пустая ли клетка
        public bool IsEmpty(int row, int column)
        {
            if (IsInside(row, column) && field[row, column] == 0)
                return true;
            else return false;
        }

        // пустая ли клетка (для проверки на карте cMap, где расставлены значения путей)
        public bool IsEmptyWave(int row, int column)
        {
            if (IsInside(row, column) && field[row, column] >= 0)
                return true;
            else return false;
        }

        public bool IsWall(int row, int column)
        {
            if (IsInside(row, column) && field[row, column] != 0)
                return true;
            else return false;
        }

        public List<Position> IsIsolatedPositions()
        {
            int result;
            List<Position> listCheck = new List<Position>();

            List<Position> list = new List<Position>() {
                            new Position(0, 3),
                            new Position(0, 5),
                            new Position(1, 3),
                            new Position(1, 4),
                            new Position(1, 5),
                            new Position(0, 4)
                        };

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (list.Contains(new Position(i, j)) == false)
                    {
                        Field cMap = DynamicField.CreateWave(i, j, 0, 4, this);
                        result = cMap[0, 4];

                        List<Position> path = DynamicField.FindKingPath(0, 4, result, ref cMap, false);

                        if (path.Count == 0)
                            listCheck.Add(new Position(i, j));
                    }
                }
            }
            return listCheck;
        }

        public bool IsIsolatedPositionIndividual(int row, int column)
        {
            int result;

            List<Position> list = new List<Position>() {
                            new Position(0, 3),
                            new Position(0, 5),
                            new Position(1, 3),
                            new Position(1, 4),
                            new Position(1, 5),
                            new Position(0, 4)
                        };


            if (list.Contains(new Position(row, column)) == false)
            {
                Field cMap = DynamicField.CreateWave(row, column, 0, 4, this);
                result = cMap[0, 4];

                List<Position> path = DynamicField.FindKingPath(0, 4, result, ref cMap, false);

                if (path.Count == 0)
                    return true;
                else return false;
            }
            return false;
        }

        public bool IsCorridor(int row, int column)
        {
            if (IsWall(row, column)) return false;
            //if (IsIsolatedPosition(row, column, kingRow, kingColumn)) return false;
            if ((column == 0 && (IsWall(row, column + 1) || IsWall(row, column + 2))) ||
                (column == 1 && IsWall(row, column + 1)) ||
                (IsWall(row, column - 1) && (IsWall(row, column + 1) || IsWall(row, column + 2))) ||
                (IsWall(row, column + 1) && IsWall(row, column - 2)) ||
                (column == 7 && (IsWall(row, column - 1) || IsWall(row, column - 2))) ||
                (column == 6 && IsWall(row, column - 1)))
                return true;
            return false;
        }

        public bool Equals(Field other)
        {
            if (other == null || Positions.Count != other.Positions.Count)
                return false;

            foreach (var kvp in Positions)
            {
                if (!other.Positions.TryGetValue(kvp.Key, out var position) || !kvp.Value.Equals(position))
                    return false;
            }

            return true;
        }

        public bool HasConfigurationOccurredFiveTimes(Dictionary<int, (int, Position)> history1, Dictionary<int, (int, Position)> history2)
        {
            if (history1.Keys.Count != 0 && history2.Keys.Count != 0)
            {
                int maxMove = Math.Max(history1.Keys.Max(), history2.Keys.Max());

                // Используем строковое представление состояния доски в качестве ключа
                Dictionary<string, int> boardStateOccurrences = new Dictionary<string, int>();

                for (int move = 1; move <= maxMove; move++)
                {
                    Field boardState = new Field(8, 8);

                    // Восстанавливаем состояние доски для каждого игрока
                    foreach (var history in new[] { history1, history2 })
                    {
                        foreach (var entry in history.Where(e => e.Key <= move))
                        {
                            boardState.Positions[entry.Value.Item1] = entry.Value.Item2;
                        }
                    }

                    // Получаем строковое представление состояния доски
                    string boardStateKey = GetBoardStateKey(boardState);

                    // Увеличиваем количество встречаемости состояния доски, если оно уже встречалось
                    if (boardStateOccurrences.TryGetValue(boardStateKey, out int count))
                    {
                        boardStateOccurrences[boardStateKey] = count + 1;
                    }
                    else
                    {
                        boardStateOccurrences[boardStateKey] = 1;
                    }

                    // Проверяем, встречалось ли текущее состояние доски 5 раз
                    if (boardStateOccurrences[boardStateKey] == 5)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private string GetBoardStateKey(Field boardState)
        {
            var positions = boardState.Positions.OrderBy(kvp => kvp.Key).Select(kvp => $"{kvp.Key}:{kvp.Value.Row},{kvp.Value.Column}");
            return string.Join("|", positions);
        }

        public Field Copy()
        {
            Field NewGameField = new Field(8, 8);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    NewGameField[i, j] = field[i, j];
                }
            }
            return NewGameField;
        }
        public void Clone(Field NewGameField)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    field[i, j] = NewGameField[i, j];
                }
            }
        }

        public void Draw()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (field[i, j] < 0)
                    {
                        Console.Write(field[i, j] + "   ");
                    }
                    else
                    {
                        Console.Write(" " + field[i, j] + "   ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("************************");
        }
    }
}
