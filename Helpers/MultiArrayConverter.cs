using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sudoku.Helpers
{
    public class MultiArrayConverter : JsonConverter<int[,]>
    {
#nullable enable
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(int[,]) == typeToConvert;
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override int[,] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                var values = JsonSerializer.Deserialize<int[][]>(ref reader, options);
                var dim0 = values.Length;
                var dim1 = values.GroupBy(x => x.Length).Single().Key;

                var result = new int[dim0, dim1];

                for (var x = 0; x < dim0; x++)
                    for (var y = 0; y < dim1; y++)
                        result[x, y] = values[x][y];

                return result;
            }
            catch
            {
                return new int[0, 0];
            }
        }

        public override string? ToString()
        {
            return base.ToString();
        }

        public override void Write(Utf8JsonWriter writer, int[,] value, JsonSerializerOptions options)
        {
            var dim0 = value.GetLength(0);
            var dim1 = value.GetLength(1);

            var result = new int[dim0][];

            for (var x = 0; x < dim0; x++)
            {
                result[x] = new int[dim1];
                for (var y = 0; y < dim1; y++)
                    result[x][y] = value[x, y];
            }

            JsonSerializer.Serialize(writer, result, options);
        }
#nullable restore
    }
}
