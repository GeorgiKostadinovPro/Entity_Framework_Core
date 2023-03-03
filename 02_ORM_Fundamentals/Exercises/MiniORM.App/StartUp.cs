using MiniORM.App.Data;
using MiniORM.App.Data.Models;

namespace MiniORM.App;

public class StartUp
{
    public static void Main(string[] args)
    {
        SoftUniDbContext dbContext = new SoftUniDbContext(Config.ConnectionString);

        Employee newEmployee = dbContext.Employees
            .FirstOrDefault(e => e.FirstName == "Test"
                              && e.LastName == "Test");

        if (newEmployee == null)
        {
            Console.WriteLine("Employee was not found!");
            return;
        }

        //dbContext.Employees.Add(newEmployee);

        dbContext.Employees.Remove(newEmployee);

        dbContext.SaveChanges();
    }
}