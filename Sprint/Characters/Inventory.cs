using System.Collections.Generic;
using System.Diagnostics;
using Sprint.HUD;
using Sprint.Interfaces;
using Sprint.Items;

namespace Sprint.Characters;

internal class Inventory
{



    public delegate void SelectorChooseDelegate(IPowerup powerup);
    public event SelectorChooseDelegate SelectorChooseEvent;

    private List<IPowerup> allPowerups;
    private IAbility selectedA, selectedB;
    private IAbility[,] abilitySlots;

    public Inventory()
    {
        allPowerups = new();
        abilitySlots = new IAbility[CharacterConstants.INVENTORY_ROWS, CharacterConstants.INVENTORY_COLUMNS];
    }


    /// <summary>
    /// Adds item to object's inventory
    /// </summary>
    /// <param name="item">ItemType to increment</param>
    public void AddItem(IPowerup powerup)
    {
        allPowerups.Add(powerup);
    }

    /// <summary>
    /// Removes item from object's inventory
    /// </summary>
    /// <param name="item">ItemType to decrement</param>
    public void DeleteItem(IPowerup powerup)
    {
        allPowerups.Remove(powerup);
        if(powerup is  IAbility)
        {
            for (int i = 0; i < abilitySlots.GetLength(0); i++){
                for (int j = 0; i < abilitySlots.GetLength(1); j++)
                {
                    if (abilitySlots[i, j] == powerup)
                    {
                        abilitySlots[i, j] = null;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Check if object has any of a ItemType
    /// </summary>
    /// <param name="item"> ItemType to check</param>
    /// <returns>true - has at least 1 item false - has less than 1</returns>
    /*public bool HasItem(ItemType item)
    {
        return itemDictionary[item] > 0;
    }*/

    /// <summary>
    /// Check the amount of items that the player has.
    /// </summary>
    /// <param name="item"></param>
    /// <returns>The amount of item</returns>
    /*public int GetItemAmount(ItemType item)
    {
        return itemDictionary[item];
    }*/

    public void Select(int r, int c)
    {
        SelectorChooseEvent?.Invoke(abilitySlots[r, c]);
    }

    public IAbility GetSelectionA()
    {
        return selectedA;
    }

    public IAbility GetSelectionB()
    {
        return selectedB;
    }
}