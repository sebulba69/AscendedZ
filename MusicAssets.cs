using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    public class MusicAssets
    {
        public static readonly string OVERWORLD_1 = "res://music/overworld1.ogg";

        public static readonly string DUNGEON1_4 = "res://music/dungeon1-4.ogg";
        public static readonly string DUNGEON5 = "res://music/dungeon5.ogg";

        public static string GetDungeonTrack(int tier)
        {
            if(tier < 5)
            {
                return DUNGEON1_4;
            }
            else if(tier == 5)
            {
                return DUNGEON5;
            }
            else
            {
                return "temp";
            }
        }
    }
}
