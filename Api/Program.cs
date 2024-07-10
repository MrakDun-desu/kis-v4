using Api.DAL.EF;

var builder = WebApplication.CreateBuilder(args);

ConfigureCors(builder.Services);
ConfigureOpenApiDocuments(builder.Services);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddEntityFrameworkDAL(connectionString!);

var app = builder.Build();

app.UseCors();
app.UseHttpsRedirection();
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();
UseDevelopmentSettings(app);
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
    routeBuilder.MapGet("hello-world", () => "Hello, world!");
}
