using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Sprint.Characters;
using Sprint.Interfaces.Powerups;

namespace Sprint.Items;

internal class Inventory
{

    public const string KeyLabel = "key";
    public const string RupeeLabel = "rupee";

    public delegate void SelectorChooseDelegate(IAbility ability);
    public event SelectorChooseDelegate SelectorChooseEvent;

    public delegate void ListingUpdateDelegate(Dictionary<string, IPowerup> listing);
    public event ListingUpdateDelegate ListingUpdateEvent;

    private Dictionary<string, IPowerup> allPowerups;
    private IAbility[,] abilitySlots;
    private IAbility selectedA, selectedB;

    public Inventory()
    {
        allPowerups = new();
        abilitySlots = new IAbility[CharacterConstants.INVENTORY_ROWS, CharacterConstants.INVENTORY_COLUMNS];
    }


    public void AddPowerup(IPowerup powerup)
    {
        string label = powerup.GetLabel();
        Debug.Assert(!HasPowerup(label));
        if (allPowerups.ContainsKey(label))
        {
            allPowerups[label] = powerup;
        }
        else
        {
            allPowerups.Add(label, powerup);
        }
        ListingUpdateEvent?.Invoke(allPowerups);
    }

    public void AddToSlots(IAbility ability)
    {
        for (int i = 0; i < abilitySlots.GetLength(0); i++)
        {
            for (int j = 0; j < abilitySlots.GetLength(1); j++)
            {
                if (abilitySlots[i, j] == null)
                {
                    abilitySlots[i, j] = ability;
                    return;
                }
            }
        }
        Debug.Assert(false);
    }

    public void DeletePowerup(IPowerup powerup)
    {
        string label = powerup.GetLabel();
        Debug.Assert(HasPowerup(label));
        allPowerups.Remove(label);
        ListingUpdateEvent?.Invoke(allPowerups);

        if (powerup is IAbility)
        {
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

            if (selectedA != null && selectedA.GetLabel() == label)
            {
                selectedA = null;
            }
            if (selectedB != null && selectedB.GetLabel() == label)
            {
                selectedB = null;
                SelectorChooseEvent?.Invoke(null);
            }
        }
    }

    public int StackQuantity(string resource)
    {
        if (HasPowerup(resource))
        {
            Debug.Assert(allPowerups[resource] is IStackedPowerup);
            return ((ResourcePowerup)allPowerups[resource]).Quantity();
        }
        else
        {
            return 0;
        }
    }

    public bool TryConsumeStack(string resource)
    {
        if (StackQuantity(resource) <= 0)
            return false;
        ((ResourcePowerup)allPowerups[resource]).AddAmount(-1);
        return true;
    }

    public bool HasPowerup(string label)
    {
        return allPowerups.ContainsKey(label) && allPowerups[label] != null;
    }

    public IPowerup GetPowerup(string label)
    {
        if (!allPowerups.ContainsKey(label))
        {
            return null;
        }
        return allPowerups[label];
    }

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

    public void Select(int r, int c)
    {
        selectedB = abilitySlots[r, c];
        SelectorChooseEvent?.Invoke(abilitySlots[r, c]);
    }

    public void Drop(int r, int c)
    {
        DeletePowerup(abilitySlots[r, c]);
    }

    public IAbility GetSelectionA()
    {
        return selectedA;
    }

    public IAbility GetSelectionB()
    {
        return selectedB;
    }

    public void ReplaceWithDecorator(string prev, IPowerup next)
    {
        if (next is IAbility)
        {
            IAbility abilityNext = next as IAbility;
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

            if (selectedA != null && selectedA.GetLabel() == prev)
            {
                selectedA = abilityNext;
            }
            if (selectedB != null && selectedB.GetLabel() == prev)
            {
                selectedB = abilityNext;
                SelectorChooseEvent?.Invoke(abilityNext);
            }
        }
        Debug.Assert(allPowerups.ContainsKey(prev));
        allPowerups[prev] = next;
        ListingUpdateEvent?.Invoke(allPowerups);
    }

    public IAbility[,] GetAbilities()
    {
        return abilitySlots;
    }
    public Dictionary<string, IPowerup> GetListing()
    {
        return allPowerups;
    }
}