using System;
using System.Windows.Forms;
using System.Drawing;

namespace WinFormsApp1
{
    public interface ITerrain
    {
        Cell[,] Cells { get; }
        int N { get; }
        void Update();
        void Draw(Graphics g, int cellSize);
    }
}
