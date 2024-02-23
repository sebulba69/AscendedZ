using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.game_object.quests
{
    public class PartyQuest : Quest
    {
        private List<string> _skillBaseNames = new List<string>();

        /// <summary>
        /// Party Member Name <-- includes Grade
        /// </summary>
        public string PartyMemberName { get; set; }

        public List<string> SkillBaseNames { get => _skillBaseNames; set => _skillBaseNames = value; }
    }
}
