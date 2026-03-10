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
    protected ToolData Tool { get; set; }
    
    [Export] 
    protected Array<ItemData> DroppableMaterials = [];

	// --- Node references ---
    protected Timer GatherTimer { get; set; }
    protected VisibleOnScreenNotifier3D VisibleBox { get; set; }
    protected CollisionShape3D Collision { get; set; }
    protected HealthComponent HealthComponent { get; set; }

	// --- Flags ---
	protected bool CanGather = false;

	// Get references ready when entering scene tree
    public override void _EnterTree()
    {
        GatherTimer = GetNode<Timer>("%GatherTimer");
		VisibleBox = GetNode<VisibleOnScreenNotifier3D>("%VisibleOnScreen");
		Collision = GetNode<CollisionShape3D>("CollisionShape3D");
        HealthComponent = GetNode<HealthComponent>("%HealthComponent");
    }

    public override void _Ready()
    {
		// Remove the object when outside the game screen
        VisibleBox.ScreenExited += () => QueueFree();
        UpdateToolStats();

        // Set up signals for the different nodes
        this.InputEvent += ClickedOnMaterial;
        GatherTimer.Timeout += OnGatherTimeout;
    }

    protected void DropMaterials()
    {
        foreach (var item in DroppableMaterials)
        {
            Inventory.Instance.AddItem(item, GD.RandRange(1, 5));
        }
    }

    protected void UpdateToolStats()
    {
        GatherTimer.WaitTime = Tool.GatheringTime;
    }

    protected virtual void ClickedOnMaterial(Node camera, InputEvent @event, 
        Vector3 eventPosition, Vector3 normal, long shapeIdx) {}

    protected virtual void OnGatherTimeout() {}
    
}