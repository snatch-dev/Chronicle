### Chronicle
![42150754](https://user-images.githubusercontent.com/7096476/64911747-ef4be100-d725-11e9-98f3-43331714afa7.png)



Chronicle is simple **process manager/saga pattern** implementation for .NET Core that helps you manage long-living and distributed transactions.

|   | master  | develop  |
|---|--------|----------|
|AppVeyor|[![Build status](https://ci.appveyor.com/api/projects/status/rma8prlvhjtql7ct/branch/master?svg=true)](https://ci.appveyor.com/project/GooRiOn/chronicle/branch/master)|[![Build status](https://ci.appveyor.com/api/projects/status/rma8prlvhjtql7ct/branch/develop?svg=true)](https://ci.appveyor.com/project/GooRiOn/chronicle/branch/develop)|
|CodeCov|[![codecov](https://codecov.io/gh/chronicle-stack/Chronicle/branch/master/graph/badge.svg)](https://codecov.io/gh/chronicle-stack/Chronicle)|[![codecov](https://codecov.io/gh/chronicle-stack/Chronicle/branch/develop/graph/badge.svg)](https://codecov.io/gh/chronicle-stack/Chronicle)|

# Installation
Chronicle is available on [NuGet](https://www.nuget.org/packages/Chronicle_/)
### Package manager
```bash
Install-Package Chronicle_ -Version 3.2.1
```

### .NET CLI
```bash
dotnet add package Chronicle_ --version 3.2.1
```

# Getting started
In order to create and process a saga you need to go through a few steps:
1. Create a class that derives from either ``Saga`` or ``Saga<TData>``.
2. Inside your saga implementation, inherit from one or several ``ISagaStartAction<TMessage>`` and ``ISagaAction<TMessage>`` to implement ``HandleAsync()`` and ``CompensateAsync()`` methods for each message type. An initial step must be implemented as an ``ISagaStartAction<TMessage>``, while the rest can be ``ISagaAction<TMessage>``. It's worth mentioning that you can implement as many ``ISagaStartAction<TMessage>`` as you want. In this case, the first incoming message is going to initialize the saga and any subsequent ``ISagaStartAction<TMessage>`` or ``ISagaAction<TMessage>`` will only update the current saga state.
3. Register all your sagas in ``Startup.cs`` by calling ``services.AddChronicle()``. By default, ``AddChronicle()`` will use the ``InMemorySagaStateRepository`` and ``InMemorySagaLog`` for maintaining ``SagaState`` and for logging ``SagaLogData`` in the ``SagaLog``. The ``SagaLog`` maintains a historical record of which message handlers have been executed. Optionally, ``AddChronicle()`` accepts an ``Action<ChronicleBuilder>`` parameter which provides access to ``UseSagaStateRepository<ISagaStateRepository>()`` and ``UseSagaLog<ISagaLog>()`` for custom implementations of ``ISagaStateRepository`` and ``ISagaLog``. **If either method is called, then both methods need to be called**.
4. Inject ``ISagaCoordinator`` and invoke ``ProcessAsync()`` methods passing a message. The coordinator will take care of everything by looking for all implemented sagas that can handle a given message.
5. To complete a successful saga, call ``CompleteSaga()`` or ``CompleteSagaAsync()``. This will update the ``SagaState`` to Completed. To flag a saga which has failed or been rejected, call the ``Reject()`` or ``RejectAsync()`` methods to update the ``SagaState`` to Rejected. Doing so will utilize the ``SagaLog`` to call each message type's ``CompensateAsync()`` in the reverse order of their respective ``HandleAsync()`` method was called. Additionally, an unhanded exception thrown from a ``HandleAsync()`` method will cause ``Reject()`` to be called and begin the compensation.

Below is the very simple example of saga that completes once both messages (``Message1`` and ``Message2``) are received:

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
