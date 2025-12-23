using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace WinFormsApp1
{
    public class Colony
    {
        public HashSet<Point> members { get; } = new();
        public Point Direction { get; set; }
        private static int _nextId = 1;
        public int Id { get; }
        public bool SkipNextTurn { get; set; } = false;
        private Random rnd = new();

        public Colony(IEnumerable<Point> cells)
        {
            Id = _nextId++;
            foreach (var p in cells) members.Add(p);
            Direction = RandomDirection(new Point(0, 0));
        }

        public void SetNewRandomDirection()
        {
            Direction = RandomDirection(Direction);
        }

        private Point RandomDirection(Point old)
        {
            var dirs = new List<Point>
            {
                new Point(1, 0),
                new Point(-1, 0),
                new Point(0, 1),
                new Point(0, -1)
            };
            dirs.RemoveAll(d => d == old);
            var rnd = new Random();
            return dirs[rnd.Next(dirs.Count)];
        }
    }

    public class Colonys
    {
        private readonly Terrain terrain;
        private readonly List<Colony> colonys = new();
        private readonly Random rnd = new();
        public Colonys(Terrain terrain)
        {
            this.terrain = terrain ?? throw new ArgumentNullException(nameof(terrain));
        }

        public void CreateColoniesFromScanner(Scanner scanner, bool replace = false)
        {
            if (replace) colonys.Clear();
            for (int i =0; i < terrain.N; i++)
            {
                for (int j = 0; j< terrain.N; j++)
                {
                    if ((terrain.Cells[i, j].Colony!=null || terrain.Cells[i, j].Type == CellType.Black) && !terrain.Cells[i, j].Colony.members.Contains(new Point(i, j)))
                    {
                        terrain.Cells[i, j].Colony.members.Add(new Point(i, j));
                    }
                    if (terrain.Cells[i, j].Type == CellType.White) //если клетка уже практически былая то удаляем из колонии и делаем белой
                    {
                        foreach (var colony in colonys)
                        {
                            if (colony.members.Contains(new Point(i, j)))
                            {
                                colony.members.Remove(new Point(i, j));
                            }
                        }
                    }
                }
            }
            var found = scanner.ScanField();
            int N = terrain.N;
            foreach (var (i, j, p) in found)
            {
                var coords = scanner.Patterns[p]
                    .Select(off => new Point(i + off.x, j + off.y))
                    .ToList();

                if (coords.Count == 0) continue;
                if (coords.All(pt => terrain.Cells[pt.X, pt.Y].Colony != null))
                    continue;
                var newCol = new Colony(coords);
                foreach (var pt in coords)
                {
                    terrain.Cells[pt.X, pt.Y].AssignToColony(newCol);
                    terrain.Cells[pt.X, pt.Y].AssignType(CellType.Black);
                }
                colonys.Add(newCol);
                
            }
        }
        private void MergeColonies(Colony a, Colony b)
        {
            if (a == b || b == null) return;
            foreach (var t in b.members)
            {
                a.members.Add(t);
                terrain.Cells[t.X, t.Y].AssignToColony(a);
            }
            colonys.Remove(b);
            a.SetNewRandomDirection();
        }
        public void MoveColonies()
        {
            var colonysCopy = colonys.ToList();
            foreach (var colony in colonysCopy)
            {
                if (colonys.Contains(colony)) TryMove(colony);
            }
        }

        private void TryMove(Colony colony)
        {
            if (colony.SkipNextTurn)
            {
                colony.SkipNextTurn = false;
                return;
            }
            var dir = colony.Direction;
            var target = new HashSet<Point>();
            foreach (var p in colony.members)
            {
                int nx = p.X + dir.X;
                int ny = p.Y + dir.Y;
                if (nx < 0 || ny < 0 || nx >= terrain.N || ny >= terrain.N)
                {
                    colony.SetNewRandomDirection();
                    return;
                }
                var a = terrain.Cells[nx, ny];
                if (a.Colony != null && a.Colony != colony)
                {
                    MergeColonies(colony, a.Colony);
                    colony.SkipNextTurn = true;
                    a.Colony.SkipNextTurn = true;
                    return;
                }
                target.Add(new Point(nx, ny));
            }
            foreach (var p in colony.members)
            {
                terrain.Cells[p.X, p.Y].RemoveFromColony();
            }
            foreach (var p in target)
            {
                terrain.Cells[p.X, p.Y].AssignToColony(colony);
            }
            colony.members.Clear();
            foreach (var p in target) colony.members.Add(p);
        }
    }
}
