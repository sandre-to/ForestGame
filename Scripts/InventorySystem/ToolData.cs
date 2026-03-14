using Godot;

namespace Scripts.InventorySystem;
[GlobalClass]
public partial class ToolData : ItemData
{
    [Export] public string ShopId = "";
    [Export] public float MinDamage = 0.5f;
    [Export] public float MaxDamage = 1.0f;
    [Export] public double GatheringTime = 3.0f;
    [Export] public bool Equippable = true;
}
