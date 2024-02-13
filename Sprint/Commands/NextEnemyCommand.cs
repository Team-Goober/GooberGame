using Sprint.Interfaces;
using Sprint.Sprite;
using System.Diagnostics;

namespace Sprint.Commands
{
	internal class NextEnemyCommand : ICommand
	{
		private CycleEnemy enemies;

		public NextEnemyCommand(CycleEnemy enemies)
		{
			this.enemies = enemies;
		}

		public void Execute()
		{
			
			enemies.NextEnemy();
		}
	}
}
