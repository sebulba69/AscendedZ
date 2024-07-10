using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.battledc
{
    public delegate Task PlayEffect(string effect);
    public delegate void UpdateHP(int hpPercentage, long hp);
    public delegate void PlayDamageNumberAnimation(long damage, string resultString);
    public class GBEntity
    {
        public PlayEffect PlayEffect;
        public UpdateHP UpdateHP;
        public PlayDamageNumberAnimation PlayDamageNumberAnimation;

        public long MaxHP { get; set; }
        public long HP { get; set; }
        public string Image { get; set; }

        public void Heal(long amount)
        {
            HP += amount;
            if(HP > MaxHP) HP = MaxHP;
        }

        public int GetHPPercentage()
        {
            double percent = (HP / (double)MaxHP) * 100;
            return (int)percent;
        }
    }
}
