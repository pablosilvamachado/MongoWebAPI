using MongoDatabaseSettings = WebAPI.Infra.MongoDatabaseSettings;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.Configure<MongoDatabaseSettings>(
    builder.Configuration.GetSection("SettingsDatabase")
);

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddNewtonsoftJson();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();