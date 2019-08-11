Chronicle is simple **process manager/saga pattern** implementation for .NET Core that helps you manage long-living and disitrbuted transactions.

|   | master  | develop  |
|---|--------|----------|
|AppVeyor|[![Build status](https://ci.appveyor.com/api/projects/status/rma8prlvhjtql7ct/branch/master?svg=true)](https://ci.appveyor.com/project/GooRiOn/chronicle/branch/master)|[![Build status](https://ci.appveyor.com/api/projects/status/rma8prlvhjtql7ct/branch/develop?svg=true)](https://ci.appveyor.com/project/GooRiOn/chronicle/branch/develop)|
|CodeCov|[![codecov](https://codecov.io/gh/chronicle-stack/Chronicle/branch/master/graph/badge.svg)](https://codecov.io/gh/chronicle-stack/Chronicle)|[![codecov](https://codecov.io/gh/chronicle-stack/Chronicle/branch/develop/graph/badge.svg)](https://codecov.io/gh/chronicle-stack/Chronicle)|

# Installation
Chornicle is available on [NuGet](https://www.nuget.org/packages/Chronicle_/)
### Package manager
```bash
Install-Package Chronicle_ -Version 2.0.1
```

### .NET CLI
```bash
dotnet add package Chronicle_ --version 2.0.1
```

# Getting started
In order to create and process saga you need to go through few steps:
1. Create a class that dervies either from ``Saga`` or ``Saga<TData>``.
2. Inside saga implement particular steps that needs to be done or compensated in case of error. The initial step must be implemented as ``ISagaStartAction<TMessage>`` while the rest ``ISagaAction<TMessage>``. It's worth mentioning that up can implement as many start actions as you want. In this case first incomming message is going to initialize saga.
3. Register all your sagas in ``Startup.cs`` by calling ``services.AddChronicle()``.
4. Inject ``ISagaCoordinator`` and inoke ``ProcessAsync()`` methods passing a message. Cooridnator will take care of everything by looking for all implemented sagas that can handle given message.

Bellow is the very simple example of saga that completes once both messages (``Message1`` and ``Message2``) are received:

```csharp
public class Message1
{
    public string Text { get; set; }
}

public class Message2
{
    public string Text { get; set; }
}

public class SagaData
{
    public bool IsMessage1Received { get; set; }
    public bool IsMessage2Received { get; set; }
}

public class SampleSaga : Saga<SagaData>, ISagaStartAction<Message1>, ISagaAction<Message2>
{
    public Task HandleAsync(Message1 message, ISagaContext context)
    {
        Data.IsMessage1Received = true;
        Console.WriteLine($"Received message1 with message: {message.Text}");
        CompleteSaga();
        return Task.CompletedTask;
    }
    
    public Task HandleAsync(Message2 message, ISagaContext context)
    {
        Data.IsMessage2Received = true;
        Console.WriteLine($"Received message2 with message: {message.Text}");
        CompleteSaga();
        return Task.CompletedTask;
    }

    public Task CompensateAsync(Message1 message, ISagaContext context)
        => Task.CompletedTask;

    public Task CompensateAsync(Message2 message, ISagaContext context)
        => Task.CompletedTask;

    private void CompleteSaga()
    {
        if(Data.IsMessage1Received && Data.IsMessage2Received)
        {
            Complete();
            Console.WriteLine("SAGA COMPLETED");
        }
    }
}

```

Both messages are processed by mentioned coordinator:

```csharp
var coordinator = app.ApplicationServices.GetService<ISagaCoordinator>();

var context = SagaContext
    .Create()
    .WithCorrelationId(Guid.NewGuid())
    .Build();

coordinator.ProcessAsync(new Message1 { Text = "Hello" }, context);
coordinator.ProcessAsync(new Message2 { Text = "World" }, context);
```

The result looks as follows:

![Result](https://user-images.githubusercontent.com/7096476/53180548-0c885900-35f6-11e9-864b-6b6d13641f2a.png)

# Documentation
If you're looking for documentation, you can find it [here](https://chronicle.readthedocs.io/en/latest/).

# Icon
Icon made by Smashicons from [www.flaticon.com](http://flaticon.com) is licensed by [Creative Commons BY 3.0](http://creativecommons.org/licenses/by/3.0/)
