using System;
using System.Collections.Generic;

using WinFormsApp1;
using System.Diagnostics;
namespace WinFormsApp1
{
    public interface IWorldFactory
    {
        ICellStrategyProvider CreateStrategies();
        ICellFactory CreateCellFactory(ICellStrategyProvider strategies);
    }
    public class ClassicWorldFactory : IWorldFactory
    {
        public ICellStrategyProvider CreateStrategies()
        {
            var map = new Dictionary<CellType, ICellLifeStrategy>
        {
            { CellType.Empty, new ClassicEmptyStrategy() },
            { CellType.White, new ClassicWhiteStrategy() },
            { CellType.Black, new ClassicWhiteStrategy() }
        };

            return new CellStrategyProvider(map);
        }

        public ICellFactory CreateCellFactory(ICellStrategyProvider strategies)
        {
            return new SimpleCellFactory(strategies);
        }
    }
    public class ColoniesWorldFactory : IWorldFactory
    {
        public ICellStrategyProvider CreateStrategies()
        {
            var map = new Dictionary<CellType, ICellLifeStrategy>
        {
            { CellType.Empty, new ClassicEmptyStrategy() },
            { CellType.White, new ColonyWhiteStrategy() },
            { CellType.Black, new ColonyBlackStrategy() }
        };

            return new CellStrategyProvider(map);
        }

        public ICellFactory CreateCellFactory(ICellStrategyProvider strategies)
        {
            return new SimpleCellFactory(strategies);
        }
    }
}