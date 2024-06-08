using Microsoft.EntityFrameworkCore;
using Serilog;
using TopupBeneficiaries.DBContext;
using TopupBeneficiaries.Repositories;
using TopupBeneficiaries.Model;
using TopupBeneficiaries.Services;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/TopupBeneficiaries.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();


var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITopUpRepository,TopUpRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddTransient<ITopupValidator, TopupValidatior>();
builder.Services.AddTransient<IUserFinanceService, UserFinanceService>();
builder.Services.AddSingleton<LimitsAndCharges>();
builder.Services.AddSingleton<ServiceUrlsConfig>();

builder.Services.AddDbContext<TopupBeneficiaryContext>(
    dbContextOptions => {dbContextOptions.UseSqlite(
        builder.Configuration["ConnectionStrings:TopupBeneficiariesConnection"]);
        dbContextOptions.EnableSensitiveDataLogging(true);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

