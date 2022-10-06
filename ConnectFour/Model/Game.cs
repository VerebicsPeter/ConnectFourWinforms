using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace ConnectFour.Model
{
    internal enum Player
    {
        RED, YELLOW
    }

    internal enum GameState 
    {
        NONE, WON_RED, WON_YELLOW, DRAW
    }

    internal class Game
    {
        private readonly int _h, _w;

        public Tile[,]   Tiles;
        public Player    CurrentPlayer;
        public GameState CurrentState;

        // Should take size as arguments!
        public Game(int height, int width)
        {
            _h = height; _w = width;
            Tiles = new Tile[_h, _w];

            for (int i = 0; i < _h; i++)
                for (int j = 0; j < _w; j++)
                    Tiles[i, j] = new Tile();

            CurrentState  = GameState.NONE;
            CurrentPlayer = Player.RED;
        }

        public void Move(int col)
        {
            int row = GetRow(col);
            if (row > -1)
            {
                if (Tiles[row, col].IsEmpty())
                {
                    if (CurrentPlayer == Player.RED)
                    {
                        Tiles[row, col].Value = TileValue.RED;
                        CurrentPlayer = Player.YELLOW;
                    }
                    else
                    {
                        Tiles[row, col].Value = TileValue.YELLOW;
                        CurrentPlayer = Player.RED;
                    }
                }
            }
        }

        private int GetRow(int col)
        {
            for (int i = _h - 1; i > -1; i--)
            {
                if (Tiles[i, col].IsEmpty())
                {
                    return i;
                }
            }
            return -1;
        }

        public void PrintGame() // for console only
        {
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 10; j++)
                {
                    if (Tiles[i, j].IsEmpty())  Console.Write("- ");
                    if (Tiles[i, j].IsRed())    Console.Write("R ");
                    if (Tiles[i, j].IsYellow()) Console.Write("Y ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void Play() // for console only
        {
            PrintGame();
            while (true) {
                int col;
                do {
                    try {
                        col = Int32.Parse(Console.ReadLine());
                    }
                    catch {
                        col = 0;
                    }
                } while (col < -1 || col > _w - 1);
                Move(col);

                PrintGame();
            }
        }
    }
}
