using System;
using System.Collections.Generic;
using System.Text;

namespace Battleship
{
    public enum Cell_State
    {
        Fog,
        Ship,
        Hit,
        Miss
    }
    public class Cell
    {
        public int X { get; set; }

        public int Y { get; set; }

        public Cell_State CellState { get; set; }
        public Cell()
        {

        }
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }
        public Cell(int x, int y, Cell_State cellState)
        {
            X = x;
            Y = y;
            CellState = cellState;
        }
    }
}
