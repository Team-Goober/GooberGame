using System.Collections.Generic;
using System.Diagnostics;
using Sprint.Items;

namespace Sprint.Characters;

internal class Inventory
{

    private Dictionary<ItemType, int> itemDictionary;
    private DungeonState dungeon;

    public Inventory(DungeonState dungeon)
    {
        this.dungeon = dungeon;

        itemDictionary = new Dictionary<ItemType, int>()
        {
            { ItemType.Arrow, 0 },
            { ItemType.Heart, 0 },
            { ItemType.SpecialKey, 0 },
            { ItemType.Rupee, 0 },
            { ItemType.Triforce, 0 },
            { ItemType.Shield, 0 },
            { ItemType.Clock, 0 },
            { ItemType.Bow, 0 },
            { ItemType.Fence, 0 },
            { ItemType.Ladder, 0 },
            { ItemType.BlueArrow, 0 },
            { ItemType.Fairy, 0 },
            { ItemType.BlueOrb, 0 },
            { ItemType.BlueRing, 0 },
            { ItemType.RedRing, 0 },
            { ItemType.BlueBoomerang, 0 },
            { ItemType.Boomerang, 0 },
            { ItemType.BlueHeart, 0 },
            { ItemType.BlueTorch, 0 },
            { ItemType.Sword, 0 },
            { ItemType.Citem, 0 },
            { ItemType.ShieldRed, 0 },
            { ItemType.Potion, 0 },
            { ItemType.Cake, 0 },
            { ItemType.Key, 0 },
            { ItemType.Compass, 0 },
            { ItemType.Paper, 0}
        };
    }

    /// <summary>
    /// Adds item to object's inventory
    /// </summary>
    /// <param name="item">ItemType to increment</param>
    public void PickupItem(ItemType item)
    {
        itemDictionary[item]++;

        // Handle map-updating items
        switch (item)
        {
            case ItemType.Paper:
                dungeon.GetMap().RevealAll();
                break;
            case ItemType.Compass:
                dungeon.GetMap().PlaceCompass();
                break;
            default:
                break;
        }

    }

    /// <summary>
    /// Removes item from object's inventory
    /// </summary>
    /// <param name="item">ItemType to decrement</param>
    public void ConsumeItem(ItemType item)
    {
        itemDictionary[item]--;
    }

    /// <summary>
    /// Check if object has any of a ItemType
    /// </summary>
    /// <param name="item"> ItemType to check</param>
    /// <returns>true - has at least 1 item false - has less than 1</returns>
    public bool HasItem(ItemType item)
    {
        return itemDictionary[item] > 0;
    }

    /// <summary>
    /// Check the amount of items that the player has.
    /// </summary>
    /// <param name="item"></param>
    /// <returns>The amount of item</returns>
    public int getItemAmount(ItemType item)
    {
        return itemDictionary[item];
    }
}