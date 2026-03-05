using Godot;
using Scripts.UpgradeSystem;

public partial class UpgradePanel : Control
{
    private Button _levelOneButton;
    private Button _levelTwoButton;
    private Button _levelThreeButton;

    public override void _Ready()
    {
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

    private void OnLevelOnePressed()
    {
        if (Upgrade.Instance.CanUpgrade("axe_level_1"))
        {
            
        }
    }
}
