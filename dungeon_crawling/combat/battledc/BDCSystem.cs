using AscendedZ.dungeon_crawling.combat.battledc.gbskills;
using AscendedZ.dungeon_crawling.combat.player_combat_elements;
using AscendedZ.dungeon_crawling.combat.skillsdc;
using AscendedZ.entities;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AscendedZ.dungeon_crawling.combat.battledc
{
    public delegate void ResetAttackButton();
    public delegate void EndBattle(bool didPlayerWin);
    public delegate void CurrentMoveQueue(Queue<GBQueueItem> queue);

    public class BDCSystem
    {
        public ResetAttackButton ResetAttackButton;
        public EndBattle EndBattle;
        public CurrentMoveQueue ShowCurrentMoveQueue;

        private Queue<GBQueueItem> _queue;
        private List<GBBattlePlayer> _players;
        private BEnemyDC _enemy;
        private Random _rng;
        private bool _endBattle, _playerTurn;
        private int _queueLimit;

        public List<GBBattlePlayer> Players { get => _players; }
        public BEnemyDC EnemyDC { get => _enemy; }

        public BDCSystem(GBBattlePlayer player, BEnemyDC enemy)
        {
            _players = new List<GBBattlePlayer>() { player };
            _players.AddRange(player.Minions);

            _enemy = enemy;
            _rng = new Random();
            _queue = new Queue<GBQueueItem>();
            _queueLimit = _players.Count;
            _playerTurn = true;
        }

        public void Start()
        {
            _players[_queue.Count].SetCurrent(true);
        }

        private async void EnqueueAction(GBQueueItem item)
        {
            _queue.Enqueue(item);
            if (_queue.Count >= _queueLimit)
            {
                while (_queue.Count > 0 && !_endBattle) 
                {
                    if(_playerTurn)
                        ShowCurrentMoveQueue(_queue);
                    var move = _queue.Dequeue();
                    await HandleAttack(move);
                }

                if (!_endBattle)
                {
                    // do opposite entity turn
                    _playerTurn = !_playerTurn;
                    if (_playerTurn)
                    {
                        _queueLimit = _players.Count;
                        ResetAttackButton();
                    }
                    else
                    {
                        ShowCurrentMoveQueue(_queue);

                        _queueLimit = _enemy.Turns;
                        GBSkill skill = _enemy.GetSkill();
                        GBQueueItem enemyItem = new GBQueueItem()
                        {
                            Skill = skill,
                            User = _enemy,
                            Target = _players[_rng.Next(_players.Count)]
                        };
                        EnqueueAction(enemyItem);
                    }
                }
                else
                {
                    EndBattle(_enemy.HP == 0);
                }
            }
            else
            {
                _players[_queue.Count].SetCurrent(true);
                ShowCurrentMoveQueue(_queue);
                ResetAttackButton();
            }
        }

        public void _OnDoPlayerAttack(object sender, EventArgs e)
        {
            GBBattlePlayer player = _players[_queue.Count];
            player.SetCurrent(false);

            GBQueueItem item = new GBQueueItem()
            {
                Weapon = player.Weapon,
                User = player,
                Target = _enemy
            };

            EnqueueAction(item);
        }

        private async Task HandleAttack(GBQueueItem item)
        {
            if (item.Weapon != null)
            {
                for (int h = 0; h < item.Weapon.HitRate; h++)
                {
                    string result = "";
                    long attack = item.Weapon.Attack;
                    if (_rng.Next(1, 101) <= item.Weapon.CritChance * 100)
                    {
                        result = "CRITICAL";
                        attack = (long)(attack * 1.25);
                    }

                    item.Target.HP -= attack;
                    if (item.Target.HP < 0)
                        item.Target.HP = 0;

                    ISkill skill = SkillDatabase.GetSkillByElement(1, item.Weapon.Element);
                    string effect = skill.EndupAnimation;

                    await item.User.PlayEffect(SkillAssets.STARTUP1_MG);
                    await item.Target.PlayEffect(effect);
                    item.Target.PlayDamageNumberAnimation(attack, result);
                    item.Target.UpdateHP(item.Target.GetHPPercentage(), item.Target.HP);
                    await Task.Delay(750);

                    if (item.Target.HP == 0)
                    {
                        _endBattle = true;
                        break;
                    }
                }
            }
            else if(item.Skill != null)
            {
                if(item.Skill.Type == GBSkillType.EnemyElement)
                {
                    string result = "";
                    long attack = item.Skill.Value;

                    item.Target.HP -= attack;
                    if (item.Target.HP < 0)
                        item.Target.HP = 0;

                    ISkill skill = SkillDatabase.GetSkillByElement(1, item.Skill.Element);
                    string effect = skill.EndupAnimation;

                    await item.User.PlayEffect(SkillAssets.STARTUP1_MG);
                    await item.Target.PlayEffect(effect);
                    item.Target.PlayDamageNumberAnimation(attack, result);
                    item.Target.UpdateHP(item.Target.GetHPPercentage(), item.Target.HP);
                    await Task.Delay(750);

                    if (item.Target.HP == 0)
                    {
                        _endBattle = true;
                    }
                }


            }
        }

        private bool DidCrit(Weapon weapon)
        {
            return _rng.Next(1, 101) <= weapon.CritChance * 100;
        }
    }
}
