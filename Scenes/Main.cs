using Godot;
using Assets.Tree;

namespace Scenes;
[GlobalClass]
public partial class Main : Node3D
{
    private static readonly PackedScene TreeScene = 
        GD.Load<PackedScene>("res://Assets/Tree/Tree.tscn");
    private Node3D _trees;
    private CsgBox3D _ground;

    public override void _Ready()
    {
        _trees = GetNode<Node3D>("Trees");
        _ground = GetNode<CsgBox3D>("%Floor");
        SpawnTrees(50);
    }

    private void SpawnTrees(int amount)
    {
        RandomNumberGenerator rng = new();
        rng.Randomize();

        for (int i = 0; i < amount; i++)
        {
            var newTree = TreeScene.Instantiate() as WoodTree;
            
            float x = rng.RandfRange(-3.5f, 3.5f);
            float z = rng.RandfRange(-3.5f, 3.5f);
            
            newTree.Position = new Vector3(x, 0, z);

            _trees.AddChild(newTree);
        }
    }
}