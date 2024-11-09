using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyCitiesInfo.API;
using MyCitiesInfo.API.DbContexts;
using MyCitiesInfo.API.Services;
using Serilog;
using System.Reflection;


Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("myLogs/myCitiesInfoesLogFile.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();



var builder = WebApplication.CreateBuilder(args);

//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers(
        options =>
        {
            options.ReturnHttpNotAcceptable = true;
        }
    ).AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();


builder.Services.AddProblemDetails();
//builder.Services.AddProblemDetails(options =>
//{
//    options.CustomizeProblemDetails = ctx =>
//    {
//        ctx.ProblemDetails.Extensions.Add("additionalInfo", "Additional Information Example");
//        ctx.ProblemDetails.Extensions.Add("serverName", Environment.MachineName);

//    };

//});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();



builder.Services.AddSingleton<FileExtensionContentTypeProvider>();



//Registering our custom service here
#if DEBUG
builder.Services.AddTransient<IMyMailService, MyLocalMailService>();
#else
builder.Services.AddTransient<IMyMailService, MyCloudMailService>();
#endif

builder.Services.AddSingleton<MyCitiesInfoesDataStore>();

builder.Services.AddDbContext<MyCitiesInfoContext>(
     dbContextOptions => 
        dbContextOptions.UseSqlite(
            builder.Configuration["ConnectionStrings:MyCitiesInfoesDBConnectionString"]
            ));


builder.Services.AddScoped<IMyCitiesInfoesRepository, MyCitiesInfoesRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Authentication:Issuer"],
                        ValidAudience = builder.Configuration["Authentication:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                                Convert.FromBase64String(builder.Configuration["Authentication:SecretForKey"]))

                    };
                });


//--Creating an authorization-policy:
//------------
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustBeFromManamaBH", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("city", "Manama,BH");

    });


});
//------------


builder.Services.AddApiVersioning(setupAction =>
{
    setupAction.ReportApiVersions = true;
    setupAction.AssumeDefaultVersionWhenUnspecified = true;
    setupAction.DefaultApiVersion = new ApiVersion(1, 0);

}).AddMvc()
.AddApiExplorer(setupAction =>
{
    setupAction.SubstituteApiVersionInUrl = true;


});

var apiVersionDescriptionProvider = builder.Services.BuildServiceProvider()
                          .GetRequiredService<IApiVersionDescriptionProvider>();

builder.Services.AddSwaggerGen(setupAction =>
{
    foreach(var myApiDescription in
            apiVersionDescriptionProvider.ApiVersionDescriptions)
    {
        setupAction.SwaggerDoc($"{myApiDescription.GroupName}", new()
        {
            Title = "MyCities Info API",
            Version = myApiDescription.ApiVersion.ToString(),
            Description = "Through this WebApi you can access MyCities and their Points-Of-Interests."

        });


    }//--end-foreach


    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

    setupAction.IncludeXmlComments(xmlCommentsFullPath);


    setupAction.AddSecurityDefinition("MyCitiesInfoesApiBearerAuth", new()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Input a valid Token to access the Web-API"

    });

    setupAction.AddSecurityRequirement(new()
    {

        {
            new()
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "MyCitiesInfoesApiBearerAuth"
                }
                
            },
            new List<string>()

        }


    });




});//--end-AddSwaggerGen


builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor
                        | ForwardedHeaders.XForwardedProto;


});

var app = builder.Build();



// Configure the HTTP request pipeline.
// Below:

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

app.UseForwardedHeaders();




//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(setupAction =>
    {
        var myDescriptions = app.DescribeApiVersions();
        foreach (var singleDescription in myDescriptions)
        {
            setupAction.SwaggerEndpoint(
                    $"/swagger/{singleDescription.GroupName}/swagger.json",
                    singleDescription.GroupName.ToUpperInvariant());

        }//--end-foreach

    });
//}

app.UseHttpsRedirection();


//-------Enabling endpoint-routing----

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
                {

                    endpoints.MapControllers();
                    
                });


//app.MapControllers();
//------------------------------------


app.Run();
