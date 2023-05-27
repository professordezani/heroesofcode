using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/teste", () =>
{

    Dictionary<string, object> programParams = new Dictionary<string, object>();
    programParams.Add("exercicio", 2); //int.Parse(args[0]));
    programParams.Add("script", "script2.py"); //args[1]);

    // Define the test cases
    List<Exercicio> exercicios = new List<Exercicio>();

    // exercicios.Add(new Exercicio(1, new TestCase[] {                
    //     new TestCase(1, "8\n4\n", "0\n"),
    //     new TestCase(2, "12\n9\n", "1\n"),
    //     new TestCase(3, "15\n5\n", "2\n"),
    //     new TestCase(4, "9\n3\n", "3\n"),
    //     new TestCase(5, "11\n6\n", "4\n"),
    // }));

    Console.WriteLine((string)programParams["script"]);

    exercicios.Add(new Exercicio(2, new TestCase[] {
                new TestCase(1, "[10, 15, 3, 7]\n17\n", "True\n"),
                new TestCase(2, "[10, 15, 3, 7]\n18\n", "True\n"),
                new TestCase(3, "[10, 15, 3, 7]\n10\n", "True\n"),
            }));

    exercicios.Add(new Exercicio(3, new TestCase[] {
                new TestCase(1, "'carrace'\n", "True\n"),
                new TestCase(2, "'daily'\n", "False\n")
            }));

    exercicios.Add(new Exercicio(4, new TestCase[] {
                new TestCase(1, "[2, 1, 5, 7, 2, 0, 5]\n", "2\n1.5\n2\n3.5\n2\n2.0\n2\n")
            }));

    exercicios.Add(new Exercicio(5, new TestCase[] {
                new TestCase(1, "27\n", "[27, 82, 41, 124, 62, 31, 94, 47, 142, 71, 214, 107, 322, 161, 484, 242, 121, 364, 182, 91, 274, 137, 412, 206, 103, 310, 155, 466, 233, 700, 350, 175, 526, 263, 790, 395, 1186, 593, 1780, 890, 445, 1336, 668, 334, 167, 502, 251, 754, 377, 1132, 566, 283, 850, 425, 1276, 638, 319, 958, 479, 1438, 719, 2158, 1079, 3238, 1619, 4858, 2429, 7288, 3644, 1822, 911, 2734, 1367, 4102, 2051, 6154, 3077, 9232, 4616, 2308, 1154, 577, 1732, 866, 433, 1300, 650, 325, 976, 488, 244, 122, 61, 184, 92, 46, 23, 70, 35, 106, 53, 160, 80, 40, 20, 10, 5, 16, 8, 4, 2, 1]\n")
            }));

    Exercicio exercicioSelecionado = exercicios.SingleOrDefault((e) => e.ExercicioId == (int)programParams["exercicio"]);

    // Console.Clear();

    // Execute the Python code for each test case
    foreach (var testCase in exercicioSelecionado.TestCases)
    {
        Console.WriteLine($"Running Test Case #{testCase.TestCaseId}:");

        // Execute the Python code
        ProcessStartInfo pythonInfo = new ProcessStartInfo
        {
            FileName = "python",
            Arguments = (string)programParams["script"],
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true

        };



        using (Process pythonProcess = Process.Start(pythonInfo)!)
        {
            // Write the Python code and test case input to the standard input
            // pythonProcess.StandardInput.WriteLine(pythonCode);
            pythonProcess.StandardInput.WriteLine(testCase.Inputs);
            pythonProcess.StandardInput.Close();

            // Read the output and error streams
            string output = pythonProcess.StandardOutput.ReadToEnd();
            string error = pythonProcess.StandardError.ReadToEnd();

            pythonProcess.WaitForExit();

            if (pythonProcess.ExitCode == 0)
            {
                Console.Write("Output: ");
                Console.Write(output);

                if (testCase.TestCaseId <= 2)
                {
                    Console.Write("Expected Output: ");
                    Console.Write(testCase.ExpectedOuput);
                }
            }
            else
            {
                Console.WriteLine("Error executing Python code:");
                Console.WriteLine(error);
            }

            if (output.Trim() == testCase.ExpectedOuput.Trim())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Result: Correct");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Result: Incorrect");
                Console.ResetColor();
            }
        }

        Console.WriteLine();
    }


}).WithName("Testar");

app.Run();

record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}




public class Exercicio
{
    public int ExercicioId { get; set; }
    public TestCase[] TestCases { get; set; }

    public Exercicio(int id, TestCase[] testCases)
    {
        this.ExercicioId = id;
        this.TestCases = testCases;
    }
}

public class TestCase
{
    public int TestCaseId { get; set; }
    public string Inputs { get; set; }
    public string ExpectedOuput { get; set; }

    public TestCase(int id, string inputs, string expectedOuput)
    {
        this.TestCaseId = id;
        this.Inputs = inputs;
        this.ExpectedOuput = expectedOuput;
    }
}