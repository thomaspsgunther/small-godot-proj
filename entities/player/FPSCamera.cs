using Godot;

public partial class FPSCamera : Camera3D
{
    private Vector3 _rotationDegrees = Vector3.Zero;

    public override void _PhysicsProcess(double delta)
    {
        GlobalPosition = GetNode<CharacterBody3D>("../Player").GlobalPosition;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion inputEventMouseMotion)
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;
			_rotationDegrees.X -= inputEventMouseMotion.Relative.Y * 0.2f;
			_rotationDegrees.Y -= inputEventMouseMotion.Relative.X * 0.2f;
			_rotationDegrees.X = Mathf.Clamp(_rotationDegrees.X, -90f, 90f);
			
            RotationDegrees = _rotationDegrees;
		}
    }
}
