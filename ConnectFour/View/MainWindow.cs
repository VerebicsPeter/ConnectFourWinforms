using ConnectFour.Model;
using ConnectFour.View;
using Timer = System.Windows.Forms.Timer;

namespace ConnectFour
{
    public partial class MainWindow : Form
    {
        private Point  _gameSize;
        private readonly Timer  _timer;
        private bool   _paused;
        private double _turnTime;
        private double _xTime;
        private double _oTime;
        // The game's model agregated inside view
        private Game _game;
        // Picture box grid for the fields of the game
        private PictureBox[,] _pictureBoxGrid;

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            StartGameMenuItem.Click += StartGameMenuItem_Click;

            _gameSize = new Point(10, 10);
            _timer = new Timer();
            _timer.Tick += new EventHandler(Timer_Tick);
            _timer.Interval = 10;
            _paused = false;

            _game              = new Game(_gameSize.X, _gameSize.Y);
            _game.TileChanged += new EventHandler<TileChangedEventArgs>(Game_TileChanged);
            _game.GameWon     += new EventHandler<GameWonEventArgs>(Game_GameWon);
            _game.GameEnd     += new EventHandler(Game_GameEnded);
            _pictureBoxGrid    = new PictureBox[_gameSize.X, _gameSize.Y];
            InitializePictureGrid();
        }
        #endregion

        private void InitializeGame()
        {
            if (_timer.Enabled) _timer.Stop();

            _turnTime = 0; _xTime = 0; _oTime = 0;

            toolStripPlayerLabel.Text  = "Player: ";
              toolStripTimeLabel.Text  = "Time: ";
            toolStripPauseButton.Image = Image.FromFile(@".\resources\pause.png");
            toolStripPauseButton.Text  = "Pause";
            UpdateLabels();

            if (GamePanel.Controls.Count > 0) GamePanel.Controls.Clear();

            if (_gameSize.X == 10 && _gameSize.Y == 10) {
                this.Size = new Size(640, 720);
                GamePanel.Size = new Size(600, 600);
            }
            else if (_gameSize.X == 20 && _gameSize.Y == 20) {
                this.Size = new Size(760, 840);
                GamePanel.Size = new Size(720, 720);
            }
            else if (_gameSize.X == 30 && _gameSize.Y == 30) {
                this.Size = new Size(940, 1020);
                GamePanel.Size = new Size(900, 900);
            }

            StartGame();
        }

        private void InitializePictureGrid()
        {
            int bx = GamePanel.Height / _game.Height;
            int by = GamePanel.Width / _game.Width;

            for (int i = 0; i < _game.Height; i++)
            {
                for (int j = 0; j < _game.Width; j++)
                {
                    PictureBox pictureBox = new()
                    {
                        Size     = new Size(bx, by),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Location = new Point(j * bx, i * by),
                        Tag      = $"{j}",
                        Image    = Image.FromFile(@".\resources\border.png")
                    };
                    _pictureBoxGrid[i, j] = pictureBox;
                    _pictureBoxGrid[i, j].Click      += PictureBox_Click;
                    _pictureBoxGrid[i, j].MouseEnter += PictureBox_MouseEnter;
                    _pictureBoxGrid[i, j].MouseLeave += PictureBox_MouseLeave;
                    GamePanel.Controls.Add(pictureBox);
                }
            }
        }

        private void UpdatePictureGrid()
        {
            for (int i = 0; i < _gameSize.X; i++)
            {
                for (int j = 0; j < _gameSize.Y; j++)
                {
                    _pictureBoxGrid[i, j].Image = Image.FromFile(@".\resources\border.png");
                }
            }
        }

        private void UpdateLabels()
        {
            toolStripXLabel.Text = $"X Time: {(int)_xTime / 100}s";
            toolStripOLabel.Text = $"O Time: {(int)_oTime / 100}s";
        }

        private void SetTimer()
        {
            if (!_paused && _game.Moves > 0)
            {
                _paused = true; _timer.Stop();
                toolStripPauseButton.Image = Image.FromFile(@".\resources\resume.png");
                toolStripPauseButton.Text = "Resume";
            }
            else
            {
                _paused = false; if (_game.Moves > 0) _timer.Start();
                toolStripPauseButton.Image = Image.FromFile(@".\resources\pause.png");
                toolStripPauseButton.Text = "Pause";
            }
        }

        private void StartGame()
        {
            _game = new Game(_gameSize.X, _gameSize.Y);
            _game.TileChanged += new EventHandler<TileChangedEventArgs>(Game_TileChanged);
            _game.GameWon     += new EventHandler<GameWonEventArgs>(Game_GameWon);
            _game.GameEnd     += new EventHandler(Game_GameEnded);
            _paused = false;
            _pictureBoxGrid = new PictureBox[_gameSize.X, _gameSize.Y];
            InitializePictureGrid();
        }

        private void RestartGame()
        {
            if (_timer.Enabled) _timer.Stop();
            toolStripPlayerLabel.Text = "Player: ";
            toolStripTimeLabel.Text = "Time: ";
            toolStripPauseButton.Image = Image.FromFile(@".\resources\pause.png");
            toolStripPauseButton.Text = "Pause";
            _paused = false;

            _game = new Game(_gameSize.X, _gameSize.Y);
            _game.TileChanged += new EventHandler<TileChangedEventArgs>(Game_TileChanged);
            _game.GameWon     += new EventHandler<GameWonEventArgs>(Game_GameWon);
            _game.GameEnd     += new EventHandler(Game_GameEnded);
            UpdatePictureGrid();
        }

        #region Model Event Handlers

        private void Game_TileChanged(object? sender, TileChangedEventArgs e)
        {
            _pictureBoxGrid[e.X, e.Y].Image = e.PlayerOnTile == Player.X ? Image.FromFile(@".\resources\x.png") : Image.FromFile(@".\resources\o.png");
            
            toolStripPlayerLabel.Text = e.PlayerOnTile == Player.X ? "Player: O" : "Player: X"; // Set text on toolStripLabel
            if (!_timer.Enabled) _timer.Start();
            if (e.PlayerOnTile == Player.X) _xTime += _turnTime; else _oTime += _turnTime;
            _turnTime = 0;

            UpdateLabels();
        }

        private void Game_GameWon(object? sender, GameWonEventArgs e)
        {
            if (_timer.Enabled) _timer.Stop();
            toolStripPlayerLabel.Text = "Player: ";
            toolStripTimeLabel.Text = "Time: ";

            foreach (var v in _game.WinningPointStack) { Console.WriteLine(v.ToString()); }
            
            if (e.State == GameState.WON_BY_X)
            {
                for (int i = 0; i < e.WinningCoordList.Count; i++)
                {
                    _pictureBoxGrid[e.WinningCoordList[i].X, e.WinningCoordList[i].Y].Image = Image.FromFile($@".\resources\win_x.png");
                }
                MessageBox.Show("X won the game!", "Game Won!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            if (e.State == GameState.WON_BY_O)
            {
                for (int i = 0; i < e.WinningCoordList.Count; i++)
                {
                    _pictureBoxGrid[e.WinningCoordList[i].X, e.WinningCoordList[i].Y].Image = Image.FromFile($@".\resources\win_o.png");
                }
                MessageBox.Show("O won the game!", "Game Won!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            
            RestartGame();
        }

        private void Game_GameEnded(object? sender, EventArgs e)
        {
            MessageBox.Show("Draw!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            
            RestartGame();
        }
        
        #endregion

        #region Picture Box Event Handlers

        private void PictureBox_Click(object? sender, EventArgs e)
        {
            if (sender != null && !_paused)
            {
                PictureBox pictureBox = sender as PictureBox;
                _game.Move(Convert.ToInt32(pictureBox!.Tag.ToString()));
            }
        }

        private void PictureBox_MouseEnter(object? sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            
            int col = Convert.ToInt32(pictureBox!.Tag.ToString());
            if (_game.GetRow(col) > -1) _pictureBoxGrid[_game.GetRow(col), col].Image =
                _game.CurrentPlayer == Player.X ? Image.FromFile(@".\resources\prev_x.png") : Image.FromFile(@".\resources\prev_o.png");
        }

        private void PictureBox_MouseLeave(object? sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;

            int col = Convert.ToInt32(pictureBox!.Tag.ToString());
            if (_game.GetRow(col) > -1) _pictureBoxGrid[_game.GetRow(col), col].Image = Image.FromFile(@".\resources\border.png");
        }

        #endregion

        #region Form Event Handlers

        private void MainWindow_Load(object sender, EventArgs e)
        {
            BackColor = Color.WhiteSmoke;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _turnTime++;
            if (_turnTime % 100 < 10) toolStripTimeLabel.Text = $"Time: {(int)(_turnTime / 100)}:0{_turnTime % 100}";
            else toolStripTimeLabel.Text = $"Time: {(int)(_turnTime / 100)}:{_turnTime % 100}";
        }

        private void ToolStripPauseButton_Click(object sender, EventArgs e)
        {
            SetTimer();
        }

        #endregion

        #region Menu Event Handlers

        private void StartGameMenuItem_Click(object? sender, EventArgs e)
        {
            RestartGame();
        }

        private void SetSizeMenuItem_Click(object sender, EventArgs e)
        {
            SizeWindow sizeWindow = new();
            sizeWindow.ShowDialog();

            if (!_gameSize.Equals(sizeWindow.GameSize))
            {
                _gameSize = sizeWindow.GameSize;
                InitializeGame();
            }
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
    }
}