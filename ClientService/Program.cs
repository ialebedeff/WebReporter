using CommunicationMessageHandler;
using CommunicationService;
using Entities;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

SignalClient client = new SignalClient("https://localhost:7187/proxy", connection => {

    var handler = new ClientHandler(connection);

    connection.On<Message<DatabaseCommand>>("ExecuteDatabaseCommand", async message =>
    {
        Console.WriteLine(message.Data.Command);
        Console.WriteLine();

        await handler.Database.ExecuteAsync(message);
    });
});

await client.StartAsync();

Console.ReadLine();