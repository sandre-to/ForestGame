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

        tool.Damage *= percentage;
        tool.GatheringTime /= percentage;
        GD.Print($"New stats: Damage: {tool.Damage}, Gathering time: {tool.GatheringTime}");
    }

    public bool CanUpgrade(string reqId)
    {
        // Find the requirements array first
        Requirement requirement = FindRequirement(reqId);

        // Find all the items in the inventory
        Array<ItemData> currentInventory = inventory.GetInventory();

        foreach (var item in requirement.Materials)
        {
            GD.Print(item);
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
