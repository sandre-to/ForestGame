using Godot;

namespace Scenes;
[GlobalClass]
public partial class Main : Node3D
{
    private Node3D _trees;
    private CsgBox3D _ground;

    public override void _Ready()
    {
        _trees = GetNode<Node3D>("Trees");
        _ground = GetNode<CsgBox3D>("%Floor");
    }
}
