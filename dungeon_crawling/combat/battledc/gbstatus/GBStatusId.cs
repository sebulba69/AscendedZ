using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.battledc.gbstatus
{
    public enum GBStatusId
    {
        None,
        DaggerParry, DaggerLifeSteal,
        SwordAttackBuff, SwordDefenseBuff,
        FlailAgro, FlailAgroCooldown,
        AxeCharge,
        WhipHPStore,
        StaffMana,
        BowCritRate, BowHPHeal,
        CrossbowDefDown, CrossbowBoostUserCrit,
        FlintlockDekaja, FlintlockDekundaSingle,
        ClawBloodStance,
        SpearDefense, SpearSkill2Buff,
        GSCharge1, GSCharge2,
        HammerCharge1, HammerStun
    }
}
