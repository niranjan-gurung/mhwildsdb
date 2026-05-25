namespace mhwildsdb.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ISkillService, SkillService>();
        services.AddScoped<IArmourService, ArmourService>();
        services.AddScoped<IArmourSetService, ArmourSetService>();
        services.AddScoped<ICharmService, CharmService>();
        
        // TODO
        //services.AddScoped<IDecorationService, DecorationService>();

        return services;
    }
}
