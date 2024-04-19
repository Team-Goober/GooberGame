using Sprint.Characters;
using Sprint.Interfaces.Powerups;
using Sprint.Projectile;
namespace Sprint.Items.Effects
{
    internal class HeavyBootsUpgrade : IUpgradeEffect
    {

        private IAbility baseAbility;
        private bool prevState; // Last heavyness value

        public void Execute(Player player)
        {
            // Activate base to shoot hook
            baseAbility.Activate();
            // Make hook heavy shooter
            SpawnOrRetractHookEffect effect = baseAbility.GetEffect() as SpawnOrRetractHookEffect;
            Hook hook = effect.GetHook();
            prevState = hook.GetHeavyShooter();
            hook.SetHeavyShooter(true);
        }

        public void Reverse(Player player)
        {
            // If hook is out, remove the heavyness
            SpawnOrRetractHookEffect effect = baseAbility.GetEffect() as SpawnOrRetractHookEffect;
            if(effect?.GetHook() != null)
            {
                effect.GetHook().SetHeavyShooter(prevState);
            }
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
