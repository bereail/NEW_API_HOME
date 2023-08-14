using Microsoft.EntityFrameworkCore;
using toner_API.Models;

// Crear un nombre para la pol�tica de CORS
string policy = "MyPolicy";

// Crear el constructor de la aplicaci�n web
var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor de inyecci�n de dependencias
builder.Services.AddDbContext<tonerStoreContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar CORS
builder.Services.AddCors(options => {
    options.AddPolicy(name: policy, build =>
    {
        // Permitir solicitudes solo desde "localhost"
        build.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
            .AllowAnyHeader() // Permitir cualquier encabezado
            .AllowAnyMethod(); // Permitir cualquier m�todo HTTP
    });
});

// Agregar controladores al servicio
builder.Services.AddControllers();

// Agregar Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Construir la aplicaci�n
var app = builder.Build();

// Usar la pol�tica de CORS configurada
app.UseCors(policy);

// Configurar el flujo de solicitud HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Habilitar Swagger en entorno de desarrollo
    app.UseSwaggerUI(); // Habilitar la interfaz gr�fica de Swagger en entorno de desarrollo
}

app.UseHttpsRedirection(); // Redireccionar todas las solicitudes HTTP a HTTPS

app.UseAuthorization(); // Habilitar la autorizaci�n

app.MapControllers(); // Mapear las rutas de los controladores

app.Run(); // Iniciar la aplicaci�n
