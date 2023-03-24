using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Position
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public override bool Equals(object value)
        {
            Position number = value as Position;
            return (number != null)
                    && (Row == number.Row)
                    && (Column == number.Column);

        }
    }
}
