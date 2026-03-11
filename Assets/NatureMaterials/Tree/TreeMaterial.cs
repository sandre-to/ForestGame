using System.Threading.Tasks;
using Godot;
using Scripts.InventorySystem;

namespace Assets.NatureMaterials;
[GlobalClass]
public partial class TreeMaterial : BaseMaterial
{
    PackedScene Axe = GD.Load<PackedScene>("res://Assets/Tools/Axe/Axe.tscn");
    
    // --- Component references ---
    private Node3D AxeMesh { get; set; }

    public override void _Ready()
    {
        base._Ready();

        AxeMesh = GetNode<Node3D>("Axe");
        AxeMesh.Hide();
    }

    protected override void ClickedOnMaterial(Node camera, InputEvent @event, 
        Vector3 eventPosition, Vector3 normal, long shapeIdx)
    {
        if (@event.IsActionPressed("Click"))
        {
            if (Tool.Amount >= 1)
            {
                Inventory.RemoveTool(Tool);
                GD.Print("Starting timer...");
                ChopTree();
                GatherTimer.Start();
                AxeMesh.Show();
            }
        }
    }

    protected override void OnGatherTimeout()
    {
        ChopTree();
    }

    protected override void ChangeMouseCursor()
    {
        Input.SetCustomMouseCursor(ResourceLoader.Load("res://Assets/Cursor/tool_axe_single.png"));
    }

    private async void ChopTree()
    {
        Inventory.AddItem(DroppableMaterials[1], GD.RandRange(3, 5));

        // All the visuals starts here
        GatherSound.Play();
        var originalRotation = AxeMesh.Rotation;
        var tween = GetTree().CreateTween();
        tween.TweenProperty(AxeMesh, "rotation_degrees", new Vector3(0, -180f, -90), 0.1);
        tween.TweenProperty(AxeMesh, "rotation_degrees", new Vector3(0, -90f, -90), Tool.GatheringTime);
        
        await ToSignal(tween, Tween.SignalName.Finished);
        HealthComponent.TakeDamage(Tool.Damage);
        GD.Print($"Health: {HealthComponent.Health}");
    }
}
