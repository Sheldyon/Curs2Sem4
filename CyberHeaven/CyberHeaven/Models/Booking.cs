using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberHeaven.Models
{
    [Table("Bookings")]
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PlaceId { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } // "Подтверждено", "Отменено", "Завершено"
        [Column(TypeName = "nvarchar(100)")] // Указываем тип и длину
        public string? PromoCodeUsed { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("PlaceId")]
        public Place Place { get; set; }
        public DateTime SelectedDate { get; set; }
        // Навигационные свойства

    }
}
