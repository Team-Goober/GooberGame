using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection.Emit;
using Sprint.Characters;
using Sprint.Interfaces.Powerups;

namespace Sprint.Items;

internal class Inventory
{

    public const string KeyLabel = "key";
    public const string RupeeLabel = "rupee";

    public delegate void SelectorChooseDelegate(int box, IAbility ability);
    public event SelectorChooseDelegate SelectorChooseEvent;

    public delegate void ListingUpdateDelegate(Dictionary<string, IPowerup> listing);
    public event ListingUpdateDelegate ListingUpdateEvent;

    private Dictionary<string, IPowerup> allPowerups;
    private IAbility[,] abilitySlots;
    private IAbility[] selectedBoxes;

    public Inventory()
    {
        allPowerups = new();
        abilitySlots = new IAbility[CharacterConstants.INVENTORY_ROWS, CharacterConstants.INVENTORY_COLUMNS];
        selectedBoxes = new IAbility[CharacterConstants.SELECT_BOXES];
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

    public void Select(int b, int r, int c)
    {
        selectedBoxes[b] = abilitySlots[r, c];
        SelectorChooseEvent?.Invoke(b, abilitySlots[r, c]);
    }

    public void Drop(int r, int c)
    {
        DeletePowerup(abilitySlots[r, c]);
    }

    public IAbility GetSelection(int b)
    {
        return selectedBoxes[b];
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
            for (int i = 0; i < selectedBoxes.Length; i++)
            {
                if (selectedBoxes[i] != null && selectedBoxes[i].GetLabel() == prev)
                {
                    selectedBoxes[i] = abilityNext;
                    SelectorChooseEvent?.Invoke(i, abilityNext);
                }
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