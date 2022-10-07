using System.Reflection.Metadata.Ecma335;

namespace ConnectFour.Model
{
    internal enum Player
    {
        RED, YELLOW
    }

    internal enum GameState
    {
        NONE, WON_BY_RED, WON_BY_YELLOW
    }

    internal class Game
    {
        #region Private Fields

        private readonly int _h, _w;

        private Tile[,]   Tiles; // game table represented by tiles
        private Player    CurrentPlayer;
        private GameState CurrentState;

        #endregion

        #region Public Properties

        public int Height { get { return _h; } }

        public int Width  { get { return _w; } }

        #endregion

        #region Constructor

        public Game (int height = 6, int width = 7)
        {
            _h = height; _w = width;
            Tiles = new Tile[_h, _w];

            StartGame();
        }
        #endregion

        #region Public Methods

        public void StartGame()
        {
            for (int i = 0; i < _h; i++)
                for (int j = 0; j < _w; j++)
                    Tiles[i, j] = new Tile();

            CurrentState = GameState.NONE;
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

        public GameState GetCurrentState()
        {
            // Horizontals
            for (int i = 0; i < _h; i++)
            {
                Stack<TileValue> stack = new();

                for (int j = 0; j < _w; j++)
                {
                    PushSameTile(i, j, stack);

                    if (stack.Count == 4)
                    {
                        if (stack.Peek() == TileValue.RED) return GameState.WON_BY_RED;
                        return GameState.WON_BY_YELLOW;
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

                    if (stack.Count == 4)
                    {
                        if (stack.Peek() == TileValue.RED) return GameState.WON_BY_RED;
                        return GameState.WON_BY_YELLOW;
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

                            if (stack.Count == 4)
                            {
                                if (stack.Peek() == TileValue.RED) return GameState.WON_BY_RED;
                                return GameState.WON_BY_YELLOW;
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

                            if (stack.Count == 4)
                            {
                                if (stack.Peek() == TileValue.RED) return GameState.WON_BY_RED;
                                return GameState.WON_BY_YELLOW;
                            }
                        }
                    }
                }
            }
            // Lower diagonals (excluding the main diagonal)
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

                            if (stack.Count == 4)
                            {
                                if (stack.Peek() == TileValue.RED) return GameState.WON_BY_RED;
                                return GameState.WON_BY_YELLOW;
                            }
                        }
                    }
                }
            }
            #endregion

            return GameState.NONE;
        }

        #endregion

        #region Private Methods

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
                    else
                    {
                        stack.Clear();
                        stack.Push(Tiles[i, j].Value);
                    }
                }
            }
            else stack.Clear();
        }

        #endregion

        #region Events

        public event EventHandler? GameEnd;
        public event EventHandler<GameWonEventArgs>? GameWon;
        public event EventHandler<TileChangedEventArgs>? TileChanged;

        #endregion
    }
}
