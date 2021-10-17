using System;
using System.Collections.Generic;
using System.Text;

namespace Battleship
{
    enum Direction
    {
        None,
        Left,
        Right,
        Up,
        Down
    }
    class EnemyAI
    {
        List<Cell> excludedCells = new List<Cell>();
        List<Cell> hitList = new List<Cell>();
        Cell lastHitCell;
        bool shipSunk = false;
        Direction dir = Direction.None;
        public EnemyAI()
        {

        }
        public void PlaceShips(List<Ship> ships, Board board)
        {
            Random rand = new Random();
            var counter = 0;
            while (counter < ships.Count)
            {
                excludedCells.AddRange(DetermineExcludedCells(board));

                var x = rand.Next(1, board.Width + 1);
                var y = rand.Next(1, board.Height + 1);
                var direction = rand.Next(0, 2);

                if (CheckIsExcludedCell(excludedCells, ships[counter], new Cell(x, y, Cell_State.Fog), direction))
                {
                    continue;
                }

                if (board.CanPlaceShip(ships[counter], x, y, direction))
                {
                    board.PlaceShip(ships[counter], x, y, direction);
                    counter++;
                }
            }
        }

        public List<Cell> DetermineExcludedCells(Board board)
        {
            var cells = new List<Cell>();
            for (int i = 0; i < board.Positions.Count; i++)
            {
                if (board.Positions[i].CellState == Cell_State.Ship)
                {
                    if (board.Positions[i].X + 1 <= board.Width && board.Positions[i].X + 1 > 0)
                        cells.Add(new Cell(board.Positions[i].X + 1, board.Positions[i].Y, Cell_State.Fog));
                    if (board.Positions[i].X - 1 <= board.Width && board.Positions[i].X - 1 > 0)
                        cells.Add(new Cell(board.Positions[i].X - 1, board.Positions[i].Y, Cell_State.Fog));
                    if (board.Positions[i].Y + 1 <= board.Height && board.Positions[i].Y + 1 > 0)
                        cells.Add(new Cell(board.Positions[i].X, board.Positions[i].Y + 1, Cell_State.Fog));
                    if (board.Positions[i].Y - 1 <= board.Height && board.Positions[i].Y - 1 > 0)
                        cells.Add(new Cell(board.Positions[i].X, board.Positions[i].Y - 1, Cell_State.Fog));
                }
            }
            return cells;
        }

        public bool CheckIsExcludedCell(List<Cell> excludedCells, Ship ship, Cell cell, int direction)
        {
            bool result = false;
            for (int i = 0; i < ship.Length; i++)
            {
                if (result)
                {
                    break;
                }

                if (direction == 0)
                {
                    var tempCell = new Cell(cell.X + i, cell.Y, Cell_State.Fog);
                    result = excludedCells.Exists(cell => cell.X == tempCell.X && cell.Y == tempCell.Y);
                }
                else
                {
                    var tempCell = new Cell(cell.X, cell.Y + i, Cell_State.Fog);
                    result = excludedCells.Exists(cell => cell.X == tempCell.X && cell.Y == tempCell.Y);
                }
            }
            return result;
        }

        public Cell FindAttackPosition(Board targetBoard)
        {
            Cell targetCell = new Cell();
            Random rand = new Random();
            if (lastHitCell == null || shipSunk)
            {
                shipSunk = false;
                int x = rand.Next(1, targetBoard.Width + 1);
                int y = rand.Next(1, targetBoard.Height + 1);
                if (targetBoard.Positions[(y - 1) * targetBoard.Width + x - 1].CellState == Cell_State.Ship)
                {
                    targetCell = new Cell(x, y, Cell_State.Hit);
                    lastHitCell = targetCell;
                    if (targetCell.X != targetBoard.Width)
                        hitList.Add(new Cell(targetCell.X + 1, targetCell.Y));
                    if (targetCell.X != 1)
                        hitList.Add(new Cell(targetCell.X - 1, targetCell.Y));
                    if (targetCell.Y != targetBoard.Height)
                        hitList.Add(new Cell(targetCell.X, targetCell.Y + 1));
                    if (targetCell.Y != 1)
                        hitList.Add(new Cell(targetCell.X, targetCell.Y - 1));
                }
                else
                {
                    targetCell = new Cell(x, y, Cell_State.Miss);
                }
            }
            else
            {
                int cellIndex = rand.Next(0, hitList.Count);
                if (targetBoard.Positions[(hitList[cellIndex].Y - 1) * targetBoard.Width + hitList[cellIndex].X - 1].CellState == Cell_State.Ship)
                {
                    dir = targetCell.X - hitList[cellIndex].X < 0 ? Direction.Right : targetCell.X - hitList[cellIndex].X > 0 ? Direction.Left : targetCell.Y - hitList[cellIndex].Y < 0 ? Direction.Down : Direction.Up;
                    targetCell = new Cell(hitList[cellIndex].X, hitList[cellIndex].Y, Cell_State.Hit);
                    lastHitCell = targetCell;
                    hitList.Clear();
                    switch (dir)
                    {
                        case Direction.Left:
                            hitList.Add(new Cell(targetCell.X - 1, targetCell.Y));
                            break;
                        case Direction.Right:
                            hitList.Add(new Cell(targetCell.X + 1, targetCell.Y));
                            break;
                        case Direction.Up:
                            hitList.Add(new Cell(targetCell.X, targetCell.Y + 1));
                            break;
                        case Direction.Down:
                            hitList.Add(new Cell(targetCell.X, targetCell.Y - 1));
                            break;
                    }
                }
                else
                {
                    if (dir != Direction.None)
                    {
                        dir = Direction.None;
                        shipSunk = true;
                        lastHitCell = null;
                    }
                    targetCell = new Cell(hitList[cellIndex].X, hitList[cellIndex].Y, Cell_State.Miss);
                    hitList.RemoveAt(cellIndex);
                }
            }

            return targetCell;
        }
    }
}
