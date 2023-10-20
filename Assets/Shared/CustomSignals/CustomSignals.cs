using Godot;
using System;

public class CustomSignals : Node
{
    [Signal]
    public delegate void DamagePlayer(int damageAmount);
}
