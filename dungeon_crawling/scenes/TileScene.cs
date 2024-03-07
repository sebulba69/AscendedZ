using Godot;
using System;
using System.Linq;

public partial class TileScene : Node2D
{
	private Line2D _line;
	private Marker2D _center, _left, _right, _bottomRight, _bottomLeft;

    public delegate Vector2 Clicked();

	public override void _Ready()
	{
		_line = this.GetNode<Line2D>("%Line2D");
        _center = this.GetNode<Marker2D>("%Center");
		_left = this.GetNode<Marker2D>("%Left");
		_right = this.GetNode<Marker2D>("%Right");
		_bottomRight = this.GetNode<Marker2D>("%BottomRight");
        _bottomLeft = this.GetNode<Marker2D>("%BottomLeft");

        Area2D area = this.GetNode<Area2D>("%Area2D");

        _line.AddPoint(_center.Position);
    }

	public void AddRightLine()
	{
        _line.AddPoint(_right.Position);
        _line.AddPoint(_center.Position);
    }

	public void AddLeftLine()
	{
        _line.AddPoint(_left.Position);
        _line.AddPoint(_center.Position);
    }

	public void AddBottomRightLine()
	{
        _line.AddPoint(_bottomRight.Position);
        _line.AddPoint(_center.Position);
    }

    public void AddBottomLeftLine()
    {
        _line.AddPoint(_bottomLeft.Position);
        _line.AddPoint(_center.Position);
    }

    public Vector2 GetRightPosition()
	{
		return _right.GlobalPosition;
	}

	public Vector2 GetLeftPosition()
	{
        return _left.GlobalPosition;
	}

    public Vector2 GetBRightPosition()
    {
        return _bottomRight.GlobalPosition;
    }

    public Vector2 GetBLeftPosition()
    {
        return _bottomLeft.GlobalPosition;
    }

    public void ClearPoints()
	{
		_line.ClearPoints();
	}
}
