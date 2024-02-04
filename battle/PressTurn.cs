﻿using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.battle
{
    public class PressTurn
    {
        private int _turns = 0;
        public bool TurnEnded { get; set; }

        private List<int> _turnIcons;
        public List<int> TurnIcons { get => _turnIcons; }

        public PressTurn()
        {
            _turnIcons = new List<int>();
        }

        /// <summary>
        /// Set the turn icons for the battle.
        /// </summary>
        /// <param name="turns"></param>
        public void SetTurns(int turns)
        {
            _turnIcons.Clear();
            for (int i = 0; i < turns; i++)
            {
                _turnIcons.Add(2);
            }
        }

        public void HandleTurns(BattleResultType resultType)
        {
            switch (resultType)
            {
                case BattleResultType.Dr:
                    this.EndTurn();
                    break;
                case BattleResultType.Nu:
                    this.NullTurn();
                    break;
                case BattleResultType.Wk:
                    this.HalfTurn();
                    break;
                case BattleResultType.Pass:
                    this.PassTurn();
                    break;
                case BattleResultType.Retreat:
                    break;
                default:
                    this.FullTurn();
                    break;
            }
        }

        /// <summary>
        /// Find the first 2 and turn it into a 1. If no 2 exists, then do a full turn.
        /// </summary>
        private void HalfTurn()
        {
            int index = _turnIcons.FindIndex(x => x == 2);
            if(index == -1)
            {
                FullTurn();
            }
            else
            {
                _turnIcons[index] = 1;
            }
        }

        /// <summary>
        /// If the first icon is a 2, make it a 1, else remove it from the list.
        /// </summary>
        private void PassTurn()
        {
            if (_turnIcons[0] == 2)
                _turnIcons[0] = 1;
            else
                FullTurn();
        }

        private void FullTurn()
        {
            _turnIcons.RemoveAt(0);
            if (_turnIcons.Count == 0)
                TurnEnded = true;
        }

        private void NullTurn()
        {
            if (_turnIcons.Count >= 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    FullTurn();
                }
            }
            else
            {
                EndTurn();
            }    
        }

        private void EndTurn()
        {
            _turnIcons.Clear();
        }

    }
}
