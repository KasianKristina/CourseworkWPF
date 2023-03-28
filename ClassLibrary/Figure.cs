using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Figure
    {
        public Position StartOffset { get; set; }
        public int Id;
        public Position Offset { get; set; }
        protected Field GameField { get; set; }
        protected Color Color { get; set; }
        public Position EndingPosition { get; set; }
        public int RowConst { get; set; }
        public bool LeaveSquareFlag { get; set; }


        // метод премещения, который перемещает блок на заданную позицию
        public void MoveBlock(int rows, int columns)
        {
            if (GameField[rows, columns] >= 0)
            {
                GameField[Offset.Row, Offset.Column] = 0;
                Offset.Row = rows;
                Offset.Column = columns;
                GameField[Offset.Row, Offset.Column] = Id;
            }
        }
    }
}
