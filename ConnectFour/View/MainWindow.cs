using ConnectFour.Model;

namespace ConnectFour
{
    public partial class MainWindow : Form
    {
        private Game _game = new Game(7, 7);

        public MainWindow()
        {
            InitializeComponent();

            Console.WriteLine("\nConsole for form MainWindow:\n");

            _game.Play(); // console testing method call
        }
    }
}