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
using System;
using System.Collections.Generic;
using System.Linq;

namespace WinFormsApp1
{
    public class Scanner
    {
        public event Action<List<(int i, int j, int pattern)>> PatternDetected;
        public List<List<(int x, int y)>> allPatterns = new();
        private readonly Func<CellType, bool> isAlive;
        private readonly Func<CellType, bool> isNeighbor;
        private readonly ITerrain terrain;

        public List<List<(int x, int y)>> Patterns => allPatterns;

        public Scanner(ITerrain terrain, Func<CellType, bool> isAlivePredicate, Func<CellType, bool> isNeighborPredicate = null)
        {
            this.terrain = terrain ?? throw new ArgumentNullException(nameof(terrain));
            isAlive = isAlivePredicate ?? throw new ArgumentNullException(nameof(isAlivePredicate));
            isNeighbor = isNeighborPredicate ?? isAlive;
            allPatterns.Add(new List<(int, int)> { (-1, 0), (0, 1), (-1, 1), (0, 0) }); 
            allPatterns.Add(new List<(int, int)> { (1, -1), (1, 1), (2, -1), (2, 1), (3, 0), (0, 0) });
            allPatterns.Add(Rotate90(new List<(int, int)> { (1, -1), (1, 1), (2, -1), (2, 1), (3, 0), (0, 0) }));
            allPatterns.Add(new List<(int, int)> { (1, 0), (2, 0), (0, 0) });
            allPatterns.Add(new List<(int, int)> { (0, 1), (0, 2), (0, 0) });
            allPatterns.Add(new List<(int, int)> { (1, 1), (2, 1), (2, 0), (2, -1), (0, 0) });
            allPatterns.Add(new List<(int, int)> { (0, 1), (0, 3), (0, 4), (0, 5), (0, 6), (0, 8), (0, 9), (-1, +2), (1, +2), (-1, +7), (1, +7), (0, 0) });
        }

        public void RaisePatternDetected(List<(int i, int j, int pattern)> patterns) => PatternDetected?.Invoke(patterns);

        public List<(int i, int j, int pattern)> ScanField()
        {
            var Cells = terrain.Cells;
            var res = new List<(int, int, int)>();
            for (int p = 0; p < allPatterns.Count; p++)
            {
                var pattern = allPatterns[p];
                for (int i = 0; i < terrain.N; i++)
                    for (int j = 0; j < terrain.N; j++)
                        if (Matches(Cells, pattern, i, j) && CheckingNeighbors(Cells, pattern, i, j))
                            res.Add((i, j, p));
            }
            return res;
        }

        private bool Matches(Cell[,] Cells, List<(int x, int y)> pattern, int i, int j)
        {
            foreach (var pair in pattern)
            {
                int x = i + pair.x, y = j + pair.y;
                if (x < 0 || y < 0 || x >= terrain.N || y >= terrain.N) return false;
                if (!isAlive(Cells[x, y].Type)) return false;
            }
            return true;
        }

        private bool CheckingNeighbors(Cell[,] Cells, List<(int x, int y)> pattern, int i, int j)
        {
            var neighbors = new List<(int dx, int dy)> { (-1, 0), (1, 0), (0, -1), (0, 1) };
            var coords = pattern.Select(p => (p.x + i, p.y + j)).ToList();
            foreach (var (cx, cy) in coords)
            {
                foreach (var nb in neighbors)
                {
                    int nx = cx + nb.dx, ny = cy + nb.dy;
                    if (nx < 0 || ny < 0 || nx >= terrain.N || ny >= terrain.N) continue;
                    if (!coords.Contains((nx, ny)) && isNeighbor(Cells[nx, ny].Type)) return false;
                }
            }
            return true;
        }

        private List<(int x, int y)> Rotate90(List<(int x, int y)> pattern)
        {
            var r = new List<(int, int)>();
            foreach (var p in pattern) r.Add((-p.y, p.x));
            return r;
        }
        private List<(int x, int y)> Rotate180(List<(int x, int y)> pattern) => Rotate90(Rotate90(pattern));
        private List<(int x, int y)> Rotate270(List<(int x, int y)> pattern) => Rotate90(Rotate180(pattern));
    }
}
