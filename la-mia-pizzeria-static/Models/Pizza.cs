using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace la_mia_pizzeria_static.Models
{
    public class PizzaFormModel
    {
        public Pizza Pizza { get; set; }
        public List<PizzaType>? Types { get; set; }

        public List<SelectListItem>? Ingredients { get; set; }
        public List<string>? SelectedIngredients { get; set; }
    }
    public class Max5WordsOfDescription : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value == null) return null;
            string fieldValue = (string)value;
            string[] words = fieldValue.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            return words.Length <= 5 ? ValidationResult.Success : new ValidationResult("Ella madonna, ma quanto vuoi scrivere? Massimo 5 parole.") ;
        }
    }

    [Table("pizza")]
    public class Pizza
    {
        [Key] public int Id { get; set; }

        [Required(ErrorMessage = "Ehi, non puoi creare una pizza senza nome (poverina)!")]
        [StringLength(50, ErrorMessage = "Amico, cerma un nome meno descrittivo, magari con meno di 50 caratteri")]
        public string Name { get; set; }
        [Required(ErrorMessage = "E i clienti che non sanno come è questa pizza? Dai scrivi qualcosa.")]
        [Max5WordsOfDescription]
        public string Description { get; set; }
        [Required(ErrorMessage = "Che brutta una pizza senza immagine, cerca su google almeno...")]
        public string Image {  get; set; }
        [Required(ErrorMessage = "Certo, senza prezzo come credi di fare i soldi??")]
        [Range(0.01, 100000000, ErrorMessage = "Fammi capire, tu vuoi vendere una pizza gratis oppure dare dei soldi ai clienti. Mi sa che dovresti cambiare lavoro amico...")]
        public double Price { get; set; }


        public int? TypeId { get; set; }
        public PizzaType? Type { get; set; }

        public List<Ingredient>? Ingredients { get; set; }

        public Pizza() { }
    }
}
