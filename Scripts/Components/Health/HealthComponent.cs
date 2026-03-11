using Assets.NatureMaterials;
using Godot;
using Scripts.InventorySystem;

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

        if (Health <= 0)
        {
            Inventory.Instance.AddTool(ParentNode.Tool);
            ParentNode.DropMaterials();
            await ToSignal(ParentNode.GatherSound, AudioStreamPlayer3D.SignalName.Finished);
            ParentNode.QueueFree();
        }
    }

    public void ChangeHealth(float newHealth)
    {
        Health = newHealth;
    }
}
