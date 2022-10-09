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

    // Model for the game
    internal class Game
    {
        #region Private Fields

        private readonly int _h, _w;

        private Tile[,]   Tiles; // game table represented by tiles
        private Player    _currentPlayer;
        private GameState _currentState;
        // TODO: moves counter

        #region Events

        public event EventHandler? GameEnd;
        public event EventHandler<GameWonEventArgs>? GameWon;
        public event EventHandler<TileChangedEventArgs>? TileChanged;

        #endregion

        #endregion

        #region Public Properties

        public int Moves { get; private set; }

        public int Height { get { return _h; } }

        public int Width  { get { return _w; } }

        public Player CurrentPlayer  { get { return _currentPlayer; } }

        public GameState CurrentState { get { return _currentState; } }

        #endregion

        #region Constructor

        public Game (int height = 7, int width = 7)
        {
            _h = height; _w = width;
            Tiles = new Tile[_h, _w];

            StartGame();

            Console.WriteLine("Game model instanciated.");
        }
        #endregion

        #region Public Methods

        public void StartGame()
        {
            for (int i = 0; i < _h; i++)
                for (int j = 0; j < _w; j++)
                    Tiles[i, j] = new Tile();

            _currentState  = GameState.NONE;
            _currentPlayer = Player.RED;
            Moves = 0;
        }

        // Sets tile and changes current player
        public void Move(int col)
        {
            int row = GetRow(col);
            if (row > -1)
            {
                if (Tiles[row, col].IsEmpty())
                {
                    if (_currentPlayer == Player.RED)
                    {
                        Tiles[row, col].Value = TileValue.RED;
                        OnTileChanged(row, col, _currentPlayer);
                        
                    }
                    else
                    {
                        Tiles[row, col].Value = TileValue.YELLOW;
                        OnTileChanged(row, col, _currentPlayer);
                    }
                    Console.WriteLine($"tile {row},{col} changed to {_currentPlayer}.");
                    _currentPlayer = _currentPlayer == Player.RED ? Player.YELLOW : Player.RED; Moves++;
                    _currentState  = GetCurrentState();
                    if (Moves == Height * Width)        OnGameEnd();
                    if (CurrentState != GameState.NONE) OnGameWon();
                }
            }
        }

        // Checks and sets game state
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

        public int GetRow(int col)
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

        #endregion

        #region Private Methods

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

        #region Event Triggers

        private void OnTileChanged(int x, int y, Player player)
        {
            if (TileChanged != null)
            {
                TileChanged(this, new TileChangedEventArgs(x, y, player));
            }
        }

        // Event trigger for draw
        private void OnGameEnd()
        {
            if (GameEnd != null) GameEnd(this, EventArgs.Empty);
        }

        private void OnGameWon()
        {
            if (GameWon != null) GameWon(this, new GameWonEventArgs(_currentState));
        }

        #endregion
    }
}
