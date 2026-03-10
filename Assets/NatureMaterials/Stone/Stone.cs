using Godot;
using System;

namespace Assets.NatureMaterials;

[GlobalClass]
public partial class Stone : BaseMaterial
{
        protected override void ClickedOnMaterial(Node camera, InputEvent @event, 
        Vector3 eventPosition, Vector3 normal, long shapeIdx)
    {
        if (@event.IsActionPressed("Click"))
        {
            if (Tool.Amount >= 1)
            {
                Inventory.RemoveTool(Tool);
                GD.Print("Starting timer...");
                GatherTimer.Start();
            }
        }
    }

    protected override void OnGatherTimeout()
    {
        HealthComponent.TakeDamage(Tool.Damage);
        GD.Print($"Health: {HealthComponent.Health}");
    }

    protected override void ChangeMouseCursor()
    {
        Input.SetCustomMouseCursor(ResourceLoader.Load("res://Assets/Cursor/tool_pickaxe.png"));
    }
}
