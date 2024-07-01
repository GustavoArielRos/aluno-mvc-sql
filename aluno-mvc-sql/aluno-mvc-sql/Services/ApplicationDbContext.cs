using aluno_mvc_sql.Models;
using Microsoft.EntityFrameworkCore;

namespace aluno_mvc_sql.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) 
        { 
        
        }

        public DbSet<Aluno> Alunos { get;set; } 

    }
}
