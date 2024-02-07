using Sprint.Interfaces;
using Sprint.Sprite;
using System.Diagnostics;

namespace Sprint.Commands
{
	internal class NextEnemyCommand : ICommand
	{
		private EnemyManager enemyManager;

		public NextEnemyCommand(EnemyManager enemyManager)
		{
			this.enemyManager = enemyManager;
		}

		public void Execute()
		{
			
			enemyManager.NextEnemy();
		}
	}
}
