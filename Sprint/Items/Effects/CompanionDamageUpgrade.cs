using Sprint.Characters;
using Sprint.Characters.Companions;
using Sprint.Interfaces.Powerups;
using System.Collections.Generic;

namespace Sprint.Items.Effects
{
    internal class CompanionDamageUpgrade : IUpgradeEffect
    {

        private float multiplier;
        private IPowerup basePowerup;
        private int prevStackSize; // Used to track the number of new swords added
        private Player player;


        public CompanionDamageUpgrade(float multiplier)
        {
            this.multiplier = multiplier;
        }

        public void Execute(Player player)
        {
            if (player != null)
                this.player = player;
            // Activate base to spawn sword
            basePowerup.GetEffect().Execute(player);

            // Set damage of newest swords
            Stack<SwordCompanion> currentStack = GetSwords();
            Stack<SwordCompanion> tempStack = new();
            // If size of stack increases, must add to all the new ones on top
            for(int i = 0; i < currentStack.Count - prevStackSize; i++)
            {
                SwordCompanion top = currentStack.Pop();
                top.MultiplyDamage(multiplier);
                tempStack.Push(top);
            }
            // Add them back to the stack
            SwordCompanion sc;
            while (tempStack.TryPop(out sc))
            {
                currentStack.Push(sc);
            }
        }

        public void Reverse(Player player)
        {
            // Do nothing
        }

        // Returns the list of sword this applies to
        private Stack<SwordCompanion> GetSwords()
        {
            IUpgradePowerup asUpgrade = basePowerup as IUpgradePowerup;
            if(asUpgrade == null)
            {
                // Get swords from a base
                return ((SwordCompanionEffect)basePowerup.GetEffect()).GetSwords();
            }
            else
            {
                // Get swords from an upgrade by going down the chain
                return ((SwordCompanionEffect)asUpgrade.GetBaseEffect()).GetSwords();
            }
        }

        public void SetBase(IPowerup powerup)
        {
            basePowerup = powerup;


            // Set damage of each one in base
            Stack<SwordCompanion> tempStack = new();
            Stack<SwordCompanion> currentStack = GetSwords();

            SwordCompanion sc;
            while(currentStack.TryPop(out sc))
            {
                // Set damage
                sc.MultiplyDamage(multiplier);
                tempStack.Push(sc);
            }
            // Put swords back
            while (tempStack.TryPop(out sc))
            {
                currentStack.Push(sc);
            }

            prevStackSize = currentStack.Count;
        }

    }
}
