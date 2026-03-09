using Godot;
using Scripts.UpgradeSystem;

public partial class UpgradePanel : Control
{
    private Button _levelOneButton;
    private Button _levelTwoButton;
    private Button _levelThreeButton;

    private Upgrade _upgrade;

    public override void _Ready()
    {
        _upgrade = Upgrade.Instance;
        _levelOneButton = GetNode<Button>("%LevelOneButton");
        _levelTwoButton = GetNode<Button>("%LevelTwoButton");
        _levelThreeButton = GetNode<Button>("%LevelThreeButton");

        // The player needs to upgrade sequential
        // The first button is available, the rest are not
        _levelTwoButton.Disabled = true;
        _levelThreeButton.Disabled = true;

        // Connect all the signals for the buttons
        _levelOneButton.Pressed += OnLevelOnePressed;
    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("UpgradePanel"))
        {
            this.Visible = !this.Visible;
        }
    }


    private void OnLevelOnePressed()
    {
        if (_upgrade.CanUpgrade("axe_level_1") && !_levelOneButton.Disabled)
        {
            _upgrade.UpgradeStats("axe_tool", 0.12f);
            _levelOneButton.Disabled = true;
        }
    }
}
