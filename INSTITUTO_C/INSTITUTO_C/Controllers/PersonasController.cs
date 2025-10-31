using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using INSTITUTO_C.Data;
using INSTITUTO_C.Models;
using Microsoft.AspNetCore.Identity;
using INSTITUTO_C.Helpers;

namespace INSTITUTO_C.Controllers
{
    public class PersonasController : Controller
    {
        private readonly UserManager<Persona> _userManager;

        public PersonasController(UserManager<Persona> userManager)
        {
            _userManager = userManager;
        }

        // GET: Personas
        public async Task<IActionResult> Index()
        {

            
            return View(await _userManager.Users.ToListAsync());
        }

        // GET: Personas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _userManager.FindByIdAsync(id.ToString());

            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // GET: Personas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Personas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Email,Nombre,Apellido,DNI,Telefono,Direccion,Activo")] Persona persona)
        {

            if (ModelState.IsValid) {
                persona.UserName = persona.Email;
            var resultAgregar = await _userManager.CreateAsync(persona, Configs.Password);
            if (resultAgregar.Succeeded)
            {
                var resultadoAddRole = await _userManager.AddToRoleAsync(persona, Configs.Usuario);

                if (resultadoAddRole.Succeeded)
                {
                    return RedirectToAction("Index", "Personas");
                }
                else
                {
                    return Content("No se pudo agregar rol");
                }

            }
            foreach (var error in resultAgregar.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return RedirectToAction(nameof(Index));
        }
            return View(persona);
        }

        // GET: Personas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _userManager.FindByIdAsync(id.ToString());
            if (persona == null)
            {
                return NotFound();
            }
            return View(persona);
        }

        // POST: Personas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Email,Nombre,Apellido,DNI,Telefono,Direccion,Activo")] Persona persona)
        {
            if (id != persona.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
               
                    var personaEnDb = await _userManager.FindByIdAsync(persona.Id.ToString());
                    if (personaEnDb != null)
                    {
                        personaEnDb.Nombre = persona.Nombre;
                        personaEnDb.Apellido = persona.Apellido;
                        personaEnDb.Direccion = persona.Direccion;
                        personaEnDb.Telefono = persona.Telefono;
                        personaEnDb.DNI = persona.DNI;
                        personaEnDb.Activo = persona.Activo;

                        var resultado = await _userManager.UpdateAsync(personaEnDb);
                        if (resultado.Succeeded)
                            return RedirectToAction(nameof(Index));

                        foreach (var error in resultado.Errors)
                            ModelState.AddModelError("", error.Description);
                    }
                    else
                    {
                        return NotFound();
                    }


                    
               
                return RedirectToAction(nameof(Index));
            }
            return View(persona);
        }

        // GET: Personas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _userManager.FindByIdAsync(id.ToString());
   
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var persona = await _userManager.FindByIdAsync(id.ToString());
            if (persona != null)
            {
                var resultado = await _userManager.DeleteAsync(persona);
                if (!resultado.Succeeded)
                {
                    foreach (var error in resultado.Errors)
                        ModelState.AddModelError("", error.Description);
                    return View(persona);
                }
            }

            else
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> PersonaExists(int id)
        {
            var persona = await _userManager.FindByIdAsync(id.ToString());
            return persona != null;
        }
    }
}
