
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;

namespace Sprint.Characters
{
    public abstract class EnemyAI
    {

        MoveAI(GameTime gameTime);

        public enum Directions
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        }

    }
}
