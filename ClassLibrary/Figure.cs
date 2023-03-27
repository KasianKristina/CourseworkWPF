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
        public Position offset { get; set; }
        protected Field GameField { get; set; }
        protected Color Color { get; set; }
        public Position endingPosition { get; set; }
        public int RowConst { get; set; }
        public bool LeaveSquareFlag { get; set; }


        // метод премещения, который перемещает блок на заданную позицию
        public void MoveBlock(int rows, int columns)
        {
            if (GameField[rows, columns] >= 0)
            {
                GameField[offset.Row, offset.Column] = 0;
                offset.Row = rows;
                offset.Column = columns;
                GameField[offset.Row, offset.Column] = Id;
            }
        }
    }
}
