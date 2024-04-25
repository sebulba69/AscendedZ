using AscendedZ.dungeon_crawling.combat.skillsdc;
using AscendedZ.json_interface_converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.armordc
{

    public class ArmorDC
    {
        private string _icon = string.Empty;
        public string Icon 
        {
            get 
            {
                if(string.IsNullOrEmpty(_icon))
                {
                    _icon = SkillAssets.GetArmorIconFromPiece(Piece);
                }
                return _icon; 
            }
            set => _icon = value; 
        }

        public string Name { get; set; }

        public ArmorPiece Piece { get; set; }

        public StatsDC StatsDC { get; set; } = new();

        public override string ToString()
        {
            return $"[{StatsDC.HP}] {Name} {StatsDC.Level}";
        }
    }
}
