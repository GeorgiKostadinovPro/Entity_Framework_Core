// ReSharper disable InconsistentNaming

namespace TeisterMask.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;
    using TeisterMask.Utilities;
    using System.Text;
    using TeisterMask.Data.Models;
    using System.Globalization;
    using TeisterMask.Data.Models.Enums;
    using Castle.Core.Internal;
    using TeisterMask.DataProcessor.ImportDto.Projects;
    using TeisterMask.DataProcessor.ImportDto.Employees;
    using Newtonsoft.Json;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        private static XmlHelper xmlHelper;
        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            xmlHelper = new XmlHelper();

            ImportProjectDTO[] deserializedProjects
                = xmlHelper.Deserialize<ImportProjectDTO[]>(xmlString, "Projects");

            ICollection<Project> projectsToImport = new HashSet<Project>();

            foreach (var projectDto in deserializedProjects)
            {
                if (!IsValid(projectDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime projectOpenDate;
                bool isOpenDateValid = DateTime.TryParseExact(projectDto.OpenDate, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out projectOpenDate);

                if (!isOpenDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime? projectDueDate = null;

                if (!string.IsNullOrWhiteSpace(projectDto.DueDate))
                {
                    DateTime dueDateDt;
                    bool isDueDateValid = DateTime.TryParseExact(projectDto.DueDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out dueDateDt);

                    if (!isDueDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    projectDueDate = dueDateDt;
                }

                Project project = new Project()
                {
                    Name = projectDto.Name,
                    OpenDate = projectOpenDate,
                    DueDate = projectDueDate
                };

                foreach (var taskDto in projectDto.Tasks)
                {
                    if (!IsValid(taskDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime taskOpenDate;
                    bool isTaskOpenDateValid = DateTime.TryParseExact(taskDto.OpenDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out taskOpenDate);

                    if (!isTaskOpenDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime taskDueDate;
                    bool isTaskDueDateValid = DateTime.TryParseExact(taskDto.DueDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out taskDueDate);

                    if (!isTaskDueDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (taskOpenDate < projectOpenDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (projectDueDate.HasValue && taskDueDate > projectDueDate.Value)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Task task = new Task
                    {
                        Name = taskDto.Name,
                        OpenDate = taskOpenDate,
                        DueDate = taskDueDate,
                        ExecutionType = (ExecutionType)taskDto.ExecutionType,
                        LabelType = (LabelType)taskDto.LabelType
                    };

                    project.Tasks.Add(task);
                }

                projectsToImport.Add(project);

                sb.AppendLine(string.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));
            }

            context.Projects.AddRange(projectsToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<ImportEmployeeDTO> deserializedEmployees =
                JsonConvert.DeserializeObject<IEnumerable<ImportEmployeeDTO>>(jsonString);

            ICollection<Employee> employeesToImport = new HashSet<Employee>();

            int[] validTaskIds = context.Tasks
                  .Select(t => t.Id)
                  .ToArray();

            foreach (var employeeDto in deserializedEmployees)
            {
                if (!IsValid(employeeDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Employee employee = new Employee
                {
                    Username = employeeDto.Username,
                    Email = employeeDto.Email,
                    Phone = employeeDto.Phone,
                };

                foreach (var taskId in employeeDto.TaskIds.Distinct())
                {
                    if (!validTaskIds.Contains(taskId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    EmployeeTask employeeTask = new EmployeeTask
                    {
                        Employee = employee,
                        TaskId = taskId
                    };

                    employee.EmployeesTasks.Add(employeeTask);
                }

                employeesToImport.Add(employee);

                sb.AppendLine(string.Format(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count));
            }

            context.Employees.AddRange(employeesToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
         }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}