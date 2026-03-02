using Godot;
using Scripts.InventorySystem;
namespace Assets;

[GlobalClass]
public partial class Tree : RigidBody3D
{
    // Exported properties
    [Export] public float GatherTime = 3.5f;

    // --- Variables ---
    private Item Stick { get; } = GD.Load<Item>("Assets/Tree/Stick.tres");
    private bool _isGathering = false;

    // --- Node references ---
    private Timer _gatherTimer;
    private AnimationPlayer _animation;
    
    public override void _Ready()
    {
        _animation = GetNode<AnimationPlayer>("AnimationPlayer");

        // Set up gathering timer properties
        _gatherTimer = GetNode<Timer>("GatherTimer");
        _gatherTimer.OneShot = false;
        _gatherTimer.WaitTime = GatherTime;
        
        _gatherTimer.Timeout += OnGatherTimeout;

        InputEvent += OnMouseClick;
    }

    private void OnMouseClick(Node camera, InputEvent @event, 
        Vector3 eventPosition, Vector3 normal, long shapeIdx)
    {
        if (@event is InputEventMouseButton mouseButton && 
            mouseButton.ButtonIndex == MouseButton.Left &&
            mouseButton.Pressed)
        {
            ToggleGathering();
        }
    }

    private void ToggleGathering()
    {
        _isGathering = !_isGathering;

        if (_isGathering)
        {
            _gatherTimer.Start();
        }
        else
        {
            _gatherTimer.Stop();
        }
    }

    private void OnGatherTimeout()
    {
        InventorySystem.Instance.AddItem(new ItemStack(Stick), GD.RandRange(2, 4));
        InventorySystem.Instance.PrintItems();
        _animation.Play("Wobble");
    }
}
