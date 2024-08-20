using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TemplatesWebsite.Data;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using TemplatesWebsite.Models;
using TemplatesWebsite.Files;

namespace TemplatesWebsite.Controllers
{
    [Authorize]
    public class TemplatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TemplatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Отображает страницу для управления шаблонами
        public async Task<IActionResult> Manage()
        {
            var templates = await _context.Templates.ToListAsync();
            return View(templates);
        }

        /*
         * Загрузка нового шаблона.
         * Если в базе данных находится шаблон с таким же id, но
         * с более старой датой обновления, то он ЗАМЕНЯЕТСЯ новым.
         * В случае, если шаблона с таким id нет, то просто ДОБАВЛЯЕТСЯ.
         */
        [HttpPost]
        public async Task<IActionResult> LoadTemplate(IFormFile file)
        {
            if (file == null)
            {
                TempData["ErrorMessage"] = "Файл не выбран!";
                return RedirectToAction(nameof(Manage));
            }

            if (file.Length == 0)
            {
                TempData["ErrorMessage"] = "Загружен пустой файл!";
                return RedirectToAction(nameof(Manage));
            }

            try
            {
                // Парсим название загруженного шаблона
                var newTemplate = FilenameParser.ParseTemplateFilename(file.FileName);

                // Читаем шаблон 
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    newTemplate.Content = memoryStream.ToArray();
                }

                // Пытаемся найти шаблон с таким же id, как у загруженного
                var oldTemplate = await _context.Templates.FindAsync(newTemplate.Id);

                // Если такого шаблона еще нет, то добавляем в качестве нового
                if (oldTemplate == null)
                {
                    _context.Templates.Add(newTemplate);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Новый шаблон успешно добавлен!";
                }
                // Но если такой шаблон уже существует, то проверяем дату последнего обновления
                else
                {
                    if (oldTemplate.LastUpdated < newTemplate.LastUpdated)
                    {
                        oldTemplate.TemplateFilename = file.FileName;
                        oldTemplate.LastUpdated = newTemplate.LastUpdated;
                        oldTemplate.Version = newTemplate.Version;
                        oldTemplate.Content = newTemplate.Content;

                        _context.Templates.Update(oldTemplate);
                        await _context.SaveChangesAsync();

                        TempData["SuccessMessage"] = "Существующий шаблон успешно обновлен!";
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Загруженный шаблон уже имеет актуальную версию!";
                    }
                }
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            // Обновляем страницу
            return RedirectToAction(nameof(Manage));
        }
    }
}

