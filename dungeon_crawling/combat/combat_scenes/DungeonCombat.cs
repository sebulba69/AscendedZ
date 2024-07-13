using AscendedZ;
using AscendedZ.dungeon_crawling.combat.battledc;
using AscendedZ.dungeon_crawling.combat.player_combat_elements;
using Godot;
using System;
using System.Collections.Generic;

public partial class DungeonCombat : Node2D
{
	private List<PlayerScene> _playerScenes;
	private EnemyScene _enemyScene;
	private BDCSystem _bdcSystem;

	private ShakeCamera _camera;
	private GBSkillList _gbSkillList;
	private HBoxContainer _moveQueue;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var playerScene = this.GetNode<PlayerScene>("%PlayerScene");
		var minion1Scene = this.GetNode<PlayerScene>("%MinionScene1");
		var minion2Scene = this.GetNode<PlayerScene>("%MinionScene2");
        
		_playerScenes = new List<PlayerScene>(new PlayerScene[]{ playerScene, minion1Scene, minion2Scene });
		_enemyScene = this.GetNode<EnemyScene>("%EnemyScene");
		_gbSkillList = this.GetNode<GBSkillList>("%GbSkillList");
		_moveQueue = this.GetNode<HBoxContainer>("%GbPlayerMoveQueue");
		_camera = this.GetNode<ShakeCamera>("%ShakeCamera");
	}

    public void Initialize(GBBattlePlayer player, BEnemyDC enemy)
	{
		_bdcSystem = new BDCSystem(player, enemy);

		List<GBEntity> players = new List<GBEntity> { player };
		players.AddRange(player.Minions);

		for (int p = 0; p < players.Count; p++) 
		{
			_playerScenes[p].Visible = true;
			_playerScenes[p].InitializeEntityValues(players[p]);
			_playerScenes[p].Shake += _camera.Shake;
		}

        _enemyScene.Shake += _camera.Shake;
        _enemyScene.InitializeEntityValues(enemy);

        _gbSkillList.DoPlayerAttack += _bdcSystem._OnDoPlayerAttack;
		_gbSkillList.SetUIForPlayerTurn();

		_bdcSystem.ResetAttackButton += _gbSkillList.SetUIForPlayerTurn;
		_bdcSystem.EndBattle += EndBattle;
		_bdcSystem.ShowCurrentMoveQueue += DisplayCurrentMoveQueue;

		_bdcSystem.Start();
    }

	public void DisplayCurrentMoveQueue(Queue<GBQueueItem> queue)
	{
		var children = _moveQueue.GetChildren();
		foreach(var child in children)
		{
			_moveQueue.RemoveChild(child);
		}

		foreach(var item in queue)
		{
			var icon = ResourceLoader.Load<PackedScene>(Scenes.ICON).Instantiate<Icon>();
			_moveQueue.AddChild(icon);
			if(item.Skill != null)
			{
				icon.SetIcon(item.Skill.Icon);
			}
			else
			{
				icon.SetIcon(item.Weapon.Icon);
			}
		}

    }

	private async void EndBattle(bool didPlayerWin)
	{
		if (didPlayerWin) 
		{
			foreach(var playerScene in _playerScenes)
                playerScene.Visible = false;

			_enemyScene.Visible = false;
			_gbSkillList.Visible = false;
			_moveQueue.Visible = false;

            var rewardScene = ResourceLoader.Load<PackedScene>(Scenes.REWARDS).Instantiate<RewardScreen>();
            this.GetTree().Root.AddChild(rewardScene);
            rewardScene.InitializeGranblueEncounterRewards();
            await ToSignal(rewardScene, "tree_exited");

			Visible = false;
        }
	}
}
