using Godot;
using System;

namespace Scripts.InventorySystem;
[GlobalClass]
public partial class Item : Resource
{
    [Export] public string Id { get; set; }
    [Export] public string Name { get; set; }
    [Export] public Texture2D Image { get; set; } 

    [Export] public bool Equippable = false;

    [Export(PropertyHint.MultilineText)] 
    public string Description { get; set; }

    public override string ToString()
    {
        return $"Item: {Name}";
    }
}
