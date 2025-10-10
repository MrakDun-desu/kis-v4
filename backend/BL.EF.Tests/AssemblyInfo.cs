// disables all parallel test runs in assembly to prevent inconsistent test results
[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly, DisableTestParallelization = true)]
namespace BL.EF.Tests;
