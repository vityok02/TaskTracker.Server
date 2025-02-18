namespace Domain.Entities;

public class ResetToken : BaseEntity
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
    public Guid UserId { get; set; }
}
