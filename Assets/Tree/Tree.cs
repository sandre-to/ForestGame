using Godot;
using Scripts.InventorySystem;
namespace Assets;

[GlobalClass]
public partial class Tree : RigidBody3D
{
    // --- Exported properties ---
    [Export]
    private Item Stick { get; set; }
    
    [Export]
    private float _torque = 80f;

    // --- Variables ---
    private bool _mouseEntered;
    
    public override void _Ready()
    {
        MouseEntered += () => _mouseEntered = true;
        MouseExited += () => _mouseEntered = false;
    }

    public override void _IntegrateForces(PhysicsDirectBodyState3D state)
    {   
        // Apply a rotational force on the tree
        if (_mouseEntered && Input.IsActionJustPressed("Click"))
        {
            var direction = GD.RandRange(-1 , 1);
            state.ApplyTorque(direction * Vector3.One * _torque);
            InventorySystem.Instance.AddItem(new ItemStack(Stick), 3);
            InventorySystem.Instance.PrintItems();
        }
    }   
}
