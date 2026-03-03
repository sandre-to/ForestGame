
using System.Threading.Tasks;
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
    private ProgressBar _healthBar;
    private Timer _chopTimer;

    // --- Flags ---
    private bool _canChop = true;

    public override void _Ready()
    {
        // Set the node references
        _animation = GetNode<AnimationPlayer>("AnimationPlayer");
        _chopTimer = GetNode<Timer>("ChopTimer");
        _healthBar = GetNode<ProgressBar>("HealthBar");

        // Set progress bar properties
        _healthBar.Hide();
        _healthBar.MaxValue = Health;
        _healthBar.Value = Health; 


        // Connected signals
        InputEvent += OnMouseClick;
        _chopTimer.Timeout += () => _canChop = true;
        MouseEntered += () => 
        {
            _healthBar.Show();
            Input.SetCustomMouseCursor(ResourceLoader.Load("res://Assets/Tools/Axe/tool_axe_single.png"));
        };

        MouseExited += () =>
        {
            _healthBar.Hide();
            Input.SetCustomMouseCursor(ResourceLoader.Load("res://Assets/Cursor/hand_point.png"));  
        };
    }

    private async void OnMouseClick(Node camera, InputEvent @event, 
        Vector3 eventPosition, Vector3 normal, long shapeIdx)
    {
        if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
        {
            ChopTree();
            await RemoveTree();
        }
    }

    private void ChopTree()
    {
        if (!_canChop) return;
        _canChop = false;
        _chopTimer.Start();

        _animation.Play("Wobble");
        InventorySystem.Instance.AddItem(Stick, GD.RandRange(2, 5));
        Health--;
        _healthBar.Value = Health;
    }

    private async Task RemoveTree()
    {
        if (Health <= 0)
        {
            await ToSignal(_animation, AnimationPlayer.SignalName.AnimationFinished);
            Input.SetCustomMouseCursor(ResourceLoader.Load("res://Assets/Cursor/hand_point.png"));
            _healthBar.Value = 0;
            InventorySystem.Instance.AddItem(Wood, GD.RandRange(3, 8));
            QueueFree();
        }
        
    }
}
