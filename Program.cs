using MyTasks.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using MyTasks.Models;
var builder = WebApplication.CreateBuilder(args);


//authentication

builder.Services.AddAuthentication(options => { options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
.AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.TokenValidationParameters = TasksTokenService.GetTokenValidationParameters();
});



builder.Services.AddAuthorization(authorizationOptions =>
{
    authorizationOptions.AddPolicy("Admin", policy => policy.RequireClaim("type", "Admin"));
    authorizationOptions.AddPolicy("User", policy => policy.RequireClaim("type", "User","Admin"));
});
// Add services to the container.


builder.Services.AddControllers();
builder.Services.AddTaskManagement();
builder.Services.AddTask();
builder.Services.AddPasswordHasher();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TASKS", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Pleas enter jwt with Bearer info field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme,Id = "Bearer"}
            },
            new string[] {}
        }
        });
});

var app = builder.Build();

//app.UselogMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "html/index.html" }
});
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
