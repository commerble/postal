using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Commerble.Postal
{
    public class PostalLoader
    {
        public static PostalCode Parse(string line)
        {
            var csv = line.Split(',');
            Func<string, string> trim = s => s.Trim('"').Trim();

            var postal = new PostalCode
            {
                Jis = csv[0],
                Code = trim(csv[2]),
                Prefecture = trim(csv[6]),
                City = trim(csv[7]),
                Street = trim(csv[8])
            };

            return postal;
        }

        public static IEnumerable<PostalCode> Load(string filePath, Encoding encoding)
        {
            var postals = new List<PostalCode>();
            foreach (var line in File.ReadLines(filePath, encoding))
            {
                var postal = Parse(line);
                postals.Add(postal);
            }

            return postals;
        }
    }
}
