using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace MiniORM.App.Data.Models;

public class Employee
{
    public Employee()
    {
        this.EmployeeProjects= new HashSet<EmployeeProject>();
    }


    [Key]
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    [Required]
    public string LastName { get; set; }

    public bool IsEmployed { get; set; }

    [ForeignKey(nameof(Department))]
    public int DepartmentId { get; set; }

    public virtual Department Department { get; set; }

    public virtual ICollection<EmployeeProject> EmployeeProjects { get; set; }
}