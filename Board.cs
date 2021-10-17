using System;
using System.Collections.Generic;
using System.Text;

namespace Battleship
{
    public class Board
    {
        public int Height { get; set; } = 0;
        public int Width { get; set; } = 0;

        public List<Cell> Positions { get; set; } = new List<Cell>();

        public Board()
        {
        }
        public Board(int height, int width)
        {
            Height = height;
            Width = width;
            CreateBoard();
        }

        public void CreateBoard()
        {
            for (int y = 1; y <= Height; y++)
            {
                for (int x = 1; x <= Width; x++)
                {
                    Positions.Add(new Cell(x, y, Cell_State.Fog));
                }
            }
        }
        public void DrawBoard()
        {
            for (int y = 0; y <= Height; y++)
            {
                for (int x = 0; x <= Width; x++)
                {
                    if (x == 0 && y == 0)
                    {
                        Console.Write("    ");
                    }
                    else if (x == 0 && y != 0)
                    {
                        if (y >= 10)
                            Console.Write(" {0} ", y);
                        else
                            Console.Write(" {0}  ", y);

                    }
                    else if (x != 0 && y == 0)
                    {
                        if (x >= 10)
                            Console.Write(" {0} ", x);
                        else
                            Console.Write(" {0}  ", x);
                    }
                    else
                    {
                        switch (Positions[(y - 1) * Width + x - 1].CellState)
                        {
                            case Cell_State.Fog:
                                Console.Write("~~~ ");
                                break;
                            case Cell_State.Ship:
                                Console.Write(" S  ");
                                break;
                            case Cell_State.Hit:
                                Console.Write(" X  ");
                                break;
                            case Cell_State.Miss:
                                Console.Write(" .  ");
                                break;
                            default:
                                break;
                        }
                    }
                }
                Console.WriteLine("\n");
            }
        }
        public virtual void PlaceShip(Ship ship, int headingPosX, int headingPosY, int direction)
        {
            if (direction == 0)
            {
                for (int i = 0; i < ship.Length; i++)
                {
                    Positions[(headingPosY - 1) * Width + (headingPosX - 1) + i].CellState = Cell_State.Ship;
                }
            }
            else
            {
                for (int i = 0; i < ship.Length; i++)
                {
                    Positions[(headingPosY - 1) * Width + (headingPosX - 1) + i * Width].CellState = Cell_State.Ship;
                }
            }
        }

        public Cell_State GetHit(int hitX, int hitY)
        {

            if (Positions[(hitY - 1) * Width + hitX - 1].CellState == Cell_State.Ship)
            {
                Positions[(hitY - 1) * Width + hitX - 1].CellState = Cell_State.Hit;
                Console.WriteLine("Aferin! Sende iş var. Böyle devam et.");
                return Cell_State.Hit;
            }
            else
            {
                Positions[(hitY - 1) * Width + hitX - 1].CellState = Cell_State.Miss;
                Console.WriteLine("Gelişinden anlamalıydım ne kadar beceriksiz olduğunu... Kaçırdın!");
                return Cell_State.Miss;
            }
        }

        public bool CanPlaceShip(Ship ship, int headingPosX, int headingPosY, int direction)
        {
            return IsInBounds(ship, (headingPosX), (headingPosY), direction) && IsCellAvailable(ship, (headingPosX - 1), (headingPosY - 1), direction) ? true : false;
        }

        public bool IsInBounds(Ship ship, int headingPosX, int headingPosY, int direction)
        {
            if (direction == 0)
            {
                if (headingPosX + ship.Length - 1 <= Width)
                    return true;
                else return false;

            }
            else
            {
                if (headingPosY + ship.Length - 1 <= Height)
                    return true;
                else return false;
            }
        }

        public bool IsCellAvailable(Ship ship, int headingPosX, int headingPosY, int direction)
        {
            bool available = false;
            if (direction == 0)
            {
                for (int i = 0; i < ship.Length; i++)
                {
                    if (Positions[headingPosX + i].CellState != Cell_State.Ship)
                        available = true;
                    else available = false;
                }
            }
            else
            {
                for (int i = 0; i < ship.Length; i++)
                {
                    if (Positions[headingPosX + Width * i].CellState != Cell_State.Ship)
                        available = true;
                    else available = false;
                }
            }
            return available;
        }
    }
}
