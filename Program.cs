using CommandLine;
using CommandLine.Text;
using CodeCoverageBuilder;

Parser parser = new(with =>
{
    with.CaseSensitive = false;
    with.EnableDashDash = true;
    with.AutoHelp = false;
    with.AutoVersion = false;
});

var parserResult = parser.ParseArguments<CreateReportOptions, VersionOptions>(args);

await parserResult.MapResult(
    async (CreateReportOptions options) => { await VerbsHandler.CreateReport(options); return 0; },
    (VersionOptions options) => { Display.Version(); return Task.FromResult(0); },
    (errors) => { Display.Help(parserResult, errors); return Task.FromResult(1); });

internal static class ScriptData
{
    public static readonly HeadingInfo HeadingInfo = new("CodeCoverageBuilder", "1.0.0");
    public static readonly CopyrightInfo CopyrightInfo = new("dr-marek-jaskula", 2022);
    public static readonly string Path = Directory.GetCurrentDirectory();

}