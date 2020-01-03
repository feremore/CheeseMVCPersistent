using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace CheeseMVC.Models
{
    public class Cheese
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ID { get; set; }
        //[ForeignKey("CheeseCategory")]
        public int CategoryID { get; set; }
        public CheeseCategory Category { get; set; }

    }
}
