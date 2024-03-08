

using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Projectile;

namespace Sprint.Functions.SecondaryItem
{
    internal class PlaceSmoke : ICommand
    {

        IProjectile proj;
        Smoke smoke;

        public PlaceSmoke(IProjectile proj, Smoke smoke)
        {
            this.proj = proj;
            this.smoke = smoke;
        }

        public void Execute()
        {
            Vector2 position = proj.GetPosition();
            proj.Delete();
            smoke.SetPosition(position);
            smoke.Create();
        }
    }
}
