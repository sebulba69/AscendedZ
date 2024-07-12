using AscendedZ.dungeon_crawling.combat.player_combat_elements;
using AscendedZ.dungeon_crawling.combat.skillsdc;
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

    public class BDCSystem
    {
        public ResetAttackButton ResetAttackButton;
        public EndBattle EndBattle;

        private Queue<GBQueueItem> _queue;
        private GBBattlePlayer _player;
        private BEnemyDC _enemy;
        private Random _rng;
        private bool _endBattle;
        private int _queueLimit;

        public GBBattlePlayer Player { get => _player; }
        public BEnemyDC Enemy { get => _enemy; }

        public BDCSystem(GBBattlePlayer player, BEnemyDC enemy)
        {
            _player = player;
            _enemy = enemy;
            _rng = new Random();
            _queue = new Queue<GBQueueItem>();
            _queueLimit = 1;
        }

        private async void EnqueueAction(GBQueueItem item)
        {
            _queue.Enqueue(item);
            if (_queue.Count >= _queueLimit)
            {
                while (_queue.Count > 0) 
                {
                    await HandleAttack(_queue.Dequeue());
                }

                if (!_endBattle)
                {
                    // do opposite entity turn
                    ResetAttackButton();
                }
                else
                {
                    EndBattle(_enemy.HP == 0);
                }
            }
        }

        public void _OnDoPlayerAttack(object sender, EventArgs e)
        {
            GBQueueItem item = new GBQueueItem() 
            {
                Weapon = _player.Weapon,
                User = _player,
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
        }

        private bool DidCrit(Weapon weapon)
        {
            return _rng.Next(1, 101) <= weapon.CritChance * 100;
        }
    }
}
