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

namespace INSTITUTO_C.Controllers
{
    [Authorize]
    public class CalificacionesController : Controller
    {
        private readonly InstitutoContext _context;

        public CalificacionesController(InstitutoContext context)
        {
            _context = context;
        }

        // GET: Calificaciones
        public async Task<IActionResult> Index()
        {
            var calificaciones = await _context.Calificaciones
                .Include(c => c.Profesor)
                .Include(c => c.Alumno)
                .Include(c => c.Inscripcion)
                    .ThenInclude(i => i.MateriaCursada)
                .ToListAsync();

            return View(calificaciones);
        }

        // GET: Calificaciones/Details/5
        public async Task<IActionResult> Details(int alumnoId, int materiaCursadaId)
        {
            var calificacion = await _context.Calificaciones
                .Include(c => c.Alumno)
                .Include(c => c.Profesor)
                .Include(c => c.Inscripcion)
                .ThenInclude(i => i.MateriaCursada)
                .FirstOrDefaultAsync(c => c.AlumnoId == alumnoId && c.MateriaCursadaId == materiaCursadaId);

            if (calificacion == null)
            {
                return NotFound();
            }

            return View(calificacion);
        }

        // GET: Calificaciones/Create
        public IActionResult Create()
        {

            ViewBag.ProfesorId = new SelectList(_context.Profesores, "Id", "Apellido");
            ViewBag.MateriaCursadaId = new SelectList(_context.MateriasCursadas.Include(m => m.Materia), "Id", "Nombre");
            ViewBag.AlumnoId = new SelectList(_context.Alumnos, "Id", "Apellido");
            return View();
        }

        // POST: Calificaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlumnoId,MateriaCursadaId,Fecha,Nota,ProfesorId")] Calificacion calificacion)
        {
            if (ModelState.IsValid)
            {
                _context.Calificaciones.Add(calificacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(calificacion);
        }

        // GET: Calificaciones/Edit/5
        public async Task<IActionResult> Edit(int alumnoId, int materiaCursadaId)
        {
            var calificacion = await _context.Calificaciones
                .Include(c => c.Alumno)
                .Include(c => c.Inscripcion)
                .ThenInclude(i => i.MateriaCursada)
                .FirstOrDefaultAsync(c => c.AlumnoId == alumnoId && c.MateriaCursadaId == materiaCursadaId);

            if (calificacion == null)
            {
                return NotFound();
            }

            ViewBag.ProfesorId = new SelectList(_context.Profesores, "Id", "Apellido", calificacion.ProfesorId);
            ViewBag.MateriaCursadaId = new SelectList(_context.MateriasCursadas.Include(m => m.Materia), "Id", "Nombre", calificacion.MateriaCursadaId);
            ViewBag.AlumnoId = new SelectList(_context.Alumnos, "Id", "Apellido", calificacion.AlumnoId);

            return View(calificacion);
        }

        // POST: Calificaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int alumnoId, int materiaCursadaId, [Bind("AlumnoId,MateriaCursadaId,Nota")] Calificacion calificacion)
        {
            if (alumnoId != calificacion.AlumnoId || materiaCursadaId != calificacion.MateriaCursadaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Solo actualizamos la Nota
                    var calificacionExistente = await _context.Calificaciones
                        .FirstOrDefaultAsync(c => c.AlumnoId == alumnoId && c.MateriaCursadaId == materiaCursadaId);

                    if (calificacionExistente == null)
                        return NotFound();

                    calificacionExistente.Nota = calificacion.Nota;

                    _context.Update(calificacionExistente);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CalificacionExists(calificacion.AlumnoId, calificacion.MateriaCursadaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(calificacion);
        }



        // GET: Calificaciones/Delete
        public async Task<IActionResult> Delete(int alumnoId, int materiaCursadaId)
        {
            var calificacion = await _context.Calificaciones
                .Include(c => c.Alumno)
                .Include(c => c.Profesor)
                .Include(c => c.Inscripcion)
                .ThenInclude(i => i.MateriaCursada)
                .FirstOrDefaultAsync(c => c.AlumnoId == alumnoId && c.MateriaCursadaId == materiaCursadaId);

            if (calificacion == null)
            {
                return NotFound();
            }

            return View(calificacion);
        }

        // POST: Calificaciones/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int alumnoId, int materiaCursadaId)
        {
            var calificacion = await _context.Calificaciones
                .FirstOrDefaultAsync(c => c.AlumnoId == alumnoId && c.MateriaCursadaId == materiaCursadaId);

            if (calificacion != null)
            {
                _context.Calificaciones.Remove(calificacion);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CalificacionExists(int alumnoId, int materiaCursadaId)
        {
            return _context.Calificaciones.Any(c => c.AlumnoId == alumnoId && c.MateriaCursadaId == materiaCursadaId);
        }
    }
}
