
using Microsoft.Xna.Framework;
using Sprint.Interfaces;

namespace Sprint.Commands
{
    internal class ShootCommand : ICommand
    {

        private IProjectileFactory factory;
        private ISprite item;
        private float speed;

        public ShootCommand(IProjectileFactory factory, ISprite proj, float newSpeed)
        {
            this.factory = factory;
            this.item = proj;
            this.speed = newSpeed;
        }

        public void Execute()
        {
            factory.Stage(this.speed);
            factory.Create(this.item);
        }
    }
}
