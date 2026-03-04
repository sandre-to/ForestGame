using Godot;

namespace Scripts.InventorySystem;
[GlobalClass]
public partial class ToolData : ItemData
{
    [Export] public float Damage = 0.0f;
    [Export] public double GatheringTime = 0.0f;
    [Export] public bool Equippable = true;
}
