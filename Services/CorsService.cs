namespace api_fit.Services
{
    public static class CorsService
    {
        public static IServiceCollection Cors(this IServiceCollection services)
        {
            services.AddCors(a => a
                .AddPolicy("AllowFrontend", cors => cors
                .WithOrigins("https://-hmg-9712aeff20c4.herokuapp.com",
                             "https://localhost:7265",
                             "http://localhost:3030",
                             "http://localhost:8081")
                .AllowAnyHeader()
                .AllowAnyMethod()));

            return services;
        }
    }
}
