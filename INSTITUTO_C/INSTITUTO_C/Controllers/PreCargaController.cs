using INSTITUTO_C.Data;
using INSTITUTO_C.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace INSTITUTO_C.Controllers
{
    public class PreCargaController : Controller
    {

        private readonly UserManager<Persona> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly InstitutoContext _context;
        private List<string> roles = new List<string>() {"Admin", "Empleado", "Profesor", "Alumno"};
        public PreCargaController(UserManager<Persona> userManager, RoleManager<IdentityRole<int>> roleManager, InstitutoContext context)
        {
            this._userManager = userManager;
            this._roleManager = roleManager; 
            this._context = context;

        }



        public IActionResult Seed()
        {
            CrearRoles().Wait();



            return RedirectToAction("Index","Home");
        }

        private async Task CrearRoles()
        {
            foreach (var rolName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(rolName))
                {
                   await _roleManager.CreateAsync(new IdentityRole<int> { Name= rolName});
                }
            }
        }
    }
}
