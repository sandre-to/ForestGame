using Godot;

namespace Scripts.InventorySystem;
[GlobalClass]
public partial class ToolData : ItemData
{
    [Export] public string ShopId = "";
    [Export] public int MinDamage = 2;
    [Export] public int MaxDamage = 4;
    [Export] public double GatheringTime = 0.0f;
    [Export] public bool Equippable = true;
}
