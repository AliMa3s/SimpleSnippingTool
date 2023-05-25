using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnippingTool {
    public partial class SnippingArea : Form {
        private Point _startPoint;
        private Rectangle _rect;

        public Rectangle Rectangle { get { return _rect; } }

        public SnippingArea() {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.BackColor = Color.White;
            this.Opacity = 0.2;
            this.ShowInTaskbar = false;
            this.DoubleBuffered = true; //Enable double buffering

            //Calculate the bounds to include all screens
            Rectangle bounds =  Screen.AllScreens.Aggregate(Rectangle.Empty, (total, screen) => Rectangle.Union(total, screen.Bounds));
            this.Bounds = bounds;

            this.MouseDown += new MouseEventHandler(SnippingArea_MouseDown);
            this.MouseUp += new MouseEventHandler(SnippingArea_MouseUp);
            this.MouseMove += new MouseEventHandler(SnippingArea_MouseMove);
            this.Paint += new PaintEventHandler(SnippingArea_Paint);
        }

        private void SnippingArea_MouseDown(object sender, MouseEventArgs e) {
            _startPoint = e.Location;
            _rect = new Rectangle(e.Location, new Size(0, 0));
        }

        private void SnippingArea_MouseMove(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Left) return;
            _rect = new Rectangle(_startPoint.X, _startPoint.Y, e.X - _startPoint.X, e.Y - _startPoint.Y);
            this.Invalidate();
        }

        private void SnippingArea_MouseUp(object sender, MouseEventArgs e) {
            if (_rect.Width <= 0 || _rect.Height <= 0) return;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void SnippingArea_Paint(object sender, PaintEventArgs e) {
            using (Brush br = new SolidBrush(Color.FromArgb(120, Color.Black))) {
                e.Graphics.FillRectangle(br, this.ClientRectangle);
            }
            if (_rect.Width > 0 && _rect.Height > 0) {
                // Shrink the rectangle by 2 pixels on all sides
                Rectangle innerRect = new Rectangle(_rect.Left + 1, _rect.Top + 1, _rect.Width - 2, _rect.Height - 2);

                using (Pen pen = new Pen(Color.Red, 8)) {
                    e.Graphics.DrawRectangle(pen, _rect);
                }
                using (Brush br = new SolidBrush(Color.Transparent)) {
                    e.Graphics.FillRectangle(br, innerRect);
                }
            }
        }



        private void SnippingArea_Load(object sender, EventArgs e) {

        }
    }
}
