using Godot;
using Godot.Collections;
using Scripts.Components;
using Scripts.InventorySystem;

namespace Assets.NatureMaterials;

[GlobalClass]
public partial class BaseMaterial : RigidBody3D
{
	// --- Exported properties ---
	[Export] 
    public ToolData Tool { get; set; }
    
    [Export] 
    protected Array<ItemData> DroppableMaterials = [];

	// --- Node references ---
    public Timer GatherTimer { get; set; }
    public AudioStreamPlayer3D GatherSound { get; set; }
    protected VisibleOnScreenNotifier3D VisibleBox { get; set; }
    protected CollisionShape3D Collision { get; set; }
    protected HealthComponent HealthComponent { get; set; }

	// --- Flags ---
	protected bool CanGather = true;

    // INVENTORY
    protected Inventory Inventory { get; private set; }

	// Get references ready when entering scene tree
    public override void _EnterTree()
    {
        GatherTimer = GetNode<Timer>("%GatherTimer");
        GatherSound = GetNode<AudioStreamPlayer3D>("%GatherSound");
		VisibleBox = GetNode<VisibleOnScreenNotifier3D>("%VisibleOnScreen");
		Collision = GetNode<CollisionShape3D>("CollisionShape3D");
        HealthComponent = GetNode<HealthComponent>("%HealthComponent");
    }

    public override void _Ready()
    {
        Inventory = Inventory.Instance;
		
        // Remove the object when outside the game screen
        VisibleBox.ScreenExited += () => 
        {
            Inventory.AddTool(Tool);
            QueueFree();
        };
        UpdateToolStats();

        // Set up signals for the different nodes
        this.InputEvent += ClickedOnMaterial;
        GatherTimer.Timeout += OnGatherTimeout;
        GatherTimer.OneShot = false;

        // Change cursor when mouse has entered/exited object
        MouseExited += () => Input.SetCustomMouseCursor(
            ResourceLoader.Load("res://Assets/Cursor/hand_point.png")
        );

        MouseEntered += ChangeMouseCursor;
    }

    public void DropMaterials()
    {
        // Loop through everything this base material has and add them to the inventory
        foreach (var item in DroppableMaterials)
        {
            Inventory.Instance.AddItem(item, GD.RandRange(10, 30));
        }
    }

    protected void UpdateToolStats()
    {
        GatherTimer.WaitTime = Tool.GatheringTime;
    }

    protected virtual void ClickedOnMaterial(Node camera, InputEvent @event, 
        Vector3 eventPosition, Vector3 normal, long shapeIdx) {}

    protected virtual void OnGatherTimeout() {}
    protected virtual void ChangeMouseCursor() {}
}