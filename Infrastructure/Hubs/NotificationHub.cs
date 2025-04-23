using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Hubs;

public class NotificationHub : Hub
{
    public Task RoomsUpdated(string room) =>
        Clients.All.SendAsync(HubEndpoints.RoomsUpdated, room);
}