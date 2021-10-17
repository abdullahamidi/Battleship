using System;
using System.Collections.Generic;
using System.Text;

namespace Battleship
{
    public class Ship
    {
        public int Length { get; set; }
        public string Name { get; set; }

        public Ship(string name, int length)
        {
            Name = name;
            Length = length;
        }

       
    }
}
