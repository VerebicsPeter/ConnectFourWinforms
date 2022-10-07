using ConnectFour.Model;

namespace ConnectFour
{
    public partial class MainWindow : Form
    {
        // the game's model agregated inside view
        private Game _game;
        // button grid for the fields of the game
        private Button[,] _buttonGrid;

        public MainWindow()
        {
            InitializeComponent();
            _game       = null!;
            _buttonGrid = null!;

            StartGameMenuItem.Click += StartGameMenuItem_Click;
        }

        private void InitializeButtonGrid()
        {
            int bx = GamePanel.Height / _game.Height;
            int by = GamePanel.Width / _game.Width;

            for (int i = 0; i < _game.Height; i++)
            {
                for (int j = 0; j < _game.Width; j++)
                {
                    Button button = new()
                    {
                        Size     = new Size(bx, by),
                        Location = new Point(j * bx, i * by),
                        Tag      = $"{i},{j}"
                    };
                    _buttonGrid[i, j] = button;
                    GamePanel.Controls.Add(button);
                }
            }
        }

        private void StartGameMenuItem_Click (object? sender, EventArgs e)
        {
            _game       = new Game(6, 7); // model instanciation
            _buttonGrid = new Button[6, 7]; // button grid instanciation
            InitializeButtonGrid();
        }
    }
}
