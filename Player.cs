using System;
using System.Collections.Generic;
using System.Text;

namespace Battleship
{
    class Player
    {
        public Board board = new Board();


        public List<Ship> ships = new List<Ship>()
            {
                new Ship("Amiral", 5),
                new Ship("Kuvazör", 4),
                new Ship("Muhrip", 3),
                new Ship("Muhrip", 3),
                new Ship("Hücumbot", 2),
            };

        public Cell_State Attack(Board targetBoard, int x, int y)
        {
            return targetBoard.GetHit(x, y);
        }

    }
}
