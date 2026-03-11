using Godot;
using Godot.Collections;
using Scripts.InventorySystem;
using Scripts.UpgradeSystem;

namespace Assets.ShopUI;

[GlobalClass]
public partial class Shop : Control
{
    // What is the current tool that is selected in the left side panel? 
    [Export]
    private ToolData CurrentTool { get; set; } = null;
    
    [Export]
    private Array<Texture2D> AxeImages = [];
    
    [Export]
    private Array<Texture2D> PickaxeImages = [];

    // --- Node references ---
    private AnimationPlayer Anim { get; set; }
    private MarginContainer ToolInfoDisplay { get; set; }
    private UpgradeButtons UpgradeButtons { get; set; }
    private Upgrade Upgrade { get; set; }
    private Button AxeButton { get; set; }
    private Button PickaxeButton { get; set; }
    private Button BuyButton { get; set; }
    private Label RequirementsLabel { get; set; }

    public override void _Ready()
    {
        Anim = GetNode<AnimationPlayer>("AnimationPlayer");
        ToolInfoDisplay = GetNode<MarginContainer>("%ToolInfoDisplay");
        UpgradeButtons = GetNode<UpgradeButtons>("%UpgradeButtons");
        BuyButton = GetNode<Button>("%BuyButton");
        RequirementsLabel = GetNode<Label>("%RequirementsLabel");
        Upgrade = Upgrade.Instance;

        // Button for each tool
        AxeButton = GetNode<Button>("%AxeButton");
        PickaxeButton = GetNode<Button>("%PickaxeButton");

        // Set up signals
        AxeButton.Pressed += OnAxeButtonPressed;
        PickaxeButton.Pressed += OnPickaxeButtonPressed;
        BuyButton.Pressed += OnBuyButtonPressed;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("OpenShop"))
        {
            this.Visible = !this.Visible;
        }
    }

    private void OnAxeButtonPressed()
    {
        CurrentTool = Inventory.Instance.GetTool("axe_tool");
        var requirement = Upgrade.Instance.FindRequirement("buy_axe");
        RequirementsLabel.Text = $"Sticks: {requirement.Materials[0].Amount} \nWood: {requirement.Materials[1].Amount}";
        SetButtonIcons(AxeImages);
    }

    private void OnPickaxeButtonPressed()
    {
        CurrentTool = Inventory.Instance.GetTool("pickaxe_tool");
        RequirementsLabel.Text = "";
        SetButtonIcons(PickaxeImages);
    }

    private void OnBuyButtonPressed()
    {
        if (CurrentTool == null) return;
        if (Upgrade.Instance.CanUpgrade(CurrentTool.ShopId))
        {
            Inventory.Instance.AddTool(CurrentTool);
            GD.Print("Added item!");
        }
    }

    private void SetButtonIcons(Array<Texture2D> images)
    {
        for (int i = 0; i < images.Count; i++)
        {   
            var button = UpgradeButtons.GetChild(i) as Button;
            button.Icon = images[i];
            button.ExpandIcon = true;
            button.Alignment = HorizontalAlignment.Center;
        }
    }

    private void SetRequirements()
    {
        
    }
}