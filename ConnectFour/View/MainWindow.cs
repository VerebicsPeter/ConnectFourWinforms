using ConnectFour.Model;

namespace ConnectFour
{
    public partial class MainWindow : Form
    {
        private Game _game = new Game();

        public MainWindow()
        {
            InitializeComponent();

            Console.WriteLine("\nConsole for form MainWindow:\n");

            _game.PrintGame();
        }
    }
}