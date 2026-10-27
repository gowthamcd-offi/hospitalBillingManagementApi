using hospitalBillingManagement.Data;
using hospitalBillingManagement.Repositories.Implementations;
using hospitalBillingManagement.Repositories.Interfaces;
using hospitalBillingManagement.Services.Implementations;
using hospitalBillingManagement.Services.Interfaces;
using hospitalBillingManagement.UI;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var settings = config.GetSection("MongoDbSettings").Get<MongoDbSettings>() ?? new MongoDbSettings();
var dbContext = new MongoDbContext(settings);
var patientRepo = new PatientRepository(dbContext);
var productRepo = new ProductRepository(dbContext);
var patientService = new PatientService(patientRepo, productRepo);
var billingService = new BillingService(patientRepo);
var paymentProcessor = new UpiPaymentProcessor();
var ui = new ConsoleUI(patientService, billingService, productRepo, paymentProcessor);
await ui.RunAsync();