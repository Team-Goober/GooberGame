using Sprint.Interfaces;
using Sprint.Sprite;
using System;
using System.Diagnostics;

namespace Sprint.Commands
{
	internal class PreviousEnemyCommand : ICommand
	{
		private EnemyManager enemyManager;

		public PreviousEnemyCommand(EnemyManager enemyManager)
		{
			this.enemyManager = enemyManager;
		}

		public void Execute()
		{
			
			enemyManager.PreviousEnemy();
		}
	}
}
