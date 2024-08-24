using GeniusSquareWeb.GameElements;

namespace GeniusSquareWeb.Server
{
    /// <summary>
    /// Class that bootstraps the game server.
    /// </summary>
    public static class ServerBootstrapper
    {

        public static WebApplication BootstrapServer(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // add SignalR
            builder.Services.AddSignalR();

            RegisterDefaultDices(builder.Services);
            builder.Services.AddSingleton<IGameManager, GameManager>();

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.MapHub<GameHub>("/gameHub");

            return app;
        }

        private static void RegisterDefaultDices(
            IServiceCollection serviceCollection)
        {
            foreach (IDice dice in DefaultDices.GetAllDefaultDices())
            {
                serviceCollection.AddSingleton<IDice>(dice);
            }
        }
    }
}
