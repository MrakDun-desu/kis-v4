using System.Data;
using KisV4.BL.EF;
using KisV4.DAL.EF;
using KisV4.App.Endpoints;

var builder = WebApplication.CreateBuilder(args);

ConfigureCors(builder.Services);
ConfigureOpenApiDocuments(builder.Services);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (connectionString is null) {
    throw new NoNullAllowedException("Database connection string is null");
}

builder.Services.AddEntityFrameworkDAL(connectionString);
builder.Services.AddEntityFrameworkBL();

var app = builder.Build();

app.UseCors();
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();
UseDevelopmentSettings(app);
app.UseRouting();
UseEndpoints(app);

app.Run();

return;

void ConfigureCors(IServiceCollection serviceCollection) {
    serviceCollection.AddCors(opts => {
        opts.AddDefaultPolicy(policy =>
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
    });
}

void ConfigureOpenApiDocuments(IServiceCollection serviceCollection) {
    serviceCollection.AddEndpointsApiExplorer();
    serviceCollection.AddSwaggerGen();
}

void UseDevelopmentSettings(WebApplication application) {
    var environment = application.Services.GetRequiredService<IWebHostEnvironment>();

    if (environment.IsDevelopment()) {
        application.UseDeveloperExceptionPage();
    }
}

void UseEndpoints(IEndpointRouteBuilder routeBuilder) {
    Cashboxes.MapEndpoints(routeBuilder);
}
