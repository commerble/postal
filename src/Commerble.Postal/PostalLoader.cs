using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Commerble.Postal
{
    public enum ParseMode
    {
        Ken,
        Jigyosyo
    }

    public class PostalLoader
    {
        public static PostalCode Parse(string line, ParseMode parseType)
        {
            var csv = line.Split(',');
            Func<string, string> trim = s => s.Trim('"').Trim();

            var postal = parseType == ParseMode.Ken
                ? new PostalCode
                {
                    Jis = csv[0],
                    Code = trim(csv[2]),
                    Prefecture = trim(csv[6]),
                    City = trim(csv[7]),
                    Street = trim(csv[8])
                }
                : new PostalCode
                {
                    Jis = csv[0],
                    Code = trim(csv[7]),
                    Prefecture = trim(csv[3]),
                    City = trim(csv[4]),
                    Street = trim(csv[6]),
                    Name = trim(csv[2])
                };

            return postal;
        }

        public static IEnumerable<PostalCode> Load(string filePath, Encoding encoding, ParseMode parseType)
        {
            var postals = new List<PostalCode>();
            foreach (var line in File.ReadLines(filePath, encoding))
            {
                var postal = Parse(line, parseType);
                postals.Add(postal);
            }

            return postals;
        }
    }
}
