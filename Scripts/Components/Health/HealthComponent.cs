using Assets.NatureMaterials;
using Godot;
using Scripts.InventorySystem;
using Scripts.SignalManager;

namespace Scripts.Components;

[GlobalClass]
public partial class HealthComponent : Node3D
{
    [Export] 
    public float Health { get; set; } = 0.0f;
    
    [Export] 
    public BaseMaterial ParentNode { get; set; }
    
    public override void _Ready()
    {
        if (ParentNode == null)
        {
            GD.PushError("Missing parent node. Please add it.");
        }
    }

    public async void TakeDamage(float amount)
    {
        Health -= amount;
        GD.Print(Health);
        
        if (Health <= 0)
        {
            Inventory.Instance.AddTool(ParentNode.Tool);
            ParentNode.DropMaterials();
            ParentNode.IsActive = false;
            await ToSignal(GetTree().CreateTimer(0.2f), Timer.SignalName.Timeout);
            SignalBus.Instance.EmitSignal(SignalBus.SignalName.GatheredMaterial);
            ParentNode.QueueFree();
        }
    }

    public void ChangeHealth(float newHealth)
    {
        Health = newHealth;
    }
}
