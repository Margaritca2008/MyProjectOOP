using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace WinFormsApp1
{
    public partial class Form2 : Form
    {
        private Button[,] cells;
        private Scanner scanner;
        private const int cellSize = 20;
        public Form2(Scanner scanner)
        {
            InitializeComponent();
            this.scanner = scanner;
            cells = new Button[Terrain.N, Terrain.N];
            scanner.PatternDetected += OnPatternDetected;
            for (int i = 0; i < Terrain.N; i++)
            {
                for (int j = 0; j < Terrain.N; j++)
                {
                    var b = new Button{
                        Size = new Size(cellSize, cellSize),
                        Location = new Point(j * cellSize, i * cellSize),
                        BackColor = Color.White,
                        Enabled = false
                    };
                    Controls.Add(b);
                    cells[i, j] = b;
                }
            }

            ClientSize = new Size(Terrain.N * cellSize, Terrain.N * cellSize + 40);
        }
        private void OnPatternDetected(List<(int k, int l, int pattern)> patterns)
        {
            for (int a = 0; a < Terrain.N; a++)
                for (int b = 0; b < Terrain.N; b++)
                    cells[a, b].BackColor = Color.White;
            foreach (var (i, j, p) in patterns)
            {
                var pattern = scanner.Patterns[p];
                foreach (var (dx, dy) in pattern)
                {
                    int x = i + dx;
                    int y = j + dy;
                    if (Terrain.IsInside(x, y))
                        cells[x, y].BackColor = Color.Green;
                }
            }
        }
    }
}