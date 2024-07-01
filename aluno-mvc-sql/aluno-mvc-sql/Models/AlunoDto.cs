using System.ComponentModel.DataAnnotations;

namespace aluno_mvc_sql.Models
{
    public class AlunoDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = "";

        [Required, MaxLength(100)]
        public string Matricula { get; set; } = "";

        [Required]
        public int Periodo { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
