using Godot;
using Godot.Collections;

namespace Scripts.InventorySystem;
public partial class InventorySystem : Node
{
    public static InventorySystem Instance { get; private set; }

    // Signals that are connected with inventory and HUD/UI
    [Signal]
    public delegate void UpdatedHudEventHandler(ItemData iitem);

    [Signal]
    public delegate void ToolUiUpdatedEventHandler(ToolData tool);


    // Lists of items, equipment, etc.
    [Export]
    private Array<ItemData> Items = [];

    [Export]
    private Array<ToolData> Tools = [];

    public override void _Ready()
    {
        Instance = this;   
    }


    public void AddTool(ToolData newTool)
    {
        foreach (ToolData tool in Tools)
        {
            if (tool.Id == newTool.Id)
            {
                return;
            }
        }
        Tools.Add(newTool);
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

    public void RemoveItem(string Id, int amount)
    {
        var item = GetItem(Id);

        item.Amount -= amount;
        if (item.Amount <= 0)
        {
            item.Amount = 0;
        }

        EmitSignal(SignalName.UpdatedHud, item);
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
}
