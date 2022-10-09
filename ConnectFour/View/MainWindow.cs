using System;

using ConnectFour.Model;

namespace ConnectFour
{
    public partial class MainWindow : Form
    {
        // The game's model agregated inside view
        private Game _game;
        // Picture box grid for the fields of the game
        private PictureBox[,] _pictureBoxGrid;

        public MainWindow()
        {
            InitializeComponent();
            _pictureBoxGrid  = new PictureBox[7, 7];
            _game            = new Game(7, 7);
            _game.TileChanged += new EventHandler<TileChangedEventArgs>(Game_TileChanged);
            _game.GameWon     += new EventHandler<GameWonEventArgs>(Game_GameWon);
            _game.GameEnd     += new EventHandler(Game_GameEnded);
            StartGameMenuItem.Click += StartGameMenuItem_Click;

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

        #region Model Event Handlers

        private void Game_TileChanged(object? sender, TileChangedEventArgs e)
        {
            _pictureBoxGrid[e.X, e.Y].BackColor = e.PlayerOnTile == Player.RED ? Color.Red : Color.Yellow;
        }

        private void Game_GameWon(object? sender, GameWonEventArgs e)
        {
            foreach (var v in _game.WinningPointStack)
            {
                Console.WriteLine(v.ToString());
            }

            for (int i = 0; i < e.WinningCoordList.Count; i++)
            {
                _pictureBoxGrid[e.WinningCoordList[i].X, e.WinningCoordList[i].Y].Image = Image.FromFile($@".\resources\win.png");
            }
            
            if (e.State == GameState.WON_BY_RED)
            {
                MessageBox.Show("Red won!", "Game Over!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            if (e.State == GameState.WON_BY_YELLOW)
            {
                MessageBox.Show("Yellow won!", "Game Over!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

            _game.StartGame();
            GamePanel.Controls.Clear();
            InitializePictureGrid();
        }

        private void Game_GameEnded(object? sender, EventArgs e)
        {
            MessageBox.Show("Draw!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            _game.StartGame();
            GamePanel.Controls.Clear();
            InitializePictureGrid();
        }

        #endregion

        #region Picture Box Event Handlers

        private void PictureBox_Click(object? sender, EventArgs e)
        {
            if (sender != null)
            {
                PictureBox pictureBox = sender as PictureBox;
                _game.Move(Convert.ToInt32(pictureBox!.Tag.ToString()));
            }
        }

        private void PictureBox_MouseEnter(object? sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            
            int col = Convert.ToInt32(pictureBox!.Tag.ToString());
            if (_game.GetRow(col) > -1) _pictureBoxGrid[_game.GetRow(col), col].BackColor
              = _game.CurrentPlayer == Player.RED ? Color.PaleVioletRed : Color.FromArgb(128, Color.Yellow);
        }

        private void PictureBox_MouseLeave(object? sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;

            int col = Convert.ToInt32(pictureBox!.Tag.ToString());
            if (_game.GetRow(col) > -1) _pictureBoxGrid[_game.GetRow(col), col].BackColor = Color.WhiteSmoke;
        }

        #endregion

        private void StartGameMenuItem_Click (object? sender, EventArgs e)
        {
            GamePanel.Controls.Clear();
            InitializePictureGrid();
            
            _game              = new Game(7, 7);
            _game.TileChanged += new EventHandler<TileChangedEventArgs>(Game_TileChanged);
            _game.GameWon     += new EventHandler<GameWonEventArgs>(Game_GameWon);
            _game.GameEnd     += new EventHandler(Game_GameEnded);
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            BackColor = Color.WhiteSmoke;
        }
    }
}