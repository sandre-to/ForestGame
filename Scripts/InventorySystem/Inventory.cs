using System.Runtime.Intrinsics.X86;
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

    [Signal]
    public delegate void CurrencyUpdatedEventHandler(int currency);
    
    // Lists of items, equipment, etc.
    [Export]
    private Array<ItemData> Items = [];

    [Export]
    private Array<ToolData> Tools = [];

    [Export]
    private int Currency = 0;

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
        if (currentTool == null)
        {
            GD.PrintErr($"{currentTool.Name} not found in inventory.");
            return;
        }

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
        if (item == null)
        {
            GD.PushError($"{item.Name} not found in inventory-");
        };

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
        if (requirements == null) return;

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

    public int GetCoins() { return Currency; }

    public void SellItem(string itemId, int selectedAmount)
    {
        // Get the item that will be sold and check the amount of the item.
        var item = GetItem(itemId);
        if (item == null || item.Amount <= 0) return;

        // Say one piece of sticks or wood is two coins
        RemoveItem(itemId, selectedAmount);
        Currency += selectedAmount * 2;
        EmitSignal(SignalName.CurrencyUpdated, Currency);
    }

    // A method that checks if the player has enough of the item to sell it
    public bool CanSell(string itemId, int input)
    {
        // Check if the selected item from inventory is more than the input
        var selectedItem = GetItem(itemId);
        if (selectedItem == null) return false;
        return selectedItem.Amount >= input;
    }
}
