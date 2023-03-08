using Microsoft.EntityFrameworkCore;
namespace Azure_Lab2.Models
{
    public class ApplicationContext:DbContext
    {
        public DbSet<Picture> Pictures { get; set; } = null!;

        public async Task AddImage(IFormFile image)
        {
            await this.Pictures.AddAsync(new Picture()
            {
                Name = image.FileName,
                Path = $"img/{image.FileName}"
            });

            this.SaveChanges();
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }
    }
}
