using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour.Model
{
    internal class Game
    {
        public Tile[,] Tiles;

        // Should take size as arguments!
        public Game()
        {
            Tiles = new Tile[10, 10];

            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    Tiles[i, j] = new Tile();
        }

        public void PrintGame()
        {
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 10; j++)
                {
                    if (Tiles[i, j].IsEmpty()) Console.Write("- ");
                    if (Tiles[i, j].IsRed()) Console.Write("R ");
                    if (Tiles[i, j].IsYellow()) Console.Write("Y ");
                }
                Console.WriteLine();
            }
        }
    }
}