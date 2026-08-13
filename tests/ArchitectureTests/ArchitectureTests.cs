using System.Reflection;
using NetArchTest.Rules;

namespace ExchangeTracing.ArchitectureTests;

/// <summary>
/// Enforces the module/layer boundaries described in CLAUDE.md and docs/architecture.md.
/// Project references already block wrong references at compile time; these tests also
/// catch code-level coupling (e.g. a Domain type using an Application/Infrastructure type)
/// and give a clear failure message. Rules covering still-empty layers pass today and gain
/// teeth as entities/handlers are added.
/// </summary>
public class ArchitectureTests
{
    private static readonly string[] Modules = ["Users", "Assets", "Transactions", "Portfolio"];
    private static readonly string[] Layers = ["Domain", "Application", "Infrastructure", "Presentation"];

    private const string Root = "ExchangeTracing.Modules";

    private static Assembly Load(string module, string layer)
        => Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, $"{Root}.{module}.{layer}.dll"));

    private static string Describe(string context, TestResult result)
        => $"{context} — violating types: {string.Join(", ", result.FailingTypeNames ?? [])}";

    [Fact]
    public void Domain_should_not_depend_on_other_layers()
    {
        foreach (var m in Modules)
        {
            var result = Types.InAssembly(Load(m, "Domain"))
                .ShouldNot()
                .HaveDependencyOnAny(
                    $"{Root}.{m}.Application",
                    $"{Root}.{m}.Infrastructure",
                    $"{Root}.{m}.Presentation")
                .GetResult();

            Assert.True(result.IsSuccessful, Describe($"{m}.Domain", result));
        }
    }

    [Fact]
    public void Domain_should_not_depend_on_infrastructure_frameworks()
    {
        foreach (var m in Modules)
        {
            var result = Types.InAssembly(Load(m, "Domain"))
                .ShouldNot()
                .HaveDependencyOnAny("Microsoft.EntityFrameworkCore", "Npgsql")
                .GetResult();

            Assert.True(result.IsSuccessful, Describe($"{m}.Domain", result));
        }
    }

    [Fact]
    public void Application_should_not_depend_on_infrastructure_or_presentation()
    {
        foreach (var m in Modules)
        {
            var result = Types.InAssembly(Load(m, "Application"))
                .ShouldNot()
                .HaveDependencyOnAny(
                    $"{Root}.{m}.Infrastructure",
                    $"{Root}.{m}.Presentation")
                .GetResult();

            Assert.True(result.IsSuccessful, Describe($"{m}.Application", result));
        }
    }

    [Fact]
    public void Presentation_should_not_depend_on_infrastructure()
    {
        foreach (var m in Modules)
        {
            var result = Types.InAssembly(Load(m, "Presentation"))
                .ShouldNot()
                .HaveDependencyOn($"{Root}.{m}.Infrastructure")
                .GetResult();

            Assert.True(result.IsSuccessful, Describe($"{m}.Presentation", result));
        }
    }

    [Fact]
    public void Modules_should_not_depend_on_each_other()
    {
        foreach (var module in Modules)
        {
            var forbidden = Modules
                .Where(other => other != module)
                .Select(other => $"{Root}.{other}")
                .ToArray();

            foreach (var layer in Layers)
            {
                var result = Types.InAssembly(Load(module, layer))
                    .ShouldNot()
                    .HaveDependencyOnAny(forbidden)
                    .GetResult();

                Assert.True(result.IsSuccessful, Describe($"{module}.{layer}", result));
            }
        }
    }
}
