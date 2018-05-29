using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace paypart_category_gateway.Models
{
    public class ServiceCategory
    {
        [Key]
        public int id { get; set; }
        public string title { get; set; }
        public int status { get; set; }
        public DateTime created_on { get; set; }
    }
}
