

var builder = WebApplication.CreateBuilder(args);

// add Services to the containerbuilder

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddControllers();
{
    builder.Services.AddSwaggerGen(c =>
    {
        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "JWT Authentication",
            Description = "Enter JWT Bearer token **_only_**",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer", // must be lower case
            BearerFormat = "JWT",
            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };
        c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {securityScheme, new string[] { }}
    });

        // add Basic Authentication
        var basicSecurityScheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "basic",
            Reference = new OpenApiReference { Id = "BasicAuth", Type = ReferenceType.SecurityScheme }
        };
        c.AddSecurityDefinition(basicSecurityScheme.Reference.Id, basicSecurityScheme);
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {basicSecurityScheme, new string[] { }}
    });
    });
}
builder.Services.AddCors();
builder.Services.AddIdentityService(builder.Configuration);
builder.Services.AddSignalR();

// Configure the http request pipeline 
var app = builder.Build();
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
if (env == "Development")
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BasicAuth v1"));

}
app.UseMiddleware<ExeptionMiddelware>();
app.UseHttpsRedirection();

app.UseRouting();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200"));
app.UseAuthentication();
app.UseAuthorization();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");
// app.MapFallbackToController("Index", "Fallback");

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
// var host = CreateHostBuilder(args).Build();
using var scope = app.Services.CreateScope();
var Services = scope.ServiceProvider;
try
{

    var context = Services.GetRequiredService<DataContext>();
    var userManager = Services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = Services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(userManager, roleManager);
    // await Seed.SeedUsers(context);

}
catch (Exception ex)
{
    var logger = Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An Error Accourd during Migration");
}
await app.RunAsync();