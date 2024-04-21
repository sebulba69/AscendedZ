﻿using AscendedZ.dungeon_crawling.backend.TileEvents;
using AscendedZ.dungeon_crawling.combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.dungeon_crawling.backend.Tiles
{
    public class MainEncounterTile : ITile
    {
        public bool EventTriggered { get; set; } = false;
        public string Graphic { get => Encounter.Image; }
        public bool IsMainTile { get; set; } = true;
        public bool IsExit { get; set; }
        public ITile Left { get; set; }
        public ITile Right { get; set; }
        public ITile Up { get; set; }
        public ITile Down { get; set; }
        public EnemyDC Encounter { get; set; }

        private Direction _direction;

        public MainEncounterTile(Direction direction, EnemyDC enemyDC)
        {
            _direction = direction;
            Encounter = enemyDC;
        }

        public Direction GetDirection()
        {
            return _direction;
        }

        public ITileEvent GetTileEvent()
        {
            return new EncounterEvent() { Tile = this };
        }
    }
}
