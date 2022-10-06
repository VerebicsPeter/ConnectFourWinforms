namespace ConnectFour.Model
{
    internal enum Player
    {
        RED, YELLOW
    }

    internal enum GameState 
    {
        NONE, WON_RED, WON_YELLOW
    }

    internal class Game
    {
        private readonly int _h, _w;

        public Tile[,]   Tiles;
        public Player    CurrentPlayer;
        public GameState CurrentState;

        public Game (int height, int width)
        {
            _h = height; _w = width;
            Tiles = new Tile[_h, _w];

            for (int i = 0; i < _h; i++)
                for (int j = 0; j < _w; j++)
                    Tiles[i, j] = new Tile();

            CurrentState  = GameState.NONE;
            CurrentPlayer = Player.RED;
        }

        public void Move (int col)
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

        private int GetRow (int col)
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

        public GameState GetCurrentState()
        {
            // Horizontals
            for (int i = 0; i < _h; i++)
            {
                Stack<TileValue> stack = new();

                for (int j = 0; j < _w; j++)
                {
                    PushSameTile(i, j, stack);

                    if (stack.Count == 4) {
                        if (stack.Peek() == TileValue.RED) return GameState.WON_RED;
                        return GameState.WON_YELLOW;
                    }
                }
            }
            // Verticals
            for (int j = 0; j < _w; j++) // cols
            {
                Stack<TileValue> stack = new();

                for (int i = 0; i < _h; i++) // rows
                {
                    PushSameTile(i, j, stack);

                    if (stack.Count == 4) {
                        if (stack.Peek() == TileValue.RED) return GameState.WON_RED;
                        return GameState.WON_YELLOW;
                    }
                }
            }

            #region // Right diagonals

            for (int k = 0; k < _h + _w; k++)
            {
                Stack<TileValue> stack = new();

                for (int i = 0; i < _h; i++)
                {
                    for (int j = 0; j < _w; j++)
                    {
                        if (i + j == k)
                        {
                            PushSameTile(i, j, stack);

                            if (stack.Count == 4) {
                                if (stack.Peek() == TileValue.RED) return GameState.WON_RED;
                                return GameState.WON_YELLOW;
                            }
                        }
                    }
                }
            }
            #endregion

            #region // Left diagonals
            // Upper diagonals (including the main diagonal)
            for (int k = 0; k < _w; k++)
            {
                Stack<TileValue> stack = new();

                for (int i = 0; i < _h; i++)
                {
                    for (int j = 0; j < _w; j++)
                    {
                        if (j - i == k)
                        {
                            PushSameTile(i, j, stack);

                            if (stack.Count == 4) {
                                if (stack.Peek() == TileValue.RED) return GameState.WON_RED;
                                return GameState.WON_YELLOW;
                            }
                        }
                    }
                }
            }
            // Lower diagonals
            for (int k = 1; k < _h; k++)
            {
                Stack<TileValue> stack = new();

                for (int i = 0; i < _h; i++)
                {
                    for (int j = 0; j < _w; j++)
                    {
                        if (i - j == k)
                        {
                            PushSameTile(i, j, stack);

                            if (stack.Count == 4) {
                                if (stack.Peek() == TileValue.RED) return GameState.WON_RED;
                                return GameState.WON_YELLOW;
                            }
                        }
                    }
                }
            }
            #endregion

            return GameState.NONE;
        }

        private void PushSameTile(int i, int j, Stack<TileValue> stack)
        {
            if (!Tiles[i, j].IsEmpty())
            {
                if (stack.Count == 0)
                {
                    stack.Push(Tiles[i, j].Value);
                }
                else
                {
                    if (stack.Peek() == Tiles[i, j].Value)
                    {
                        stack.Push(Tiles[i, j].Value);
                    }
                    else {
                        stack.Clear();
                        stack.Push(Tiles[i, j].Value);
                    }
                }
            }
            else stack.Clear();
        }

        public void PrintGame() // for console only
        {
            for (int i = 0; i < _h; i++) {
                for (int j = 0; j < _w; j++)
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
            for (int i = 0; i < _h * _w; i++)
            {
                int col;

                do {
                    try {
                        col = Int32.Parse(Console.ReadLine()!);
                    }
                    catch (Exception e) {
                        Console.WriteLine(e.Message);

                        col = 0;
                    }
                } while (col < -1 || col > _w - 1);
                
                Move(col);
                PrintGame();
                CurrentState = GetCurrentState();
                if (CurrentState != GameState.NONE) break;
            }
            if (CurrentState == GameState.WON_RED)
            {
                Console.WriteLine("RED wins!");
            }
            if (CurrentState == GameState.WON_YELLOW)
            {
                Console.WriteLine("YELLOW wins!");
            }
            if (CurrentState == GameState.NONE)
            {
                Console.WriteLine("Draw!");
            }
        }
    }
}