using Sprint.Characters;
using Sprint.Characters.Companions;
using Sprint.Interfaces.Powerups;
using Sprint.Projectile;
using System.Collections.Generic;

namespace Sprint.Items.Effects
{
    internal class CompanionDamageUpgrade : IUpgradeEffect
    {

        public float damage;
        private IPowerup basePowerup;
        

        public void Execute(Player player)
        {
            // Activate base to spawn sword
            basePowerup.Apply(player);

            // Set damage of newest sword
            Stack<SwordCompanion> currentStack = ((SwordCompanionEffect)basePowerup.GetEffect()).GetSwords();
            SwordCompanion latest = currentStack.Peek();
            latest.SetDamage(damage);
        }

        public void Reverse(Player player)
        {
            // Do nothing
        }

        public void SetBase(IPowerup powerup)
        {
            basePowerup = powerup;
            // Set damage of each one in base
            Stack<SwordCompanion> tempStack = new();
            Stack<SwordCompanion> currentStack = ((SwordCompanionEffect)basePowerup.GetEffect()).GetSwords();

            SwordCompanion sc;
            while(currentStack.TryPop(out sc))
            {
                // Set damage
                sc.SetDamage(damage);
                tempStack.Push(sc);
            }
            // Put swords back
            while (tempStack.TryPop(out sc))
            {
                currentStack.Push(sc);
            }
        }

        public IEffect Clone()
        {
            return new CompanionDamageUpgrade() { damage = damage };
        }
    }
}
