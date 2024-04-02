using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using System.Diagnostics;
using Sprint.Characters;
using static Sprint.Characters.Character;

namespace Sprint.Functions.Collision
{
	internal class EnemyDamaged : ICommand
	{
		private Enemy enemy;

		public EnemyDamaged(ICollidable enemy, Vector2 uselessValue, Vector2 overlap)
		{
			this.enemy = (Enemy)enemy;
		}

		public void Execute()
		{
			// receiver takes damage
			enemy.TakeDamage();
		}
	}
}
