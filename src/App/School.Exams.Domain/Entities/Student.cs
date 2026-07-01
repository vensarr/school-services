using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace School.Exams.Domain.Entities;

[Index(nameof(StudentNumber), IsUnique = true)]
public class Student
{
    [Key]
    public int Id { get; set; }

    public int StudentNumber { get; set; }

    [Column(TypeName = "varchar(30)")]
    [Required]
    [MaxLength(30)]
    public string FirstName { get; set; } = string.Empty;

    [Column(TypeName = "varchar(30)")]
    [Required]
    [MaxLength(30)]
    public string LastName { get; set; } = string.Empty;

    [Column(TypeName = "tinyint")]
    public byte Grade { get; set; }
}
