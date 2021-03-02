using CommandLine;

namespace CheckerPrice.ConsoleApp.Models
{
    [Verb("delete", HelpText = "Delete saved url")]
    public class DeleteActionParameters
    {
        [Option('i', "id", Required = true, HelpText = "Id to remove")]
        public int Id { get; set; }
    }
}
