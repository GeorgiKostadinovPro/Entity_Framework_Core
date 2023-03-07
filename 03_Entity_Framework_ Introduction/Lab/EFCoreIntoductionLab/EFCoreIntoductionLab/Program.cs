using EFCoreIntoductionLab.Data;
using EFCoreIntoductionLab.Models;

namespace EFCoreIntoductionLab;

public class Program
{
    public static void Main(string[] args)
    {
        using SoftUniContext softUniContext = new SoftUniContext();

        IEnumerable<Employee> allEmployees = softUniContext.Employees.ToArray();

        foreach (var employee in allEmployees)
        {
            Console.WriteLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
        }       
    }
}