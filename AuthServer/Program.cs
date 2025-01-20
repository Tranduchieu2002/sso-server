using AuthServer.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAppMiddleware();

app.Run();