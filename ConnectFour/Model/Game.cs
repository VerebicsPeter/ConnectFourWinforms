namespace ConnectFour.Model
{
    internal enum Player
    {
        X, O
    }

    internal enum GameState
    {
        NONE, WON_BY_X, WON_BY_O
    }

    // Model for the game
    internal class Game
    {
        #region Private Fields

        private readonly int _h, _w;

        private Tile[,]   Tiles; // Game table represented by tiles
        private Player    _currentPlayer;
        private GameState _currentState;

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

        public Stack<Point> WinningPointStack { get; private set; }
        #endregion

        #region Constructor

        public Game (int height = 10, int width = 10)
        {
            _h = height; _w = width;
            Tiles = new Tile[_h, _w];
            
            WinningPointStack = new Stack<Point>();

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
            _currentPlayer = Player.X;
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
                    if (_currentPlayer == Player.X)
                    {
                        Tiles[row, col].Value = TileValue.X;
                        
                    }
                    else
                    {
                        Tiles[row, col].Value = TileValue.O;
                    }
                    OnTileChanged(row, col, _currentPlayer);
                    Console.WriteLine($"tile {row},{col} changed to {_currentPlayer}.");
                    _currentPlayer = _currentPlayer == Player.X ? Player.O : Player.X; Moves++; // set player
                    _currentState  = GetCurrentState(); // set state
                    
                    //foreach (var coord in WinningPointStack) { Console.WriteLine(coord.ToString()); }
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
                    PushSameTile(i, j, stack, WinningPointStack);

                    if (stack.Count == 4)
                    {
                        if (stack.Peek() == TileValue.X) return GameState.WON_BY_X;
                        return GameState.WON_BY_O;
                    }
                }
            }
            // Verticals
            for (int j = 0; j < _w; j++) // cols
            {
                Stack<TileValue> stack = new();

                for (int i = 0; i < _h; i++) // rows
                {
                    PushSameTile(i, j, stack, WinningPointStack);

                    if (stack.Count == 4)
                    {
                        if (stack.Peek() == TileValue.X) return GameState.WON_BY_X;
                        return GameState.WON_BY_O;
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
                            PushSameTile(i, j, stack, WinningPointStack);

                            if (stack.Count == 4)
                            {
                                if (stack.Peek() == TileValue.X) return GameState.WON_BY_X;
                                return GameState.WON_BY_O;
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
                            PushSameTile(i, j, stack, WinningPointStack);

                            if (stack.Count == 4)
                            {
                                if (stack.Peek() == TileValue.X) return GameState.WON_BY_X;
                                return GameState.WON_BY_O;
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
                            PushSameTile(i, j, stack, WinningPointStack);

                            if (stack.Count == 4)
                            {
                                if (stack.Peek() == TileValue.X) return GameState.WON_BY_X;
                                return GameState.WON_BY_O;
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

        private void PushSameTile(int i, int j, Stack<TileValue> valueStack, Stack<Point> pointStack)
        {
            if (!Tiles[i, j].IsEmpty()) // if on an occupied tile
            {
                if (valueStack.Count == 0) // if the stack is empty
                {
                    valueStack.Push(Tiles[i, j].Value); pointStack.Push(new Point(i, j));
                }
                else // if the stack has elements
                {
                    if (valueStack.Peek() == Tiles[i, j].Value) // if top is the same as actual tile
                    {
                        valueStack.Push(Tiles[i, j].Value); pointStack.Push(new Point(i, j));
                    }
                    else // if the top is different
                    {
                        valueStack.Clear(); pointStack.Clear();
                        valueStack.Push(Tiles[i, j].Value); pointStack.Push(new Point(i, j));
                    }
                }
            }
            else { valueStack.Clear(); pointStack.Clear(); }
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
            if (GameWon != null) GameWon(this, new GameWonEventArgs(_currentState, WinningPointStack));
        }
        
        #endregion
    }
}