using Godot;
using Scripts.InventorySystem;

namespace Assets.Tree;
[GlobalClass]
public partial class WoodTree : RigidBody3D
{
    // Exported properties
    [Export] public ItemData Stick;
    [Export] public ItemData Wood;
    [Export] public float Health = 5.0f;
    
    // --- Node references ---
    private AnimationPlayer _animation;
    private ProgressBar _healthBar;
    private ProgressBar _chargeBar;
    private Timer _chopTimer;
    private VisibleOnScreenNotifier3D _visibleBox;
    private CollisionShape3D _collision;
    private AudioStreamPlayer3D _chopSound;

    // --- Flags ---
    private bool _canChop = true;

    // Equipped tool
    private ToolData _equippedAxe = ResourceLoader.Load<ToolData>("res://Assets/Tools/Axe/Axe.tres");

    public override void _Ready()
    {
        // Set the node references
        _animation = GetNode<AnimationPlayer>("AnimationPlayer");
        _chopTimer = GetNode<Timer>("ChopTimer");
        _healthBar = GetNode<ProgressBar>("HealthBar");
        _chargeBar = GetNode<ProgressBar>("ChargeBar");
        _visibleBox = GetNode<VisibleOnScreenNotifier3D>("VisibleOnScreen");
        _collision = GetNode<CollisionShape3D>("CollisionShape3D");
        _chopSound = GetNode<AudioStreamPlayer3D>("ChopSound");

        // Set progress bar properties
        _healthBar.Hide();
        _healthBar.MaxValue = Health;
        _healthBar.Value = Health;

        _chargeBar.Hide();
        _chargeBar.MaxValue = _equippedAxe.GatheringTime;
        _chargeBar.Value = _equippedAxe.GatheringTime;

        // Set gathering properties
        _chopTimer.WaitTime = _equippedAxe.GatheringTime;

        // Connected signals
        InputEvent += OnMouseClick;
        _chopTimer.Timeout += () => 
        {
            _canChop = true;
            _chargeBar.Value = 0;    
        };
        _visibleBox.ScreenExited += () => QueueFree();
        MouseEntered += () => 
        {
            _healthBar.Show();
            _chargeBar.Show();
            Input.SetCustomMouseCursor(ResourceLoader.Load("res://Assets/Tools/Axe/tool_axe_single.png"));
        };

        MouseExited += () =>
        {
            _healthBar.Hide();
            _chargeBar.Hide();
            Input.SetCustomMouseCursor(ResourceLoader.Load("res://Assets/Cursor/hand_point.png"));  
        };
    }

    public override void _Process(double delta)
    {
        // Wait for the timer to stop before chopping continues
        if (!_canChop && _chopTimer.TimeLeft > 0)
        {
            _chargeBar.Value = _chopTimer.TimeLeft;
        }
    }

    private void OnMouseClick(Node camera, InputEvent @event, 
        Vector3 eventPosition, Vector3 normal, long shapeIdx)
    {
        if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
        {
            UpdateToolStats();
            ChopTree();
        }
    }

    private void ChopTree()
    {
        if (!_canChop) return;
        _canChop = false;
        _chopTimer.Start(); 

        Inventory.Instance.AddItem(Stick, GD.RandRange(2, 5));
        _chopSound.Play();
        _animation.Play("Wobble");
        Health -= _equippedAxe.Damage;
        _healthBar.Value = Health;
        RemoveTree();
    }

    private void RemoveTree()
    {
        if (Health <= 0)
        {
            _animation.Stop();
            _canChop = false;
            var direction = GD.RandRange(-1, 1);
            ApplyTorqueImpulse(new Vector3(20, 0, 20 * direction));
            _healthBar.Value = 0;
            Inventory.Instance.AddItem(Wood, GD.RandRange(3, 6));
            GetTree().CreateTimer(2.5f).Timeout += () => QueueFree();   
        }
    }

    private void UpdateToolStats()
    {
        _chargeBar.MaxValue = _equippedAxe.GatheringTime;
        _chargeBar.Value = _chargeBar.MaxValue;
        _chopTimer.WaitTime = _equippedAxe.GatheringTime;
    }
}
