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
            var alumnos = await _context.Alumnos
                .Include(a => a.Carrera)
                .ToListAsync();

            return View(alumnos);
        }

        // GET: Alumnos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var alumno = await _context.Alumnos
                .Include(a => a.Carrera)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (alumno == null) return NotFound();

            return View(alumno);
        }

        // GET: Alumnos/Create
        [Authorize(Roles = Configs.Empleado)]
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
        [Authorize(Roles = Configs.Empleado)]
        public async Task<IActionResult> Create([Bind("Id,UserName,Email,Nombre,Apellido,DNI,Telefono,Direccion,Activo,CarreraId")] Alumno alumno)
        {
            VerificarDNIValido(alumno);
            VerificarEmailValido(alumno);

            if (ModelState.IsValid)
            {






                alumno.UserName = alumno.Email;
                alumno.NumeroMatricula = PersonasHelper.GenerarNumMatricula(_context);




              var resultAgregar = await _userManager.CreateAsync(alumno, Configs.Password);
                if (resultAgregar.Succeeded)
                    if (resultAgregar.Succeeded)
                    {
                        var resultadoAddRole = await _userManager.AddToRoleAsync(alumno, Configs.Alumno);

                        if (resultadoAddRole.Succeeded)
                        {
                            return RedirectToAction("Index", "Alumnos");
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
            ViewData["CarreraId"] = new SelectList(_context.Carreras, "Id", "Nombre", alumno.CarreraId);
            return View(alumno);
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

        // GET: Alumnos/Edit/5
        [Authorize(Roles = Configs.Empleado)]
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
        [Authorize(Roles = Configs.Empleado)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Email,Nombre,Apellido,DNI,Telefono,Direccion,Activo")] Alumno alumno)
        {
            if (id != alumno.Id)
            {
                return NotFound();
            }


            var alumnoEnDb = await _userManager.FindByIdAsync(alumno.Id.ToString()) as Alumno;

            if (alumno.DNI != alumnoEnDb.DNI)
            {
                VerificarDNIValido(alumno);
            }
            
            //VerificarEmailValido(alumno);
            if (ModelState.IsValid)
            {



                if (alumnoEnDb != null)
                    {

                        if (string.IsNullOrEmpty(alumnoEnDb.NumeroMatricula))
                        {
                            alumnoEnDb.NumeroMatricula = PersonasHelper.GenerarNumMatricula(_context);
                        }
                        alumnoEnDb.Nombre = alumno.Nombre;
                        alumnoEnDb.Apellido = alumno.Apellido;
                        alumnoEnDb.Direccion = alumno.Direccion;
                        alumnoEnDb.Telefono = alumno.Telefono;
                        alumnoEnDb.DNI = alumno.DNI;
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
                return NotFound();

            var alumno = await _context.Alumnos
                .Include(a => a.Carrera)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (alumno == null)
                return NotFound();

            return View(alumno);
        }


        // POST: Alumno/DeleteConfirmed/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var alumno = await _context.Alumnos.FindAsync(id);

            _context.Alumnos.Remove(alumno);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        private async Task<bool> AlumnoExists(int id)
        {
            var alumno = await _userManager.FindByIdAsync(id.ToString()) as Alumno;
            return alumno != null;
        }
    }
}
