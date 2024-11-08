using GestaoFinanceira_v_1.Controllers;
using GestaoFinanceira_v_1.Components;
using GestaoFinanceira_v_1.Components.Account;
using GestaoFinanceira_v_1.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSignalRCore();
builder.Services.AddRazorPages();

builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options =>
    {
        options.DetailedErrors = true;
    });

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddScoped<CidadesController>();
builder.Services.AddScoped<EstadosController>();
builder.Services.AddScoped<ClientesController>();
builder.Services.AddScoped<ClientePFsController>();
builder.Services.AddScoped<ClientePJsController>();
builder.Services.AddScoped<ServicoController>();
builder.Services.AddScoped<EmpresaController>();
builder.Services.AddScoped<CMCControllers>();
builder.Services.AddScoped<VeiculoController>();
builder.Services.AddScoped<FuncionarioController>();
builder.Services.AddScoped<CaixaController>();
builder.Services.AddScoped<VendaController>();
builder.Services.AddScoped<DispositivoController>();
builder.Services.AddScoped<RecebimentoController>();
builder.Services.AddScoped<FornecedorController>();
builder.Services.AddScoped<DespesaController>();
builder.Services.AddScoped<PlanoContasController>();
builder.Services.AddScoped<EncargoController>();
builder.Services.AddScoped<BancoController>();
builder.Services.AddScoped<LogCaixaController>();
builder.Services.AddScoped<Cabecalho>();

builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    options.EnableSensitiveDataLogging();

});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true;
})
    .AddRoles<IdentityRole>()
    .AddRoleManager<RoleManager<IdentityRole>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders()
    .AddClaimsPrincipalFactory<IdentityExtensions>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("admin",
         policy => policy.RequireRole("admin"));
});

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
});

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
