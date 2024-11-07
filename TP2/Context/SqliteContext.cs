using Microsoft.EntityFrameworkCore;
using TP2.Models;

public class SqliteContext : DbContext
{
    public DbSet<Customer> customers { get; set; }
    public DbSet<Movie> movies { get; set; }
    public DbSet<Genre> genres { get; set; }

    public SqliteContext(DbContextOptions<SqliteContext> options) : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSqlite("Data Source=tp2.db");
    }

}

