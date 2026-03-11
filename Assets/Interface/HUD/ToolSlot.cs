using Godot;
using Scripts.InventorySystem;
using System;
namespace Assets.HUD;

[GlobalClass]
public partial class ToolSlot : Panel
{
    [Export]
    private ToolData Tool { get; set;}
    
    private TextureRect ToolImage { get; set; }
    private Label AmountLabel { get; set; }

    public override void _Ready()
    {
        ToolImage = GetNode<TextureRect>("TextureRect");
        AmountLabel = GetNode<Label>("AmountLabel");
        if (Tool != null) SetSlot();

        Inventory.Instance.ToolUiUpdated += SetSlot;
    }

    public void SetSlot()
    {
        ToolImage.Texture = Tool.Image;
        AmountLabel.Text = Tool.Amount.ToString();
    }
}
