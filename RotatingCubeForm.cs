using System;
using System.Drawing;
using System.Windows.Forms;

namespace RotatingCubeDemo
{
    public partial class RotatingCubeForm : Form
    {
        private float angle = 0f;
        private readonly System.Windows.Forms.Timer timer;
        private readonly PictureBox pictureBox;

        // Cube vertices (centered at origin, edge length 2)
        private readonly Point3D[] vertices = new Point3D[]
        {
            new Point3D(-1, -1, -1), // 0
            new Point3D(1, -1, -1),  // 1
            new Point3D(1, 1, -1),   // 2
            new Point3D(-1, 1, -1),  // 3
            new Point3D(-1, -1, 1),  // 4
            new Point3D(1, -1, 1),   // 5
            new Point3D(1, 1, 1),    // 6
            new Point3D(-1, 1, 1)    // 7
        };

        // Cube edges (connecting vertex indices)
        private readonly int[,] edges = new int[,]
        {
            {0, 1}, {1, 2}, {2, 3}, {3, 0}, // Front face
            {4, 5}, {5, 6}, {6, 7}, {7, 4}, // Back face
            {0, 4}, {1, 5}, {2, 6}, {3, 7}  // Connecting edges
        };

        public RotatingCubeForm()
        {
            // Initialize form
            this.Text = "Rotating Cube";
            this.Size = new Size(400, 400);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Initialize PictureBox
            pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Black
            };
            pictureBox.Paint += PictureBox_Paint;
            this.Controls.Add(pictureBox);

            // Initialize timer
            timer = new System.Windows.Forms.Timer
            {
                Interval = 16 // ~60 FPS
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            angle += 0.05f; // Rotate 0.05 radians per frame
            pictureBox.Invalidate(); // Trigger redraw
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Center of the PictureBox
            float centerX = pictureBox.Width / 2f;
            float centerY = pictureBox.Height / 2f;

            // Projection parameters
            float scale = 100f; // Size of cube
            float distance = 4f; // Camera distance for perspective

            // Project 3D vertices to 2D
            PointF[] projected = new PointF[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                // Rotate around Y-axis
                float x = vertices[i].X * (float)Math.Cos(angle) + vertices[i].Z * (float)Math.Sin(angle);
                float z = -vertices[i].X * (float)Math.Sin(angle) + vertices[i].Z * (float)Math.Cos(angle);
                float y = vertices[i].Y;

                // Perspective projection
                float factor = distance / (distance - z);
                float px = x * factor * scale + centerX;
                float py = y * factor * scale + centerY;
                projected[i] = new PointF(px, py);
            }

            // Draw edges
            using (Pen pen = new Pen(Color.White, 2f))
            {
                for (int i = 0; i < edges.GetLength(0); i++)
                {
                    int v1 = edges[i, 0];
                    int v2 = edges[i, 1];
                    g.DrawLine(pen, projected[v1], projected[v2]);
                }
            }

            // Draw vertices
            using (Brush brush = new SolidBrush(Color.Red))
            {
                foreach (PointF p in projected)
                {
                    g.FillEllipse(brush, p.X - 3, p.Y - 3, 6, 6);
                }
            }
        }
    }

    // Simple 3D point structure
    public struct Point3D
    {
        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        public Point3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}