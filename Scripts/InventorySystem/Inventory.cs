using Godot;
using Godot.Collections;
using Scripts.UpgradeSystem;

namespace Scripts.InventorySystem;
public partial class Inventory : Node
{
    public static Inventory Instance { get; private set; }

    // Signals that are connected with inventory and HUD/UI
    [Signal]
    public delegate void UpdatedHudEventHandler(ItemData item);

    [Signal]
    public delegate void ToolUiUpdatedEventHandler();

    
    // Lists of items, equipment, etc.
    [Export]
    private Array<ItemData> Items = [];

    [Export]
    private Array<ToolData> Tools = [];


    public override void _Ready() { Instance = this; }   

    public void AddTool(ToolData newTool)
    {
        foreach (ToolData tool in Tools)
        {
            if (tool.Id == newTool.Id)
            {
                tool.Amount++;
                EmitSignal(SignalName.ToolUiUpdated);
                return;
            }
        }

        Tools.Add(newTool);
        newTool.Amount++;
        EmitSignal(SignalName.ToolUiUpdated);

    }

    public void RemoveTool(ToolData tool)
    {
        var currentTool = GetTool(tool.Id);
        currentTool.Amount--;

        if (currentTool.Amount <= 0)
        {
            currentTool.Amount = 0;
        } 

        EmitSignal(SignalName.ToolUiUpdated);
    }

    public ToolData GetTool(string id)
    {
        foreach (ToolData tool in Tools)
        {
            if (tool.Id == id)
            {
                return tool;
            }
        }
        return null;
    }

    public bool ToolExists(string id)
    {
        foreach (ToolData tool in Tools)
        {
            if (tool.Id == id)
            {
                return true;
            }
        }
        return false;
    }

    public void AddItem(ItemData newItem, int amount)
    {
        // Only increase amount if the item already exists
        foreach (ItemData item in Items)
        {
            if (item.Id == newItem.Id)
            {
                item.Amount += amount;
                EmitSignal(SignalName.UpdatedHud, item);
                return;
            }
        }

        Items.Add(newItem);
        newItem.Amount += amount;
        EmitSignal(SignalName.UpdatedHud, newItem);
    }

    public void RemoveItem(string itemId, int amount)
    {
        var item = GetItem(itemId);

        item.Amount -= amount;
        if (item.Amount <= 0)
        {
            item.Amount = 0;
        }

        EmitSignal(SignalName.UpdatedHud, item);
    }

    public void RemoveItems(string reqId)
    {
        // Get the requirements first and store them in a variable
        var requirements = Upgrade.Instance.FindRequirement(reqId).Materials;

        // Loop through the materials needed and check if it exists in inventory
        foreach (var req in requirements)
        {
            foreach (var item in Items)
            {
                if (req.Id == item.Id)
                {
                    RemoveItem(item.Id, req.Amount);
                }
            }
        }
    }

    public ItemData GetItem(string Id)
    {
        foreach (ItemData item in Items)
        {
            if (item.Id == Id)
            {
                return item;
            }
        }
        return null;
    }

    public void PrintItems()
    {
        foreach (ItemData item in Items)
        {
            GD.Print(item);
        }
    }

    public Array<ItemData> GetInventory()
    {
        return Items;
    }
}
