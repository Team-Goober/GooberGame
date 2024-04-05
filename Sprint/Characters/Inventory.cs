using System.Collections.Generic;
using System.Diagnostics;
using Sprint.HUD;
using Sprint.Items;

namespace Sprint.Characters;

internal class Inventory
{

    private Dictionary<ItemType, int> itemDictionary;

    public delegate void InventoryUpdateDelegate(ItemType it, int prev, int next);
    public event InventoryUpdateDelegate InventoryEvent;

    public delegate void SelectorChooseDelegate(ItemType item);
    public event SelectorChooseDelegate SelectorChooseEvent;

    // Array of Items representing the item that fills each slot once picked up
    public static readonly ItemType[,] Slots = { { ItemType.BlueBoomerang, ItemType.Bomb, ItemType.Bow, ItemType.Candle},
        { ItemType.Flute, ItemType.Meat, ItemType.RedPotion, ItemType.BlueWand } };
    private ItemType selected;

    public Inventory()
    {

        itemDictionary = new Dictionary<ItemType, int>()
        {
            { ItemType.Arrow, 0 },
            { ItemType.HeartPiece, 0 },
            { ItemType.SpecialKey, 0 },
            { ItemType.Rupee, 0 },
            { ItemType.Triforce, 0 },
            { ItemType.Shield, 0 },
            { ItemType.Clock, 0 },
            { ItemType.Bow, 0 },
            { ItemType.Raft, 0 },
            { ItemType.Ladder, 0 },
            { ItemType.BlueArrow, 0 },
            { ItemType.Fairy, 0 },
            { ItemType.Bomb, 0 },
            { ItemType.BlueRing, 0 },
            { ItemType.RedRing, 0 },
            { ItemType.BlueBoomerang, 0 },
            { ItemType.Boomerang, 0 },
            { ItemType.Heart, 0 },
            { ItemType.BlueWand, 0 },
            { ItemType.Sword, 0 },
            { ItemType.Bracelet, 0 },
            { ItemType.Book, 0 },
            { ItemType.BluePotion, 0 },
            { ItemType.RedPotion, 0 },
            { ItemType.Key, 0 },
            { ItemType.Compass, 0 },
            { ItemType.Map, 0},
            { ItemType.Candle, 0},
            { ItemType.Flute, 0},
            { ItemType.Meat, 0},
        };
    }


    /// <summary>
    /// Adds item to object's inventory
    /// </summary>
    /// <param name="item">ItemType to increment</param>
    public void PickupItem(ItemType item)
    {
        itemDictionary[item]++;
        InventoryEvent?.Invoke(item, itemDictionary[item] - 1, itemDictionary[item]);
    }

    /// <summary>
    /// Removes item from object's inventory
    /// </summary>
    /// <param name="item">ItemType to decrement</param>
    public void ConsumeItem(ItemType item)
    {
        itemDictionary[item]--;
        InventoryEvent?.Invoke(item, itemDictionary[item] + 1, itemDictionary[item]);
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

    public void Select(ItemType item)
    {
        selected = item;
        SelectorChooseEvent?.Invoke(item);
    }

    public ItemType GetSelection()
    {
        return selected;
    }
}