using Godot;
using Assets.Tree;
using Scripts.UpgradeSystem;

namespace Scenes;
[GlobalClass]
public partial class Main : Node3D
{
    private static readonly PackedScene TreeScene = 
        GD.Load<PackedScene>("res://Assets/Tree/Tree.tscn");

    [Export]
    private int _maxTrees = 0;

    private Node3D _trees;

    public override void _Ready()
    {
        _trees = GetNode<Node3D>("Trees");
        SpawnTrees(_maxTrees);
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