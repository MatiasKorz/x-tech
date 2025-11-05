using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using INSTITUTO_C.Data;
using INSTITUTO_C.Models;
using Microsoft.AspNetCore.Authorization;
using INSTITUTO_C.Helpers;
using Microsoft.AspNetCore.Identity;

namespace INSTITUTO_C.Controllers
{
    [Authorize]
    public class InscripcionesController : Controller
    {
        private readonly InstitutoContext _context;
        private readonly UserManager<Persona> _userManager;

        public InscripcionesController(InstitutoContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Inscripciones
        public async Task<IActionResult> Index(int? alumnoId)
        {
            var usuarioId = Int32.Parse( _userManager.GetUserId(User));

            List<Inscripcion> inscripciones = null;
            if (alumnoId is not null && (alumnoId == usuarioId|| User.IsInRole("Empleado")))
            {
                //para un alumno especifico
                inscripciones = await _context.Inscripciones.Include(i => i.Alumno).Include(i => i.MateriaCursada)
                    .Where(i => i.AlumnoId == alumnoId)
                    .ToListAsync(); 
                
            }
            else if(User.IsInRole("Alumno")){
                return Content("No podes ver las inscripciones de otro alumno");
            }
            else
            {
                inscripciones = await _context.Inscripciones.Include(i => i.Alumno).Include(i => i.MateriaCursada).ToListAsync();
            }
            return View(inscripciones);
        }

        // GET: Inscripciones/Details/5
        public async Task<IActionResult> Details(int alumnoId, int materiaCursadaId)
        {
            var inscripcion = await _context.Inscripciones
                .Include(i => i.Alumno)
                .Include(i => i.MateriaCursada)
                .FirstOrDefaultAsync(i => i.AlumnoId == alumnoId && i.MateriaCursadaId == materiaCursadaId);

            if (inscripcion == null)
                return NotFound();

            return View(inscripcion);
        }

        // GET: Inscripciones/Create
        [Authorize(Roles = Configs.Alumno)]
        public IActionResult Create()
        {
          //  ViewData["AlumnoId"] = new SelectList(_context.Alumnos, "Id", "Apellido");
            ViewData["MateriaCursadaId"] = new SelectList(_context.MateriasCursadas, "Id", "Nombre");
            return View();
        }

        // POST: Inscripciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Configs.Alumno)]
        public async Task<IActionResult> Create([Bind("MateriaCursadaId")] Inscripcion inscripcion)
        {
            var user = await _userManager.GetUserAsync(User);
            inscripcion.AlumnoId = user.Id;
            var alumno = await _context.Alumnos.FindAsync(user.Id);
            if (alumno == null || !alumno.Activo)
            {
                TempData["Error"] = "No podés inscribirte porque no estás activo.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                _context.Inscripciones.Add(inscripcion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           // ViewData["AlumnoId"] = new SelectList(_context.Alumnos, "Id", "Apellido", inscripcion.AlumnoId);
            ViewData["MateriaCursadaId"] = new SelectList(_context.MateriasCursadas, "Id", "Nombre", inscripcion.MateriaCursadaId);
            return View(inscripcion);
        }

        // GET: Inscripciones/Edit/5
        public async Task<IActionResult> Edit(int alumnoId, int materiaCursadaId)
        {
            var inscripcion = await _context.Inscripciones
             .Include(i => i.Alumno)
             .Include(i => i.MateriaCursada)
             .FirstOrDefaultAsync(i => i.AlumnoId == alumnoId && i.MateriaCursadaId == materiaCursadaId);

            if (inscripcion == null)
            {
                return NotFound();
            }
            //ViewData["AlumnoId"] = new SelectList(_context.Alumnos, "Id", "Apellido", inscripcion.AlumnoId);
            ViewData["MateriaCursadaId"] = new SelectList(_context.MateriasCursadas, "Id", "Nombre", inscripcion.MateriaCursadaId);
            return View(inscripcion);
        }

        // POST: Inscripciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int alumnoId, int materiaCursadaId, [Bind("MateriaCursadaId,AlumnoId")] Inscripcion inscripcion)
        {
            if (alumnoId != inscripcion.AlumnoId || materiaCursadaId != inscripcion.MateriaCursadaId)
                return NotFound();


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inscripcion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var exists = await _context.Inscripciones.AnyAsync(i =>
                        i.AlumnoId == inscripcion.AlumnoId && i.MateriaCursadaId == inscripcion.MateriaCursadaId);

                    if (!exists)
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlumnoId"] = new SelectList(_context.Alumnos, "Id", "Apellido", inscripcion.AlumnoId);
            ViewData["MateriaCursadaId"] = new SelectList(_context.MateriasCursadas, "Id", "Nombre", inscripcion.MateriaCursadaId);
            return View(inscripcion);
        }

        // GET: Inscripciones/Delete/5
        public async Task<IActionResult> Delete(int alumnoId, int materiaCursadaId)
        {
            var inscripcion = await _context.Inscripciones
                .Include(i => i.Alumno)
                .Include(i => i.MateriaCursada)
                .FirstOrDefaultAsync(i => i.AlumnoId == alumnoId && i.MateriaCursadaId == materiaCursadaId);

            if (inscripcion == null)
                return NotFound();

            return View(inscripcion);
        }

        // POST: Inscripciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int alumnoId, int materiaCursadaId)
        {
            var inscripcion = await _context.Inscripciones.FindAsync(alumnoId, materiaCursadaId);
            if (inscripcion != null)
                _context.Inscripciones.Remove(inscripcion);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InscripcionExists(int id)
        {
            return _context.Inscripciones.Any(e => e.AlumnoId == id);
        }
    }
}
