using INSTITUTO_C.Helpers;
using INSTITUTO_C.Models;
using INSTITUTO_C.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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




        public IActionResult IniciarSesion()
        {
            return View();


        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(Login viewModel)
        {

            if (ModelState.IsValid)
            {
               var resultado = await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.Recordarme, false);

                if (resultado.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Acceso denegado");
            }


            return View(viewModel);


        }


        public async Task<IActionResult> CerrarSesion()
        {

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }



    }

}
