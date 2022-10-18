using ConnectFour.Persistence;
using System.Text;

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

        private int _h, _w;

        private Tile[,] Tiles; // Game table represented by tiles
        private Player _currentPlayer;
        private GameState _currentState;
        private IGameDataAccess _dataAccess;

        #endregion

        #region Events

        public event EventHandler? GameEnd;
        public event EventHandler<GameWonEventArgs>? GameWon;
        public event EventHandler<TileChangedEventArgs>? TileChanged;

        #endregion

        #region Public Properties

        public int Moves { get; private set; }

        public int Height { get { return _h; } }

        public int Width { get { return _w; } }

        public Player CurrentPlayer  { get { return _currentPlayer; } }

        public GameState CurrentState { get { return _currentState; } }

        public Stack<Point> WinningPointStack { get; private set; }
        
        #endregion

        #region Constructor

        public Game (IGameDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            _h = 10; _w = 10;
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

            Console.WriteLine("Game started.");
        }

        public void SetSize(int height, int width)
        {
            _h = height;
            _w = width;
            Tiles = new Tile[height, width];
        }

        // Sets tile and changes current player
        public void Move(int col)
        {
            int row = GetRow(col);
            if (row > -1)
            {
                if (Tiles[row, col].IsEmpty())
                {
                    if (_currentPlayer == Player.X) {
                        Tiles[row, col].Value = TileValue.X;
                    }
                    else {
                        Tiles[row, col].Value = TileValue.O;
                    }
                    OnTileChanged(row, col, _currentPlayer);
                    Console.WriteLine($"tile {row},{col} changed to {_currentPlayer}.");
                    _currentPlayer = _currentPlayer == Player.X ? Player.O : Player.X; Moves++; // Set player
                    _currentState  = GetCurrentState(); // Set state

                    if (CurrentState != GameState.NONE) OnGameWon();
                    if (Moves == Height * Width) OnGameEnd();
                }
            }
        }

        // Checks and sets game's state and returns it
        public GameState GetCurrentState()
        {
            // Horizontals
            for (int i = 0; i < _h; i++)
            {
                Stack<TileValue> stack = new();
                WinningPointStack.Clear();

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
                WinningPointStack.Clear();

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
                WinningPointStack.Clear();

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
                WinningPointStack.Clear();

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
                WinningPointStack.Clear();

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
            if (0 <= col && col < _w)
            {
                for (int i = _h - 1; i > -1; i--)
                {
                    if (Tiles[i, col].IsEmpty())
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        
        public char GetValue(int i, int j)
        {
            if (i >= 0 && i < _h && j >=0 && j < _w)
            {
                if (Tiles[i, j].IsX()) return 'x';
                if (Tiles[i, j].ISO()) return 'o'; 
            }
            return 'e';
        }

        public int GetMoves()
        {
            int count = 0;
            for (int i = 0; i < _h; i++)
            {
                for (int j = 0; j < _w; j++)
                {
                    if (!Tiles[i, j].IsEmpty()) count++;
                }
            }
            return count;
        }

        public async Task<Point> LoadGameAsync(string path)
        {
            if (_dataAccess == null) throw new InvalidOperationException("No data acesss is provided.");
            GameTable table = await _dataAccess.LoadAsync(path);

            Tiles = new Tile[table.X, table.Y];

            _h = table.X; _w = table.Y;

            try
            {
                for (int i = 0; i < table.X; i++)
                {
                    // Check if the current string is the same length as the one provided in table.y:
                    if (table.Table[i].Length == table.Y)
                    {
                        char[] chars = table.Table[i].ToCharArray();

                        for (int j = 0; j < chars.Length; j++)
                        {
                            Tiles[i, j] = Tile.MakeTile(chars[j]);
                        }
                    }
                }

                Moves = GetMoves();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            _currentPlayer = table.currPlayer == "x" ? Player.X : Player.O;
            _currentState  = GetCurrentState();
            // If state is invalid start new game with the size read:
            if (_currentState != GameState.NONE)
            {
                StartGame();
                return new Point(0, 0);
            }
            return new Point(table.time1, table.time2);
        }

        public async Task SaveGameAsync(string path, int time1, int time2)
        {
            if (_dataAccess == null) throw new InvalidOperationException("No data acesss is provided.");

            int X, Y;
            X = _h; Y = _w;
            string curr = CurrentPlayer == Player.X ? "x" : "o";

            GameTable table = new (X, Y, time1, time2, curr);

            for (int i = 0; i < _h; i++)
            {
                StringBuilder sb = new();

                for (int j = 0; j < _w; j++)
                {
                    switch (Tiles[i, j].Value)
                    {
                        case TileValue.X: sb.Append('x'); break;
                        case TileValue.O: sb.Append('o'); break;
                        default: sb.Append('e'); break;
                    }
                }
                table.Table[i] = sb.ToString();
            }

            await _dataAccess.SaveAsync(path, table);
        }

        #endregion

        #region Private Methods
        // pushes into the stack (or clears it) to checks game state
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
        //Event trigger for tile change
        private void OnTileChanged(int x, int y, Player player)
        {
            if (TileChanged != null) TileChanged(this, new TileChangedEventArgs(x, y, player));
        }
        // Event trigger for draw
        private void OnGameEnd()
        {
            if (GameEnd != null) GameEnd(this, EventArgs.Empty);
        }
        // Event trigger for win
        private void OnGameWon()
        {
            if (GameWon != null) GameWon(this, new GameWonEventArgs(_currentState, WinningPointStack));
        }
        #endregion
    }
}