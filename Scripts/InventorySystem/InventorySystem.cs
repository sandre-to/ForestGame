using Godot;
using Godot.Collections;

namespace Scripts.InventorySystem;
public partial class InventorySystem : Node
{
    public static InventorySystem Instance { get; private set; }

    // Lists of items, equipment, etc.
    [Export]
    public Array<ItemStack> Items = [];

    public override void _Ready()
    {
        Instance = this;   
    }

    public void AddItem(ItemStack item, int amount)
    {
        // Only increase amount if the item stack already exists
        foreach (ItemStack stack in Items)
        {
            if (stack.ItemData.Id == item.ItemData.Id)
            {
                stack.Amount += amount;
                return;
            }
        }

        // Always duplicate the resource so each item stack is unique
        Items.Add(item.Duplicate() as ItemStack);
    }

    public void PrintItems()
    {
        foreach (ItemStack stack in Items)
        {
            GD.Print(stack);
        }
    }
}
