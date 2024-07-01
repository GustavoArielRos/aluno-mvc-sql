using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;//precisaria se eu tivesse usado o "precision"

namespace aluno_mvc_sql.Models
{
    public class Aluno
    {   

        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = "";

        [MaxLength(100)]
        public string Matricula { get; set; } = "";

        public int Periodo { get; set; }

        [MaxLength(100)]
        public string ImageFileName { get; set; } = "";

        public DateTime CreatedAt { get; set; }

    }
}
