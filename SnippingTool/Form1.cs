namespace SnippingTool {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            this.Hide();
            using (var snipper = new SnippingArea()) {
                if (snipper.ShowDialog() == DialogResult.OK) {
                    Rectangle rect = snipper.Rectangle;
                    Bitmap bmp = new Bitmap(rect.Width, rect.Height);
                    using (Graphics g = Graphics.FromImage(bmp)) {
                        g.CopyFromScreen(rect.Left, rect.Top, 0, 0, rect.Size);
                    }
                    pictureBox1.Image = bmp;
                }
            }
            this.Show();
        }
    }
    }
