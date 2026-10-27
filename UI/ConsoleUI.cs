using hospitalBillingManagement.Repositories.Interfaces;
using hospitalBillingManagement.Services.Interfaces;
using hospitalBillingManagement.Models;

namespace hospitalBillingManagement.UI;

public class ConsoleUI
{
    private readonly IPatientService _patientService;
    private readonly IBillingService _billingService;
    private readonly IProductRepository _productRepository;
    private readonly IPaymentProcessor _paymentProcessor;

    public ConsoleUI(IPatientService patientService, IBillingService billingService, IProductRepository productRepository, IPaymentProcessor paymentProcessor)
    {
        _patientService = patientService;
        _billingService = billingService;
        _productRepository = productRepository;
        _paymentProcessor = paymentProcessor;
    }

    public async Task RunAsync()
    {
        Console.ResetColor();
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Green;
        while (true)
        {
            Console.Clear();
            DisplayHeader();
            DisplayMenu();
            ConsoleKey key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Escape)
            {
                Console.WriteLine("    Exited...!\n");
                break;
            }
            switch (key)
            {
                case ConsoleKey.C:
                    await CreatePatientPromptAsync();
                    break;
                case ConsoleKey.D:
                    await DisplayPatientsAsync();
                    break;
                case ConsoleKey.G:
                    await GenerateBillPromptAsync();
                    break;
                case ConsoleKey.M:
                    await CreateMedicineBillPromptAsync();
                    break;
                case ConsoleKey.S:
                    await SearchPatientPromptAsync();
                    break;
                case ConsoleKey.P:
                    await FindPatientsByProductPromptAsync();
                    break;
            }
        }
    }

    private void DisplayHeader()
    {
        Console.WriteLine(@"
    ██   ██  ██████  ███████ ██████  ██ ████████  █████  ██          ██████  ██ ██      ██      ██ ███    ██  ██████  
    ██   ██ ██    ██ ██      ██   ██ ██    ██    ██   ██ ██          ██   ██ ██ ██      ██      ██ ████   ██ ██       
    ███████ ██    ██ ███████ ██████  ██    ██    ███████ ██          ██████  ██ ██      ██      ██ ██ ██  ██ ██   ███ 
    ██   ██ ██    ██      ██ ██      ██    ██    ██   ██ ██          ██   ██ ██ ██      ██      ██ ██  ██ ██ ██    ██ 
    ██   ██  ██████  ███████ ██      ██    ██    ██   ██ ███████     ██████  ██ ███████ ███████ ██ ██   ████  ██████  
                                                                                                                    
        ");
    }

    private void DisplayMenu()
    {
        Console.WriteLine("    Menu:");
        Console.WriteLine(@"
    Press [ C ] : Create Patient
    Press [ D ] : Display Patients
    Press [ G ] : Generate Bill
    Press [ M ] : Add Medicine to Patient
    Press [ S ] : Search Patient (Email/Mobile/Name)
    Press [ P ] : Find Patients by Product
    Press [ Esc ] : Exit
                            ");
    }

    private async Task CreatePatientPromptAsync()
    {
        Console.WriteLine("\n    ----------------------------------------Enter Patient Details----------------------------------------\n");
        Console.Write("    First Name  : ");
        string firstName = Console.ReadLine() ?? "";
        Console.Write("    Last Name   : ");
        string lastName = Console.ReadLine() ?? "";
        Console.Write("    Email       : ");
        string email = Console.ReadLine() ?? "";
        Console.Write("    Mobile      : ");
        long.TryParse(Console.ReadLine(), out long mobile);
        Console.WriteLine("\n    Press [ Enter ] To save Data\n    Press any key to cancel data\n");
        if (Console.ReadKey(true).Key == ConsoleKey.Enter)
        {
            await _patientService.CreateAsync(firstName, lastName, email, mobile);
            Console.WriteLine("    Patient created successfully.");
        }
        Pause();
    }

    private async Task DisplayPatientsAsync()
    {
        var patients = await _patientService.GetAllAsync();
        Console.WriteLine("    --- All Patients ---\n");
        foreach (var p in patients)
        {
            Console.WriteLine($"    ID        : {p.Id}");
            Console.WriteLine($"    Name      : {p.FirstName} {p.LastName}");
            Console.WriteLine($"    Email     : {p.Email}");
            Console.WriteLine($"    Mobile    : {p.MobileNo}");
            Console.WriteLine($"    Total     : {p.TotalAmountSpent}");
            Console.WriteLine($"    Pending   : {p.PendingAmount}");
            Console.WriteLine($"    Paid      : {p.IsPaid}");
            Console.WriteLine($"    UPI ID    : {p.UpiId ?? "Not paid"}");
            Console.WriteLine("    Products Purchased:");
            if (p.PurchasedProducts.Any())
            {
                foreach (var item in p.PurchasedProducts)
                {
                    Console.WriteLine($"      - {item.ProductName} (Qty: {item.Quantity}, Rate: {item.Rate})");
                }
            }
            else Console.WriteLine("      No products purchased");
            Console.WriteLine();
        }
        Pause();
    }

    private async Task CreateMedicineBillPromptAsync()
    {
        Console.WriteLine("    Search Patient By Email/Name/Mobile :");
        string identifier = Console.ReadLine() ?? "";

        var patient = await _patientService.SearchAsync(identifier);
        if (patient == null)
        {
            Console.WriteLine("    Patient not found...!");
            Pause();
            return;
        }


        Console.WriteLine($"\n    Patient: {patient.FirstName} {patient.LastName}");
        Console.WriteLine("    Available Medicines:\n");
        var products = await _productRepository.GetAllAsync();
        foreach (var prod in products)
        {
            Console.WriteLine($"    ID: {prod.ProductId,-5} Name: {prod.Name,-25} Rate: {prod.Rate}");
        }

        Console.Write("\n    Enter Medicine ID : ");
        if (!int.TryParse(Console.ReadLine(), out int medId))
        {
            Console.WriteLine("    Invalid ID...!");
            Pause();
            return;
        }

        Console.Write("    Enter Quantity    : ");
        if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0)
        {
            Console.WriteLine("    Invalid quantity...!");
            Pause();
            return;
        }

        bool success = await _patientService.AddMedicineAsync(identifier, medId, qty);
        if (success) Console.WriteLine("    Medicine added successfully.");
        else Console.WriteLine("    Failed to add medicine...!");
        Pause();
    }

    private async Task GenerateBillPromptAsync()
    {
        Console.WriteLine("    Search Patient By Email/Name/Mobile :");
        string identifier = Console.ReadLine() ?? "";

        var patient = await _billingService.GetBillDetailsAsync(identifier);
        if (patient == null)
        {
            Console.WriteLine("    Patient not found...!");
            Pause();
            return;
        }

        Console.WriteLine($"\n    --- Bill ---");
        Console.WriteLine($"    ID            : {patient.Id}");
        Console.WriteLine($"    Name          : {patient.FirstName} {patient.LastName}");
        Console.WriteLine($"    Email         : {patient.Email}");
        Console.WriteLine($"    Mobile        : {patient.MobileNo}");
        Console.WriteLine($"    Total Amount  : {patient.TotalAmountSpent}");
        Console.WriteLine($"    Pending       : {patient.PendingAmount}");
        Console.WriteLine($"    UPI ID        : {patient.UpiId ?? "Not paid"}");
        Console.WriteLine($"    Last Payment  : {patient.PaymentDate}");
        Console.WriteLine($"\n    --- Products ---");
        if (patient.PurchasedProducts.Any())
        {
            foreach (var item in patient.PurchasedProducts)
            {
                Console.WriteLine($"      {item.ProductName,-25} Qty: {item.Quantity,-5} Rate: {item.Rate}");
            }
        }
        else Console.WriteLine("      No products purchased");
        Console.WriteLine($"\n    Generated at: {DateTime.Now}");
        Console.WriteLine("\n    Press [ P ] : Proceed to payment");
        Console.WriteLine("    Press [ C ] : Cancel");
        if (Console.ReadKey(true).Key == ConsoleKey.P)
        {
            Console.Write("\n    Enter UPI ID : ");
            string upiId = Console.ReadLine() ?? "";
            bool paid = await _billingService.SettleBillAsync(identifier, upiId, _paymentProcessor);
            if (paid) Console.WriteLine("\n    Payment successful...!");
            else Console.WriteLine("\n    Payment failed..!");
        }
        Pause();
    }

    private async Task SearchPatientPromptAsync()
    {
        Console.WriteLine("    Search Patient By Email/Name/Mobile :");
        string identifier = Console.ReadLine() ?? "";

        var patient = await _patientService.SearchAsync(identifier);
        if (patient is null) Console.WriteLine("    Patient not found....!");
        else
        {
            Console.WriteLine($"\n    --- Patient Found ---");
            Console.WriteLine($"    ID        : {patient.Id}");
            Console.WriteLine($"    Name      : {patient.FirstName} {patient.LastName}");
            Console.WriteLine($"    Email     : {patient.Email}");
            Console.WriteLine($"    Mobile    : {patient.MobileNo}");
            Console.WriteLine($"    Total     : {patient.TotalAmountSpent}");
            Console.WriteLine($"    Pending   : {patient.PendingAmount}");
            Console.WriteLine($"    Paid      : {patient.IsPaid}");
        }
        Pause();
    }

    private async Task FindPatientsByProductPromptAsync()
    {
        Console.WriteLine("    --- Find Patients by Product ---\n");
        var products = await _productRepository.GetAllAsync();
        Console.WriteLine("    Available Products:");
        foreach (var prod in products)
        {
            Console.WriteLine($"    ID: {prod.ProductId,-5} Name: {prod.Name,-25} Rate: {prod.Rate}");
        }
        Console.Write("\n    Enter Product ID to find patients: ");
        if (!int.TryParse(Console.ReadLine(), out int productId) || productId == 0)
        {
            Console.WriteLine("    Invalid product ID...!");
            Pause();
            return;
        }
        var patients = await _patientService.GetPatientsByProductAsync(productId);
        if (!patients.Any()) Console.WriteLine($"\n    No patients found who bought this product...!");
        else
        {
            Console.WriteLine($"\n    --- Patients who bought this product ({patients.Length}) ---\n");
            foreach (var p in patients)
            {
                Console.WriteLine($"    ID        : {p.Id}");
                Console.WriteLine($"    Name      : {p.FirstName} {p.LastName}");
                Console.WriteLine($"    Email     : {p.Email}");
                Console.WriteLine($"    Mobile    : {p.MobileNo}");
                Console.WriteLine();
            }
        }
        Pause();
    }

    private void Pause()
    {
        Console.WriteLine("\n    Press [ Enter ] to continue...");
        while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
    }
}