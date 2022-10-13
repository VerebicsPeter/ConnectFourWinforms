using ConnectFour.Model;
using ConnectFour.View;

using Timer = System.Windows.Forms.Timer;

namespace ConnectFour
{
    public partial class MainWindow : Form
    {
        private Point  _gameSize;
        private Timer  _timer;
        private double _turnTime; private bool _paused;
        
        private Game _game; // The game's model agregated inside view
        private PictureBox[,] _pictureBoxGrid; // Picture box grid for the fields of the game

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            StartGameMenuItem.Click += StartGameMenuItem_Click;

            _gameSize = new Point(10, 10);

            _game              = new Game(_gameSize.X, _gameSize.Y);
            _game.TileChanged += new EventHandler<TileChangedEventArgs>(Game_TileChanged);
            _game.GameWon     += new EventHandler<GameWonEventArgs>(Game_GameWon);
            _game.GameEnd     += new EventHandler(Game_GameEnded);
            _pictureBoxGrid    = new PictureBox[_gameSize.X, _gameSize.Y];
            InitializePictureGrid();

            _timer       = new Timer();
            _timer.Tick += new EventHandler(UpdateTimeLabel);
            _timer.Interval = 10;
            _paused = false;
        }

        #endregion

        private void InitializeGame()
        {
            if (_timer.Enabled) _timer.Stop();
            toolStripPlayerLabel.Text = "Player: ";
            toolStripTimeLabel.Text = "Time: ";

            GamePanel.Controls.Clear();

            if (_gameSize.X == 10 && _gameSize.Y == 10) {
                this.Size = new Size(640, 720);
                GamePanel.Size = new Size(600, 600);
            }
            else if (_gameSize.X == 20 && _gameSize.Y == 20) {
                this.Size = new Size(760, 840);
                GamePanel.Size = new Size(720, 720);
            }
            else if (_gameSize.X == 30 && _gameSize.Y == 30) {
                this.Size = new Size(940, 1024);
                GamePanel.Size = new Size(900, 900);
            }

            _game              = new Game(_gameSize.X, _gameSize.Y);
            _game.TileChanged += new EventHandler<TileChangedEventArgs>(Game_TileChanged);
            _game.GameWon     += new EventHandler<GameWonEventArgs>(Game_GameWon);
            _game.GameEnd     += new EventHandler(Game_GameEnded);
            _pictureBoxGrid    = new PictureBox[_gameSize.X, _gameSize.Y];
            InitializePictureGrid();
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

        private void RestartGame()
        {
            if (_timer.Enabled) _timer.Stop();
            // Update label text
            toolStripPlayerLabel.Text = "Player: ";
            toolStripTimeLabel.Text = "Time: ";

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
            // Set text on toolStripLabel
            toolStripPlayerLabel.Text = e.PlayerOnTile == Player.X ? "Player: O" : "Player: X";
            if (!_timer.Enabled) {_timer.Start();}
            _turnTime = 0;
        }

        private void Game_GameWon(object? sender, GameWonEventArgs e)
        {
            if (_timer.Enabled) _timer.Stop();
            toolStripPlayerLabel.Text = "Player: ";
            toolStripTimeLabel.Text = "Time: ";

            foreach (var v in _game.WinningPointStack)
            {
                Console.WriteLine(v.ToString());
            }
            
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

        private void StartGameMenuItem_Click(object? sender, EventArgs e)
        {
            RestartGame();
        }

        private void SetSizeMenuItem_Click(object sender, EventArgs e)
        {
            SizeWindow sizeWindow = new();
            sizeWindow.ShowDialog();

            _gameSize = sizeWindow.GameSize;

            InitializeGame();
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void UpdateTimeLabel(object? sender, EventArgs e)
        {
            _turnTime++;
            if (_turnTime % 100 < 10) toolStripTimeLabel.Text = $"Time: {Math.Round(_turnTime / 100)}:0{_turnTime % 100}";
            else toolStripTimeLabel.Text = $"Time: {Math.Round(_turnTime / 100)}:{_turnTime % 100}";
        }

        private void ToolStripPauseButton_Click(object sender, EventArgs e)
        {
            if (!_paused && _game.Moves > 0)
            {
                _paused = true;
                toolStripPauseButton.Image = Image.FromFile(@".\resources\resume.png");
                toolStripPauseButton.Text  = "Resume";
                _timer.Stop();
            }
            else
            {
                _paused = false;
                toolStripPauseButton.Image = Image.FromFile(@".\resources\pause.png");
                toolStripPauseButton.Text  = "Pause";
                if (_game.Moves > 0) _timer.Start();
            }
        }
        #endregion
    }
}
