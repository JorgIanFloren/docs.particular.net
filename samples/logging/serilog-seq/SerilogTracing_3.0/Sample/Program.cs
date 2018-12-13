﻿using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Serilog;
using NServiceBus.Serilog.Tracing;
using Serilog;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Logging.SerilogTracing";
        #region ConfigureSerilog
        var tracingLog = new LoggerConfiguration()
            .WriteTo.Seq("http://localhost:5341")
            .MinimumLevel.Information()
            .CreateLogger();
        var serilogFactory = LogManager.Use<SerilogFactory>();
        serilogFactory.WithLogger(tracingLog);
        #endregion

        #region UseConfig

        var endpointConfiguration = new EndpointConfiguration("Samples.Logging.SerilogTracing");
        endpointConfiguration.EnableFeature<TracingLog>();
        endpointConfiguration.SerilogTracingTarget(tracingLog);

        #endregion

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        var createUser = new CreateUser
        {
            UserName = "jsmith",
            FamilyName = "Smith",
            GivenNames = "John",
        };
        await endpointInstance.SendLocal(createUser)
            .ConfigureAwait(false);
        Console.WriteLine("Message sent");
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        #region Cleanup
        await endpointInstance.Stop()
            .ConfigureAwait(false);
        Log.CloseAndFlush();
        #endregion
    }
}