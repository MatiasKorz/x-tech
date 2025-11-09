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
using Microsoft.Data.SqlClient;

namespace INSTITUTO_C.Controllers
{
    [Authorize]
    public class MateriasController : Controller
    {
        private readonly InstitutoContext _context;

        public MateriasController(InstitutoContext context)
        {
            _context = context;
        }

        // GET: Materias
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var institutoContext = _context.Materias.Include(m => m.Carrera);
            return View(await institutoContext.ToListAsync());
        }

        // GET: Materias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materia = await _context.Materias
                .Include(m => m.Carrera)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (materia == null)
            {
                return NotFound();
            }

            return View(materia);
        }

        // GET: Materias/Create
        [Authorize(Roles = Configs.Empleado)]
        public IActionResult Create()
        {
            ViewData["CarreraId"] = new SelectList(_context.Carreras, "Id", "Nombre");
            return View();
        }

        // POST: Materias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Configs.Empleado)]
        public async Task<IActionResult> Create([Bind("Id,CarreraId,Nombre,CodigoMateria,Descripcion,CupoMaximo")] Materia materia)
        {
            VerificarCodigoValido(materia);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Materias.Add(materia);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));


                }
                catch (DbUpdateException dbex)
                {
                    ProcesarDuplicado(dbex);
                    ViewData["CarreraId"] = new SelectList(_context.Carreras, "Id", "Nombre", materia.CarreraId);
                    return View(materia);

                }
            }
            ViewData["CarreraId"] = new SelectList(_context.Carreras, "Id", "Nombre", materia.CarreraId);
            return View(materia);
        }


        private void VerificarCodigoValido(Materia materia)
        {
            if (CodigoMateriaExists(materia.CodigoMateria))
            {
                ModelState.AddModelError("CodigoMateria", ErrorMesseges.CodigoEnUso);
            }
        }

        private bool CodigoMateriaExists(string codigo)
        {
            bool resultado = false;
           if (!string.IsNullOrEmpty(codigo))
            {
                resultado = _context.Materias.Any(m => m.CodigoMateria == codigo);
           }
            return resultado;
        }


        private void ProcesarDuplicado(DbUpdateException dbex)
        {
            SqlException innerException = dbex.InnerException as SqlException;
            if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601))
            {
                ModelState.AddModelError("CodigoMateria", ErrorMesseges.CodigoEnUso);
            }
            else
            {
                ModelState.AddModelError(string.Empty, dbex.Message);
            }
        }

        // GET: Materias/Edit/5
        [Authorize(Roles = Configs.Empleado)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materia = await _context.Materias.FindAsync(id);
            if (materia == null)
            {
                return NotFound();
            }
            ViewData["CarreraId"] = new SelectList(_context.Carreras, "Id", "Nombre", materia.CarreraId);
            return View(materia);
        }

        // POST: Materias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Configs.Empleado)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CarreraId,Nombre,CodigoMateria,Descripcion,CupoMaximo")] Materia materia)
        {
            if (id != materia.Id)
            {
                return NotFound();
            }

            var materiaEnDB = _context.Materias.Find(materia.Id);

            if (materia.CodigoMateria != materiaEnDB.CodigoMateria)
            {
                VerificarCodigoValido(materia);
            }



            if (ModelState.IsValid)
            {
                try
                {



                    if (materiaEnDB != null)
                    {
                        materiaEnDB.Nombre = materia.Nombre;
                        materiaEnDB.Descripcion = materia.Descripcion;  
                        materiaEnDB.CodigoMateria = materia.CodigoMateria;
                        materiaEnDB.CupoMaximo = materia.CupoMaximo;




                        _context.Materias.Update(materiaEnDB);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return NotFound();
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MateriaExists(materia.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                catch (DbUpdateException dbex)
                {
                    ProcesarDuplicado(dbex);
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["CarreraId"] = new SelectList(_context.Carreras, "Id", "Nombre", materia.CarreraId);
            return View(materia);
        }

        // GET: Materias/Delete/5
        [Authorize(Roles = Configs.Empleado)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materia = await _context.Materias
                .Include(m => m.Carrera)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (materia == null)
            {
                return NotFound();
            }

            return View(materia);
        }

        // POST: Materias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Configs.Empleado)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var materia = await _context.Materias.FindAsync(id);
            if (materia != null)
            {
                _context.Materias.Remove(materia);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MateriaExists(int id)
        {
            return _context.Materias.Any(e => e.Id == id);
        }
    }
}
