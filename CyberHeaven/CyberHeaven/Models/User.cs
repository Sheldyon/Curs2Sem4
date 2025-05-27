using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberHeaven.Models
{
    [Table("Users")] // Указывает имя таблицы в БД
    public class User
    {
        [Key] // Первичный ключ
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Автоинкремент
        public int Id { get; set; }

        [Required] // Обязательное поле
        [MaxLength(50)] // Ограничение длины
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [RegularExpression(@"^\+?[\d\s\-\(\)]{7,20}$", ErrorMessage = "Invalid phone number")]
        [MaxLength(50)]
        public string Phone { get; set; }

        [Required]
        [EmailAddress] // Валидация email
        public string Email { get; set; }

        [Required]
        [MaxLength(30)]
        public string Username { get; set; }
        [Required]
        [MaxLength(300)]
        public string AvatarPath { get; set; } = "D:\\ООП\\CyberHeaven\\CyberHeaven\\images\\FirstAva.png";

        [Required]
        [MaxLength(100)] // Пароль будет храниться в хешированном виде
        public string Password { get; set; }

        [MaxLength(20)]
        public string Role { get; set; } = "admin"; // Значение по умолчанию
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public bool IsBlocked { get; set; } = false;
        public string? BlockReason { get; set; }
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
