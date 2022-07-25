using CommandLine;

namespace CodeCoverageBuilder;

[Verb("create", HelpText = "Creates a code coverage HTML report.")]
public class CreateReportOptions
{
    private string _projectName = string.Empty;
    private string _excludeNamespaces = string.Empty;

    [Option('n', "name", Required = true, HelpText = "Test project name.")]
    public string TestProjectName 
    {
        get => _projectName; 
        set => _projectName = !value.Contains(".dll") ? $"{value}.dll" : value; 
    }

    [Option('e', "exclude", Required = false, Default = "", HelpText = "CSV format: namespaces that will be excluded from calculating the project's code coverage.")]
    public string ExcludeNamespaces 
    {
        get => _excludeNamespaces;
        set => _excludeNamespaces = string.Join(',', value.Split(',').Select(x => $"[*]{x}*"));
    }
}

[Verb("version", Hidden = true)]
public class VersionOptions
{
}