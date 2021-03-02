using CommandLine;

namespace CheckerPrice.ConsoleApp.Models
{
    [Verb("add", HelpText = "Add new link to check")]
    public class AddActionParameters
    {
        [Option('u', "url", Required = true, HelpText = "Url to check.")]
        public string Url { get; set; }
    }
}
