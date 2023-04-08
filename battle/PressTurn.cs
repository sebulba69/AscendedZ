using Godot;
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

        public EventHandler TurnEnded;

        public int Turns
        {
            get
            {
                return _turns;
            }
            set
            {
                _turns = value;
                if (_turns <= 0)
                {
                    _turns = 0;
                    TurnEnded?.Invoke(this, EventArgs.Empty);
                }
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
                    this.HalfTurn();
                    break;
                case BattleResultType.Retreat:
                    break;
                default:
                    this.FullTurn();
                    break;
            }
        }

        private void HalfTurn()
        {
            Turns--;   
        }

        private void FullTurn()
        {
            Turns -= 2;
        }

        private void NullTurn()
        {
            if (Turns >= 4)
                Turns -= 4;
            else
                EndTurn();
        }

        private void EndTurn()
        {
            Turns = 0;
        }

    }
}
