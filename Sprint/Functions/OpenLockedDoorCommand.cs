using Microsoft.Xna.Framework;
using Sprint.Characters;
using Sprint.Factory.Door;
using Sprint.Interfaces;
using Sprint.Levels;

namespace Sprint.Functions;

public class OpenLockedDoorCommand: ICommand
{
    private Player receiver;
    private Door effector;
    private Vector2 distance; // displacement to push over

    public OpenLockedDoorCommand(ICollidable receiver, ICollidable effector, Vector2 overlap) {
        this.receiver = (Player)receiver;
        this.effector = (Door)effector;
        distance = -overlap;
    }

    public void Execute()
    {
        // Moves receiver by displacement
        if (receiver.inventory.HasItem(ItemType.SpecialKey))
        {
            effector.SetOpen(true);
            receiver.inventory.ConsumeItem(ItemType.SpecialKey);
        }
        else
        {
            // Moves receiver by displacement
            receiver.Move(distance);
        }
    }
}