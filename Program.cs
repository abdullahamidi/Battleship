using System;
using System.Collections.Generic;
using System.Threading;

namespace Battleship
{
    class Program
    {
        static void Main(string[] args)
        {
            int difficulty = 2;
            int shipCounter = 0;
            bool isPlayerTurn = true;
            Player player = new Player();
            Enemy enemy = new Enemy();
            Board hitInfoBoard = new Board();



            Console.WriteLine("Lütfen oyunun zorluğunu seçin (Kolay için 1, Orta için 2, Zor için 3)");
            while (!int.TryParse(Console.ReadLine(), out difficulty) || (difficulty < 1 || difficulty > 3))
            {
                Console.WriteLine("Geçersiz bir sayı yazdın!");
                Console.WriteLine("Lütfen oyunun zorluğunu seçin (Kolay için 1, Orta için 2, Zor için 3)");
            }

            switch (difficulty)
            {
                case 1:
                    player.board.Width = 8;
                    player.board.Height = 8;
                    enemy.board.Width = 8;
                    enemy.board.Height = 8;
                    hitInfoBoard.Width = 8;
                    hitInfoBoard.Height = 8;
                    player.board.CreateBoard();
                    enemy.board.CreateBoard();
                    hitInfoBoard.CreateBoard();
                    break;
                case 2:
                    player.board.Width = 10;
                    player.board.Height = 10;
                    enemy.board.Width = 10;
                    enemy.board.Height = 10;
                    hitInfoBoard.Width = 10;
                    hitInfoBoard.Height = 10;
                    player.board.CreateBoard();
                    enemy.board.CreateBoard();
                    hitInfoBoard.CreateBoard();
                    break;
                case 3:
                    player.board.Width = 15;
                    player.board.Height = 15;
                    enemy.board.Width = 15;
                    enemy.board.Height = 15;
                    hitInfoBoard.Width = 15;
                    hitInfoBoard.Height = 15;
                    player.board.CreateBoard();
                    enemy.board.CreateBoard();
                    hitInfoBoard.CreateBoard();
                    break;
                default:
                    break;
            }
            enemy.ai.PlaceShips(enemy.ships, enemy.board);

            Console.WriteLine("Rakip gemilerini yerleştirdi. Şimdi sıra sende! Göster bakalım hünerlerini.");


            #region Placing Ships

            while (shipCounter < player.ships.Count)
            {
                Console.WriteLine("\nKonumlandırılacak Gemi: {0}-{1} birim\nLütfen yatayda 1-{2} ve dikeyde 1-{3} aralıklarında olmak üzere geminin başlangıç noktasını yazın." +
                    "\nGemiyi yatay yerleştirmek için 0, dikey yerleştirmek için 1 yazın." +
                    " \nÖrneğin; 1 2 0 (X Y Yön)", player.ships[shipCounter].Name, player.ships[shipCounter].Length, player.board.Width, player.board.Height);
                string rawData = Console.ReadLine();
                string[] readData = rawData.Split(" ");
                int shipXPos = int.Parse(readData[0]);
                int shipYPos = int.Parse(readData[1]);
                int shipDirection = int.Parse(readData[2]);

                if (player.board.CanPlaceShip(player.ships[shipCounter], shipXPos, shipYPos, shipDirection))
                {
                    player.board.PlaceShip(player.ships[shipCounter], shipXPos, shipYPos, shipDirection);
                    player.board.DrawBoard();
                    shipCounter++;
                }
                else
                {
                    Console.WriteLine("Bu konuma gemi yerleştiremezsin!");
                }
            }
            #endregion


            
           
            while (!GameManager.GameOver(player.board, enemy.board))
            {
                if (isPlayerTurn)
                {
                    Console.WriteLine("Ateş etmek istediğin pozisyonu yaz.\nÖrneğin; 1 2 (X Y)");
                    var rawData = Console.ReadLine();
                    var readData = rawData.Split(" ");
                    int hitX = 0;
                    int hitY = 0;
                    if (int.TryParse(readData[0], out int x) && x <= enemy.board.Width && x >= 1)
                    {
                        hitX = x;
                    }
                    else
                    {
                        Console.WriteLine("Hey! Kafama geldi! Bidahaki sefer rakibin denizini vurmaya çalış.");
                        continue;
                    }
                    if (int.TryParse(readData[1], out int y) && y <= enemy.board.Width && y >= 1)
                    {
                        hitY = y;
                    }
                    else
                    {
                        Console.WriteLine("Hey! Kafama geldi! Bidahaki sefer rakibin denizini vurmaya çalış.");
                        continue;
                    }

                    Console.WriteLine("\n({0},{1}) koordinatına atış yapıyorsun...", x, y);
                    Thread.Sleep(2000);
                    hitInfoBoard.Positions[(y - 1) * enemy.board.Width + x - 1].CellState = player.Attack(enemy.board, x, y);
                    hitInfoBoard.DrawBoard();
                    isPlayerTurn = false;
                }
                else
                {
                    var attackCell = enemy.ai.FindAttackPosition(player.board);
                    Console.WriteLine("\nRakibin ({0},{1}) koordinatına atış yapıyor...", attackCell.X, attackCell.Y);
                    Thread.Sleep(2000);
                    player.board.Positions[(attackCell.Y - 1) * enemy.board.Width + attackCell.X - 1].CellState = enemy.Attack(player.board, attackCell.X, attackCell.Y);
                    player.board.DrawBoard();
                    isPlayerTurn = true;
                }
            }

            Console.WriteLine("\n{0} oyunu kazandı.", GameManager.winner);
        }
    }
}
