using System.ComponentModel.DataAnnotations;

namespace MiniORM.App.Data.Models;

public class Department
{
    public Department()
    {
        this.Employees = new HashSet<Employee>();
    }   

    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public virtual ICollection<Employee> Employees { get; set; }
}
