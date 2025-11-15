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
    public class EmpleadosController : Controller
    {
        private readonly InstitutoContext _context;
        private readonly UserManager<Persona> _userManager;

        public EmpleadosController(InstitutoContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Empleados
        public async Task<IActionResult> Index()
        {
            var empleados = _userManager.Users.OfType<Empleado>();
            return View(await Task.FromResult(empleados.ToList()));
        }

        // GET: Empleados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var empleado = await _userManager.FindByIdAsync(id.ToString()) as Empleado;
            if (empleado == null) return NotFound();
            return View(empleado);
        }

        // GET: Empleados/Create
        [Authorize(Roles = Configs.Empleado)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empleados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Configs.Empleado)]
        public async Task<IActionResult> Create([Bind("Id,UserName,Email,Nombre,Apellido,DNI,Telefono,Direccion,Activo")] Empleado empleado)
        {

            VerificarDNIValido(empleado);
            VerificarEmailValido(empleado);
            if (ModelState.IsValid)
            {

                empleado.UserName = empleado.Email;

                empleado.Legajo = PersonasHelper.GenerarLegajo(_context);




                var resultAgregar = await _userManager.CreateAsync(empleado, Configs.Password);
                if (resultAgregar.Succeeded)
                { 
                    var resultadoAddRole = await _userManager.AddToRoleAsync(empleado, Configs.Empleado);

                if (resultadoAddRole.Succeeded)
                {
                    return RedirectToAction("Index", "Empleados");
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
            return View(empleado);
        }


        private void VerificarDNIValido(Persona persona)
        {
            if (PersonasHelper.PersonaDNIExists(_context, persona.DNI))
            {
                ModelState.AddModelError("DNI", ErrorMesseges.DNIExistente);
            }
        }

        private void VerificarEmailValido(Persona persona)
        {
            if (PersonasHelper.PersonaEmailExists(_context, persona.Email))
            {
                ModelState.AddModelError("Email", ErrorMesseges.EmailExistente);

            }
        }

        // GET: Empleados/Edit/5
        [Authorize(Roles = Configs.Empleado)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _userManager.FindByIdAsync(id.ToString()) as Empleado;
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        // POST: Empleados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Configs.Empleado)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Email,Nombre,Apellido,DNI,Telefono,Direccion,Activo")] Empleado empleado)
        {
            if (id != empleado.Id)
            {
                return NotFound();
            }


            var empleadoEnDB = await _userManager.FindByIdAsync(empleado.Id.ToString()) as Empleado;

            if (empleado.DNI != empleadoEnDB.DNI)
            {
                VerificarDNIValido(empleado);
            }

            //VerificarEmailValido(empleado);
            if (ModelState.IsValid)
            {
               

                    if (empleadoEnDB != null)
                    {

                        if (string.IsNullOrEmpty(empleadoEnDB.Legajo))
                        {
                            empleadoEnDB.Legajo = PersonasHelper.GenerarLegajo(_context);
                        }
                        empleadoEnDB.Nombre = empleado.Nombre;
                        empleadoEnDB.Apellido = empleado.Apellido;
                        empleadoEnDB.Direccion = empleado.Direccion;
                        empleadoEnDB.Telefono = empleado.Telefono;
                        empleadoEnDB.DNI = empleado.DNI;
                        empleadoEnDB.Activo = empleado.Activo;



                    var resultado = await _userManager.UpdateAsync(empleadoEnDB);
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
            return View(empleado);
        }

        // GET: Empleados/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var empleado = await _userManager.FindByIdAsync(id.ToString()) as Empleado;

        //    if (empleado == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(empleado);
        //}

        //// POST: Empleados/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var empleado = await _userManager.FindByIdAsync(id.ToString()) as Empleado;
        //    if (empleado == null) return NotFound();

        //    var resultado = await _userManager.DeleteAsync(empleado);
        //    if (!resultado.Succeeded)
        //    {
        //        foreach (var error in resultado.Errors)
        //            ModelState.AddModelError("", error.Description);
        //        return View(empleado);
        //    }
          
        //    return RedirectToAction(nameof(Index));
        //}

        private async Task<bool> EmpleadoExists(int id)
        {
            var empleado = await _userManager.FindByIdAsync(id.ToString()) as Empleado;
            return empleado != null;
        }
    }
}
