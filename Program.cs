using Kojg_Ragnarock_Guide.Interfaces;
using Kojg_Ragnarock_Guide.Models;
using Kojg_Ragnarock_Guide.Services;
using RagnarockTourGuide.Interfaces;
using RagnarockTourGuide.Models;
using RagnarockTourGuide.Services;

namespace RagnarockTourGuide
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddSession();

            builder.Services.AddTransient<IFIleRepository<IFormFile>, FileRepository>();
            builder.Services.AddTransient<IQuizCRUDRepository<Quiz>, QuizCRUDRepository>();
            builder.Services.AddTransient<IExhibitionCRUDRepoistory<Exhibition>, ExhibitionCRUDRepository>();
            builder.Services.AddTransient<IUserCRUDRepository<User>, UserCRUDRepository>();
            builder.Services.AddTransient<IUserValidator, UserValidator>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSession(); // Brug session middleware
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
