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
using Microsoft.Data.SqlClient;

namespace INSTITUTO_C.Controllers
{
    [Authorize]
    public class MateriasCursadasController : Controller
    {
        private readonly InstitutoContext _context;
        private readonly UserManager<Persona> _userManager;

        public MateriasCursadasController(InstitutoContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: MateriasCursadas

        [Authorize(Roles = Configs.Empleado)]
        public async Task<IActionResult> Index()
        {

            var cursadas = await _context.MateriasCursadas
               .Include(m => m.Materia)
               .ThenInclude(m => m.Carrera)
               .Include(m => m.Profesor)
               .OrderByDescending(m => m.Activo) 
               .ThenBy(m => m.Nombre)
               .ToListAsync();

            return View("Index",cursadas);
        }



        public async Task<IActionResult> Actuales(int? personaId)
        {

            List<MateriaCursada> cursadas = null;
            if (User.IsInRole(Configs.Empleado))
            {
                cursadas = await _context.MateriasCursadas
                 .Include(m => m.Materia)
                 .Include(m => m.Profesor)
                 .Where(m => m.Activo)
                 .ToListAsync();
            }
            else 
            {
                var usuarioId = Int32.Parse(_userManager.GetUserId(User));
                if (personaId is null)
                {
                    personaId = usuarioId;
                }else if (!User.IsInRole(Configs.Empleado) && personaId != usuarioId)
                {
                    return Content("No podes ver las cursadas de otro");
                }
                if (User.IsInRole(Configs.Profesor))
                {
                    cursadas = await _context.MateriasCursadas
                     .Include(m => m.Materia)
                     .Include(m => m.Profesor)
                     .Where(m=>m.ProfesorId== personaId)
                     .Where(m => m.Activo)
                     .ToListAsync();
                }
                else if(User.IsInRole(Configs.Alumno))
                {
                    cursadas = await _context.Inscripciones
                        .Include(i => i.MateriaCursada)
                            .ThenInclude(mc => mc.Materia)
                        .Include(i => i.MateriaCursada)
                            .ThenInclude(mc => mc.Profesor)
                        .Where(i => i.AlumnoId == personaId && i.MateriaCursada.Activo)
                        .Select(i => i.MateriaCursada)
                        .ToListAsync();
                }

            }


            return View("Index", cursadas);
        }

        public async Task<IActionResult> Finalizadas(int? personaId)
        {

            List<MateriaCursada> cursadas = null;
            if (User.IsInRole(Configs.Empleado))
            {
                cursadas = await _context.MateriasCursadas
                .Include(m => m.Materia)
                .Include(m => m.Profesor)
                .Where(m => !m.Activo)
                .ToListAsync();
            }
            else
            {
                var usuarioId = Int32.Parse(_userManager.GetUserId(User));
                if (personaId is null)
                {
                    personaId = usuarioId;
                }
                else if (!User.IsInRole(Configs.Empleado) && personaId != usuarioId)
                {
                    return Content("No podes ver las cursadas de otro");
                }
                if (User.IsInRole(Configs.Profesor))
                {
                    cursadas = await _context.MateriasCursadas
                     .Include(m => m.Materia)
                     .Include(m => m.Profesor)
                     .Where(m => m.ProfesorId == personaId)
                     .Where(m => !m.Activo)
                     .ToListAsync();
                }
                else if(User.IsInRole(Configs.Alumno))
                {
                    cursadas = await _context.Inscripciones
                        .Include(i => i.MateriaCursada)
                            .ThenInclude(mc => mc.Materia)
                        .Include(i => i.MateriaCursada)
                            .ThenInclude(mc => mc.Profesor)
                        .Where(i => i.AlumnoId == personaId && !i.MateriaCursada.Activo)
                        .Select(i => i.MateriaCursada)
                        .ToListAsync();

                }
            }

            return View("Index", cursadas);
        }


        // GET: MateriasCursadas/Details/5
        public async Task<IActionResult> Details(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materiaCursada = await _context.MateriasCursadas
              .Include(m => m.Materia)
              .Include(m => m.Profesor)
              .Include(m => m.Inscripciones)
              .ThenInclude(i => i.Alumno)
              .Include(m => m.Inscripciones)
              .ThenInclude(i => i.Calificacion)
              .FirstOrDefaultAsync(m => m.Id == id);
            if (materiaCursada == null)
            {
                return NotFound();
            }

            ViewBag.ReturnUrl = returnUrl;

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
                try
                {
                    materiaCursada.Materia = await _context.Materias.FindAsync(materiaCursada.MateriaId);

                    materiaCursada.GenerarNombre();
                    //VerificarNombreValido(materiaCursada);

                    materiaCursada.Activo = true;


                    _context.MateriasCursadas.Add(materiaCursada);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));




                }
                catch (DbUpdateException dbex)
                {
                    ProcesarDuplicado(dbex);
                    CargarViewData(materiaCursada);
                    return View(materiaCursada);

                }
            }
            CargarViewData(materiaCursada);
            return View(materiaCursada);
        }

        private void CargarViewData(MateriaCursada materiaCursada)
        {
            ViewData["MateriaId"] = new SelectList(_context.Materias, "Id", "CodigoMateria", materiaCursada.MateriaId);
            var profesoresActivos = _context.Profesores
             .Where(p => p.Activo)
             .ToList();

            ViewData["ProfesorId"] = new SelectList(profesoresActivos, "Id", "Apellido");
        }

        private void ProcesarDuplicado(DbUpdateException dbex)
        {
            SqlException innerException = dbex.InnerException as SqlException;
            if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601))
            {
                ModelState.AddModelError("Nombre", ErrorMesseges.CursadaDuplicada);
            }
            else
            {
                ModelState.AddModelError(string.Empty, dbex.Message);
            }
        }

        //private void VerificarNombreValido(MateriaCursada cursada)
        //{
        //    if (CursadaNombreExists(cursada.Nombre))
        //    {
        //        ModelState.AddModelError("Nombre", ErrorMesseges.CursadaDuplicada);
        //    }
        //}

        //private bool CursadaNombreExists(string nombre)
        //{
        //    bool resultado = false;
        //    if (!string.IsNullOrEmpty(nombre))
        //    {
        //        resultado = _context.MateriasCursadas.Any(c => c.Nombre == nombre);
        //    }
        //    return resultado;
        //}

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
            CargarViewData(materiaCursada);
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
            CargarViewData(materiaCursada);
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
