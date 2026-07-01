using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace School.Exams.Domain.Entities;

[Index(nameof(SubjectCode), IsUnique = true)]
public class Subject
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "char(3)")]
    [Required]
    [MaxLength(3)]
    public string SubjectCode { get; set; } = string.Empty;

    [Column(TypeName = "varchar(30)")]
    [Required]
    [MaxLength(30)]
    public string Name { get; set; } = string.Empty;

    [Column(TypeName = "tinyint")]
    public byte Grade { get; set; }

    [Column(TypeName = "varchar(20)")]
    [Required]
    [MaxLength(20)]
    public string TeacherFirstName { get; set; } = string.Empty;

    [Column(TypeName = "varchar(20)")]
    [Required]
    [MaxLength(20)]
    public string TeacherLastName { get; set; } = string.Empty;
}
