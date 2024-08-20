using TemplatesAPI.Data;
using TemplatesWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                Id = t.Id,
                TemplateFilename = t.TemplateFilename,
                LastUpdated = t.LastUpdated,
                Version = t.Version
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
                Id = template.Id,
                TemplateFilename = template.TemplateFilename,
                LastUpdated = template.LastUpdated,
                Version = template.Version
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

            return File(template.Content, "application/octet-stream", template.TemplateFilename);
        }
    }
}
