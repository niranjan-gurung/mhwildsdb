using FluentValidation;
using mhwildsdb.Exceptions.Handlers;
using mhwildsdb.Filters;
using mhwildsdb.Persistance;
using mhwildsdb.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting server..");
    
    var builder = WebApplication.CreateBuilder(args);

    // contains db connection string
    builder.Configuration.AddUserSecrets<Program>();

    builder.Services.AddSerilog((services, lc) => lc
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services));

    builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

    builder.Services.AddProblemDetails(options =>
    {
        options.CustomizeProblemDetails = ctx =>
        {
            ctx.ProblemDetails.Extensions["traceId"] = ctx.HttpContext.TraceIdentifier;
            ctx.ProblemDetails.Extensions["timestamp"] = DateTime.UtcNow;
            ctx.ProblemDetails.Instance = $"{ctx.HttpContext.Request.Method} {ctx.HttpContext.Request.Path}";
        };
    });

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();     // fallback

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();

    builder.Services.AddScoped(typeof(ValidateFilter<>));
    builder.Services.AddTransient<ISkillService, SkillService>();

    // register database context
    builder.Services.AddDbContext<MhwildsDbContext>(options => 
        options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

    var app = builder.Build();
    
    app.UseSerilogRequestLogging();
    app.UseExceptionHandler();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
        Log.Information("Using database: {Database}",
            builder.Configuration.GetConnectionString("Database"));
    }

    /* CORS setup 
     * add origins for python parser here...
     */

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch(HostAbortedException)
{
    // thrown by ef tools during migrations
}
catch (Exception ex)
{
    Log.Fatal(ex, "Server terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}

