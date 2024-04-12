using AscendedZ.dungeon_crawling.backend.Tiles;
using Godot;
using System;
using System.Linq;

public partial class TileScene : Node2D
{
	private Line2D _line;
	private Marker2D _center, _left, _right, _down, _up;
    private NormalTile _tile;

    public delegate Vector2 Clicked();
    private bool _addedRight, _addedLeft, _addedUp, _addedDown;

	public override void _Ready()
	{
        _addedRight = false;
        _addedLeft = false;
        _addedUp = false;
        _addedDown = false;

        _line = this.GetNode<Line2D>("%Line2D");

        _center = this.GetNode<Marker2D>("%Center");

        _up = this.GetNode<Marker2D>("%Up");
        _left = this.GetNode<Marker2D>("%Left");
		_right = this.GetNode<Marker2D>("%Right");
		_down = this.GetNode<Marker2D>("%Down");
 
        Area2D area = this.GetNode<Area2D>("%Area2D");

        _line.AddPoint(_center.Position);
    }

    public void AddUpLine()
    {
        if (_addedUp)
            return;

        _line.AddPoint(_up.Position);
        _line.AddPoint(_center.Position);
        _addedUp = true;
    }

    public void AddRightLine()
	{
        if (_addedRight)
            return;

        _line.AddPoint(_right.Position);
        _line.AddPoint(_center.Position);
        _addedRight = true;
    }

	public void AddLeftLine()
	{
        if (_addedLeft)
            return;

        _line.AddPoint(_left.Position);
        _line.AddPoint(_center.Position);
        _addedLeft = true;
    }

	public void AddDownLine()
	{
        if (_addedDown)
            return;

        _line.AddPoint(_down.Position);
        _line.AddPoint(_center.Position);
        _addedDown = true;
    }

    public Vector2 GetRightPosition()
	{
		return _right.GlobalPosition;
	}

	public Vector2 GetLeftPosition()
	{
        return _left.GlobalPosition;
	}

    public Vector2 GetDownPosition()
    {
        return _down.GlobalPosition;
    }

    public Vector2 GetUpPosition()
    {
        return _up.GlobalPosition;
    }

    public Vector2 GetCenterPosition()
    {
        return _center.GlobalPosition;
    }

    public double GetTileDistance()
    {
        return Math.Abs(_center.GlobalPosition.Y - _up.GlobalPosition.Y);
    }

    public void ClearPoints()
	{
		_line.ClearPoints();
	}
}
