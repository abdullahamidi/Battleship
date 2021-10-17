using System;
using System.Collections.Generic;
using System.Text;

namespace Battleship
{
    public static class GameManager
    {
        public static string winner = "";
        public static bool GameOver(Board playerBoard, Board enemyBoard)
        {
            var hasPlayerShip = playerBoard.Positions.Exists(p => p.CellState == Cell_State.Ship);
            var hasEnemyShip = enemyBoard.Positions.Exists(e => e.CellState == Cell_State.Ship);
            if (hasPlayerShip && !hasEnemyShip)
            {
                winner = "Player";
                return true;
            }
            if (hasEnemyShip && !hasPlayerShip)
            {
                winner = "Enemy";
                return true;
            }
            return false;
        }
    }
}
