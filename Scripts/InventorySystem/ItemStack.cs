using Godot;

namespace Scripts.InventorySystem;
[GlobalClass]
public partial class ItemStack : Resource
{
    [Export] public Item ItemData { get; set;}
    [Export] public int Amount { get; set; }

    public ItemStack()
    {
        
    }

    public ItemStack(Item item, int amount = 1)
    {
        ItemData = item;
        Amount = amount;
    }

    public override string ToString()
    {
        return $"{ItemData.Name}: {Amount}";
    }
}
