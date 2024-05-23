using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using la_mia_pizzeria_static;
using la_mia_pizzeria_static.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Composition;
using Microsoft.AspNetCore.Authorization;

namespace la_mia_pizzeria_static.Controllers
{
    [Authorize]
    public class PizzaController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Pizza> pizze = PizzaManager.GetAllPizzas();
            //pizze = [];  //<= era per verificare che funzionasse in caso non ci fossero pizze
            return View(pizze);
        }

        [HttpGet]
        public IActionResult Show(int id)
        {
            return View(PizzaManager.GetPizzaById(id));
        }

        //[Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Create()
        {
            using PizzaContext db = new PizzaContext();
            {
                List<PizzaType> types = db.PizzaTypes.ToList();
                List<Ingredient> ingredients = db.Ingredients.ToList();

                PizzaFormModel model = new PizzaFormModel();
                model.Pizza = new Pizza();
                model.Types = types;

                List<SelectListItem> listIngrediemts = new List<SelectListItem>();

                foreach(Ingredient i in ingredients)
                {
                    listIngrediemts.Add(new SelectListItem()
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
                }

                model.Ingredients = listIngrediemts;
                return View(model);
            }

        }
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public IActionResult Create(PizzaFormModel data)
        {
            if(!ModelState.IsValid)
            {
                using(PizzaContext db = new PizzaContext())
                {
                    data.Types = db.PizzaTypes.ToList();
                    List<Ingredient> ingredients = db.Ingredients.ToList();
                    List<SelectListItem> listIngrediemts = new List<SelectListItem>();

                    foreach (Ingredient i in ingredients)
                    {
                        listIngrediemts.Add(new SelectListItem()
                        {
                            Text = i.Name,
                            Value = i.Id.ToString()
                        });
                    }

                    data.Ingredients = listIngrediemts;
                }
                return View("Create", data);
            }

            Pizza pizzatoAdd = new Pizza();
            pizzatoAdd.Name = data.Pizza.Name;
            pizzatoAdd.Description = data.Pizza.Description;
            pizzatoAdd.Image = data.Pizza.Image;
            pizzatoAdd.Price = data.Pizza.Price;
            pizzatoAdd.TypeId = data.Pizza.TypeId;

            foreach(string selectId in data.SelectedIngredients)
            {
                int selectedIngId = int.Parse(selectId);
                Ingredient i = new Ingredient();
                using (PizzaContext db = new PizzaContext())
                {
                    i = db.Ingredients.Where(ing => ing.Id == selectedIngId).FirstOrDefault();
                }
                pizzatoAdd.Ingredients.Add(i);

            }

            PizzaManager.AddPizza(pizzatoAdd);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Update(int id) {
            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizzaToEdit = PizzaManager.GetPizzaById(id);
                if (pizzaToEdit == null)
                {
                    return NotFound();
                }
                else
                {
                    List<PizzaType> types = db.PizzaTypes.ToList();
                    PizzaFormModel model = new PizzaFormModel();
                    List<Ingredient> ingredients = db.Ingredients.ToList();
                    model.Pizza = pizzaToEdit;
                    model.Types = types;

                    List<SelectListItem> listIngrediemts = new List<SelectListItem>();

                    foreach (Ingredient i in ingredients)
                    {
                        listIngrediemts.Add(new SelectListItem()
                        {
                            Text = i.Name,
                            Value = i.Id.ToString()
                        });
                    }

                    model.Ingredients = listIngrediemts;

                    return View(model);
                }
            }
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public IActionResult Update(int id, PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                using(PizzaContext db = new PizzaContext())
                {
                    data.Types = db.PizzaTypes.ToList();
                    List<Ingredient> ingredients = db.Ingredients.ToList();
                    List<SelectListItem> listIngrediemts = new List<SelectListItem>();

                    foreach (Ingredient i in ingredients)
                    {
                        listIngrediemts.Add(new SelectListItem()
                        {
                            Text = i.Name,
                            Value = i.Id.ToString()
                        });
                    }
                    data.Ingredients = listIngrediemts;
                    return View("Update", data);
                }
            }

            if (PizzaManager.UpdatePizza(data.Pizza, id, data.SelectedIngredients))
            {
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
             if (PizzaManager.DeletePizzaFromId(id))
             {
                 return RedirectToAction("Index");
             }

             return NotFound();
        }
    }
}
