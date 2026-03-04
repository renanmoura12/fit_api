namespace api_fit.Services
{
    public static class CorsService
    {
        public static IServiceCollection Cors(this IServiceCollection services)
        {
            services.AddCors(a => a
                .AddPolicy("AllowFrontend", cors => cors
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()));

            return services;
        }
    }
}
