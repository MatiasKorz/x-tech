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

namespace INSTITUTO_C.Controllers
{
    public class AlumnosController : Controller
    {
        private readonly UserManager<Persona> _userManager;
        private readonly InstitutoContext _context;

        public AlumnosController(UserManager<Persona> userManager, InstitutoContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Alumnos
        public async Task<IActionResult> Index()
        {
            var alumnos = _userManager.Users.OfType<Alumno>();
            return View(await Task.FromResult(alumnos.ToList()));
        }

        // GET: Alumnos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var alumno = await _userManager.FindByIdAsync(id.ToString()) as Alumno;
            if (alumno == null) return NotFound();

            return View(alumno);
        }

        // GET: Alumnos/Create
        public IActionResult Create()
        {
            ViewData["CarreraId"] = new SelectList(_context.Carreras, "Id", "Nombre");
            return View();
        }

        // POST: Alumnos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Email,Nombre,Apellido,DNI,Telefono,Direccion,Activo,CarreraId")] Alumno alumno)
        {
            if (ModelState.IsValid)
            {







                alumno.NumeroMatricula = AlumnoHelper.GenerarNumMatricula(_context);




              var result = await _userManager.CreateAsync(alumno, "Password1!");
                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));
            }
            ViewData["CarreraId"] = new SelectList(_context.Carreras, "Id", "Nombre", alumno.CarreraId);
            return View(alumno);
        }
        

        // GET: Alumnos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alumno = await _userManager.FindByIdAsync(id.ToString()) as Alumno;
            if (alumno == null)
            {
                return NotFound();
            }
            ViewData["CarreraId"] = new SelectList(_context.Carreras, "Id", "Nombre", alumno.CarreraId);
            return View(alumno);
        }

        // POST: Alumnos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Email,Nombre,Apellido,DNI,Telefono,Direccion,Activo")] Alumno alumno)
        {
            if (id != alumno.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {


                var alumnoEnDb = await _userManager.FindByIdAsync(alumno.Id.ToString()) as Alumno;
                if (alumnoEnDb != null)
                    {

                        if (string.IsNullOrEmpty(alumnoEnDb.NumeroMatricula))
                        {
                            alumnoEnDb.NumeroMatricula = AlumnoHelper.GenerarNumMatricula(_context);
                        }
                        alumnoEnDb.Nombre = alumno.Nombre;
                        alumnoEnDb.Apellido = alumno.Apellido;
                        alumnoEnDb.Direccion = alumno.Direccion;
                        alumnoEnDb.Email = alumno.Email;
                        alumnoEnDb.Telefono = alumno.Telefono;
                        alumnoEnDb.DNI = alumno.DNI;
                        alumnoEnDb.UserName = alumno.UserName;
                        alumnoEnDb.Activo = alumno.Activo;
                    //alumnoEnDb.Carrera = alumno.Carrera; ver que onda

                    var resultado = await _userManager.UpdateAsync(alumnoEnDb);
                    if (resultado.Succeeded)
                        return RedirectToAction(nameof(Index));

                    foreach (var error in resultado.Errors)
                        ModelState.AddModelError("", error.Description);
                }
                else
                    {
                        return NotFound();
                    }


                }
             
            
            ViewData["CarreraId"] = new SelectList(_context.Carreras, "Id", "Nombre", alumno.CarreraId);
            return View(alumno);
        }

        // GET: Alumnos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alumno = await _userManager.FindByIdAsync(id.ToString()) as Alumno;
        
            if (alumno == null)
            {
                return NotFound();
            }

            return View(alumno);
        }

        // POST: Alumnos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var alumno = await _userManager.FindByIdAsync(id.ToString()) as Alumno;
            if (alumno == null) return NotFound();

            var resultado = await _userManager.DeleteAsync(alumno);
            if (!resultado.Succeeded)
            {
                foreach (var error in resultado.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(alumno);
            }

            return RedirectToAction(nameof(Index));



        }
        private async Task<bool> AlumnoExists(int id)
        {
            var alumno = await _userManager.FindByIdAsync(id.ToString()) as Alumno;
            return alumno != null;
        }
    }
}
