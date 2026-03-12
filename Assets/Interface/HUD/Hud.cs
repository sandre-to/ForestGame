using System.Linq;
using Assets.ShopUI;
using Godot;
using Scripts.InventorySystem;

namespace Assets.HUD;

[GlobalClass]
public partial class Hud : CanvasLayer
{
    [Export] 
    private PackedScene _slotScene;

    private VBoxContainer _slots;
    private Shop _shopUi;
    private Button _openShop;

    public override void _Ready()
    {
        _slots = GetNode<VBoxContainer>("%SlotContainer");
        _openShop = GetNode<Button>("OpenShop");
        _shopUi = GetNode<Shop>("Shop");

        Inventory.Instance.UpdatedHud += OnHudUpdated;
        _openShop.Pressed += () => _shopUi.Visible = !_shopUi.Visible; 
    }

    private void OnHudUpdated(ItemData item)
    {
        foreach (Slot slot in _slots.GetChildren().Cast<Slot>())
        {
            if (_slots.GetChildren().Count == 0) break;

            if (slot.CurrentData?.Id == item.Id)
            {
                slot.IncrementAmount();
                return;
            }
        }

        // Add a new slot when the slot has no material attached to it
        var newSlot = _slotScene.Instantiate() as Slot;
        _slots.AddChild(newSlot);
        newSlot.SetData(item);
    }
    
}
