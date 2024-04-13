using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using Sprint.Characters;
using Sprint.HUD;
using Sprint.Interfaces;
using Sprint.Projectile;
using static System.Net.Mime.MediaTypeNames;

namespace Sprint.Items;

internal class Inventory
{

    public const string KeyLabel = "key";
    public const string RupeeLabel = "rupee";

    public delegate void SelectorChooseDelegate(IAbility ability);
    public event SelectorChooseDelegate SelectorChooseEvent;

    public delegate void PowerupGainedDelegate(IPowerup powerup);
    public event PowerupGainedDelegate PowerupGainedEvent;

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
        PowerupGainedEvent?.Invoke(powerup);
    }

    public void AddToSlots(IAbility ability)
    {
        for (int i = 0; i < abilitySlots.GetLength(0); i++)
        {
            for (int j = 0; i < abilitySlots.GetLength(1); j++)
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
        if (powerup is IAbility)
        {
            for (int i = 0; i < abilitySlots.GetLength(0); i++)
            {
                for (int j = 0; i < abilitySlots.GetLength(1); j++)
                {
                    if (abilitySlots[i, j] != null && abilitySlots[i, j].GetLabel() == label)
                    {
                        abilitySlots[i, j] = null;
                    }
                }
            }
        }
    }

    public int ResourceQuantity(string resource)
    {
        if (HasPowerup(resource))
        {
            Debug.Assert(allPowerups[resource] is ResourcePowerup);
            return ((ResourcePowerup)allPowerups[resource]).Quantity();
        }
        else
        {
            return 0;
        }
    }

    public bool TryConsumeResource(string resource)
    {
        if (ResourceQuantity(resource) <= 0)
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
            for (int j = 0; i < abilitySlots.GetLength(1); j++)
            {
                if (abilitySlots[i, j].GetLabel() == null)
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
            for (int i = 0; i < abilitySlots.GetLength(0); i++)
            {
                for (int j = 0; i < abilitySlots.GetLength(1); j++)
                {
                    if (abilitySlots[i, j].GetLabel() == prev)
                    {
                        abilitySlots[i, j] = (IAbility)next;
                    }
                }
            }
        }
        Debug.Assert(allPowerups.ContainsKey(prev));
        allPowerups[prev] = next;
    }
}