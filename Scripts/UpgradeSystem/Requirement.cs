using Godot;
using Godot.Collections;
using Scripts.InventorySystem;

namespace Scripts.UpgradeSystem;

[GlobalClass]
public partial class Requirement : Resource
{
    [Export] public string Id;
    [Export] public Array<ItemData> Materials = [];
}
