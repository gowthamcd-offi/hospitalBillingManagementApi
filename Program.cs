using hospitalBillingManagement.Data;
using hospitalBillingManagement.Repositories.Implementations;
using hospitalBillingManagement.Repositories.Interfaces;
using hospitalBillingManagement.Services.Implementations;
using hospitalBillingManagement.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure MongoDB settings
var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>() ?? new MongoDbSettings();
builder.Services.AddSingleton(new MongoDbContext(mongoDbSettings));

// Register repositories
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Register services
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBillingService, BillingService>();

// Register payment processor
builder.Services.AddScoped<IPaymentProcessor, UpiPaymentProcessor>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
