using Microsoft.Xna.Framework;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Levels;

namespace Sprint.Door;

public class OpenLockedDoorCommand : ICommand
{
    private Player receiver;
    private Door effector;
    private Vector2 distance; // displacement to push over

    public OpenLockedDoorCommand(ICollidable receiver, ICollidable effector, Vector2 overlap)
    {
        this.receiver = (Player)receiver;
        this.effector = (Door)effector;
        distance = -overlap;
    }

    public void Execute()
    {
        // Unlock if able to
        if (receiver.inventory.HasItem(ItemType.Key))
        {
            effector.SetOpen(true);
            // Open other side of the door
            effector.GetOtherFace().SetOpen(true);
            // Use up a key
            receiver.inventory.ConsumeItem(ItemType.Key);
        }
        else
        {
            // Moves receiver by displacement
            receiver.Move(distance);
        }
    }
}