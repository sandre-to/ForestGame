using Godot;
using Scripts.InventorySystem;

namespace Assets.HUD;
[GlobalClass]
public partial class Slot : HBoxContainer
{
    public ItemData CurrentData;
    private TextureRect _texture;
    private Label _amountLabel;

    public override void _EnterTree()
    {
        _texture = GetNode<TextureRect>("SlotPanel/TextureRect");
        _amountLabel = GetNode<Label>("AmountLabel");
    }

    public void SetData(ItemData item)
    {
        CurrentData = item;
        _texture.Texture = CurrentData.Image;
        _amountLabel.Text = CurrentData.Amount.ToString();
    }

    public void IncrementAmount()
    {
        _amountLabel.Text = CurrentData.Amount.ToString();
    }
}
