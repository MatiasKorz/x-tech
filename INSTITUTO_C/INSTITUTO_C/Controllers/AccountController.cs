using INSTITUTO_C.Data;
using INSTITUTO_C.Helpers;
using INSTITUTO_C.Models;
using INSTITUTO_C.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace INSTITUTO_C.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private readonly UserManager<Persona> _userManager;
        private readonly SignInManager<Persona> _signInManager;
        private readonly InstitutoContext _context;
        public AccountController(UserManager<Persona> userManager, SignInManager<Persona> signInManager, InstitutoContext context)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._context = context;
        }

        [AllowAnonymous]
        public IActionResult Registrar()
        {

            ViewData["CarreraNombre"] = new SelectList(_context.Carreras, "Nombre", "Nombre");
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registrar(RegistroUsuario viewModel)
        {
            if (ModelState.IsValid)
            {
                var carreraSeleccionada = _context.Carreras
                 .FirstOrDefault(c => c.Nombre == viewModel.CarreraNombre);

                if (carreraSeleccionada == null)
                {
                    ModelState.AddModelError("CarreraNombre", "Carrera inválida");
                    ViewData["CarreraNombre"] = new SelectList(_context.Carreras, "Nombre", "Nombre", viewModel.CarreraNombre);
                    return View(viewModel);
                }

                Alumno alumno = new Alumno()
                {
                    

                     Email = viewModel.Email,
                     UserName = viewModel.Email,
                    CarreraId = carreraSeleccionada.Id


                };


                var resultadoCreate = await _userManager.CreateAsync(alumno, viewModel.Password);


                if (resultadoCreate.Succeeded)
                {

                    var resultadoAddRole = await _userManager.AddToRoleAsync(alumno, Configs.Alumno);

                    if (resultadoAddRole.Succeeded)
                    {
                        await _signInManager.SignInAsync(alumno, isPersistent: false);
                        return RedirectToAction("Edit", "Alumnos", new { id = alumno.Id });
                    }
                    else
                    {
                        return Content("No se pudo agregar rol");
                    }


         



                }

                foreach (var error in resultadoCreate.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);


                }
                
            }

            ViewData["CarreraNombre"] = new SelectList(_context.Carreras, "Nombre", "Nombre", viewModel.CarreraNombre);
            return View(viewModel);
        }



        [AllowAnonymous]
        public IActionResult IniciarSesion(string returnUrl)
        {
       
            TempData["ReturnUrl"] = returnUrl;
            return View();


        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> IniciarSesion(Login viewModel)
        {

      
            var returnUrl = TempData["ReturnUrl"] as string;
            if (ModelState.IsValid)
            {
               var resultado = await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.Recordarme, false);

                if (resultado.Succeeded)
                {
                    if(!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
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


        public IActionResult AccesoDenegado(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        
            
 



    }

}
