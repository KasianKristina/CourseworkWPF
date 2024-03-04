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

        public bool IsCorridor(int row, int column)
        {
            if ((column == 0 && (IsWall(row, column + 1) || IsWall(row, column + 2))) ||
                (column == 1 && IsWall(row, column + 1)) ||
                (IsWall(row, column - 1) && (IsWall(row, column + 1) || IsWall(row, column + 2))) ||
                (IsWall(row, column + 1) && IsWall(row, column - 2)) ||
                (column == 7 && (IsWall(row, column - 1) || IsWall(row, column - 2))) ||
                (column == 6 && IsWall(row, column - 1)))
                return true;
            return false;
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
