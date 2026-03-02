using Godot;
using Scripts.CraftingSystem;
using System;

namespace Scenes;

[GlobalClass]
public partial class Main : Node3D
{
    private Button _button;

    public override void _Ready()
    {
        _button = GetNode<Button>("%Button");
        _button.Pressed += () => CraftingSystem.Instance.Craft("axe_recipe", "stick_id");
    }

}
