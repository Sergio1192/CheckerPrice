using CommandLine;

namespace CheckerPrice.Models
{
    [Verb("add", HelpText = "Add new link to check")]
    public class AddModel
    {
        [Option('u', "url", Required = true, HelpText = "Url to check.")]
        public string Url { get; set; }
    }
}
