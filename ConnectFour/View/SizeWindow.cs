namespace ConnectFour.View
{
    public partial class SizeWindow : Form
    {
        public Size GameSize { get; private set; }

        public SizeWindow()
        {
            GameSize = new Size(10, 10);
            InitializeComponent();
        }

        private void button_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked) { GameSize = new Size(20, 20); }
            if (radioButton3.Checked) { GameSize = new Size(30, 30); }
            Close();
        }
    }
}