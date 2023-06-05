using CodeMechanic.FileSystem;
using CodeMechanic.Types;
using Neo4j.Driver;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


static void ConfigureServices(IServiceCollection services)
{
    string uri = Environment.GetEnvironmentVariable("NEO4J_URI") ?? string.Empty;
    string user = Environment.GetEnvironmentVariable("NEO4J_USER") ?? string.Empty;
    string password = Environment.GetEnvironmentVariable("NEO4J_PASSWORD") ?? string.Empty;

    string airtable_bearer_token = Environment.GetEnvironmentVariable("TPOT_PAT") ?? string.Empty;

    bool devmode = Environment.GetEnvironmentVariable("DEVMODE").ToBoolean();

    services.AddControllers();
    // services.ConfigureAirtable();

    services.AddSingleton(GraphDatabase.Driver(
        uri
        , AuthTokens.Basic(
            user,
            password
        )
    ));

    // services.AddTransient<IAirtableRepo, AirtableRepo>();
}

ConfigureServices(builder.Services);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// app.UseExceptionHandler(config =>
// {
//     config.Run(async context =>
//     {
//         //TODO: Global Error handling:
//         // context.Response.StatusCode = 500;
//         // context.Response.ContentType = "application/json";

//         // var error = context.Features.Get<IExceptionHandlerFeature>();
//         // if (error != null)
//         // {
//         //     var ex = error.Error;

//         //     await context.Response.WriteAsync(new ErrorModel()
//         //     {
//         //     //  StatusCode = """<span class="enlighter-text">context.</span><span class="enlighter-m3">Response</span><span class="enlighter-text">.</span><span class="enlighter-m3">StatusCode</span>""",

//         //         ErrorMessage = ex.Message 
//         //     }.ToString()); //ToString() is overridden to Serialize object
//         // }
//     });
// });

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
