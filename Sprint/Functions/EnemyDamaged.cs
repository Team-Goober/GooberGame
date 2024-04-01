using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using System.Diagnostics;
using Sprint.Characters;
using static Sprint.Characters.Character;

namespace Sprint.Functions.Collision
{
	internal class EnemyProjectile : ICommand
	{
		private Enemy enemy;

		public EnemyProjectile(ICollidable enemy, Vector2 uselessValue)
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
