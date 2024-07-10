using AscendedZ.dungeon_crawling.combat.skillsdc;
using AscendedZ.skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.battledc
{
    public delegate void ResetAttackButton();
    public delegate void EndBattle(bool didPlayerWin);

    public class BDCSystem
    {
        public ResetAttackButton ResetAttackButton;
        public EndBattle EndBattle;

        private GBBattlePlayer _player;
        private BEnemyDC _enemy;
        private Random _rng;
        private bool _endBattle;

        public GBBattlePlayer Player { get => _player; }
        public BEnemyDC Enemy { get => _enemy; }

        public BDCSystem(GBBattlePlayer player, BEnemyDC enemy)
        {
            _player = player;
            _enemy = enemy;
            _rng = new Random();
        }

        public void _OnDoPlayerAttack(object sender, EventArgs e)
        {
            // normally, you'd queue up the move here
            PerformPlayerAttack();
        }

        private void PerformPlayerAttack()
        {
            long attack = _player.Attack;
            int hitRate = _player.HitRate;
            int critChance = (int)(_player.CritChance * 100);
            Elements element = _player.Element;

            HandleAttack(attack, hitRate, critChance, element, _player, _enemy);
        }

        private async void HandleAttack(long attack, int hitRate, int critChance, Elements element, GBEntity user, GBEntity target)
        {
            for (int h = 0; h < hitRate; h++) 
            {
                bool crit = _rng.Next(1, 101) <= critChance;
                string resultString = "";
                if (crit) 
                {
                    resultString = "CRITICAL";
                    attack *= 2;
                }

                target.HP -= attack;
                if (target.HP <= 0)
                    target.HP = 0;

                ISkill skill = SkillDatabase.GetSkillByElement(1, element);
                string effect = skill.EndupAnimation;

                await user.PlayEffect(SkillAssets.STARTUP1_MG);
                await target.PlayEffect(effect);
                target.PlayDamageNumberAnimation(attack, resultString);
                target.UpdateHP(target.GetHPPercentage(), target.HP);
                await Task.Delay(750);

                if(target.HP == 0)
                {
                    _endBattle = true;
                    break;
                }
            }

            if (!_endBattle)
            {
                ResetAttackButton();
            }
            else
            {
                EndBattle(_enemy.HP == 0);
            }
        }
    }
}
