using hospitalBillingManagement.Data;
using hospitalBillingManagement.Repositories.Implementations;
using hospitalBillingManagement.Repositories.Interfaces;
using hospitalBillingManagement.Services.Implementations;
using hospitalBillingManagement.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>() ?? new MongoDbSettings();
builder.Services.AddSingleton(new MongoDbContext(mongoDbSettings));

builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBillingService, BillingService>();
builder.Services.AddScoped<IPaymentProcessor, UpiPaymentProcessor>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
