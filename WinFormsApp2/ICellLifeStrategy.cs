using System.Windows.Forms;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp1;
namespace WinFormsApp1
{
    public interface ICellLifeStrategy
    {
        CellType GetNext(CellContext ctx);
    }
    public class ClassicEmptyStrategy : ICellLifeStrategy
    {
        public CellType GetNext(CellContext ctx)
        {
            return ctx.WhiteN == 3 ? CellType.White : CellType.Empty;
        }
    }
    public class ClassicWhiteStrategy : ICellLifeStrategy
    {
        public CellType GetNext(CellContext ctx)
        {
            return (ctx.AliveN == 2 || ctx.AliveN==3) ? CellType.White : CellType.Empty;
        }
    }
    public class ColonyWhiteStrategy : ICellLifeStrategy
    {
        public CellType GetNext(CellContext ctx)
        {
            if (ctx.BlackN > ctx.WhiteN + 1)
            {
                ctx.myCell.AssignToColony(ctx.FirstBlackNeighbor.Colony);
                return CellType.Black;
            }
            return (ctx.WhiteN == 2 || ctx.WhiteN == 3) ? CellType.White : CellType.Empty;
        }
    }

    public class ColonyBlackStrategy : ICellLifeStrategy
    {
        public CellType GetNext(CellContext ctx)
        {
            if (ctx.WhiteN > ctx.BlackN)
            {
                return CellType.White;
            }
            return (ctx.BlackN == 2 || ctx.BlackN == 3) ? CellType.Black : CellType.Empty;
        }
    }

}
