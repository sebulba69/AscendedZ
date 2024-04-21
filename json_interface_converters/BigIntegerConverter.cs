﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace AscendedZ.json_interface_converters
{
    public class BigIntegerConverter : JsonConverter<BigInteger>
    {
        public override BigInteger Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.Number)
                throw new JsonException(string.Format("Found token {0} but expected token {1}", reader.TokenType, JsonTokenType.Number));
            using var doc = JsonDocument.ParseValue(ref reader);
            return BigInteger.Parse(doc.RootElement.GetRawText(), NumberFormatInfo.InvariantInfo);
        }

        public override void Write(Utf8JsonWriter writer, BigInteger value, JsonSerializerOptions options) =>
            writer.WriteRawValue(value.ToString(NumberFormatInfo.InvariantInfo), false);
    }
}
