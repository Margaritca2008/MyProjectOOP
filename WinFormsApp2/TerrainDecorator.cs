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
using System.Drawing;

namespace WinFormsApp1
{
    public abstract class TerrainDecorator : ITerrain
    {
        protected readonly ITerrain inner;
        protected TerrainDecorator(ITerrain inner) => this.inner = inner;
        public virtual Cell[,] Cells => inner.Cells;
        public virtual int N => inner.N;
        public virtual void Update() => inner.Update();
        public virtual void Draw(Graphics g, int cellSize) => inner.Draw(g, cellSize);
    }
}
