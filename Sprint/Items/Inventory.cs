using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection.Emit;
using Sprint.Characters;
using Sprint.Interfaces.Powerups;

namespace Sprint.Items;

internal class Inventory
{
    // Labels for resources to be used by other classes
    public const string KeyLabel = "key";
    public const string RupeeLabel = "rupee";

    // Event for an ability being chosen
    public delegate void SelectorChooseDelegate(int box, IAbility ability);
    public event SelectorChooseDelegate SelectorChooseEvent;

    // Event for list of all items changing
    public delegate void ListingUpdateDelegate(Dictionary<string, IPowerup> listing);
    public event ListingUpdateDelegate ListingUpdateEvent;

    private Dictionary<string, IPowerup> allPowerups; // All powerups, identified by string label
    private IAbility[,] abilitySlots; // Array of slots to assign abilities to
    private IAbility[] selectedBoxes; // Boxes for abilities bound to buttons

    public Inventory()
    {
        allPowerups = new();
        abilitySlots = new IAbility[CharacterConstants.INVENTORY_ROWS, CharacterConstants.INVENTORY_COLUMNS];
        selectedBoxes = new IAbility[CharacterConstants.SELECT_BOXES];
    }

    // Adds a powerup to the full list but not the slots
    public void AddPowerup(IPowerup powerup)
    {
        string label = powerup.GetLabel();
        // Must not already have the powerup
        Debug.Assert(!HasPowerup(label));
        // If powerup already has an entry, replace it
        if (allPowerups.ContainsKey(label))
        {
            allPowerups[label] = powerup;
        }
        // Otherwise, make new dictionary entry
        else
        {
            allPowerups.Add(label, powerup);
        }
        ListingUpdateEvent?.Invoke(allPowerups);
    }

    // Adds an ability to the first available slot
    public void AddToSlots(IAbility ability)
    {
        // Search for empty slot
        for (int i = 0; i < abilitySlots.GetLength(0); i++)
        {
            for (int j = 0; j < abilitySlots.GetLength(1); j++)
            {
                if (abilitySlots[i, j] == null)
                {
                    // Add to empty slot
                    abilitySlots[i, j] = ability;
                    return;
                }
            }
        }
        // Error if no available slot found
        Debug.Assert(false);
    }

    // Deletes all references to the powerup
    public void DeletePowerup(IPowerup powerup)
    {
        string label = powerup.GetLabel();
        // Must have valid entry for powerup
        Debug.Assert(HasPowerup(label));
        // Remove from full list
        allPowerups.Remove(label);
        ListingUpdateEvent?.Invoke(allPowerups);

        if (powerup is IAbility)
        {
            // Check ability array and delete if found
            for (int i = 0; i < abilitySlots.GetLength(0); i++)
            {
                for (int j = 0; j < abilitySlots.GetLength(1); j++)
                {
                    if (abilitySlots[i, j] != null && abilitySlots[i, j].GetLabel() == label)
                    {
                        abilitySlots[i, j] = null;
                    }
                }
            }
            // Check bound boxes and delete if found
            for (int i = 0; i < selectedBoxes.Length; i++)
            {
                if (selectedBoxes[i] != null && selectedBoxes[i].GetLabel() == label)
                {
                    selectedBoxes[i] = null;
                    SelectorChooseEvent?.Invoke(i, null);
                }
            }
        }
    }

    // Returns the number of powerups in a stack
    public int StackQuantity(string resource)
    {
        if (HasPowerup(resource))
        {
            // Powerup must have type IStackedPowerup
            Debug.Assert(allPowerups[resource] is IStackedPowerup);
            return ((IStackedPowerup)allPowerups[resource]).Quantity();
        }
        // Default to zero
        else
        {
            return 0;
        }
    }

    // Try to consume a powerup from stack, and return true if succeeded
    public bool TryConsumeStack(string resource)
    {
        // No resources owned
        if (StackQuantity(resource) <= 0)
            return false;
        // Consume one
        ((IStackedPowerup)allPowerups[resource]).AddAmount(-1);
        return true;
    }

    // True if invnetory has a valid entry for the powerup
    public bool HasPowerup(string label)
    {
        return allPowerups.ContainsKey(label) && allPowerups[label] != null;
    }

    // Returns the entry for a powerup. Null if not owned
    public IPowerup GetPowerup(string label)
    {
        if (!allPowerups.ContainsKey(label))
        {
            return null;
        }
        return allPowerups[label];
    }

    // Returns true if there is an empty slot
    public bool SlotsAvailable()
    {
        for (int i = 0; i < abilitySlots.GetLength(0); i++)
        {
            for (int j = 0; j < abilitySlots.GetLength(1); j++)
            {
                if (abilitySlots[i, j] == null)
                {
                    return true;
                }
            }
        }
        return false;

    }

    // Assign powerup at row r and column c in slots to bound box b
    public void Select(int b, int r, int c)
    {
        selectedBoxes[b] = abilitySlots[r, c];
        SelectorChooseEvent?.Invoke(b, abilitySlots[r, c]);
    }

    // Delete the ability at a slot
    public void Drop(int r, int c)
    {
        DeletePowerup(abilitySlots[r, c]);
    }

    // Return the ability bound to box b
    public IAbility GetSelection(int b)
    {
        return selectedBoxes[b];
    }

    // Replaces references to a powerup with an upgrade
    public void ReplaceWithDecorator(string prev, IPowerup next)
    {
        if (next is IAbility)
        {
            IAbility abilityNext = next as IAbility;
            // Replace reference in the slot array
            for (int i = 0; i < abilitySlots.GetLength(0); i++)
            {
                for (int j = 0; j < abilitySlots.GetLength(1); j++)
                {
                    if (abilitySlots[i, j] != null && abilitySlots[i, j].GetLabel() == prev)
                    {
                        abilitySlots[i, j] = abilityNext;
                    }
                }
            }
            // Replace references in bound boxes
            for (int i = 0; i < selectedBoxes.Length; i++)
            {
                if (selectedBoxes[i] != null && selectedBoxes[i].GetLabel() == prev)
                {
                    selectedBoxes[i] = abilityNext;
                    SelectorChooseEvent?.Invoke(i, abilityNext);
                }
            }
        }
        // Inventory must have the item
        Debug.Assert(allPowerups.ContainsKey(prev));
        // Replace reference
        allPowerups[prev] = next;
        ListingUpdateEvent?.Invoke(allPowerups);
    }

    // List of ability slots
    public IAbility[,] GetAbilities()
    {
        return abilitySlots;
    }
    // Dictionary of all owned powerups
    public Dictionary<string, IPowerup> GetListing()
    {
        return allPowerups;
    }
}