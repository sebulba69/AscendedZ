using AscendedZ.dungeon_crawling.combat.skillsdc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.combat.armordc
{
    public enum ArmorPiece
    {
        Head, Torso, Arms, Waist, Legs
    }

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
        public BigInteger HP { get; set; }
        public string Name { get; set; }
        public BigInteger Level { get; set; }
        public ArmorPiece Piece { get; set; }
        public List<SkillDC> Skills { get; set; } = new List<SkillDC>();

        public void LevelUp()
        {
            Level++;
            HP += 5;
            
            foreach (var skill in Skills)
                skill.LevelUp();
        }

        public override string ToString()
        {
            return $"[{HP}] {Name} {Level}";
        }
    }
}
