using System.ComponentModel.DataAnnotations;

namespace MiniORM.App.Data.Models;

public class Project
{
    public Project()
    {
        this.EmployeeProjects = new HashSet<EmployeeProject>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public virtual ICollection<EmployeeProject> EmployeeProjects { get; set; }
}
