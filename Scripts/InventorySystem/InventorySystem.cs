using Godot;
using Godot.Collections;

namespace Scripts.InventorySystem;
public partial class InventorySystem : Node
{
    public static InventorySystem Instance { get; private set; }

    // Signals that are connected with inventory and HUD/UI
    [Signal]
    public delegate void UpdatedHudEventHandler(ItemStack itemStack);

    [Signal]
    public delegate void ToolUiUpdatedEventHandler(Item tool);

    // Lists of items, equipment, etc.
    [Export]
    private Array<ItemStack> Items = [];

    [Export]
    private Array<Item> Tools = [];

    public override void _Ready()
    {
        Instance = this;   
    }

    public void AddTool(Item item)
    {
        foreach (Item tool in Tools)
        {
            if (tool.Id == item.Id)
            {
                return;
            }
        }

        Tools.Add(item);
    }

    public Item GetTool(string id)
    {
        foreach (Item tool in Tools)
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
        foreach (Item tool in Tools)
        {
            if (tool.Id == id)
            {
                return true;
            }
        }
        return false;
    }

    public void AddItem(ItemStack item, int amount)
    {
        // Only increase amount if the item stack already exists
        foreach (ItemStack stack in Items)
        {
            if (stack.ItemData.Equippable) continue;

            if (stack.ItemData.Id == item.ItemData.Id)
            {
                stack.Amount += amount;
                EmitSignal(SignalName.UpdatedHud, stack);
                return;
            }
        }

        if (!item.ItemData.Equippable)
        {
            Items.Add(item);
            EmitSignal(SignalName.UpdatedHud, item);
        }
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

    public ItemStack GetItem(string Id)
    {
        foreach (ItemStack stack in Items)
        {
            if (stack.ItemData.Id == Id)
            {
                return stack;
            }
        }
        return null;
    }

    public void PrintItems()
    {
        foreach (ItemStack stack in Items)
        {
            GD.Print(stack);
        }
    }
}
