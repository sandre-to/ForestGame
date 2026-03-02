using Godot;
using Scripts.InventorySystem;
using System;

namespace Scripts.CraftingSystem;
[GlobalClass]
public partial class Recipe : Resource
{

    [Export] public string Id = "";

    [Export]
    public ItemStack Input;

    [Export]
    public Item Output;
}
