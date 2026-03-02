using Godot;
using Scripts.InventorySystem;

namespace Assets.HUD;
[GlobalClass]
public partial class Slot : HBoxContainer
{
    private TextureRect _texture;
    private Label _amountLabel;

    public override void _Ready()
    {
        _texture = GetNode<TextureRect>("%TextureRect");
        _amountLabel = GetNode<Label>("AmountLabel");

        InventorySystem.Instance.UpdatedHud += itemStack =>
        {
            _texture.Texture = itemStack.ItemData.Image;
            _amountLabel.Text = itemStack.Amount.ToString();
        };
    }
}
