using Sprint.Interfaces;
using Sprint.Sprite;
using System;
using System.Diagnostics;

namespace Sprint.Commands
{
	internal class PreviousEnemyCommand : ICommand
	{
		private CycleEnemy enemies;

		public PreviousEnemyCommand(CycleEnemy enemies)
		{
			this.enemies = enemies;
		}

		public void Execute()
		{
			
			enemies.PreviousEnemy();
		}
	}
}
