using AscendedZ;
using AscendedZ.dungeon_crawling.combat.battledc;
using Godot;
using System;

public partial class DungeonCombat : Node2D
{
	private PlayerScene _playerScene;
	private EnemyScene _enemyScene;
	private BDCSystem _bdcSystem;

	private ShakeCamera _camera;
	private GBSkillList _gbSkillList;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_playerScene = this.GetNode<PlayerScene>("%PlayerScene");
        _enemyScene = this.GetNode<EnemyScene>("%EnemyScene");
		_gbSkillList = this.GetNode<GBSkillList>("%GbSkillList");
		_camera = this.GetNode<ShakeCamera>("%ShakeCamera");
	}

    public void Initialize(GBBattlePlayer player, BEnemyDC enemy)
	{
		_bdcSystem = new BDCSystem(player, enemy);
		_playerScene.InitializeEntityValues(player);
        _enemyScene.InitializeEntityValues(enemy);

		_playerScene.Shake += _camera.Shake;
        _enemyScene.Shake += _camera.Shake;

		_gbSkillList.DoPlayerAttack += _bdcSystem._OnDoPlayerAttack;
		_gbSkillList.SetUIForPlayerTurn();

		_bdcSystem.ResetAttackButton += _gbSkillList.SetUIForPlayerTurn;
		_bdcSystem.EndBattle += EndBattle;
    }

	private async void EndBattle(bool didPlayerWin)
	{
		if (didPlayerWin) 
		{
			_playerScene.Visible = false;
			_enemyScene.Visible = false;
			_gbSkillList.Visible = false;

            var rewardScene = ResourceLoader.Load<PackedScene>(Scenes.REWARDS).Instantiate<RewardScreen>();
            this.GetTree().Root.AddChild(rewardScene);
            rewardScene.InitializeGranblueEncounterRewards();
            await ToSignal(rewardScene, "tree_exited");
            this.QueueFree();
        }
	}
}
