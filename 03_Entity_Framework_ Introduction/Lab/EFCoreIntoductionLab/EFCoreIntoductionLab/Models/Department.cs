using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EFCoreIntoductionLab.Models
{
    public partial class Department
    {
        public Department()
        {
            DeletedEmployees = new HashSet<DeletedEmployee>();
            Employees = new HashSet<Employee>();
        }

        [Key]
        [Column("DepartmentID")]
        public int DepartmentId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column("ManagerID")]
        public int ManagerId { get; set; }

        [ForeignKey(nameof(ManagerId))]
        [InverseProperty(nameof(Employee.Departments))]
        public virtual Employee Manager { get; set; } = null!;
        [InverseProperty(nameof(DeletedEmployee.Department))]
        public virtual ICollection<DeletedEmployee> DeletedEmployees { get; set; }
        [InverseProperty(nameof(Employee.Department))]
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
