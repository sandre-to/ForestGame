using Godot;
using Scripts.InventorySystem;
using Godot.Collections;
using Scripts.UpgradeSystem;
using System;

namespace Assets.ShopUI;

[GlobalClass]
public partial class UpgradeButtons : HBoxContainer
{
    // Gets the current tool from owner node
    public ToolData Tool = null;

    // Hold all the upgrades for each tool
    [Export] 
    public Array<Requirement> AxeUpgrades = [];
    [Export] 
    public Array<Requirement> PickaxeUpgrades = [];

    // Buttons
    private Button _level1;
    private Button _level2;
    private Button _level3;

    public override void _Ready()
    {
        _level1 = GetNode<Button>("Level1");
        _level2 = GetNode<Button>("Level2");
        _level3 = GetNode<Button>("Level3");

        _level1.Pressed += OnFirstUpgradePressed;
        _level2.Pressed += OnSecondUpgradePressed;
        _level3.Pressed += OnThirdUpgradePressed;
    }

    private void OnFirstUpgradePressed()
    {
        if (Upgrade.Instance.CanUpgrade(AxeUpgrades[0].Id))
        {
            _level1.Disabled = true;
            _level2.Disabled = false;
            Upgrade.Instance.UpgradeStats(AxeUpgrades[0].Id, Tool.Id, 0.5f);
        }
    }

    private void OnSecondUpgradePressed()
    {
        
    }

    private void OnThirdUpgradePressed()
    {
        
    }

    public void SetTooltip(Array<Requirement> upgrades)
    {
        for (int i = 0; i < upgrades.Count;i++)
        {
            Array<String> description = [];
            var button = GetChild(i) as Button;
            
            // Store every requirements into an array to use later
            foreach (var req in upgrades[i].Materials)
            {
                description.Add(req.ToString());
            }
            
            GD.Print(description);
            string tooltip = string.Join("\n", description);
            button.TooltipText = tooltip;
        }
    }
}