using System.Diagnostics.Tracing;
using Godot;
using Scripts.InventorySystem;

namespace Scripts.UpgradeSystem;
public partial class Upgrade : Node
{
    public enum ToolLevels
    {
        LevelOne = 300,
        LevelTwo = 700,
        LevelThree = 1500
    }

    public static Upgrade Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }

    public void UpgradeStats(int level, string toolId, string itemId, float updatePercentage)
    {
        // Get the tool that needs upgrading
        var tool = Inventory.Instance.GetTool(toolId);
        if (tool == null) return;
        
        // What material does the tool need to upgrade?
        var material = Inventory.Instance.GetItem(itemId);
        if (material == null) return;
        
        if (CanUpgrade(level, material))
        {
            if (level == 1)
            {
                Inventory.Instance.RemoveItem(material.Id, (int)ToolLevels.LevelOne);
                GD.Print("Tool Level 1!");
            }
            else if (level == 2)
            {
                Inventory.Instance.RemoveItem(material.Id, (int)ToolLevels.LevelTwo);
                GD.Print("Tool Level 2!");
            }
            else if (level == 3)
            {
                Inventory.Instance.RemoveItem(material.Id, (int)ToolLevels.LevelThree);
                GD.Print("Tool Level 3!");
            }

            tool.GatheringTime /= updatePercentage;
            tool.Damage *= updatePercentage;
            GD.Print($"Tool stats: Damage {tool.Damage}, Gather Timer {tool.GatheringTime}");
        }
        else
        {
            GD.Print("Missing some resources");
        }
    }

    public bool CanUpgrade(int level, ItemData item)
    {
        switch (level)
        {
            case 1: 
                return item.Amount >= (int)ToolLevels.LevelOne;
            case 2:
                return item.Amount >= (int)ToolLevels.LevelTwo;
            case 3:
                return item.Amount >= (int)ToolLevels.LevelThree;
            default: 
                return false; 
        }
    }
}
