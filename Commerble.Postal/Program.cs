using CommandLine;
using CommandLine.Text;
using RazorTemplates.Core;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Commerble.Postal
{
    public class Options
    {
        [Option('i', "Input csv file path", Required = true)]
        public string Input { get; set; }

        [Option('t', "Razor template file path")]
        public string Template { get; set; }

        [Option('e', "Input file encoding", DefaultValue = "Shift_JIS")]
        public string Encoding { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (!Parser.Default.ParseArguments(args, options))
                return;

            var postals = PostalLoader.Load(options.Input, Encoding.GetEncoding(options.Encoding));
            var normalizar = new PostalNormalizar();
            var normalized = normalizar.Normalize(postals).Distinct();

            // display only
            if (string.IsNullOrEmpty(options.Template))
            {
                foreach (var n in normalized)
                {
                    Console.WriteLine(n.ToString());
                }
            }
            else
            {
                var source = File.ReadAllText(options.Template);
                var template = Template.Compile(source);
                Console.WriteLine(template.Render(normalized));
            }
        }
    }
}
