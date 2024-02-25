using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AscendedZ.json_interface_converters
{
    /// <summary>
    /// Newtonsoft cannot natively handle interface deserialization so we
    /// have to extend its converter to allow for our interfaces to be processed
    /// correctly.
    /// </summary>
    public partial class SkillConverter : JsonConverter<ISkill>
    {
        public override ISkill Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            ISkill skill = default(ISkill);

            using(var jsonDocument = JsonDocument.ParseValue(ref reader))
            {
                int id = jsonDocument.RootElement.GetProperty("Id").GetInt32();
                switch (id)
                {
                    case (int)SkillId.Elemental:
                        skill = JsonSerializer.Deserialize<ElementSkill>(jsonDocument);
                        break;
                    case (int)SkillId.Status:
                        skill = JsonSerializer.Deserialize<StatusSkill>(jsonDocument);
                        break;
                    case (int)SkillId.Healing:
                        skill = JsonSerializer.Deserialize<HealSkill>(jsonDocument);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            return skill;
        }

        public override void Write(Utf8JsonWriter writer, ISkill value, JsonSerializerOptions options)
        {
            switch (value.Id)
            {
                case SkillId.Elemental:
                    JsonSerializer.Serialize(writer, value, typeof(ElementSkill), options);
                    break;
                case SkillId.Status:
                    JsonSerializer.Serialize(writer, value, typeof(StatusSkill), options);
                    break;
                case SkillId.Healing:
                    JsonSerializer.Serialize(writer, value, typeof(HealSkill), options);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
