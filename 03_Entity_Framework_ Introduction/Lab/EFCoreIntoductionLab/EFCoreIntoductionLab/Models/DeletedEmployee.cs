using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EFCoreIntoductionLab.Models
{
    [Table("Deleted_Employees")]
    public partial class DeletedEmployee
    {
        [Key]
        public int EmployeeId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string FirstName { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string LastName { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string? MiddleName { get; set; }
        [StringLength(150)]
        [Unicode(false)]
        public string JobTitle { get; set; } = null!;
        public int DepartmentId { get; set; }
        [Column(TypeName = "money")]
        public decimal Salary { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        [InverseProperty("DeletedEmployees")]
        public virtual Department Department { get; set; } = null!;
    }
}
