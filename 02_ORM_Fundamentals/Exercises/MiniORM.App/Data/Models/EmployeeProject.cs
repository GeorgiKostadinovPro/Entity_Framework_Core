using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniORM.App.Data.Models;

public class EmployeeProject
{
    [Key]
    [ForeignKey(nameof(Employee))]
    public int EmployeeId { get; set; }

    public virtual Employee Employee { get; set; }

    [Key]
    [ForeignKey(nameof(Project))]
    public int ProjectId { get; set; }

    public virtual Employee Project { get; set; }
}
