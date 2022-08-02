// See https://aka.ms/new-console-template for more information
using Serilog;

Console.WriteLine("Hello, World Serilog");

var configuration = new { DBConnection = "ConnectionString", ApiEndpoint = "google.com/api" };

//global logger
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:w3}] {Message:lj}{NewLine}{Exception}{Properties}") // logs to console with output formatting
    .WriteTo.Debug()   // logs to IDE output window 
    .WriteTo.File(new Serilog.Formatting.Json.JsonFormatter(),"../../../Logs/log.txt")
    .Enrich.WithProperty("configuration",configuration) // add configuration property value for log where configuration is added
    .CreateLogger();

var logger2 = new LoggerConfiguration()
    .WriteTo.Console() // logs to console
    .WriteTo.File(new Serilog.Formatting.Json.JsonFormatter(), "../../../Logs/Logger2/log.txt",
        rollingInterval: RollingInterval.Minute) // create a log file every minut
    .Enrich.With(new ThreadIdEnricher()) //add threadid to the log message
    .CreateLogger();

var logger3 = new LoggerConfiguration()
    .WriteTo.Console() // logs to console
    .WriteTo.Logger(l => l.Filter.ByExcluding(f => { return true; }) // create a sub logger of file sink, which filters messages based on some predicate function
        .WriteTo.File(new Serilog.Formatting.Json.JsonFormatter(), "../../../Logs/Logger3/log.txt", rollingInterval: RollingInterval.Minute))
    .Enrich.With(new ThreadIdEnricher())
    .CreateLogger();

//Global logger
Log.Information("This is Log information from serilog");

Employee employee = new Employee(1, "venki");

Log.Information("This is an employee {employee} configuration {@configuration}", employee);
logger2.Information("This is a log from logger2 {@employee} ThreadId {@ThreadId}", employee);

logger3.Information("This is a log from logger3 {@employee} ThreadId {@ThreadId}", employee);

Task.Run(()=> { logger2.Information("This is a log from logger2 {@employee} ThreadId {@ThreadId}", employee); }).GetAwaiter().GetResult();


int a = 10;
Log.Information("Logging a primitive type {a}", a);

var numbers = new int[] { 1, 2, 3, 4, 5 };
Log.Information("Logging a array of integers {numbers}",numbers);

var words = new string[] { "Hi", "Hello", "How are you" };
Log.Information("Logging an array of words {words}", words);

var obj = new Book("C#", "Venki");
Log.Information("Logging an object {$obj}",obj);
Employee e1 = new Employee(2, "Arun");
Log.Information("Logging an Employee object {@e1}",e1);

//Logging to filter log with context
var myLog = Log.ForContext<Employee>();
myLog.Information("This is a log for employee class");


var specificBookLog = Log.ForContext("Book Name", obj.BookName);
specificBookLog.Information("This is a log for Book class");