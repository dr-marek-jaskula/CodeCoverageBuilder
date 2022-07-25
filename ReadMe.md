## Overview

This tool automates the process of creating the code coverage reports, both machine readable and human readable.
Tool can be used for c# test projects that use xUnit.

- Json file *coverage* will be generated. Human unreadable. 
- TestResults folder with xml file *coverage.cobertura*. Human unreadable.
- codecoverage folder with *index.html* file that provides human readable code coverage report.

For more report file formats, file excludes and other settings visit: [coverlet.runsettings](https://github.com/coverlet-coverage/coverlet/blob/master/Documentation/VSTestIntegration.md).

## Prerequisites

coverlet.console tool
> dotnet tool install -g coverlet.console

reportgenerator tool
> dotnet tool install -g dotnet-reportgenerator-globaltool

## How to change command name?

- Change the content of the \<ToolCommandName\>ccb\</ToolCommandName\> in the .csproj file.

## How to install a global tool?

- Open PowerShell
- Navigate to the directory where the script is
> dotnet pack
> dotnet tool install --global --add-source ./NuGetPackage CodeCoverageBuilder

## How to use the ccb?

- Open PowerShell
- Navigate to the test project 
> ccb create --name \<TestProjectName>.dll --exclude <CSV_format_namespaces>

For instance,
> ccb create --name Users.Api.Tests.Unit.dll --exclude Users.Api.Repositories,Users.Api.Logging

## How to uninstall a global tool?

> dotnet tool uninstall codecoveragebuilder --global