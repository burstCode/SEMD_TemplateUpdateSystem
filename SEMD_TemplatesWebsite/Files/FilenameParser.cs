using TemplatesWebsite.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Globalization;

namespace TemplatesWebsite.Files
{
    /* Реализован парсер, который вытаскивает из наименования шаблона 
     * все метаданные: id, дата последнего обновления и текущая версия.
     */
    public static class FilenameParser
    {
        public static Template ParseTemplateFilename(string filename)
        {
            // Проверяем формат
            if (!filename.EndsWith(".xsl"))
            {
                throw new ArgumentException("Неверный формат файла!");
            }

            // Удаляем .xsl и разбиваем на части
            var fileName = Path.GetFileNameWithoutExtension(filename);
            var parts = fileName.Split('_');

            // Парсим-парсим-парсим
            if (
                parts.Length != 5 || 
                !int.TryParse(parts[0], out int id) ||
                !DateTime.TryParseExact(parts[2], "ddMMyyyy", 
                    CultureInfo.InvariantCulture, 
                    DateTimeStyles.None,
                    out DateTime lastUpdated))
            {
                throw new ArgumentException("Неверный формат имени файла!");
            }

            // Вуаля!
            return new Template
            {
                Template_Id = id,
                Template_Filename = filename,
                Template_LastUpdated = lastUpdated,
                Template_Version = parts[3] + "." + parts[4]
            };
        }
    }
}
