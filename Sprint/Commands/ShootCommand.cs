
using Microsoft.Xna.Framework;
using Sprint.Interfaces;

namespace Sprint.Commands
{
    internal class ShootCommand : ICommand
    {

        private IProjectileFactory factory;

        public ShootCommand(IProjectileFactory factory)
        {
            this.factory = factory;
        }

        public void Execute()
        {
            factory.Create();
        }
    }
}
