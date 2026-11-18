using Microsoft.EntityFrameworkCore;
using WebApi3.Model;

namespace WebApi3.Data;

public class AppDbContext: DbContext
{
    public DbSet<Blog> Blogs { get; set; }//reconoce la tabla blog de la base de datos

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
            .Property(b=>b.Url)
            .HasMaxLength(500);//la longitud del db context tiene prioridad sobre data anotation, la clase
    }
}