using Microsoft.EntityFrameworkCore;
using Notes_bot.Models;

namespace Notes_bot.Services;

public class NoteService
{
    public static void CreateNote(long userId, string title, string userName)
    {
        using var dbContext = new NotesDbContext();
        
        if (!dbContext.Users.Any(u => u.Id == userId))
        {
            var user = new User()
            {
                Id = userId,
                Username = userName
            };
            
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }

        var note = new Note()
        {
            UserId = userId,
            Title = title,
            CreationTime = DateTime.UtcNow,
        };
        
        dbContext.Notes.Add(note);
        dbContext.SaveChanges();
    }

    public static List<Note> GetNotes(long userId)
    {
        using var dbContext = new NotesDbContext();

        var notes = dbContext.Notes
            .AsNoTracking()    
            .Where(n => n.UserId == userId)
            .ToList();
        
        return notes;
    }
}