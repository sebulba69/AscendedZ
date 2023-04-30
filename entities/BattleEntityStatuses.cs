using AscendedZ.battle;
using AscendedZ.entities.battle_entities;
using AscendedZ.statuses;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities
{
    /// <summary>
    /// Handle all statuses applied to a BattleEntity
    /// </summary>
    public class BattleEntityStatuses
    {
        private List<Status> _statuses;

        public List<Status> Statuses { get => _statuses; }

        public BattleEntityStatuses()
        {
            _statuses = new List<Status>();
        }

        public void Clear()
        {
            _statuses.Clear();
        }

        public bool HasStatus(StatusId id)
        {
            return _statuses.FindAll(status => status.Id == id).Count > 0;
        }

        public void AddStatus(BattleEntity entity, Status status)
        {
            bool inList = false;
            foreach(Status s in _statuses)
            {
                if(s.Id == status.Id)
                {
                    s.ApplyStatus();
                    inList = true;
                    break;
                }
            }

            if (!inList)
            {
                var statusToAdd = status.Clone();
                statusToAdd.ActivateStatus(entity);
                _statuses.Add(statusToAdd);
            }
        }

        public void ApplyBattleResult(BattleResult result)
        {
            List<Status> removeStatus = new List<Status>();
            foreach (Status s in _statuses)
            {
                s.UpdateStatus(result);
                if (s.RemoveStatus)
                    removeStatus.Add(s);
            }

            if (removeStatus.Count > 0)
                foreach (var status in removeStatus)
                    _statuses.Remove(status);
        }

        public void UpdateStatusTurns(BattleEntity entity)
        {
            List<Status> removeStatus = new List<Status>();

            foreach (Status s in _statuses)
            {
                s.UpdateStatusTurns(entity);
                if (s.RemoveStatus)
                {
                    removeStatus.Add(s);
                } 
            }

            foreach (Status s in removeStatus)
                _statuses.Remove(s);
        }
    }
}
