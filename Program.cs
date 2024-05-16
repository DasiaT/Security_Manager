using Manager_Security_BackEnd.DBContext;
using Manager_Security_BackEnd.Interfaces;
using Manager_Security_BackEnd.Models.Generals;
using Manager_Security_BackEnd.Models.Generals.General_Valid_Exist;
using Manager_Security_BackEnd.Services.Access_Page_Rols;
using Manager_Security_BackEnd.Services.Application_Rol_Privileges_Services;
using Manager_Security_BackEnd.Services.Application_Services;
using Manager_Security_BackEnd.Services.Authentications;
using Manager_Security_BackEnd.Services.Company_Services;
using Manager_Security_BackEnd.Services.Element_Pages;
using Manager_Security_BackEnd.Services.Error_Services;
using Manager_Security_BackEnd.Services.Information_User_Services;
using Manager_Security_BackEnd.Services.Pages_Services;
using Manager_Security_BackEnd.Services.Privileges_Services;
using Manager_Security_BackEnd.Services.Rol_Services;
using Manager_Security_BackEnd.Services.User_Services;
using Manager_Security_BackEnd.Services.Users_Applications_Privileges;
using Manager_Security_BackEnd.Services.Users_Applications_Rols;
using Manager_Security_BackEnd.Services.Workstation_Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

//PARA JSON WEB TOKEN
builder.Services
    .AddHttpContextAccessor()
    .AddAuthorization()
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtTokenSettings:ValidIssuer"],
            ValidAudience = builder.Configuration["JwtTokenSettings:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtTokenSettings:SymmetricSecurityKey"]))
        };
});
//FIN DE JSON WEB TOKEN

builder.Services.AddControllers();
builder.Services.AddHttpClient();

//SERVICIOS QUE SE CREAN
builder.Services.AddDistributedMemoryCache();
builder.Services.AddScoped<conectionDBcontext>();
builder.Services.AddScoped<Access_Page_Rol_Error_Manager>();
builder.Services.AddScoped<Application_Error_Manager>();
builder.Services.AddScoped<Application_Rol_Privileges_Error_Manager>();
builder.Services.AddScoped<Company_Error_Manager>();
builder.Services.AddScoped<General_Generate_Cache_Key>();
builder.Services.AddScoped<Element_Page_Error_Manager>();
builder.Services.AddScoped<Information_User_Error_Manager>();
builder.Services.AddScoped<Pages_Error_Manager>();
builder.Services.AddScoped<Privileges_Error_Manager>();
builder.Services.AddScoped<Roles_Error_Manager>();
builder.Services.AddScoped<User_Application_Privileges_Error_Manager>();
builder.Services.AddScoped<User_Application_Rol_Error_Manager>();
builder.Services.AddScoped<User_Error_Manager>();
builder.Services.AddScoped<General_Valid_Token>();
builder.Services.AddScoped<General_Generate_Password>();
builder.Services.AddScoped<General_Generate_Token>();
builder.Services.AddScoped<General_Valid_Application_Exist>();
builder.Services.AddScoped<General_Valid_Company_Exist>();
builder.Services.AddScoped<General_Valid_User_Exist>();
builder.Services.AddScoped<General_Valid_Privileges_Exist>();
builder.Services.AddScoped<General_Valid_Rol_Exist>();
builder.Services.AddScoped<IAccess_Page_Rol, AccessPageRolServices>();
builder.Services.AddScoped<IApplication_Rol_Privileges, ApplicationRolPrivilegesServices>();
builder.Services.AddScoped<IApplication, ApplicationServices>();
builder.Services.AddScoped<IAuthentication, AuthenticationServices>();
builder.Services.AddScoped<ICompany, CompanyServices>();
builder.Services.AddScoped<IElement_Page, ElementPageServices>();
builder.Services.AddScoped<IError, Error>();
builder.Services.AddScoped<IInformation_User, InformationUserServices>();
builder.Services.AddScoped<IPages, PagesServices>();
builder.Services.AddScoped<IPrivileges, PrivilegesServices>();
builder.Services.AddScoped<IRoles, RolesServices>();
builder.Services.AddScoped<IUser, UserServices>();
builder.Services.AddScoped<IUser_Application_Privileges, UserApplicationPrivilegesSevices>();
builder.Services.AddScoped<IUser_Application_Rol, UserApplicationRolSevices>();
builder.Services.AddScoped<IWorkstation, WorkstationServices>();
//FIN DE SERVICIOS

builder.Services.AddEndpointsApiExplorer();
//PARA AÑADIR EL CANDADO EN SWAGGER PARA EL BEARER TOKEN
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "MANAGER SECURITY APIS", Version = "v1" });

    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor ingresa un token valido.",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
//FIN DE EL CANDADO EN SWAGGER PARA EL BEARER TOKEN

// CONEXION A BASE DE DATOS DE MANAGER SECURITY (CAMBIAR A PRODUCTIVO O DESARROLLO SEGUN EL AMBIENTE)
builder.Services.AddDbContext<conectionDBcontext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionDesarrollo"))//ConnectionProductivo o ConnectionDesarrollo
);
// FIN DE CONEXION

//PARA REDIS
builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
{
    var redisConfiguration = builder.Configuration.GetValue<string>("Caching:RedisConnection");

    var multiplexer = ConnectionMultiplexer.Connect(redisConfiguration);

    return multiplexer;
});
//FIN DE REDIS

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}


//CONFIGURACION DE MIDDLIWERE
app.UseAuthentication();
app.UseAuthorization();
//FIN DE CONFIGURACION DE MIDDLIWERE

//PARA PERMITIR QUE ENVIE O RECIBA DE CUALQUIER LUGAR O APP
app.UseCors(options =>
{
    options.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod();
});
//FIN DE CORS

app.MapControllers();

app.Run();
