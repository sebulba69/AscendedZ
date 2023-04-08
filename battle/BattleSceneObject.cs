using AscendedZ.battle.battle_state_machine;
using AscendedZ.entities;
using AscendedZ.entities.battle_entities;
using AscendedZ.entities.enemy_objects;
using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Godot.HttpRequest;

namespace AscendedZ.battle
{
    public enum TurnState { PLAYER, ENEMY }

    /// <summary>
    /// An object that houses all the important stuff we want
    /// to keep track of in our Battle Scene.
    /// </summary>
    public class BattleSceneObject
    {
        // When the player selects a skill
        public EventHandler MakeEnemyDoTurn;
        public EventHandler UpdateTurnState;
        public EventHandler<int> UpdateActivePlayer;
        public EventHandler<PlayerTargetSelectedEventArgs> SkillSelected;
        public EventHandler<BattleResult> PostBattleResult;

        private const int PLAYER_STATE = 0;
        private const int ENEMY_STATE = 1;
        private IBattleState[] _states = new IBattleState[] { new PlayerTurnState(), new EnemyTurnState() }; 
        private IBattleState _currentState;
        private TurnState _turnState = TurnState.PLAYER;

        public List<BattlePlayer> Players { get; set; } = new();
        public List<Enemy> Enemies { get; set; } = new();
        public PressTurn PressTurn { get; set; } = new();
        public TurnState TurnState { get => _turnState; }
        public BattlePlayer ActivePlayer { get; private set; }

        public BattleSceneObject()
        {
            _currentState = _states[PLAYER_STATE];
            PressTurn.TurnEnded += _OnTurnStateChange;
            // each time active player is updated, set ActivePlayer
            this.UpdateActivePlayer += (sender, args) => 
            { 
                if(args >= 0)
                    this.ActivePlayer = this.Players[args]; 
            };
        }

        /// <summary>
        /// This function is only called once at the start of the battle.
        /// It kicks off our state machine.
        /// </summary>
        public void InitializePartyMembers()
        {
            this.Players = PersistentGameObjects.Instance().MakeBattlePlayerListFromParty();
            this.SetPartyMemberTurns();
        }

        public void InitializeEnemies(int tier)
        {
            this.Enemies = EntityDatabase.MakeBattleEncounter(tier);
        }

        /// <summary>
        /// A function to start the current state.
        /// Can be called at the start of battle if you pass in isBattleStart as true.
        /// </summary>
        public void StartCurrentState(bool isBattleStart = false)
        {
            _currentState.StartState(this);
            // we don't want to call this event if the battle is just starting
            // it'll force the UI to hang up on UpdateUI
            if(!isBattleStart)
                this.UpdateTurnState?.Invoke(this, EventArgs.Empty);
        }

        public void SetPartyMemberTurns()
        {
            int turns = 0;
            foreach (var party in this.Players)
            {
                if (party.HP > 0)
                {
                    turns +=2;
                }
            }
            this.PressTurn.Turns = turns;

            foreach(var enemy in this.Enemies)
                enemy.StatusHandler.UpdateStatusTurns(enemy);
        }

        public void SetupEnemyTurns()
        {
            int turns = 0;
            foreach (var enemy in this.Enemies)
            {
                if (enemy.HP > 0)
                {
                    turns += enemy.Turns;
                }
            }

            foreach(var party in this.Players)
                party.StatusHandler.UpdateStatusTurns(party);

            this.PressTurn.Turns = turns*2;
        }

        public void HandlePostTurnProcessing(BattleResult result)
        {
            if (result.Target?.HP == 0)
            {
                result.Target.StatusHandler.Clear();
            }

            result.User?.StatusHandler.ApplyBattleResult(result);
            result.Target?.StatusHandler.ApplyBattleResult(result);

            this.PressTurn.HandleTurns(result.ResultType);
            this.PostBattleResult?.Invoke(this, result);
        }


        /// <summary>
        /// Make sure all our party members are still alive.
        /// </summary>
        public bool DidEnemiesWin()
        {
            return this.Players.FindAll(party => party.HP > 0).Count == 0;
        }

        /// <summary>
        /// Make sure all our enemies are still alive.
        /// </summary>
        public bool DidPartyMembersWin()
        {
            return this.Enemies.FindAll(enemy => enemy.HP > 0).Count == 0;
        }

        public void _OnTurnStateChange(object sender, EventArgs e)
        {
            _currentState.EndState(this);
            if (_turnState == TurnState.PLAYER)
            {
                _turnState = TurnState.ENEMY;
                _currentState = _states[ENEMY_STATE];
            }
            else
            {
                _turnState = TurnState.PLAYER;
                _currentState = _states[PLAYER_STATE];
            }
            this.StartCurrentState();
        }
    }
}
