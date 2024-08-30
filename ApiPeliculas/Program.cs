using ApiPeliculas.Data;
using ApiPeliculas.PeliculasMapper;
using ApiPeliculas.Repository;
using ApiPeliculas.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Repository
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<IPeliculaRepositorio, PeliculaRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(PeliculasMapper));

//Soporte para CORS
//Se puede habilitar: 1-Un dominio, 2-multiples dominios, 3-Todos los dominios
//Se usa (*) para permitir todos los dominios
builder.Services.AddCors(options =>
{
    options.AddPolicy("PoliticaCors",
        builder => {
            builder.WithOrigins("http://localhost:4200").WithMethods().AllowAnyHeader();
            }); 
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Soporte para CORS

app.UseCors("PoliticaCors");

app.UseAuthorization();

app.MapControllers();

app.Run();
