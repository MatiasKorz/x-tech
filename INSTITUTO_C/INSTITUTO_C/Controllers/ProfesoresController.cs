using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using INSTITUTO_C.Data;
using INSTITUTO_C.Models;
using INSTITUTO_C.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace INSTITUTO_C.Controllers
{
    [Authorize]
    public class ProfesoresController : Controller
    {
        private readonly InstitutoContext _context;
        private readonly UserManager<Persona> _userManager;

        public ProfesoresController(InstitutoContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Profesores
        [AllowAnonymous]

        public async Task<IActionResult> Index()
        {
            var profesores = _userManager.Users.OfType<Profesor>();
            return View(await Task.FromResult(profesores.ToList()));
        }

        // GET: Profesores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var profesor = await _userManager.FindByIdAsync(id.ToString()) as Profesor;
            if (profesor == null) return NotFound();
            return View(profesor);
        }

        // GET: Profesores/Create
        [Authorize(Roles = Configs.Empleado)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Profesores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Configs.Empleado)]
        public async Task<IActionResult> Create([Bind("Id,UserName,Email,Nombre,Apellido,DNI,Telefono,Direccion,Activo")] Profesor profesor)
        {

            VerificarDNIValido(profesor);
            if (ModelState.IsValid)
            {
                profesor.UserName = profesor.Email;
                profesor.Legajo = PersonasHelper.GenerarLegajo(_context);

                var resultAgregar = await _userManager.CreateAsync(profesor, Configs.Password);
                if (resultAgregar.Succeeded)
                {

                    var resultadoAddRole = await _userManager.AddToRoleAsync(profesor, Configs.Profesor);

                    if (resultadoAddRole.Succeeded)
                    {
                        return RedirectToAction("Index", "Profesores");
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
            return View(profesor);
        }


        private void VerificarDNIValido(Persona persona)
        {
            if (PersonaDNIExists(persona.DNI))
            {
                ModelState.AddModelError("DNI", ErrorMesseges.DNIExistente);
            }
        }

        private bool PersonaDNIExists(string DNI)
        {
            bool resultado = false;
            if (!string.IsNullOrEmpty(DNI))
            {
                resultado = _context.Personas.Any(p => p.DNI == DNI);
            }
            return resultado;
        }

        // GET: Profesores/Edit/5
        [Authorize(Roles = Configs.Empleado)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profesor = await _userManager.FindByIdAsync(id.ToString()) as Profesor;
            if (profesor == null)
            {
                return NotFound();
            }
            return View(profesor);
        }

        // POST: Profesores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Configs.Empleado)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Email,Nombre,Apellido,DNI,Telefono,Direccion,Activo")] Profesor profesor)
        {
            if (id != profesor.Id)
            {
                return NotFound();
            }
            VerificarDNIValido(profesor);
            if (ModelState.IsValid)
            {
                
                    var profesorEnDB = await _userManager.FindByIdAsync(profesor.Id.ToString()) as Profesor;
                    if (profesorEnDB != null)
                    {

                        if (string.IsNullOrEmpty(profesorEnDB.Legajo))
                        {
                            profesorEnDB.Legajo = PersonasHelper.GenerarLegajo(_context);
                        }
                        profesorEnDB.Nombre = profesor.Nombre;
                        profesorEnDB.Apellido = profesor.Apellido;
                        profesorEnDB.Direccion = profesor.Direccion;
                        profesorEnDB.Telefono = profesor.Telefono;
                        profesorEnDB.DNI = profesor.DNI;
                        profesorEnDB.Activo = profesor.Activo;



                        var resultado = await _userManager.UpdateAsync(profesorEnDB);
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
            return View(profesor);
        }
        


        // GET: Profesores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profesor = await _userManager.FindByIdAsync(id.ToString()) as Profesor;
      
            if (profesor == null)
            {
                return NotFound();
            }

            return View(profesor);
        }

        // POST: Profesores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var profesor = await _userManager.FindByIdAsync(id.ToString()) as Profesor;
            if (profesor == null) return NotFound();

            var resultado = await _userManager.DeleteAsync(profesor);
            if (!resultado.Succeeded)
            {
                foreach (var error in resultado.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(profesor);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ProfesorExists(int id)
        {
            var profesor = await _userManager.FindByIdAsync(id.ToString()) as Profesor;
            return profesor != null;
        }
    }
}