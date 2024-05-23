using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace la_mia_pizzeria_static.Models
{
    [Table("PizzaType")]
    public class PizzaType
    {
        [Key] public int Id { get; set; }
        public string Name { get; set;}
        public List<Pizza> Pizzas { get; set;}

        public PizzaType() { }
    }
}