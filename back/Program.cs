var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddTransient<ISaveUserService, SimpleSaveUserService>();

builder.Services.AddCors(options => options.AddPolicy
    (
        "ShopApiPolicy", b => b
        .WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
    )
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("ShopApiPolicy");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();