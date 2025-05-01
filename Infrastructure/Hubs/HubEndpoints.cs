namespace Infrastructure.Hubs;

public class HubEndpoints
{
    public const string CommentsHub = "/hubs/comments";
    public const string NotificationHub = "/notifications";
    public const string RoomsUpdated = nameof(RoomsUpdated);
}
