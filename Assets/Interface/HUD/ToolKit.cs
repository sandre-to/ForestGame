using Godot;
using Godot.Collections;

namespace Assets.HUD;
[GlobalClass]
public partial class ToolKit : Control
{
    private HBoxContainer ToolContainer { get; set; }

    public override void _Ready()
    {
        ToolContainer = GetNode<HBoxContainer>("%ToolContainer");
    }

    public void PrintToolSlots()
    {
        foreach (var slot in ToolContainer.GetChildren())
        {
            GD.Print(slot);
        }
    }

    public Array<Node> GetToolSlots()
    {
        return ToolContainer.GetChildren();
    }
}
