    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using PRJ_MKS_BTT.Service;
    using System.Text;


    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();


    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddDbContext<PRJ_MKS_BTT.Data.ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddScoped<PRJ_MKS_BTT.IRepository.IUserRepository, PRJ_MKS_BTT.Repository.UserRepository>();
    builder.Services.AddScoped<PRJ_MKS_BTT.IService.IUserService, PRJ_MKS_BTT.Service.UserService>();
    builder.Services.AddScoped<PRJ_MKS_BTT.IService.IEmailService, PRJ_MKS_BTT.Service.EmailService>();
builder.Services.AddScoped<PRJ_MKS_BTT.IRepository.ICategoryRepository, PRJ_MKS_BTT.Repository.CategoryRepository>();
builder.Services.AddScoped<PRJ_MKS_BTT.IService.ICategoryService, PRJ_MKS_BTT.Service.CategoryService>();

builder.Services.AddScoped<PRJ_MKS_BTT.IService.IProductService,PRJ_MKS_BTT.Service.ProductService>();
builder.Services.AddScoped<PRJ_MKS_BTT.IRepository.IProductRepository, PRJ_MKS_BTT.Repository.ProductRepository>();
builder.Services.AddScoped<PRJ_MKS_BTT.IRepository.IUnitOfWork, PRJ_MKS_BTT.Repository.EfUnitOfWork>();


// JWT Authentication


var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var secretKey = jwtSettings["SecretKey"];

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

    // CORS
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
Console.WriteLine("DB = " + builder.Configuration.GetConnectionString("DefaultConnection"));


var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }


    app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
  


    app.MapControllers();

    app.Run();
