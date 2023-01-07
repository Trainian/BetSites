using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class Settings
    {
        [Phone]
        public string PhoneNumber { get; set; } = "+7";
        public string Password { get; set; } = "";
        public int Scrolls { get; set; } = 1;
    }
}
