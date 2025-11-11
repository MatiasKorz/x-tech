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

            if (User.IsInRole(Configs.Alumno) && alumnoId is null)
            {
                alumnoId = usuarioId;
            }

            if (alumnoId is not null && (alumnoId == usuarioId|| User.IsInRole(Configs.Empleado)))
            {
                //para un alumno especifico
                inscripciones = await _context.Inscripciones
                   .Include(i => i.Alumno)
                  .Include(i => i.MateriaCursada)
                   .Include(i => i.Calificacion)   
                   .Where(i => i.AlumnoId == alumnoId)
                 .ToListAsync();

            }
            else if(User.IsInRole(Configs.Alumno)){
                return Content("No podes ver las inscripciones de otro alumno");
            }
            else
            {
                inscripciones = await _context.Inscripciones
                    .Include(i => i.Alumno)
                    .Include(i => i.MateriaCursada)
                    .Include(i => i.Calificacion)
                    .ToListAsync();
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
        [Authorize(Roles = Configs.Empleado + "," + Configs.Alumno)]
        public IActionResult Create()
        {

            if (User.IsInRole(Configs.Empleado))
            {
                ViewData["AlumnoId"] = new SelectList(_context.Alumnos, "Id", "Apellido");
                ViewData["MateriaCursadaId"] = new SelectList(_context.MateriasCursadas, "Id", "Nombre");
            }
            else
            {

                ViewData["MateriaCursadaId"] = new SelectList(GetCursadasCarrera(), "Id", "Nombre");
            }
       
            return View();
        }

        private IEnumerable<MateriaCursada> GetCursadasCarrera()
        {
            var alumno = _context.Alumnos
             .Include(a => a.Carrera)
             .FirstOrDefault(a => a.Id == int.Parse(_userManager.GetUserId(User)));

            return _context.MateriasCursadas
                    .Include(mc => mc.Materia)
                    .Where(mc => mc.Materia.CarreraId == alumno.CarreraId && mc.Activo)
                    .ToList();
        }

        // POST: Inscripciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Configs.Alumno + "," + Configs.Empleado)]
        public async Task<IActionResult> Create(Inscripcion inscripcion)
        {
         
            if (User.IsInRole(Configs.Alumno))
            {
                inscripcion.AlumnoId = int.Parse(_userManager.GetUserId(User));

            }

            var alumno = await _context.Alumnos.FindAsync(inscripcion.AlumnoId);

            var materiaCursada = await _context.MateriasCursadas
              .Include(mc => mc.Materia)
              .ThenInclude(m => m.Carrera)
              .FirstOrDefaultAsync(mc => mc.Id == inscripcion.MateriaCursadaId);

            if (alumno == null || !alumno.Activo)
                return Content(ErrorMesseges.AlumnoInactivo);

            bool yaInscripto = await _context.Inscripciones
              .AnyAsync(i => i.AlumnoId == inscripcion.AlumnoId && i.MateriaCursadaId == inscripcion.MateriaCursadaId);

            if (yaInscripto)
                return Content(ErrorMesseges.AlumnoEnCursada);


            if (materiaCursada.Materia.CarreraId != alumno.CarreraId)
                return Content(ErrorMesseges.AlumnoNoCarrera);

            _context.Inscripciones.Add(inscripcion);
            await _context.SaveChangesAsync();

            // Creo una calificacion con nota pendiente

            var calificacion = new Calificacion
            {
                AlumnoId = inscripcion.AlumnoId,
                MateriaCursadaId = inscripcion.MateriaCursadaId,
                Fecha = DateTime.Now,
                Nota = Nota.Pendiente, 
                ProfesorId = materiaCursada.ProfesorId 
            };

            _context.Calificaciones.Add(calificacion);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { alumnoId = inscripcion.AlumnoId });
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

        // POST: Inscripciones/Baja
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Configs.Alumno)]
        public async Task<IActionResult> Baja(int materiaCursadaId)
        {
            var usuarioId = int.Parse(_userManager.GetUserId(User));

            // Buscar inscripción del alumno logueado
            var inscripcion = await _context.Inscripciones
                .FirstOrDefaultAsync(i => i.AlumnoId == usuarioId && i.MateriaCursadaId == materiaCursadaId);

            if (inscripcion == null)
                return NotFound("No estás inscripto en esa materia.");

            // Buscar la calificación correspondiente
            var calificacion = await _context.Calificaciones
                .FirstOrDefaultAsync(c => c.AlumnoId == usuarioId && c.MateriaCursadaId == materiaCursadaId);

            if (calificacion == null)
                return Content("No se encontró la calificación asociada.");

            if (calificacion.Nota != Nota.Pendiente)
                return Content("No podés darte de baja porque ya tenés una nota final asignada.");

            // Marcar la calificación como Baja
            calificacion.Nota = Nota.Baja;
            _context.Update(calificacion);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Te diste de baja correctamente.";
            return RedirectToAction(nameof(Index));
        }

        private bool InscripcionExists(int id)
        {
            return _context.Inscripciones.Any(e => e.AlumnoId == id);
        }
    }
}
