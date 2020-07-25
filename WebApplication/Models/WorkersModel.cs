using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workers.Models
{
    public class WorkersModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Please specify the name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please specify the surname")]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Birthday")]
        [DataType(DataType.Date)]
        [Display(Name = "Birthday")]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "Please specify the Email Adress")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(30)]
        [Index("This login already exist", IsUnique=true)]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Please specify the password")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "PasswordMin")]
        [MaxLength(20, ErrorMessage = "PasswordMax")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please specify the place of work")]
        [Display(Name = "Place")]
        public string Place { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }

        public string Culture { get; set; }
    }
}
