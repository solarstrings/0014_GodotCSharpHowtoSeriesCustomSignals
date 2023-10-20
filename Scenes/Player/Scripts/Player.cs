using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export]
    public float MaxSpeed = 350;                // The max movement speed for the player
    [Export]
    public float Acceleration = 1600;           // How fast the player accellerates
    [Export]
    public float Friction = 0.2f;               // Friction for interpolating the speed down when the player is not moving
    [Export]
    private int _health = 100;                  // The player's health
    private Vector2 _velocity = Vector2.Zero;   // The player's velocity
    private Label _healthLabel;                 // The healh label above the player
    private CustomSignals _customSignals;       // The custom signal singleton

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _healthLabel = GetNode<Label>("HealthLabel");                                                   // Get the health label
        _healthLabel.Text = "Health: " + _health.ToString();                                            // Set the text of the label to the current amount of health        
        _customSignals = GetNode<CustomSignals>("/root/CustomSignals");                                 // Get the custom signals singleton
        _customSignals.Connect(nameof(CustomSignals.DamagePlayer), this, nameof(HandleDamagePlayer));   // Connect to the damage player signal
    }
    private void HandleDamagePlayer(int damageAmount)
    {
        _health -= damageAmount;                // Decrease the player's health with the incoming damage
        _healthLabel.Text = "Health: " + _health.ToString(); // Update the health label above the player
    }

    public override void _PhysicsProcess(float delta)
    {
        MovePlayer(delta);  // move the player
    }
    private void MovePlayer(float delta)
    {
        var inputVector = Vector2.Zero;                                                             // Initialize the input vector to zero
        inputVector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");   // Get the horizontal input strength
        inputVector.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");      // Get the vertical input strength
        inputVector.Normalized();                                                                   // Normalize the lenght of the input vector

        // If the player is not moving
        if (inputVector == Vector2.Zero)
        {
            _velocity = _velocity.LinearInterpolate(Vector2.Zero, Friction);    // Interpolate the speed towards 0
        }
        // If the player is moving
        else
        {
            _velocity += inputVector * Acceleration * delta;    // Calculate the velocity
            _velocity = _velocity.LimitLength(MaxSpeed);        // Limit velocity to max speed
        }
        _velocity = MoveAndSlide(_velocity);                    // Update movement with Godot's in-built move and slide method
    }
}
