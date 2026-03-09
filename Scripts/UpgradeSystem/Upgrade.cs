using Godot;
using Godot.Collections;
using Scripts.InventorySystem;

namespace Scripts.UpgradeSystem;
public partial class Upgrade : Node
{
    public static Upgrade Instance { get; private set; }

    [Export]
    public Array<Requirement> Requirements = [];

    private Inventory inventory;

    public override void _Ready()
    {
        Instance = this;
        inventory = Inventory.Instance;
    }

    public void UpgradeStats(string toolId, float percentage)
    {
        // Get the tool that will be upgraded
        var tool = inventory.GetTool(toolId);
        if (tool == null)
        {
            GD.Print("Tool does not exist in inventory");
            return;
        }

        tool.Damage *= 1 + percentage;
        tool.GatheringTime /= 1 + percentage;
        GD.Print($"New stats: Damage: {tool.Damage}, Gathering time: {tool.GatheringTime}");
    }

    public bool CanUpgrade(string reqId)
    {
        // Find the requirements array first
        Requirement requirement = FindRequirement(reqId);
        if (requirement == null)
        {
            GD.PushError("Can't find requirements. Check the correct path.");
            return false;  
        } 

        // Find all the items in the inventory
        Array<ItemData> currentInventory = inventory.GetInventory();
        if (currentInventory == null) return false;

        // Compare requirements with the current items
        foreach (var req in requirement.Materials)
        {   
            // Always assume that the player does not have the item
            bool hasMaterial = false;

            // Now check every item in the inventory
            // Jump out of the loop if the item is the same as requirement
            // And check if the player has enough of the item
            foreach (var item in currentInventory)
            {
                if (req.Id == item.Id && item.Amount >= req.Amount)
                {
                    hasMaterial = true; // IMPORTANT CHECK!
                    GD.Print($"Checking requirement: {req.Id} found: {hasMaterial}. Enough materials? {item.Amount >= req.Amount}");
                    break;
                }
            }
            

            if (!hasMaterial)
            {
                return false;
            }
        }

        return true;
    }

    public Requirement FindRequirement(string Id)
    {
        foreach (Requirement req in Requirements)
        {
            if (req.Id == Id)
            {
                return req;
            }
        }
        return null;
    }
}
