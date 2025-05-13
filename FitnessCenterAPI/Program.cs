using FitnessCenterApi.Data;
using FitnessCenterApi.Models;
using FitnessCenterApi.Repositories;
using FitnessCenterApi.Repositories.UserRepositories;
using FitnessCenterApi.Services;
using FitnessCenterApi.Services.UserServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
 
// Add services to the container.
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<AttendanceService>();
builder.Services.AddScoped<AttendanceRepository>();
builder.Services.AddScoped<FitnessCenterRepository>();
builder.Services.AddScoped<FitnessCenterService>();
builder.Services.AddScoped<CoachService>();
builder.Services.AddScoped<CoachRepository>();
builder.Services.AddScoped<MembershipService>();
builder.Services.AddScoped<MembershipRepository>();

builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<MessageRepository>();



 
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Your API Title",
        Version = "v1"
    });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme.  
                        Enter 'Bearer' [space] and then your token in the text input below.  
                        Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});
builder.Services.AddDbContext<FitnessCenterDbContext>(options =>
    options.UseSqlite("Data Source=fitnessCenterDB.sqlite"));


builder.Services.AddIdentity<User, IdentityRole<int>>(optons =>
    {
        optons.Password.RequireDigit = true;
        optons.Password.RequireLowercase = true;
        optons.Password.RequireUppercase = true;
        optons.Password.RequireNonAlphanumeric = true;
        optons.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<FitnessCenterDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        options.DefaultChallengeScheme =
            options.DefaultForbidScheme =
                options.DefaultScheme =
                    options.DefaultSignInScheme =
                        options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
            )
        };
    }
);

builder.Services.AddAutoMapper(typeof(Program));




var app = builder.Build();
 
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fitness Center API v1");
    });
    
}



 
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
 
 
app.MapControllers();
 
app.Run();