namespace TeisterMask.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Linq;
    using TeisterMask.DataProcessor.ExportDto.Employees;
    using TeisterMask.DataProcessor.ExportDto.Projects;
    using TeisterMask.DataProcessor.ExportDto.Tasks;
    using TeisterMask.Utilities;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            var projectsToExport = context.Projects
                .ToArray()
                .Where(p => p.Tasks.Any())
                .Select(p => new ExportProjectDTO
                {
                    TasksCount = p.Tasks.Count,
                    ProjectName = p.Name,
                    HasEndDate = p.DueDate == null ? "No" : "Yes",
                    Tasks = p.Tasks
                             .Select(t => new ExportTaskForProjectDTO
                             {
                                 Name = t.Name,
                                 Label = t.LabelType.ToString()
                             })
                             .OrderBy(t => t.Name)
                             .ToArray()
                })
                .OrderByDescending(p => p.TasksCount)
                .ThenBy(p => p.ProjectName)
                .ToArray(); 
            
            XmlHelper xmlHelper = new XmlHelper();

            string result = xmlHelper.Serialize<ExportProjectDTO[]>(projectsToExport, "Projects");

            return result;
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var employeesToExport = context.Employees
                .ToArray()
                .Where(e => e.EmployeesTasks.Any(et => et.Task.OpenDate >= date))
                .Select(e => new ExportEmployeeDTO
                {
                    Username = e.Username,
                    Tasks = e.EmployeesTasks
                             .Where(et => et.Task.OpenDate >= date)
                             .OrderByDescending(et => et.Task.DueDate)
                             .ThenBy(et => et.Task.Name)
                             .Select(et => new ExportTaskForEmployeeDTO
                             {
                                 TaskName = et.Task.Name,
                                 OpenDate = et.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                                 DueDate = et.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                                 LabelType = et.Task.LabelType.ToString(),
                                 ExecutionType = et.Task.ExecutionType.ToString()
                             })
                             .ToArray()
                })
                .OrderByDescending(e => e.Tasks.Length)
                .ThenBy(e => e.Username)
                .Take(10)
                .ToArray();

            string result = JsonConvert.SerializeObject(employeesToExport, Formatting.Indented);

            return result;
        }
    }
}