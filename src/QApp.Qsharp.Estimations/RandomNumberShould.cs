using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Quantum.Simulation.Core;
using Microsoft.Quantum.Simulation.Simulators;
using QApp.Qsharp;

namespace QApp.Qsharp.Estimations;

public class RandomNumberShould
{
    private readonly ITestOutputHelper _testOutputHelper;
    public RandomNumberShould(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void UseMinimumQubitCount()
    {
        int specifiedLength = 3;

        ResourcesEstimator estimator = new ResourcesEstimator();
        SampleRandomNumberInRange.Run(estimator, specifiedLength).Wait();

        var data = estimator.Data;
        var qubitCount = 0;
        int.TryParse(data.Rows.Find("QubitCount")["Max"].ToString(), out qubitCount);
        Assert.Equal(specifiedLength, qubitCount);
    }
}