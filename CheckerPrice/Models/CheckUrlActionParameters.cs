using CommandLine;

namespace CheckerPrice.ConsoleApp.Models
{
    [Verb("check-url", HelpText = "Check an url")]
    public class CheckUrlActionParameters
    {
        [Option('u', "url", Required = true, HelpText = "Url to check")]
        public string Url { get; set; }
    }
}
