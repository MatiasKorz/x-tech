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

namespace INSTITUTO_C.Controllers
{
    [Authorize]
    public class MateriasCursadasController : Controller
    {
        private readonly InstitutoContext _context;

        public MateriasCursadasController(InstitutoContext context)
        {
            _context = context;
        }

        // GET: MateriasCursadas
        public async Task<IActionResult> Index()
        {
            var institutoContext = _context.MateriasCursadas.Include(m => m.Materia).Include(m => m.Profesor);
            return View(await institutoContext.ToListAsync());
        }

        // GET: MateriasCursadas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materiaCursada = await _context.MateriasCursadas
                .Include(m => m.Materia)
                .Include(m => m.Profesor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (materiaCursada == null)
            {
                return NotFound();
            }

            return View(materiaCursada);
        }

        // GET: MateriasCursadas/Create
        [Authorize(Roles = Configs.Empleado)]
        public IActionResult Create()
        {
            ViewData["MateriaId"] = new SelectList(_context.Materias, "Id", "CodigoMateria");
            var profesoresActivos = _context.Profesores
            .Where(p => p.Activo)
            .ToList();

            ViewData["ProfesorId"] = new SelectList(profesoresActivos, "Id", "Apellido");
            return View();
        }

        // POST: MateriasCursadas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Configs.Empleado)]
        public async Task<IActionResult> Create([Bind("Id,MateriaId,CodigoCursada,Cuatrimestre,Activo,ProfesorId")] MateriaCursada materiaCursada)
        {
            if (ModelState.IsValid)
            {
                materiaCursada.Materia = await _context.Materias.FindAsync(materiaCursada.MateriaId);

                materiaCursada.GenerarNombre();

                _context.MateriasCursadas.Add(materiaCursada);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MateriaId"] = new SelectList(_context.Materias, "Id", "CodigoMateria", materiaCursada.MateriaId);
            var profesoresActivos = _context.Profesores
             .Where(p => p.Activo)
             .ToList();

            ViewData["ProfesorId"] = new SelectList(profesoresActivos, "Id", "Apellido");
            return View(materiaCursada);
        }

        // GET: MateriasCursadas/Edit/5
        [Authorize(Roles = Configs.Empleado)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materiaCursada = await _context.MateriasCursadas.FindAsync(id);
            if (materiaCursada == null)
            {
                return NotFound();
            }
            ViewData["MateriaId"] = new SelectList(_context.Materias, "Id", "CodigoMateria", materiaCursada.MateriaId);
            var profesoresActivos = _context.Profesores
             .Where(p => p.Activo)
             .ToList();

            ViewData["ProfesorId"] = new SelectList(profesoresActivos, "Id", "Apellido");
            return View(materiaCursada);
        }

        // POST: MateriasCursadas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Configs.Empleado)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MateriaId,CodigoCursada,Cuatrimestre,Activo,ProfesorId")] MateriaCursada materiaCursada)
        {
            if (id != materiaCursada.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var materiaCursadaEnDB = _context.MateriasCursadas.Find(materiaCursada.Id);
                    if(materiaCursadaEnDB != null)
                    {

                        materiaCursadaEnDB.Activo = materiaCursada.Activo;
                        materiaCursadaEnDB.ProfesorId = materiaCursada.ProfesorId;








                        
                        await _context.SaveChangesAsync();
                    } else
                    {
                        return NotFound();
                    }
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MateriaCursadaExists(materiaCursada.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            // ViewData["MateriaId"] = new SelectList(_context.Materias, "Id", "CodigoMateria", materiaCursada.MateriaId);
            var profesoresActivos = _context.Profesores
               .Where(p => p.Activo)
               .ToList();

            ViewData["ProfesorId"] = new SelectList(profesoresActivos, "Id", "Apellido");
            return View(materiaCursada);
        }

        // GET: MateriasCursadas/Delete/5
        [Authorize(Roles = Configs.Empleado)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materiaCursada = await _context.MateriasCursadas
                .Include(m => m.Materia)
                .Include(m => m.Profesor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (materiaCursada == null)
            {
                return NotFound();
            }

            return View(materiaCursada);
        }

        // POST: MateriasCursadas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Configs.Empleado)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var materiaCursada = await _context.MateriasCursadas
               .Include(mc => mc.Inscripciones)
               .FirstOrDefaultAsync(mc => mc.Id == id);

            if (materiaCursada == null)
            {

                return NotFound();
            }

            if (materiaCursada.Inscripciones != null && materiaCursada.Inscripciones.Any())
            {
                TempData["Error"] = ErrorMesseges.CursadaConInscripciones;
                return RedirectToAction(nameof(Index));
            }

            _context.MateriasCursadas.Remove(materiaCursada);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MateriaCursadaExists(int id)
        {
            return _context.MateriasCursadas.Any(e => e.Id == id);
        }
    }
}
