using ConnectFour.Persistence;
using ConnectFour.Model;
using ConnectFour.View;
using Timer = System.Windows.Forms.Timer;

namespace ConnectFour
{
    public partial class MainWindow : Form
    {
        private IGameDataAccess _dataAcess = null!;
        private Game _model; // Games model
        private PictureBox[,] _pictureBoxGrid;

        private readonly Timer _timer;
        private bool _isPaused;
        private int _tTime; // current player's turn
        private int _xTime, _oTime; // sum of players' times
        
        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            StartGameMenuItem.Click += StartGameMenuItem_Click;

            _dataAcess = new SaveFileDataAcess();

            _model = new Game(_dataAcess);
            _model.GameWon     += new EventHandler<GameWonEventArgs>(Game_GameWon);
            _model.GameEnd     += new EventHandler(Game_GameEnded);
            _model.TileChanged += new EventHandler<TileChangedEventArgs>(Game_TileChanged);
            _pictureBoxGrid     = new PictureBox[_model.Height, _model.Width];
            InitializePictureGrid();

            _timer = new Timer();
            _timer.Tick    += new EventHandler(Timer_Tick);
            _timer.Interval = 10;
            _isPaused = false;
            
            toolStripPauseButton.Enabled = false; SaveGameMenuItem.Enabled = false;
        }
        #endregion

        private void InitializePictureGrid()
        {
            int bx = GamePanel.Height / _model.Height;
            int by = GamePanel.Width / _model.Width;

            for (int i = 0; i < _model.Height; i++)
            {
                for (int j = 0; j < _model.Width; j++)
                {
                    string fname = "border.png";

                    char value = _model.GetValue(i, j);
                    switch (value)
                    {
                        case 'x': fname = "x.png"; break;
                        case 'o': fname = "o.png"; break;
                        default: break;
                    }

                    PictureBox pictureBox = new()
                    {
                        Size     = new Size(bx, by),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Location = new Point(j * bx, i * by),
                        Tag      = $"{j}",
                        Image    = Image.FromFile($@".\resources\{fname}")
                    };

                    _pictureBoxGrid[i, j] = pictureBox;
                    _pictureBoxGrid[i, j].Click      += PictureBox_Click;
                    _pictureBoxGrid[i, j].MouseEnter += PictureBox_MouseEnter;
                    _pictureBoxGrid[i, j].MouseLeave += PictureBox_MouseLeave;
                    GamePanel.Controls.Add(pictureBox);
                }
            }
        }

        private void ResetPictureGrid()
        {
            for (int i = 0; i < _model.Height; i++)
            {
                for (int j = 0; j < _model.Width; j++)
                {
                    _pictureBoxGrid[i, j].Image = Image.FromFile(@".\resources\border.png");
                }
            }
        }

        private void ResetLabels()
        {
            toolStripPlayerLabel.Text = "Player:";
            toolStripTimeLabel.Text = "Time:" ;
            toolStripXLabel.Text = "X-Time:";
            toolStripOLabel.Text = "O-Time:";
        }

        private void SetPauseButton() // Sets pause button based on _isPaused
        {
            if (_isPaused)
            {
                toolStripPauseButton.Image = Image.FromFile(@".\resources\resume.png");
                toolStripPauseButton.Text = "Resume";
            }
            else
            {
                toolStripPauseButton.Image = Image.FromFile(@".\resources\pause.png");
                toolStripPauseButton.Text = "Pause";
            }
        }

        private void SetTimer()
        {
            if (_isPaused)
            {
                _isPaused = false; _timer.Start();
            }
            else
            {
                _isPaused = true; _timer.Stop();
            }
            SetPauseButton();
        }

        private void UpdatePlayerTimeLabels()
        {
            toolStripXLabel.Text = $"X-Time: {_xTime / 100}:{_xTime % 100}";
            toolStripOLabel.Text = $"O-Time: {_oTime / 100}:{_oTime % 100}";
        }

        private void SetGameSize() // Resizes window and panel
        {
            if (GamePanel.Controls.Count > 0) GamePanel.Controls.Clear();

            if (_model.Height == 10 && _model.Width == 10)
            {
                this.Size = new Size(640, 720);
                GamePanel.Size = new Size(600, 600);
            }
            else if (_model.Height == 20 && _model.Width == 20)
            {
                this.Size = new Size(760, 840);
                GamePanel.Size = new Size(720, 720);
            }
            else if (_model.Height == 30 && _model.Width == 30)
            {
                this.Size = new Size(940, 1020);
                GamePanel.Size = new Size(900, 900);
            }
        }

        private void StartGame() // Starts a new game
        {
            if (_timer.Enabled) _timer.Stop();
            _tTime = 0; _xTime = 0; _oTime = 0; // Reset the times
            toolStripPauseButton.Enabled = false; SaveGameMenuItem.Enabled = false; // These should be disabled on a new game
            _isPaused = false; SetPauseButton();
            ResetLabels();

            _model.StartGame();
            _pictureBoxGrid = new PictureBox[_model.Height, _model.Width];
            InitializePictureGrid();
        }

        private void RestartGame() // Restarts a game
        {
            if (_timer.Enabled) _timer.Stop();
            _tTime = 0; _xTime = 0; _oTime = 0; // Reset the times
            toolStripPauseButton.Enabled = false; SaveGameMenuItem.Enabled = false;
            _isPaused = false; SetPauseButton();
            ResetLabels();

            _model.StartGame(); // Starts a game in games model

            ResetPictureGrid();
        }

        #region Model Event Handlers

        private void Game_TileChanged(object? sender, TileChangedEventArgs e)
        {
            _pictureBoxGrid[e.X, e.Y].Image = e.PlayerOnTile == Player.X 
                ? Image.FromFile(@".\resources\x.png") 
                : Image.FromFile(@".\resources\o.png")
            ;
            
            toolStripPlayerLabel.Text = e.PlayerOnTile == Player.X 
                ? "Player: O" 
                : "Player: X"
            ; // Set text on toolStripPlayerLabel

            if (e.PlayerOnTile == Player.X) _xTime += _tTime; else _oTime += _tTime;

            if (!_timer.Enabled) _timer.Start();
            _tTime = 0; // Reset the turns time

            UpdatePlayerTimeLabels();
        }

        private void Game_GameWon(object? sender, GameWonEventArgs e)
        {
            if (_timer.Enabled) _timer.Stop();
            toolStripPlayerLabel.Text = "Player:"; toolStripTimeLabel.Text = "Time:";
            
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
            if (sender != null && !_isPaused)
            {
                if (!toolStripPauseButton.Enabled) toolStripPauseButton.Enabled = true;
                if (!SaveGameMenuItem.Enabled) SaveGameMenuItem.Enabled = true;

                PictureBox pictureBox = sender as PictureBox;
                _model.Move(Convert.ToInt32(pictureBox!.Tag.ToString()));
            }
        }
        
        private void PictureBox_MouseEnter(object? sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            
            int col = Convert.ToInt32(pictureBox!.Tag.ToString());
            if (_model.GetRow(col) > -1) _pictureBoxGrid[_model.GetRow(col), col].Image =
                _model.CurrentPlayer == Player.X ? Image.FromFile(@".\resources\prev_x.png") : Image.FromFile(@".\resources\prev_o.png");
        }

        private void PictureBox_MouseLeave(object? sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;

            int col = Convert.ToInt32(pictureBox!.Tag.ToString());
            if (_model.GetRow(col) > -1) _pictureBoxGrid[_model.GetRow(col), col].Image = Image.FromFile(@".\resources\border.png");
        }
        #endregion

        #region Form Event Handlers

        private void MainWindow_Load(object sender, EventArgs e)
        {
            BackColor = Color.WhiteSmoke;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _tTime++;
            toolStripTimeLabel.Text = _tTime % 100 < 10
                ? $"Time: {_tTime / 100}:0{_tTime % 100}"
                : $"Time: {_tTime / 100}:{_tTime % 100}"
            ;
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

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SetGameSizeMenuItem_Click(object sender, EventArgs e)
        {
            bool enabled = _timer.Enabled;
            _timer.Stop();

            SizeWindow sizeWindow = new();
            sizeWindow.ShowDialog();

            Size gameSize = new (_model.Height, _model.Width);
            if (!gameSize.Equals(sizeWindow.GameSize))
            {
                _model.SetSize(sizeWindow.GameSize.Height, sizeWindow.GameSize.Width);
                SetGameSize();
                StartGame();
            }
            else
            {
                if (enabled) _timer.Start();
            }
        }

        private async void SaveGameMenuItem_Click(object sender, EventArgs e)
        {
            bool enabled = _timer.Enabled;
            _timer.Stop();

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (_model.CurrentPlayer == Player.X) _xTime += _tTime; else _oTime += _tTime; // add the currents players time to the counter
                    await _model.SaveGameAsync(saveFileDialog.FileName, _xTime, _oTime);
                }
                catch
                {
                    MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (enabled) _timer.Start();
        }

        private async void LoadGameMenuItem_Click(object sender, EventArgs e)
        {
            bool enabled = _timer.Enabled;
            _timer.Stop();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ResetLabels();
                    if (GamePanel.Controls.Count > 0) GamePanel.Controls.Clear();
                    Point times = await _model.LoadGameAsync(openFileDialog.FileName);
                    
                    _isPaused = false; SetPauseButton();
                    toolStripPauseButton.Enabled = true;
                    
                    _xTime = times.X; _oTime = times.Y;
                    _tTime = 0;

                    UpdatePlayerTimeLabels();
                    if (_model.CurrentPlayer == Player.X) toolStripPlayerLabel.Text = "Player: X"; else toolStripPlayerLabel.Text = "Player: O";
                    
                    SetGameSize();
                    _pictureBoxGrid = new PictureBox[_model.Height, _model.Width];
                    InitializePictureGrid();

                    if (!_timer.Enabled) _timer.Start();
                }
                catch
                {
                    MessageBox.Show("Játék betöltése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a fájlformátum.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (enabled) _timer.Start();
        }
        #endregion
    }
}