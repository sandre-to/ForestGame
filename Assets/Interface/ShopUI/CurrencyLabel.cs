using Godot;
using Scripts.InventorySystem;
using System;

namespace Assets.ShopUI;
[GlobalClass]
public partial class CurrencyLabel : RichTextLabel
{
    public override void _Ready()
    {
        Inventory.Instance.CurrencyUpdated += OnCurrencyUpdated;
    }

    private void OnCurrencyUpdated(int coins)
    {
        Text = "Currency: " + coins.ToString();
    }
}
