using Microsoft.Xna.Framework;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Items;

namespace Sprint.Functions.Collision;

internal class PickUpItem : ICommand
{
    private Player receiver;
    private Item effector;


    public PickUpItem(ICollidable receiver, ICollidable effector, Vector2 overlap) {
        this.receiver = (Player)receiver;
        this.effector = (Item)effector;
    }

    public void Execute()
    {
        // Moves receiver by displacement
        receiver.PickupItem(effector);
    }
}