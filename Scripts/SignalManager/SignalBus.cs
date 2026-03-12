using Godot;
using System;

namespace Scripts.SignalManager;
public partial class SignalBus : Node
{
    public static SignalBus Instance;

    // --- GAMEPLAY SIGNALS ---
    [Signal]
    public delegate void GatheredMaterialEventHandler();

    public override void _Ready() { Instance = this; }

}
