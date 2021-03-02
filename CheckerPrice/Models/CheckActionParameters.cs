using CommandLine;

namespace CheckerPrice.ConsoleApp.Models
{
    [Verb("check", HelpText = "Check prices")]
    public class CheckActionParameters
    {
        [Option('i', "id", Required = false, HelpText = "Url to check")]
        public int? Id { get; set; }
    }
}
