using Godot;

namespace Scripts.Components;

[GlobalClass]
public partial class HealthComponent : Node3D
{
    [Export] 
    public float Health { get; set; } = 0.0f;
    
    [Export] 
    public Node ParentNode { get; set; }

    
    public override void _Ready()
    {
        if (ParentNode == null)
        {
            GD.PushError("Missing parent node. Please add it.");
        }
    }

    public void TakeDamage(float amount)
    {
        Health -= amount;

        if (Health <= 0)
        {
            ParentNode.QueueFree();
        }
    }

    public void ChangeHealth(float newHealth)
    {
        Health = newHealth;
    }
}
