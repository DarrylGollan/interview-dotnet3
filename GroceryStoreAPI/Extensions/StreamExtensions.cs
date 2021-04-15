using Newtonsoft.Json;
using System;
using System.IO;

namespace GroceryStoreAPI.Extensions
{
    public static class StreamExtensions
    {
        public static T ReadAndDeserializeFromJson<T>(this Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (!stream.CanRead)
            {
                throw new NotSupportedException("Can't read from this stream.");
            }

            using (var streamReader = new StreamReader(stream))
            {
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    var jsonSeralizer = new JsonSerializer();
                    return jsonSeralizer.Deserialize<T>(jsonTextReader);
                }
            }
        }

    }
}
