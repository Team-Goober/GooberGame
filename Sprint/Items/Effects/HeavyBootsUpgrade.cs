using Sprint.Characters;
using Sprint.Interfaces.Powerups;
using Sprint.Projectile;
using System.Collections.Generic;
namespace Sprint.Items.Effects
{
    internal class HeavyBootsUpgrade : IUpgradeEffect
    {

        private IAbility baseAbility;

        public void Execute(Player player)
        {
            // Activate base to shoot hook
            baseAbility.Activate();

            // Make all new moving hooks heavy
            SpawnOrRetractHookEffect effect = baseAbility.GetEffect() as SpawnOrRetractHookEffect;
            Queue<Hook> hooks = effect.GetHooks();
            for(int i=0; i<hooks.Count; i++)
            {
                Hook h = hooks.Dequeue();
                h.SetHeavyShooter(true);
                hooks.Enqueue(h);
            }


        }

        public void Reverse(Player player)
        {
            // Do nothing
        }

        public void SetBase(IPowerup powerup)
        {
            baseAbility = powerup as IAbility;
        }

        public IEffect Clone()
        {
            return new HeavyBootsUpgrade();
        }

    }
}
