using CliWrap;
using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace CodeCoverageBuilder;

public static class VerbsHandler
{
    public static async Task CreateReport(CreateReportOptions options)
    {
        DeletePreviousReport();

        CreateCoverletRunsettings(options);

        await CreateCoverageDotJson(options);

        await CreateCoverageDotCoberturaDotXml();

        await CreateCodeCoverageHTMLReport();

        Environment.Exit(0);
    }
    private static void DeletePreviousReport()
    {
        if (File.Exists("coverlet.runsettings.xml"))
            File.Delete("coverlet.runsettings.xml");

        if (File.Exists("coverage"))
            File.Delete("coverage");

        if (Directory.Exists("TestResults"))
            Directory.Delete("TestResults", true);

        if (Directory.Exists("codecoverage"))
            Directory.Delete("codecoverage", true);
    }

    private static void CreateCoverletRunsettings(CreateReportOptions options)
    {
        XmlDocument doc = new();

        XmlElement RunSettings = doc.CreateElement(string.Empty, "RunSettings", string.Empty);
        doc.AppendChild(RunSettings);

        XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
        doc.InsertBefore(xmlDeclaration, RunSettings);

        XmlElement DataCollectionRunSettings = doc.CreateElement(string.Empty, "DataCollectionRunSettings", string.Empty);
        RunSettings.AppendChild(DataCollectionRunSettings);

        XmlElement DataCollectors = doc.CreateElement(string.Empty, "DataCollectors", string.Empty);
        DataCollectionRunSettings.AppendChild(DataCollectors);

        XmlElement DataCollector = doc.CreateElement(string.Empty, "DataCollector", string.Empty);
        DataCollector.SetAttribute("friendlyName", "XPlat code coverage");
        DataCollectors.AppendChild(DataCollector);

        XmlElement Configuration = doc.CreateElement(string.Empty, "Configuration", string.Empty);
        DataCollector.AppendChild(Configuration);

        XmlElement SkipAutoProps = doc.CreateElement(string.Empty, "SkipAutoProps", string.Empty);
        XmlText SkipAutoPropsContent = doc.CreateTextNode("true");
        SkipAutoProps.AppendChild(SkipAutoPropsContent);
        Configuration.AppendChild(SkipAutoProps);

        XmlElement Exclude = doc.CreateElement(string.Empty, "Exclude", string.Empty);
        Configuration.AppendChild(Exclude);

        XmlText excludeNamespace = doc.CreateTextNode(options.ExcludeNamespaces);
        Exclude.AppendChild(excludeNamespace);

        doc.Save(Directory.GetCurrentDirectory() + "//coverlet.runsettings.xml");
    }

    private static async Task CreateCoverageDotJson(CreateReportOptions options)
    {
        var relativePath = GetRelativePath(options);

        var cmd = Cli.Wrap("coverlet")
            .WithArguments(args => args
                .Add(relativePath)
                .Add("--target")
                .Add("dotnet")
                .Add("--targetargs")
                .Add("test --no-build"));

        if (!string.IsNullOrEmpty(options.ExcludeNamespaces))
        {
            cmd.WithArguments(args => args
                .Add("--exclude")
                .Add($"[*]{options.ExcludeNamespaces}*"));
        }

        await cmd.ExecuteAsync();
    }

    private static async Task CreateCoverageDotCoberturaDotXml()
    {
        var cmd = Cli.Wrap("dotnet")
            .WithArguments(args => args
                .Add("test")
                .Add("--collect:\"XPlat Code Coverage\"")
                .Add("--settings")
                .Add("coverlet.runsettings.xml"));

        await cmd.ExecuteAsync();
    }

    private static async Task CreateCodeCoverageHTMLReport()
    {
        string coberturaPath = GetCoverageCoberturaPath();

        var cmd = Cli.Wrap("reportgenerator")
            .WithArguments(args => args
                .Add($"-reports:\"{coberturaPath}\"")
                .Add("-targetdir:\"codecoverage\"")
                .Add("-reporttypes:Html"));

        await cmd.ExecuteAsync();
    }

    private static string GetRelativePath(CreateReportOptions options)
    {
        var absolutePath = Directory.GetFiles(ScriptData.Path, options.TestProjectName, SearchOption.AllDirectories).First();
        return $".\\{Path.GetRelativePath(ScriptData.Path, absolutePath)}";
    }

    private static string GetCoverageCoberturaPath()
    {
        string cobertura = "coverage.cobertura.xml";
        var absolutePath = Directory.GetFiles($"{ScriptData.Path}\\TestResults", cobertura, SearchOption.AllDirectories).Single();
        return $".\\{Path.GetRelativePath(ScriptData.Path, absolutePath)}";
    }
}