using aluno_mvc_sql.Models;
using aluno_mvc_sql.Services;
using Microsoft.AspNetCore.Mvc;

namespace aluno_mvc_sql.Controllers
{
    public class AlunoController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AlunoController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public IActionResult Index(string PesquisaTexto = "")
        {
            List<Aluno> alunos;

            if(PesquisaTexto != "" && PesquisaTexto != null)
            {
                alunos = _context.Alunos.Where(a => a.Name.Contains(PesquisaTexto)).ToList();
            }
            else
            {
                alunos = _context.Alunos.ToList();
            }
            
            return View(alunos);
        }

        public IActionResult Criar()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Criar(AlunoDto a)
        {
            if(a.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "O arquivo de imagem é necessário");
            }

            if(!ModelState.IsValid)
            {
                return View(a);
            }

            string novoNomeArquivo = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            novoNomeArquivo += Path.GetExtension(a.ImageFile!.FileName);

            string CaminhoCompletoImagem = _environment.WebRootPath + "/alunos/" + novoNomeArquivo;

            using(var stream = System.IO.File.Create(CaminhoCompletoImagem))
            {
                a.ImageFile.CopyTo(stream);
            }

            Aluno aluno = new Aluno()
            {
                Name = a.Name,
                Matricula = a.Matricula,
                Periodo = a.Periodo,
                ImageFileName = novoNomeArquivo,
                CreatedAt = DateTime.Now,
            };

            _context.Alunos.Add(aluno);
            _context.SaveChanges();

            return RedirectToAction("Index","Aluno");

        }

       

        public IActionResult Editar(int id)
        {
            var aluno = _context.Alunos.Find(id);

            if(aluno == null)
            {
                return RedirectToAction("Index", "Aluno");
            }

            var alunoDto = new AlunoDto()
            {
                Name = aluno.Name,
                Matricula = aluno.Matricula,
                Periodo = aluno.Periodo,
            };

            ViewData["AlunoId"] = aluno.Id;
            ViewData["ImageFileName"] = aluno.ImageFileName;
            ViewData["CreatedAt"] = aluno.CreatedAt.ToString("MM/dd/yyyy");

            return View(alunoDto);
        }

        [HttpPost]
        public IActionResult Editar(int id, AlunoDto aDto)
        {

            var aluno = _context.Alunos.Find(id);

            if(aluno == null)
            {
                return RedirectToAction("Index", "Aluno");
            }

            if(!ModelState.IsValid)
            {
                ViewData["AlunoId"] = aluno.Id;
                ViewData["ImageFileName"] = aluno.ImageFileName;
                ViewData["CreatedAt"] = aluno.CreatedAt.ToString("MM/dd/yyyy");

                return View(aDto);
            }

            string novoArquivoNome = aluno.ImageFileName;

            if(aDto.ImageFile != null)
            {
                novoArquivoNome = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                novoArquivoNome += Path.GetExtension(aDto.ImageFile.FileName);

                string CaminhoCompletoImagem = _environment.WebRootPath + "/alunos/" + novoArquivoNome;

                using(var stream = System.IO.File.Create(CaminhoCompletoImagem))
                {
                    aDto.ImageFile.CopyTo(stream);
                }

                string CaminhoCompletoImagemAntiga = _environment.WebRootPath + "/alunos" + aluno.ImageFileName;

                System.IO.File.Delete(CaminhoCompletoImagemAntiga);
            }

            aluno.Name = aDto.Name;
            aluno.Matricula = aDto.Matricula;
            aluno.Periodo = aDto.Periodo;
            aluno.ImageFileName = novoArquivoNome;

            _context.SaveChanges();

            return RedirectToAction("Index", "Aluno");

        }

        public IActionResult Deletar(int id)
        {
            var aluno = _context.Alunos.Find(id);

            if(aluno == null)
            {
                return RedirectToAction("Index", "Aluno");
            }
            else
            {
                //deletando imagem
                string CaminhoCompletoImagem = _environment.WebRootPath + "/alunos/ " + aluno.ImageFileName;
                System.IO.File.Delete(CaminhoCompletoImagem);

                _context.Alunos.Remove(aluno);

                _context.SaveChanges();

                return RedirectToAction("Index", "Aluno");
            }
        }

    }
}
