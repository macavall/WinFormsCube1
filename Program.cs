using System;
using System.Windows.Forms;

namespace RotatingCubeDemo
{
    // Program entry point
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RotatingCubeForm());
        }
    }
}