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
using WinFormsApp1;
namespace WinFormsApp1
{
    public class Scanner
    {
        public event Action<List<(int i, int j, int pattern)>> PatternDetected;
        private List<List<(int x, int y)>> allPatterns = new();
        public List<List<(int x, int y)>> Patterns => allPatterns;
        public Scanner()
        {
            allPatterns.Add(new List<(int, int)> { (-1, 0), (0, 1), (-1, 1) });
            allPatterns.Add(new List<(int, int)> { (1, -1), (1, 1), (2, -1), (2, 1), (3, 0) });
            allPatterns.Add(new List<(int, int)> { (1, 0), (2, 0) });
            allPatterns.Add(new List<(int, int)> { (0, 1), (0, 2) });
            allPatterns.Add(new List<(int, int)> { (1, 1), (2, 1), (2, 0), (2, -1) });
            allPatterns.Add(new List<(int, int)> { (0, 1), (0, 3), (0, 4), (0, 5), (0, 6), (0, 8), (0, 9), (-1, +2), (1, +2), (-1, +7), (1, +7) });
        }
        public void OnTurnFinishedScan(int[,] field)
        {
            var found = ScanField(field);
            if (found.Count > 0)
                PatternDetected?.Invoke(found);
        }
        private List<(int i, int j, int pattern)> ScanField(int[,] field)
        {
            var result = new List<(int, int, int)>();
            for (int p = 0; p < allPatterns.Count; p++)
            {
                var pattern = allPatterns[p];
                for (int i = 0; i < Terrain.N; i++)
                {
                    for (int j = 0; j < Terrain.N; j++)
                    {
                        if (Matches(field, pattern, i, j))
                            result.Add((i, j, p));
                    }
                }
            }
            return result;
        }
        private bool Matches(int[,] field, List<(int x, int y)> pattern, int i, int j)
        {
            foreach (var (dx, dy) in pattern)
            {
                int x = i + dx;
                int y = j + dy;
                if (!Terrain.IsInside(x, y)) return false;
                if (field[x, y] != 1) return false;
            }
            return true;
        }
    }
}