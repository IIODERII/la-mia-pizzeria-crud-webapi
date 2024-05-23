using la_mia_pizzeria_static;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace NetCore.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PizzasController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllPizzas(string name = "")
        {
            using (PizzaContext db = new PizzaContext())
            {
                if (name == "")
                {
                    IQueryable<Pizza> pizze = db.Pizzas;
                    return Ok(pizze.ToList());
                }
                IQueryable<Pizza> pizzeF = db.Pizzas.Where(p => p.Name.Contains(name));
                return Ok(pizzeF.ToList());

            }
        }

        [HttpGet]
        public IActionResult GetPizzaById(int id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                IQueryable<Pizza> pizza = db.Pizzas.Where(p =>p.Id == id);
                return Ok(pizza.ToList());
            }
        }
        [HttpPost]
        public IActionResult CreatePizza([FromBody] Pizza data)
        {
            Pizza pizzatoAdd = new Pizza();
            pizzatoAdd.Name = data.Name;
            pizzatoAdd.Description = data.Description;
            pizzatoAdd.Image = data.Image;
            pizzatoAdd.Price = data.Price;
            pizzatoAdd.TypeId = data.TypeId;

            using(PizzaContext db = new PizzaContext())
            {
                db.Pizzas.Add(pizzatoAdd);
                db.SaveChanges();
            }
            return Ok();
        }

        [HttpPost]
        public IActionResult UpdatePizza(int id, [FromBody] Pizza data) { 
            using(PizzaContext db = new PizzaContext())
            {
                Pizza pizzatoAdd = db.Pizzas.Where(p => p.Id == id).FirstOrDefault();
                pizzatoAdd.Name = data.Name;
                pizzatoAdd.Description = data.Description;
                pizzatoAdd.Image = data.Image;
                pizzatoAdd.Price = data.Price;
                pizzatoAdd.TypeId = data.TypeId;

                db.SaveChanges();
            }
            return Ok();
        }

        [HttpPost]
        public IActionResult DeleteFromId(int id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                Pizza toDelete = db.Pizzas.Where(p => p.Id == id).FirstOrDefault();

                if (toDelete != null)
                {
                    db.Pizzas.Remove(toDelete);
                    db.SaveChanges();
                    return Ok();
                }

                return NotFound();
            }
        }
    }
}
