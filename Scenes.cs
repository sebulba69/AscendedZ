using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    /// <summary>
    /// A collection of scene paths used throughout the game.
    /// </summary>
    public partial class Scenes
    {
        // new UI screens
        public static readonly string CUTSCENE = "res://screens/CGCutsceneScreen.tscn";
        public static readonly string START = "res://screens/StartScreen.tscn";
        public static readonly string MAIN = "res://screens/MainScreen.tscn";
        public static readonly string MAIN_RECRUIT = "res://screens/RecruitScreen.tscn";
        public static readonly string MAIN_EMBARK = "res://screens/EmbarkScreen.tscn";
        public static readonly string MAIN_EMBARK_DISPLAY = "res://screens/PartyMemberDisplay.tscn";
        public static readonly string BATTLE_SCENE = "res://screens/BattleEnemyScene.tscn";
        public static readonly string MENU = "res://screens/MenuScene.tscn";
        public static readonly string UPGRADE = "res://screens/UpgradeScreen.tscn";

        // currency
        public static readonly string CURRENCY_DISPLAY = "res://misc_icons/CurrencyDisplay.tscn";

        // popups
        public static readonly string YES_NO_POPUP = "res://screens/PopupWindow.tscn";

        // reward screen
        public static readonly string REWARDS = "res://screens/RewardScreen.tscn";

        // battle assets
        public static readonly string PARTY_BOX = "res://screens/PartyBattleDisplayBox.tscn";
        public static readonly string ENEMY_BOX = "res://screens/EnemyBattleDisplayBox.tscn";
        public static readonly string BOSS_BOX = "res://screens/BossBattleDisplayBox.tscn";

        // effects
        public static readonly string EFFECTS = "res://effects/EffectSprite.tscn";
        public static readonly string DAMAGE_NUM = "res://effects/DamageNumber.tscn";
        public static readonly string STATUS = "res://statuses/StatusIcon.tscn";
    }
}
