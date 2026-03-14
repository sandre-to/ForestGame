using Godot;
using Scripts.InventorySystem;

namespace Assets.NatureMaterials;
[GlobalClass]
public partial class TreeMaterial : BaseMaterial
{
    PackedScene Axe = GD.Load<PackedScene>("res://Assets/Tools/Axe/Axe.tscn");
    
    // --- Component references ---
    private Node3D AxeMesh;
    private Node3D TreeMesh;

    public override void _Ready()
    {
        base._Ready();

        AxeMesh = GetNode<Node3D>("Axe");
        AxeMesh.Hide();

        TreeMesh = GetNode<Node3D>("%TreeMesh");
    }

    protected override void ClickedOnMaterial(Node camera, InputEvent @event, 
        Vector3 eventPosition, Vector3 normal, long shapeIdx)
    {
        if (@event.IsActionPressed("Click"))
        {
            if (Tool.Amount >= 1 && !IsActive)
            {
                IsActive = true;
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

    private void ChopTree()
    {
        // All the visuals starts here
        GatherSound.Play();


        var tween = GetTree().CreateTween();
        tween.TweenProperty(AxeMesh, "rotation_degrees", new Vector3(0, -180f, -90), 0.08);
        tween.TweenProperty(AxeMesh, "rotation_degrees", new Vector3(0, -90f, -90), Tool.GatheringTime);

        WobbleTree();
        
        HealthComponent.TakeDamage((float)GD.RandRange(Tool.MinDamage, Tool.MaxDamage));
    }

    private void WobbleTree()
    {
        var tween = GetTree().CreateTween();
        tween.SetTrans(Tween.TransitionType.Spring);

        tween.TweenProperty(TreeMesh, "rotation_degrees:z", 6f, 0.08);
        tween.TweenProperty(TreeMesh, "rotation_degrees:z", -6f, 0.08);
        tween.TweenProperty(TreeMesh, "rotation_degrees:z", 4f, 0.08);
        tween.TweenProperty(TreeMesh, "rotation_degrees:z", -4f, 0.08);
        tween.TweenProperty(TreeMesh, "rotation_degrees:z", 0f, 0.1);
    }
}
