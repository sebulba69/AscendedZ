using AscendedZ;
using AscendedZ.dungeon_crawling.combat.battledc;
using Godot;
using System;

public partial class DungeonCombat : Node2D
{
	private PlayerScene _playerScene;
	private EnemyScene _enemyScene;
	private BDCSystem _bdcSystem;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_playerScene = this.GetNode<PlayerScene>("%PlayerScene");
        _enemyScene = this.GetNode<EnemyScene>("%EnemyScene");
	}

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(Controls.RIGHT))
        {
			this.QueueFree();
        }
    }

    public void Initialize(BPlayerDC player, BEnemyDC enemy)
	{
		_bdcSystem = new BDCSystem(player, enemy);
		_playerScene.InitializeEntityValues(player.Stats.HP, player.Stats.MP, player.Image);
        _enemyScene.InitializeEntityValues(enemy.Stats.HP, enemy.Stats.MP, enemy.Image);
    }
}
