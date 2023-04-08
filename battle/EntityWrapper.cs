using AscendedZ.entities;
using AscendedZ.entities.battle_entities;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.battle
{
    /// <summary>
    /// Wrapper for Battle Entities so we can send them over to Godot Objects
    /// </summary>
    public partial class EntityWrapper : GodotObject
    {
        public BattleEntity BattleEntity { get; set; }
    }
}
