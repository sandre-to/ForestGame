using Godot;
using System;

namespace Assets.NatureMaterials;

[GlobalClass]
public partial class StoneMaterial : BaseMaterial
{
    private Node3D Pickaxe { get; set; }

    public override void _Ready()
    {
        base._Ready();

        Pickaxe = GetNode<Node3D>("Pickaxe");
        Pickaxe.Hide();
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

    private async void MineStone()
    {   
        Inventory.AddItem(DroppableMaterials[0], GD.RandRange(3, 8));

        // Animation for the pickaxe
        GatherSound.Play();
        var originalRotation = Pickaxe.RotationDegrees;
        var tween = GetTree().CreateTween();
        tween.TweenProperty(Pickaxe, "rotation_degrees", new Vector3(90, originalRotation.Y, originalRotation.Z), 0.05);
        tween.TweenProperty(Pickaxe, "rotation_degrees", originalRotation, Tool.GatheringTime);

        await ToSignal(tween, Tween.SignalName.Finished);
        HealthComponent.TakeDamage(Tool.Damage);
    }
}
