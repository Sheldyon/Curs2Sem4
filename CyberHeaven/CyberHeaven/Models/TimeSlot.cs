using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberHeaven.Models
{
    public class TimeSlot
    {
        public string Time { get; set; }
        public bool IsAvailable { get; set; }
        public string DisplayText => $"{Time} {(IsAvailable ? "" : "(занято)")}";
    }
}
