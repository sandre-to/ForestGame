using Godot;
using System;

namespace Scripts.InventorySystem;
[GlobalClass]
public partial class ItemData : Resource
{
    [Export] public string Id;
    [Export] public string Name;
    [Export] public Texture2D Image;

    [Export] public int Amount = 0;

    [Export(PropertyHint.MultilineText)] 
    public string Description { get; set; }

    public override string ToString()
    {
        return $"Item: {Name} Amount: {Amount}";
    }
}
