using Microsoft.EntityFrameworkCore;
using Notes_bot.Models;

namespace Notes_bot;

public class NotesDbContext : DbContext
{
    public DbSet<Note> Notes { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=192.168.1.86;Port=5432;Database=NotesDB;User Id=leonardo;Password=Pass1488;");
    }
}