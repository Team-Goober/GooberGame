using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Music.Sfx;
using Sprint.Items;

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
        // Only play sound if succeeded in picking up item
        if (didPickup)
            sfxFactory.PlaySoundEffect("Item Pickup");
    }
}