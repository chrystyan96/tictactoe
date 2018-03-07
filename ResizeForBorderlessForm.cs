using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace DvTicTacToe
{
    class ResizeForBorderlessForm
    {
        enum Stats
        {
            None,
            ResizeNS,
            ResizeWE,
            ResizeNSWE,
            Move
        }

        private Form target;
        private Stats current = Stats.None;
        private int diff_x, diff_y;
        public Boolean AllowMove = true, AllowResizeAll = true, AllowResizeNS = false, AllowResizeWE = false;

        public ResizeForBorderlessForm(Form source)
        {
            this.target = source;
            if (source.FormBorderStyle != FormBorderStyle.None) source.FormBorderStyle = FormBorderStyle.None;
            target.MouseMove += new MouseEventHandler(target_MouseMove);
            target.MouseDown += new MouseEventHandler(target_MouseDown);
            target.MouseUp += new MouseEventHandler(target_MouseUp);
        }

        void target_MouseUp(object sender, MouseEventArgs e)
        {
            current = Stats.None;
        }

        void target_MouseDown(object sender, MouseEventArgs e)
        {
            if (target.WindowState != FormWindowState.Maximized)
            {
                diff_x = Cursor.Position.X - target.Location.X;
                diff_y = Cursor.Position.Y - target.Location.Y;
                if (e.X > target.Width - 5 && e.Y > target.Height - 5) current = Stats.ResizeNSWE;
                else if (e.X > target.Width - 5) current = Stats.ResizeWE;
                else if (e.Y > target.Height - 5) current = Stats.ResizeNS;
                else current = Stats.Move;
            }
        }

        void target_MouseMove(object sender, MouseEventArgs e)
        {
            if (target.WindowState != FormWindowState.Maximized)
            {
                if (e.X > target.Width - 5 && e.Y > target.Height - 5)
                {
                    if (AllowResizeAll || (AllowResizeNS && AllowResizeWE)) Cursor.Current = Cursors.SizeNWSE;
                }
                else if (e.X > target.Width - 5)
                {
                    if (AllowResizeAll || AllowResizeWE) Cursor.Current = Cursors.SizeWE;
                }
                else if (e.Y > target.Height - 5)
                {
                    if (AllowResizeAll || AllowResizeNS) Cursor.Current = Cursors.SizeNS;
                }
                else Cursor.Current = Cursors.Arrow;

                switch (current)
                {
                    case Stats.Move:
                        if (AllowMove) target.Location = new Point(Cursor.Position.X - diff_x, Cursor.Position.Y - diff_y);
                        break;

                    case Stats.ResizeNSWE:
                        if (AllowResizeAll || (AllowResizeNS && AllowResizeWE)) target.Size = new Size(Cursor.Position.X - target.Location.X, Cursor.Position.Y - target.Location.Y);
                        break;

                    case Stats.ResizeNS:
                        if (AllowResizeAll || AllowResizeNS) target.Size = new Size(target.Width, Cursor.Position.Y - target.Location.Y);
                        break;

                    case Stats.ResizeWE:
                        if (AllowResizeAll || AllowResizeWE) target.Size = new Size(Cursor.Position.X - target.Location.X, target.Height);
                        break;
                }
            }
        }


    }
}
