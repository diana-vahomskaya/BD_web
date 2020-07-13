using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Workers.Models
{
    public class WorkersModel
    {
        public int Id { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "День Рождения")]
        public DateTime Birthday { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Почтовый адрес")]
        public string Email { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Место работы")]
        public string Place { get; set; }

    }
}
