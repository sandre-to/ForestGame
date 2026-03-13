using Godot;
using Scripts.InventorySystem;

namespace Assets.ShopUI;
[GlobalClass]
public partial class SellPanel : Panel
{
    // --- Buttons ---
    private Button _sellButton;

    // --- Labels ---
    private LineEdit _input; 
    private Label _valueLabel;

    // --- States ---
    private ItemData _currentItem;
    private int _materialAmount = 0;
    private int _totalValue = 0;

    public override void _Ready()
    {
        _sellButton = GetNode<Button>("SellButton");
        _valueLabel = GetNode<Label>("SellButton/ValueLabel");
        _input = GetNode<LineEdit>("Button/AmountEntered");
        _currentItem = ResourceLoader.Load<ItemData>("res://Assets/NatureMaterials/Tree/Resources/SticksData.tres");

        _sellButton.Pressed += OnSellButtonPressed;
        _input.TextChanged += OnTextChanged;
    }

    private void SetTotalValue()
    {
        _valueLabel.Text = "Total value: " + _totalValue.ToString();
    }

    private void OnSellButtonPressed()
    {   
        var input = _input.Text.ToInt();
        if (!_input.Text.IsValidInt() && input <= 0) return;
        if (!Inventory.Instance.CanSell(_currentItem.Id, input)) return;
        Inventory.Instance.SellItem(_currentItem.Id,input);
    }

    private void OnTextChanged(string text)
    {
        if (string.IsNullOrEmpty(text) || !text.IsValidInt() || text.ToInt() <= 0)
        {
            GD.Print("Invalid number!");
            return;
        }

        _totalValue = text.ToInt() * 2;
        SetTotalValue();
    }
}
