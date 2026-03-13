using Godot;

namespace Assets.NatureMaterials;

[GlobalClass]
public partial class StoneMaterial : BaseMaterial
{
    private Node3D Pickaxe { get; set; }
    private Vector3 OriginalRotation { get; set; }

    public override void _Ready()
    {
        base._Ready();

        Pickaxe = GetNode<Node3D>("Pickaxe");
        Pickaxe.Hide();
        OriginalRotation = Pickaxe.RotationDegrees;
    }

    protected override void ClickedOnMaterial(Node camera, InputEvent @event, 
    Vector3 eventPosition, Vector3 normal, long shapeIdx)
    {
        if (@event.IsActionPressed("Click"))
        {
            if (Tool.Amount >= 1)
            {
                Inventory.RemoveTool(Tool);
                MineStone();
                GD.Print("Starting timer...");
                GatherTimer.Start();
                Pickaxe.Show();
            }
        }
    }

    protected override void OnGatherTimeout()
    {
        MineStone();
    }

    protected override void ChangeMouseCursor()
    {
        Input.SetCustomMouseCursor(ResourceLoader.Load("res://Assets/Cursor/tool_pickaxe.png"));
    }

    private void MineStone()
    {   
        Inventory.AddItem(DroppableMaterials[0], GD.RandRange(3, 8));

        // Animation for the pickaxe
        GatherSound.Play();
        var tween = GetTree().CreateTween();
        tween.TweenProperty(Pickaxe, "rotation_degrees", new Vector3(90, OriginalRotation.Y, OriginalRotation.Z), 0.1);
        tween.TweenProperty(Pickaxe, "rotation_degrees", OriginalRotation, Tool.GatheringTime );
    }
}
