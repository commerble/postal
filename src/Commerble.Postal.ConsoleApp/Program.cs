using CommandLine;
using CommandLine.Text;
using RazorLight;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Commerble.Postal.ConsoleApp
{
    public class Options
    {
        [Option('i', "Input", HelpText = "Input csv file path", Required = true)]
        public string Input { get; set; }

        [Option('t', "Template", HelpText = "Razor template file path")]
        public string Template { get; set; }

        [Option('e', "Encode", HelpText = "Input file encoding", Default = "Shift_JIS")]
        public string Encoding { get; set; }

        [Option('m', "Mode", HelpText = "Parse mode(Ken|Jigyosyo)", Default = ParseMode.Ken)]
        public ParseMode Mode { get; set; }

        //[HelpOption]
        //public string GetUsage()
        //{
        //    return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        //}
    }

    class Program
    {
        static void Main(string[] args)
        {
            Options options = null;
            Parser.Default.ParseArguments<Options>(args).WithParsed(o => options = o);
            if (options == null)
                return;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var postals = PostalLoader.Load(options.Input, Encoding.GetEncoding(options.Encoding), options.Mode);
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
                var template = File.ReadAllText(options.Template);

                var razorEngine = new RazorLightEngineBuilder()
                    .UseMemoryCachingProvider()
                    .Build();
                var rendered = razorEngine.CompileRenderAsync("template", template, normalized).Result;
                Console.WriteLine(rendered);
            }
        }
    }
}
