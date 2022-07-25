using CommandLine;
using CommandLine.Text;

namespace CodeCoverageBuilder;
public static class Display
{
    public static void Help<T>(ParserResult<T> result, IEnumerable<Error> errors)
    {
        var helpText = HelpText.AutoBuild(result, ht =>
        {
            ht.Heading = ScriptData.HeadingInfo;
            ht.Copyright = ScriptData.CopyrightInfo;
            ht.AddDashesToOption = true;
            ht.AdditionalNewLineAfterOption = true;
            return HelpText.DefaultParsingErrorsHandler(result, ht);
        }, e => e);

        Console.WriteLine(helpText);

        Environment.Exit(1);
    }

    public static void Version()
    {
        Console.WriteLine(ScriptData.HeadingInfo);
        Environment.Exit(0);
    }
}