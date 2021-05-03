using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DAL.Models
{
    public class Profile
    {
        [ForeignKey("Client")]
        [Key]

        public int Id { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }

        public virtual Client Client { get; set; }
    }
}
