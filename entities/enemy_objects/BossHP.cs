﻿using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities.enemy_objects
{
    public class BossHP
    {
        /// <summary>
        /// Max limit of HP per bar
        /// </summary>
        private const int MAX_HP_PER_BAR = 5000;
        private readonly string BGCOLOR = "000000";
        private readonly string[] HPFGColors = 
        {
            "99b655", "9aff91", "ffff6b", "ffb86b", "13376b", "9c150b", "9c1582", "8fa582"
        };

        private int _bars, _currentDisplayHP, _maxHPBarDisplay, _totalHP;

        private Color _fgColor, _bgColor;

        public BossHP()
        {
            // by default, the max hp per bar is 1k
            _maxHPBarDisplay = MAX_HP_PER_BAR;
            _bars = 0;
        }

        public void InitializeBossHP(int hp, bool doNotSetMaxHPValue = false)
        {
            _totalHP = hp;
            _bars = 0;

            // if the total HP is less than MAX_HP_PER_BAR, then we
            // will just set up a custom bar with custom HP values
            // there's no need to adjust the algorithm for back colors
            if (_totalHP < MAX_HP_PER_BAR && !doNotSetMaxHPValue)
            {
                _maxHPBarDisplay = _totalHP;
                _currentDisplayHP = _totalHP;
            }
            else
            {
                int totalHP = _totalHP;
                while (totalHP > MAX_HP_PER_BAR)
                {
                    _bars++;
                    totalHP -= MAX_HP_PER_BAR;
                }

                _currentDisplayHP = totalHP;
            }

            SetBarColors();
        }

        public void ChangeHP(int hp)
        {
            if (_totalHP == hp)
                return;

            if(_bars == 0)
            {
                _currentDisplayHP = hp;
            }
            else
            {
                InitializeBossHP(hp, true);
            }
        }

        private void SetBarColors()
        {
            if(_bars > 0)
            {
                int fgIndex = (_bars % HPFGColors.Length);
                int bgIndex = fgIndex - 1;
                if (bgIndex < 0)
                {
                    bgIndex = HPFGColors.Length - 1;
                }

                _fgColor = new Color(HPFGColors[fgIndex]);
                _bgColor = new Color(HPFGColors[bgIndex]);
            }
            else
            {
                _fgColor = new Color(HPFGColors[0]);
                _bgColor = new Color(BGCOLOR);
            }
        }

        public BossHPStatus GetBossHPUIValues()
        {
            return new BossHPStatus()
            {
                CurrentBarHP = _currentDisplayHP,
                MaxBarHP = _maxHPBarDisplay,
                FG = _fgColor,
                BG = _bgColor,
                NumBars = _bars
            };
        }
    }
}
