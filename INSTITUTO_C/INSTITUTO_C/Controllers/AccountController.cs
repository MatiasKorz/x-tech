using INSTITUTO_C.Helpers;
using INSTITUTO_C.Models;
using INSTITUTO_C.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace INSTITUTO_C.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<Persona> _userManager;
        private readonly SignInManager<Persona> _signInManager;
        public AccountController(UserManager<Persona> userManager, SignInManager<Persona> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }


        public IActionResult Registrar()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Registrar(RegistroUsuario viewModel)
        {
            if (ModelState.IsValid)
            {

                //string nuevoLegajo = EmpleadoHelper.GenerarLegajo(_context);
                Empleado empleado = new Empleado()
                {
                    

                     Email = viewModel.Email,
                     UserName = viewModel.Email,
                     //Legajo = nuevoLegajo, // 👈 asignación automática
                     
                   
                };


                var resultadoCreate = await _userManager.CreateAsync(empleado, viewModel.Password);


                if (resultadoCreate.Succeeded)
                {

                    await _signInManager.SignInAsync(empleado, isPersistent: false);
                    //return RedirectToAction("Index", "Home");

                    return RedirectToAction("Edit", "Empleados", new { id = empleado.Id });

                }

                foreach (var error in resultadoCreate.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);


                }
                
            }
            return View(viewModel);
        }


    }

}
