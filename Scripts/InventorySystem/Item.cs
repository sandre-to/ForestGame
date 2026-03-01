using Godot;
using System;

namespace Scripts.InventorySystem;
[GlobalClass]
public partial class Item : Resource
{
    [Export] public string Id { get; set; }
    [Export] public string Name { get; set; }
    [Export] public AtlasTexture Image { get; set; } 

    [Export(PropertyHint.MultilineText)] 
    public string Description { get; set; }

    public override string ToString()
    {
        return $"Item: {Name}";
    }

}
