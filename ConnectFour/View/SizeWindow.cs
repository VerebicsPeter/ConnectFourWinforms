namespace ConnectFour.View
{
    public partial class SizeWindow : Form
    {
        public Point GameSize { get; private set; }

        public SizeWindow()
        {
            GameSize = new Point(10, 10);
            InitializeComponent();
        }

        private void button_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked) { GameSize = new Point(20, 20); }
            if (radioButton3.Checked) { GameSize = new Point(30, 30); } 
            Close();
        }
    }
}
