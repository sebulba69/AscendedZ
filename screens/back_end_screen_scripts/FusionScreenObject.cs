using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.screens.back_end_screen_scripts
{
    public class FusionScreenObject
    {
        private List<FusionObject> _fusions;

        public List<FusionObject> Fusions { get => _fusions; }

        private static FusionScreenObject _fusionScreenObject;

        public static FusionScreenObject Instance()
        {
            return new FusionScreenObject();
        }

        public FusionScreenObject()
        {
            _fusions = new List<FusionObject>();
        }

        public void PopulateMaterialFusionList()
        {
            _fusions.Clear();

            List<OverworldEntity> party = PersistentGameObjects.GameObjectInstance().MainPlayer.ReserveMembers;

            // save the materials + fusion results
            for(int m1 = 0; m1 < party.Count; m1++)
            {
                for(int m2 = 0; m2 < party.Count; m2++)
                {
                    if(m1 != m2)
                    {
                        if (!IsFusionRecipeGenerated(party[m1], party[m2]))
                        {
                            var fusions = EntityDatabase.MakeFusionEntities(party[m1], party[m2]);
                            if(fusions.Count > 0)
                                _fusions.AddRange(fusions);
                        }
                    }
                }
            }

            _fusions = _fusions.OrderBy(fusion => fusion.Fusion.Name).ToList();
        }

        private bool IsFusionRecipeGenerated(OverworldEntity m1, OverworldEntity m2)
        {
            bool isGenerated = false;
            
            foreach(var fusion in _fusions)
            {
                if(fusion.Material1 == m2 && fusion.Material2 == m1)
                {
                    isGenerated = true;
                    break;
                }
            }

            return isGenerated;
        }

        public List<OverworldEntity> GetMaterials(int index)
        {
            List<OverworldEntity> materials = new List<OverworldEntity>();
            var fusion = _fusions[index];

            materials.Add(fusion.Material1);
            materials.Add(fusion.Material2);

            return materials;
        }
    }
}
