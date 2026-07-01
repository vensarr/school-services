using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Exams.Domain.Entities;

public class Exam
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "char(3)")]
    [Required]
    [MaxLength(3)]
    public string SubjectCode { get; set; } = string.Empty;

    public int StudentNumber { get; set; }

    [Column(TypeName = "date")]
    public DateOnly ExamDate { get; set; }

    [Column(TypeName = "tinyint")]
    public byte Score { get; set; }
}
