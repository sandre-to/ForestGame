
using Godot;
using Scripts.InventorySystem;
namespace Assets.Tree;
[GlobalClass]
public partial class Tree : RigidBody3D
{
    // Exported properties
    [Export] public ItemData Stick;
    [Export] public ItemData Wood;
    [Export] public float Health = 5.0f;
    
    // --- Node references ---
    private AnimationPlayer _animation;

    public override void _Ready()
    {
        _animation = GetNode<AnimationPlayer>("AnimationPlayer");
        InputEvent += OnMouseClick;
        
        MouseEntered += () => Input.SetCustomMouseCursor(ResourceLoader.Load("res://Assets/Tools/Axe/tool_axe_single.png"));
        MouseExited += () => Input.SetCustomMouseCursor(ResourceLoader.Load("res://Assets/Cursor/hand_point.png"));
    }

    private void OnMouseClick(Node camera, InputEvent @event, 
        Vector3 eventPosition, Vector3 normal, long shapeIdx)
    {
        if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
        {
            ChopTree();
        }
    }

    private void ChopTree()
    {
        _animation.Play("Wobble");
        InventorySystem.Instance.AddItem(Stick, GD.RandRange(2, 5));
        Health--;

        if (Health <= 0)
        {
            Input.SetCustomMouseCursor(ResourceLoader.Load("res://Assets/Cursor/hand_point.png"));
            InventorySystem.Instance.AddItem(Wood, GD.RandRange(3, 8));
            QueueFree();
        }
    }
}
