namespace Notes_bot.Models;

public class User
{
    public long Id { get; set; }
    public string? Username { get; set; }
    public ICollection<Note> Notes { get; set; }
}