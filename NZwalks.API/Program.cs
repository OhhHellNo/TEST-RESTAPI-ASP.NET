using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NZwalks.API.Data;
using NZwalks.API.Mappings;
using NZwalks.API.Repository;
using Scalar.AspNetCore;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONTROLLERS & OPENAPI (WITH AUTH FEATURE) ---
builder.Services.AddControllers();

// Configures OpenAPI/Scalar to show the "Authorize" button
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        var scheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Name = "Authorization",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Enter your JWT token: Bearer {your_token}"
        };

        document.Components ??= new Microsoft.OpenApi.Models.OpenApiComponents();
        document.Components.SecuritySchemes.Add("Bearer", scheme);

        document.SecurityRequirements.Add(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            [new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }] = Array.Empty<string>()
        });

        return Task.CompletedTask;
    });
});

// --- 2. DATABASE CONTEXTS ---
builder.Services.AddDbContext<NZwalksDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZwalksConnectionString")));

builder.Services.AddDbContext<NZwalksAuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZwalksAuthConnectionString")));

// --- 3. REPOSITORIES & MAPPING ---
builder.Services.AddScoped<IRegionRepository, SQLRepository>();
builder.Services.AddScoped<IWalkRepository, SqlWalkRepository>();
builder.Services.AddScoped<ItokenRepository, TokenRepository>();

// Fixes Ambiguity: Explicitly uses the assembly to find AutoMapperProfiles
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

// --- 4. IDENTITY SETUP ---
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")
    .AddEntityFrameworkStores<NZwalksAuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

// --- 5. AUTHENTICATION SETUP ---
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

            // Crucial for [Authorize(Roles = "Reader")] to work
            RoleClaimType = ClaimTypes.Role,
            NameClaimType = ClaimTypes.Email
        };
    });

var app = builder.Build();

// --- 6. HTTP PIPELINE ---
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Configures Scalar with the "Bearer" scheme as preferred
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("NZ Walks API")
               .WithPreferredScheme("Bearer");
    });
}

app.UseHttpsRedirection();

// Order matters: Authentication must come BEFORE Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();