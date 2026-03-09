using Godot;

namespace Assets.NatureMaterials;
[GlobalClass]
public partial class TreeMaterial : BaseMaterial
{
    protected override void ClickedOnMaterial(Node camera, InputEvent @event, 
        Vector3 eventPosition, Vector3 normal, long shapeIdx)
    {
        if (@event.IsActionPressed("Click"))
        {
            GD.Print("Clicked on a tree");
        }

    }
}
