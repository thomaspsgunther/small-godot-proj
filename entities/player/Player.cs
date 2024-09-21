using Godot;

public partial class Player : CharacterBody3D
{
    // How fast the player moves in meters per second.
    [Export]
    public float Speed { get; set; } = 5;

    [Export]
    public float SprintSpeed { get; set; } = 9;

    // The downward acceleration when in the air, in meters per second squared.
    [Export]
    public float Gravity { get; set; } = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    // Vertical impulse applied to the character upon jumping in meters per second.
    [Export]
    public float JumpImpulse { get; set; } = 15;

    private Vector3 _targetVelocity = Vector3.Zero;

    private Vector3 _rotationDegrees = Vector3.Zero;

    public override void _PhysicsProcess(double delta)
    {
        Vector2 input = Input.GetVector("move_left", "move_right", "move_forward", "move_back");
        Vector3 direction = (Transform.Basis * new Vector3(input.X, 0, input.Y)).Normalized();

        // Vertical velocity (gravity)
        if (!IsOnFloor())
        {
            _targetVelocity.Y -= Gravity * (float)delta;
        }

        // Jumping
        if (IsOnFloor() && Input.IsActionJustPressed("jump"))
        {
            _targetVelocity.Y = JumpImpulse;
        }

        // Ground velocity
        if (direction != Vector3.Zero)
        {
            if (Input.IsActionPressed("sprint"))
            {
                _targetVelocity.X = direction.X * SprintSpeed;
                _targetVelocity.Z = direction.Z * SprintSpeed;
            }
            else
            {
                _targetVelocity.X = direction.X * Speed;
                _targetVelocity.Z = direction.Z * Speed;
            }
        }
        else
        {
            _targetVelocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            _targetVelocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
        }

        // Character movement
        Velocity = _targetVelocity;
        MoveAndSlide();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion inputEventMouseMotion)
        {
            _rotationDegrees.Y -= inputEventMouseMotion.Relative.X * 0.2f;

            RotationDegrees = _rotationDegrees;
        }
    }
}
