using Godot;
using Scripts.SignalManager;

namespace Scenes;
[GlobalClass]
public partial class Main : Node3D
{
    private PackedScene _tree = GD.Load<PackedScene>("res://Assets/NatureMaterials/Tree/TreeMaterial.tscn");
    private GridMap _gridMap;

    private int _totalTrees = 0;

    public override void _Ready()
    {
        _gridMap = GetNode<GridMap>("GridMap");
        SignalBus.Instance.GatheredMaterial += UpdateCurrentTrees;
        SpawnTrees();
    }

    private async void SpawnTrees()
    {
        foreach (var cell in _gridMap.GetUsedCells())
        {
            Vector3 pos = _gridMap.MapToLocal(cell);
            var tree = _tree.Instantiate<Node3D>();
            await ToSignal(GetTree().CreateTimer(0.8f), Timer.SignalName.Timeout);
            AddChild(tree);
            _totalTrees++;
            tree.GlobalPosition = pos + new Vector3(0, 0.5f, 0);
        }
    }

    private void UpdateCurrentTrees()
    {
        _totalTrees--;
        
        if (_totalTrees <= 0)
        {
            SpawnTrees();
        }
    }
}