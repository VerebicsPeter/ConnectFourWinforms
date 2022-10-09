using ConnectFour.Model;

namespace ConnectFour
{
    public partial class MainWindow : Form
    {
        // the game's model agregated inside view
        private Game _game;
        // button grid for the fields of the game
        private PictureBox[,] _pictureBoxGrid;

        public MainWindow()
        {
            InitializeComponent();
            _game            = new Game(10, 10);
            _pictureBoxGrid  = new PictureBox[10, 10];
            _game.TileChanged += new EventHandler<TileChangedEventArgs>(Game_TileChanged);
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
                    _pictureBoxGrid[i, j]        = pictureBox;
                    _pictureBoxGrid[i, j].Click += PictureBox_Click;
                    //_pictureBoxGrid[0, i].MouseHover += PictureBox_MouseHover;
                    GamePanel.Controls.Add(pictureBox);
                }
            }
        }

        #region Model Event Handlers

        private void Game_TileChanged(object? sender, TileChangedEventArgs e)
        {
            Console.WriteLine("from event handler");
            _pictureBoxGrid[e.X, e.Y].Image = e.PlayerOnTile == Player.RED ? Image.FromFile(@".\resources\red.png") 
                                                                           : Image.FromFile(@".\resources\yellow.png");
            _pictureBoxGrid[e.X, e.Y].Update();
        }

        #endregion

        private void PictureBox_Click (object? sender, EventArgs e)
        {
            if (sender != null)
            {
                PictureBox pictureBox = sender as PictureBox;
                _game.Move(Convert.ToInt32(pictureBox!.Tag.ToString()));
            }
        }

        private void StartGameMenuItem_Click (object? sender, EventArgs e)
        {
            GamePanel.Controls.Clear();
            InitializePictureGrid();
            
            _game              = new Game(10, 10); // model instanciation
            _game.TileChanged += new EventHandler<TileChangedEventArgs>(Game_TileChanged);

        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }
    }
}
