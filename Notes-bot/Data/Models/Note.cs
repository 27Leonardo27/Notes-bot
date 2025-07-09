namespace Notes_bot.Models;

public class Note
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string? Text { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime? NotifTime { get; set; }

    public long UserId { get; set; }
    public User User { get; set; }
}