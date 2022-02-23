using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Quantum.Simulation.Core;
using Microsoft.Quantum.Simulation.Simulators;
using QApp.Qsharp;

namespace QApp.Qsharp.Tests;

public class RandomNumberShould
{
    private readonly ITestOutputHelper _testOutputHelper;
    public RandomNumberShould(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void BeOfSpecifiedLength()
    {
        int specifiedLength = 4;

        using (var sim = new QuantumSimulator())
        {
            var result = SampleRandomNumberInRange.Run(sim, specifiedLength).Result;
            Assert.Equal(specifiedLength, result.Count);
        }
    }

    [Fact]
    public void DifferOnSubsequentRuns()
    {
        int specifiedLength = 8;

        using (var sim = new QuantumSimulator())
        {
            var result1 = SampleRandomNumberInRange.Run(sim, specifiedLength).Result;
            var result2 = SampleRandomNumberInRange.Run(sim, specifiedLength).Result;
            Assert.NotEqual(result1, result2);
        }
    }
}