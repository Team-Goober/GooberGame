using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Music.Sfx;
using Sprint.Items;
using System.Diagnostics;

namespace Sprint.Functions.Collision;

internal class PickUpItem : ICommand
{
    private Player receiver;
    private Item effector;
    private SfxFactory sfxFactory;


    public PickUpItem(ICollidable receiver, ICollidable effector, Vector2 overlap) {
        this.receiver = (Player)receiver;
        this.effector = (Item)effector;
        sfxFactory = SfxFactory.GetInstance();
    }

    public void Execute()
    {
        // Try to pickup item
        bool didPickup = receiver.PickupItem(effector);
        if (didPickup)
        {
            // Only play sound if succeeded in picking up item
            sfxFactory.PlaySoundEffect("Item Pickup");
            // Make player pay up rupees. It is assumed that they have enough to pay
            Debug.Assert(receiver.GetInventory().TryConsumeStack(Inventory.RupeeLabel, effector.GetPrice()));

        }
    }
}