using INSTITUTO_C.Data;
using INSTITUTO_C.Helpers;
using INSTITUTO_C.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace INSTITUTO_C.Controllers
{
    public class PreCargaController : Controller
    {
        //hola
        private readonly UserManager<Persona> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly InstitutoContext _context;
        private List<string> roles = new List<string>() {Configs.Empleado, Configs.Profesor, Configs.Alumno};
        public PreCargaController(UserManager<Persona> userManager, RoleManager<IdentityRole<int>> roleManager, InstitutoContext context)
        {
            this._userManager = userManager;
            this._roleManager = roleManager; 
            this._context = context;

        }



        public async Task<IActionResult> Seed()
        {

            await CrearRoles();
            await CrearEmpleados();


            await CrearCarreras();
            await CrearMaterias();
            await CrearAlumnos();
            await CrearProfesores();


            return RedirectToAction("Index","Home", new { mensaje = "Se ha realizado el seed aparentemente correctamente" });
        }


        private async Task CrearCarreras()
        {
            if (!_context.Carreras.Any())
            {
                var carreras = new List<Carrera>
        {
            new Carrera { Nombre = "MacroTecnologia" },
            new Carrera { Nombre = "Ing Agroespacial" }
        };

                _context.Carreras.AddRange(carreras);
                await _context.SaveChangesAsync();
            }

        }
        private async Task CrearMaterias()
        {

            if (!_context.Materias.Any())
            {

                var carrera1 = _context.Carreras.OrderBy(c => c.Id).FirstOrDefault();
                var carrera2 = _context.Carreras.OrderByDescending(c => c.Id).FirstOrDefault();



                var materia1 = new Materia
                {
                    Nombre = "Robotica I",
                    CodigoMateria = "R1",
                    Descripcion = "Introducción a las tecnologias utilizadas en la creacion de robots gigantes",
                    CupoMaximo = 10,
                    CarreraId = carrera1.Id
                };

                var materia2 = new Materia
                {
                    Nombre = "Flora terrestre",
                    CodigoMateria = "FT",
                    Descripcion = "Introducción a las plantas",
                    CupoMaximo = 5,
                    CarreraId = carrera2.Id
                };

                var materia3 = new Materia
                {
                    Nombre = "Aviacion",
                    CodigoMateria = "AV",
                    Descripcion = "Introducción al manejo de Jets militares",
                    CupoMaximo = 2,
                    CarreraId = carrera1.Id
                };

                var materia4 = new Materia
                {
                    Nombre = "Biocibernetica",
                    CodigoMateria = "BC",
                    Descripcion = "Cyborgs, peligros y riesgos",
                    CupoMaximo = 2,
                    CarreraId = carrera1.Id
                };

                _context.Materias.AddRange(materia1, materia2, materia3, materia4);
                await _context.SaveChangesAsync();
            }
        }

        private async Task CrearProfesores()
        {
            if (!_context.Profesores.Any())
            {
                var profesor = new Profesor
                {
                    UserName = "profesor@xtech.com",
                    Email = "profesor@xtech.com",
                    Nombre = "Hernan",
                    Apellido = "Macoy",
                    DNI = "30000002",
                    Telefono = "1112345678",
                    Direccion = "Cruze Peligroso 5",
                    Activo = false
                };

                //Password: Password1!

                var result = await _userManager.CreateAsync(profesor, Configs.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(profesor, Configs.Profesor);
                }
            }

        }

        private async Task CrearEmpleados()
        {
            if (!_context.Empleados.Any())
            {
                var empleado = new Empleado
                {
                    UserName = "empleado@xtech.com",
                    Email = "empleado@xtech.com",
                    Nombre = "Carlos",
                    Apellido = "Javier",
                    DNI = "30000001",
                    Telefono = "1112345678",
                    Direccion = "Avenida Sentinela 123",
                    Activo = false
                };

                var result = await _userManager.CreateAsync(empleado, Configs.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(empleado, Configs.Empleado);
                }
            }

        }


        private async Task CrearAlumnos()
        {
            if (!_context.Alumnos.Any())
            {

                var carrera1 = _context.Carreras.OrderBy(c => c.Id).FirstOrDefault();
                var carrera2 = _context.Carreras.OrderByDescending(c => c.Id).FirstOrDefault();
                var alumno = new Alumno
                {
                    UserName = "alumno@xtech.com",
                    Email = "alumno@xtech.com",
                    Nombre = "Kevin",
                    Apellido = "Wagner",
                    DNI = "30000003",
                    Telefono = "11333444",
                    Direccion = "Callejon Oscuro 666",
                    Activo = false,
                    CarreraId = carrera1.Id
                };
                var result = await _userManager.CreateAsync(alumno, Configs.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(alumno, Configs.Alumno);
                }

                var alumno2 = new Alumno
                {
                    UserName = "alumno2@xtech.com",
                    Email = "alumno2@xtech.com",
                    Nombre = "Oriana",
                    Apellido = "Monroe",
                    DNI = "30000004",
                    Telefono = "11333555",
                    Direccion = "Cairo 75",
                    Activo = false,
                    CarreraId = carrera2.Id
                };

                result = await _userManager.CreateAsync(alumno2, Configs.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(alumno2, Configs.Alumno);
                }


                var alumno3 = new Alumno
                {
                    UserName = "alumno3@xtech.com",
                    Email = "alumno3@xtech.com",
                    Nombre = "Pedro",
                    Apellido = "Colosso",
                    DNI = "30000005",
                    Telefono = "11222455",
                    Direccion = "Av Madre Rusia 87",
                    Activo = false,
                    CarreraId = carrera1.Id
                };

                result = await _userManager.CreateAsync(alumno3, Configs.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(alumno3, Configs.Alumno);
                }


                var alumno4 = new Alumno
                {
                    UserName = "alumno4@xtech.com",
                    Email = "alumno4@xtech.com",
                    Nombre = "Anna Maria",
                    Apellido = "Rogue",
                    DNI = "30000006",
                    Telefono = "113334448",
                    Direccion = "Calle Sugah 23",
                    Activo = false,
                    CarreraId = carrera1.Id
                };

                result = await _userManager.CreateAsync(alumno4, Configs.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(alumno4, Configs.Alumno);
                }
            }
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
