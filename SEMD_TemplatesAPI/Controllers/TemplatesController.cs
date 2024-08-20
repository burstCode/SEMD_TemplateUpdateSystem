using TemplatesAPI.Data;
using TemplatesWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TemplatesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplatesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TemplatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Templates
        // Возвращает список шаблонов
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Template>>> GetTemplates()
        {
            return await _context.Templates.Select(t => new Template
            {
                Template_Id = t.Template_Id,
                Template_Filename = t.Template_Filename,
                Template_LastUpdated = t.Template_LastUpdated,
                Template_Version = t.Template_Version
            }).ToListAsync();
        }

        // GET: api/Templates/{id}
        // Возвращает информацию о конкретном шаблоне
        [HttpGet("{id}")]
        public async Task<ActionResult<Template>> GetTemplate(int id)
        {
            var template = await _context.Templates.FindAsync(id);

            if (template == null)
            {
                return NotFound();
            }

            return new Template
            {
                Template_Id = template.Template_Id,
                Template_Filename = template.Template_Filename,
                Template_LastUpdated = template.Template_LastUpdated,
                Template_Version = template.Template_Version
            };
        }

        // GET: api/Templates/Download/{id}
        // Возвращает файл шаблона
        [HttpGet("Download/{id}")]
        public async Task<ActionResult<byte[]>> DownloadTemplate(int id)
        {
            var template = await _context.Templates.FindAsync(id);
            if (template == null)
            {
                return NotFound();
            }

            return File(template.Template_Content, "application/octet-stream", template.Template_Filename);
        }
    }
}
