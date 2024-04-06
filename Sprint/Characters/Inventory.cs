using System.Collections.Generic;
using System.Diagnostics;
using Sprint.HUD;
using Sprint.Items;

namespace Sprint.Characters;

internal class Inventory
{

    private Dictionary<ItemType, int> itemDictionary;

    public delegate void InventoryUpdateDelegate(ItemType it, int prev, int next, List<ItemType> ownedUpgrades);
    public event InventoryUpdateDelegate InventoryEvent;

    public delegate void SelectorChooseDelegate(ItemType item);
    public event SelectorChooseDelegate SelectorChooseEvent;

    // Array of Items representing the item that fills each slot once picked up
    public static readonly ItemType[,] Slots = { { ItemType.Boomerang, ItemType.Bomb, ItemType.Arrow, ItemType.BlueCandle},
        { ItemType.Flute, ItemType.Meat, ItemType.BluePotion, ItemType.BlueWand } };

    // TODO: Shield and sword upgrade not implemented
    public static readonly Dictionary<ItemType, List<ItemType>> UpgradePaths = new() {
        { ItemType.Boomerang, new() { ItemType.Boomerang, ItemType.BlueBoomerang } },
        { ItemType.Arrow, new() { ItemType.Arrow, ItemType.BlueArrow } },
        { ItemType.BlueCandle, new() { ItemType.BlueCandle, ItemType.RedCandle } },
        { ItemType.BluePotion, new() { ItemType.BluePotion, ItemType.RedPotion } } };

    public static readonly Dictionary<ItemType, int> BundleSize = new()
    {
        { ItemType.Bomb, 4 },
        { ItemType.Rupee, 5 }
    };

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
            { ItemType.BlueCandle, 0},
            { ItemType.RedCandle, 0},
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
        int quantity = 1;
        if (BundleSize.ContainsKey(item))
        {
            quantity = BundleSize[item];
        }
        itemDictionary[item] += quantity;
        InventoryEvent?.Invoke(item, itemDictionary[item] - quantity, itemDictionary[item], ownedUpgrades(item));
    }

    /// <summary>
    /// Removes item from object's inventory
    /// </summary>
    /// <param name="item">ItemType to decrement</param>
    public void ConsumeItem(ItemType item)
    {
        itemDictionary[item]--;
        InventoryEvent?.Invoke(item, itemDictionary[item] + 1, itemDictionary[item], ownedUpgrades(item));
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
    public int GetItemAmount(ItemType item)
    {
        return itemDictionary[item];
    }

    public void Select(int r, int c)
    {
        // Select the highest owned upgrade of the item
        List<ItemType> upgrades = ownedUpgrades(Slots[r, c]);
        // Check if any owned item was selected
        if (upgrades.Count > 0)
        {
            selected = upgrades[upgrades.Count - 1];
            SelectorChooseEvent?.Invoke(selected);
        }
    }

    public ItemType GetSelection()
    {
        return selected;
    }

    private List<ItemType> ownedUpgrades(ItemType it)
    {
        // Track path of upgrade to only show the highest attained upgrade
        // Default to just this item
        List<ItemType> upgradePath = new() { it };
        foreach (KeyValuePair<ItemType, List<ItemType>> upgradePair in UpgradePaths)
        {
            // Use upgrade path found
            if (upgradePair.Value.Contains(it))
            {
                upgradePath = new(upgradePair.Value);
                break;
            }
        }

        // Remove items that arent owned 
        for(int i = upgradePath.Count - 1; i>= 0; i--)
        {
            if (!HasItem(upgradePath[i]))
            {
                upgradePath.RemoveAt(i);
            }
        }

        return upgradePath;
        
    }
}