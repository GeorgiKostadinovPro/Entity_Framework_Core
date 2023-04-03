using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System.Globalization;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            // 01. Import the SoftUni Database
            // 02. Database First (Scaffolding a database from MS SQL server)
            using SoftUniContext softUniContext = new SoftUniContext();

            // 03. Employees Full Information
            /*string employeesFullInformation = GetEmployeesFullInformation(softUniContext);
            Console.WriteLine(employeesFullInformation);*/

            // 04. Employees with Salary Over 50000
            /*string employeesWithSalaryOver50000 = GetEmployeesWithSalaryOver50000(softUniContext);
            Console.WriteLine(employeesWithSalaryOver50000);*/

            // 05. Employees from Research and Development
            /*string employeesFromResearchAndDevelopmentDepartments = GetEmployeesFromResearchAndDevelopment(softUniContext);
            Console.WriteLine(employeesFromResearchAndDevelopmentDepartments);*/

            // 06. Adding a New Address and Updating Employee
            /*string employees = AddNewAddressToEmployee(softUniContext);
            Console.WriteLine(employees);*/

            // 07. Employees and Projects
            /*string employeesInPeriod = GetEmployeesInPeriod(softUniContext);
            Console.WriteLine(employeesInPeriod);*/

            // 08. Addresses by Town
            /*string addressesByTown = GetAddressesByTown(softUniContext);
            Console.WriteLine(addressesByTown);*/

            // 09. Employee 147
            /*string employee147 = GetEmployee147(softUniContext);
            Console.WriteLine(employee147);*/

            // 10. Departments with More Than 5 Employees
            /*string departmentsWithMorethan5Employees = GetDepartmentsWithMoreThan5Employees(softUniContext);
            Console.WriteLine(departmentsWithMorethan5Employees);*/

            // 11. Find Latest 10 Projects
            /*string latest10Projects = GetLatestProjects(softUniContext);
            Console.WriteLine(latest10Projects);*/

            // 12.Increase Salaries
            /*string employeesWithIncreasedSalaries = IncreaseSalaries(softUniContext);
            Console.WriteLine(employeesWithIncreasedSalaries);*/

            // 13. Find Employees by First Name Starting With Sa
            /*string employeesByFirstNameStartingWithSa = GetEmployeesByFirstNameStartingWithSa(softUniContext);
            Console.WriteLine(employeesByFirstNameStartingWithSa);*/

            // 14. Delete Project by Id
            /*string result = DeleteProjectById(softUniContext);
            Console.WriteLine(result);*/

            // 15. Remove Town
            string removeTownResult = RemoveTown(softUniContext);
            Console.WriteLine(removeTownResult);
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var allEmployeesFullInformation = context.Employees
                .AsNoTracking()
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    MiddleName = e.MiddleName,
                    JobTitle = e.JobTitle,
                    Salary = e.Salary
                })
                .ToArray();

            foreach (var employee in allEmployeesFullInformation)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} " +
                    $"{employee.MiddleName} {employee.JobTitle} {employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        { 
            StringBuilder sb = new StringBuilder();

            var employeesWithSalaryOver50000 = context.Employees
                .AsNoTracking()
                .Where(e => e.Salary > 50000)
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    Salary = e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ToArray();

            foreach (var employee in employeesWithSalaryOver50000)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employeesFromResearchAndDevelopmentDepartments = context.Employees
                .AsNoTracking()
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new 
                { 
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    DepartmentName = e.Department.Name,
                    Salary = e.Salary
                })
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToArray();

            foreach (var employee in employeesFromResearchAndDevelopmentDepartments)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from " +
                    $"{employee.DepartmentName} - ${employee.Salary:f2}");
            }

            return sb.ToString().TrimEnd(); 
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            Employee employee = context.Employees
                .FirstOrDefault(e => e.LastName == "Nakov");

            if (employee == null)
            {
                return "Employee NOT found!";
            }

            Address newAddress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            employee.Address = newAddress;
            context.SaveChanges();

            var employeeAddresses = context.Employees
                .AsNoTracking()
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => e.Address!.AddressText)
                .ToArray();

            foreach (var address in employeeAddresses)
            {
                sb.AppendLine(address);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employeesInPeriod = context.Employees
                .AsNoTracking()
                .Take(10)
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    ManagerFirstName = e.Manager!.FirstName,
                    ManagerLastName = e.Manager!.LastName,
                    Projects = e.EmployeesProjects
                                .Where(ep => ep.Project.StartDate.Year >= 2001
                                && ep.Project.StartDate.Year <= 2003)
                                .Select(ep => ep.Project)
                                .ToArray()
                })
                .ToArray();

            foreach (var employee in employeesInPeriod)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} " +
                    $"- Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");

                foreach (var project in employee.Projects)
                {
                    string endDate = !project.EndDate.HasValue ? "not finished"
                        : project.EndDate!.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                    sb.AppendLine($"--{project.Name} - {project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)}" +
                        $" - {endDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var addressesByTown = context.Addresses
                .AsNoTracking()
                .Select(a => new
                {
                    AddressText = a.AddressText,
                    TownName = a.Town.Name,
                    EmployeeCount = a.Employees.Count
                })
                .OrderByDescending(a => a.EmployeeCount)
                .ThenBy(a => a.TownName)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .ToArray();

            foreach (var address in addressesByTown)
            {
                sb.AppendLine($"{address.AddressText}, {address.TownName} - {address.EmployeeCount} employees");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee147 = context.Employees
                .AsNoTracking()
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle,
                    Projects = e.EmployeesProjects
                    .Select(ep => new
                    {
                        ProjectName = ep.Project.Name
                    })
                    .OrderBy(ep => ep.ProjectName)
                    .ToArray()
                })
                .FirstOrDefault();

            sb.AppendLine($"{employee147!.FirstName} {employee147!.LastName}" +
                $" - {employee147.JobTitle}");

            foreach (var project in employee147.Projects)
            {
                sb.AppendLine(project.ProjectName);
            }
            
            return sb.ToString().TrimEnd();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var departmentsWithMorethan5Employees = context.Departments
                .AsNoTracking()
                .Where(d => d.Employees.Count > 5)
                .Select(d => new
                {
                    DepartmentName = d.Name,
                    ManagerFirstName = d.Manager.FirstName,
                    ManagerLastName = d.Manager.LastName,
                    Employees = d.Employees
                                 .Select(e => new
                                 {
                                     FirstName = e.FirstName,
                                     LastName = e.LastName,
                                     JobTitle = e.JobTitle,
                                 })
                                 .OrderBy(e => e.FirstName)
                                 .ThenBy(e => e.LastName)
                                 .ToArray()
                })
                .OrderBy(e => e.Employees.Length)
                .OrderBy(e => e.DepartmentName)
                .ToArray();

            foreach (var department in departmentsWithMorethan5Employees)
            {
                sb.AppendLine($"{department.DepartmentName} - {department.ManagerFirstName} {department.ManagerLastName}");

                foreach (var employee in department.Employees)
                {
                    sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var latest10Projects = context.Projects
                .AsNoTracking()
                .OrderByDescending(p => p.StartDate)
                .Select(p => new 
                { 
                    Name = p.Name,
                    Description = p.Description,
                    StartDate = p.StartDate,
                })
                .Take(10)
                .OrderBy(p => p.Name)
                .ToArray();

            foreach (var project in latest10Projects)
            {
                sb.AppendLine(project.Name);
                sb.AppendLine(project.Description);
                sb.AppendLine(project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture));
            }

            return sb.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            string[] validDepartments = { "Engineering", "Tool Design", "Marketing", "Information Services" };

            var employeesToIncreaseSalaries = context.Employees
                .Where(e => validDepartments.Contains(e.Department.Name))
                .ToArray();

            foreach (var employee in employeesToIncreaseSalaries)
            {
                employee.Salary *= (decimal)1.12;
            }

            context.SaveChanges();

            var sortedEmployeesWithIncreasedSalaries = employeesToIncreaseSalaries
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Salary = e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToArray();

            foreach (var employee in sortedEmployeesWithIncreasedSalaries)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employeesByFirstNameStartingWithSa = context.Employees
                .AsNoTracking()
                .Where(e => e.FirstName.ToLower().StartsWith("sa"))
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle,
                    Salary = e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToArray();

            foreach (var employee in employeesByFirstNameStartingWithSa)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            Project projectToDelete = context.Projects
                .FirstOrDefault(p => p.ProjectId == 2);

            var employeesProjectsToDelete = context.EmployeesProjects
                .Where(p => p.ProjectId == projectToDelete.ProjectId)
                .ToArray();

            if (employeesProjectsToDelete.Any())
            {
                foreach (var ep in employeesProjectsToDelete)
                {
                    context.EmployeesProjects.Remove(ep);
                }
            }

            context.Projects.Remove(projectToDelete);
            context.SaveChanges();

            var projectsToDisplay = context.Projects
                .AsNoTracking()
                .Take(10)
                .Select(p => p.Name)
                .ToArray();

            return string.Join(Environment.NewLine, projectsToDisplay).TrimEnd();
        }

        public static string RemoveTown(SoftUniContext context)
        {
            Town townToDelete = context.Towns
                .FirstOrDefault(t => t.Name == "Seattle");

            var addressesToDelete = context.Addresses
                .Where(a => a.TownId == townToDelete.TownId)
                .ToArray();

            var employeesToSetAddressToNull = context.Employees
                .Where(e => e.Address.TownId == townToDelete.TownId)
                .ToArray();

            if (employeesToSetAddressToNull.Any())
            {
                foreach (var employee in employeesToSetAddressToNull)
                {
                    employee.AddressId = null;
                }
            }

            if (addressesToDelete.Any())
            {
                foreach (var addressToDelete in addressesToDelete)
                {
                    context.Addresses.Remove(addressToDelete);
                }
            }

            context.Towns.Remove(townToDelete);
            context.SaveChanges();

            return $"{addressesToDelete.Length} addresses in Seattle were deleted";
        }
    }
}