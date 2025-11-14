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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Identity;

namespace INSTITUTO_C.Controllers
{
    [Authorize]
    public class CarrerasController : Controller
    {
        private readonly InstitutoContext _context;
        private readonly UserManager<Persona> _userManager;

        public CarrerasController(InstitutoContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Carreras
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Carreras.ToListAsync());
        }

        // GET: Carreras/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrera = await _context.Carreras
                .Include(c => c.Materias)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carrera == null)
            {
                return NotFound();
            }

            return View(carrera);
        }


        [Authorize(Roles = Configs.Alumno)]
        public async Task<IActionResult> MiCarrera()
        {
            int usuarioId = int.Parse(_userManager.GetUserId(User));


            var alumno = await _context.Alumnos
               .Include(a => a.Carrera)
               .FirstOrDefaultAsync(a => a.Id == usuarioId);
            if (alumno == null)
            {
                return NotFound();
            }

            return RedirectToAction("Details", new { id = alumno.CarreraId });
        }

        // GET: Carreras/Create
        [Authorize(Roles = Configs.Empleado)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Carreras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Configs.Empleado)]
        public async Task<IActionResult> Create([Bind("Id,Nombre")] Carrera carrera)
        {

            VerificarNombreValido(carrera);

            if (!ModelState.IsValid)
            {

                return View(carrera);
            }

            try
            {
                _context.Carreras.Add(carrera);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }

            catch (DbUpdateException dbex) 
            {
                ProcesarDuplicado(dbex);
                return View(carrera);

            }


        }

        private void VerificarNombreValido(Carrera carrera)
        {
            if (CarreraNombreExists(carrera.Nombre))
            {
                ModelState.AddModelError("Nombre", ErrorMesseges.CarreraNombre);
            }
        }

        private bool CarreraNombreExists(string nombre)
        {
            bool resultado = false;
            if (!string.IsNullOrEmpty(nombre)){
                resultado = _context.Carreras.Any(c => c.Nombre == nombre);
            }
            return resultado;
        }

        private void ProcesarDuplicado(DbUpdateException dbex)
        {
            SqlException innerException = dbex.InnerException as SqlException;
            if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601))
            {
                ModelState.AddModelError("Nombre", ErrorMesseges.CarreraNombre);
            }
            else
            {
                ModelState.AddModelError(string.Empty, dbex.Message);
            }
        }

        // GET: Carreras/Edit/5
        [Authorize(Roles = Configs.Empleado)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrera = await _context.Carreras.FindAsync(id);
            if (carrera == null)
            {
                return NotFound();
            }
            return View(carrera);
        }

        // POST: Carreras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Configs.Empleado)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] Carrera carrera)
        {
            if (id != carrera.Id)
            {
                return NotFound();
            }
            var carreraEnDB = _context.Carreras.Find(carrera.Id);
            if (carrera.Nombre != carreraEnDB.Nombre)
            {
                VerificarNombreValido(carrera);
            }


            if (ModelState.IsValid)
            {
                try
                {

                    if (carreraEnDB != null)
                    {
                        carreraEnDB.Nombre = carrera.Nombre;
                    }

                    _context.Update(carreraEnDB);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarreraExists(carrera.Id))
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

            }
            return View(carrera);
        }

        // GET: Carreras/Delete/5
        [Authorize(Roles = Configs.Empleado)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrera = await _context.Carreras
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carrera == null)
            {
                return NotFound();
            }

            return View(carrera);
        }

        // POST: Carreras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Configs.Empleado)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carrera = await _context.Carreras.FindAsync(id);
            if (carrera != null)
            {
                _context.Carreras.Remove(carrera);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarreraExists(int id)
        {
            return _context.Carreras.Any(e => e.Id == id);
        }
    }
}
