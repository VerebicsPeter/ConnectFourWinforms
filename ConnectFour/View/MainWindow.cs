using ConnectFour.Model;

namespace ConnectFour
{
    public partial class MainWindow : Form
    {
        private Game _game = new Game(10, 10);

        public MainWindow()
        {
            InitializeComponent();

            Console.WriteLine("\nConsole for form MainWindow:\n");

            _game.Play();
        }
    }
}