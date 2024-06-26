﻿using Microsoft.Xna.Framework;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Items;

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
        // Try to use up a player key
        bool hasKey = receiver.GetInventory().TryConsumeStack(Inventory.KeyLabel);
        // Only continue if player had a key
        if (hasKey)
        {
            // Open this door
            effector.SetOpen(true);
            // Open other side of the door
            effector.GetOtherFace().SetOpen(true);
        }
        
        // Moves receiver by displacement
        receiver.Move(distance);
    }
}