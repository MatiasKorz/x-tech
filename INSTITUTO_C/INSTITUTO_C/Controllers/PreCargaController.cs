using INSTITUTO_C.Data;
using INSTITUTO_C.Helpers;
using INSTITUTO_C.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace INSTITUTO_C.Controllers
{
    public class PreCargaController : Controller
    {
        private readonly UserManager<Persona> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly InstitutoContext _context;
        private List<string> roles = new List<string>() { Configs.Empleado, Configs.Profesor, Configs.Alumno };
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
            await CrearMateriasCursadas();
            await CrearInscripciones(); 
            await CrearCalificaciones();

            return RedirectToAction("Index", "Home", new { mensaje = "Se ha realizado la Precarga de datos correctamente" });
        }

        private async Task CrearCarreras()
        {
            if (!_context.Carreras.Any())
            {
                var carreras = new List<Carrera>
        {
            new Carrera { Nombre = "Frontend Development" },
            new Carrera { Nombre = "Backend Development" },

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
                    Nombre = "HTML5,CSS3,JavaScript",
                    CodigoMateria = "FT1",
                    Descripcion = "Introduccion al desarrollo Web",
                    CupoMaximo = 2,
                    CarreraId = carrera1.Id
                };

                var materia2 = new Materia
                {
                    Nombre = "C#,.NET Core,Node.js",
                    CodigoMateria = "BK1",
                    Descripcion = "Fundamentos de desarrollo backend",
                    CupoMaximo = 2,
                    CarreraId = carrera2.Id
                };

                var materia3 = new Materia
                {
                    Nombre = "React, Vue, Angular",
                    CodigoMateria = "RVA1",
                    Descripcion = "Conceptos basicos de frameworks",
                    CupoMaximo = 2,
                    CarreraId = carrera1.Id
                };

                var materia4 = new Materia
                {
                    Nombre = "Bases de Datos SQL/NoSQL",
                    CodigoMateria = "BD1",
                    Descripcion = "Introduccion a Bases de datos",
                    CupoMaximo = 2,
                    CarreraId = carrera2.Id
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
                    UserName = "profesor@ort.edu.ar",
                    Email = "profesor@ort.edu.ar",
                    Nombre = "Hernan",
                    Apellido = "Macoy",
                    DNI = "30000002",
                    Telefono = "1112345678",
                    Direccion = "Cruze Peligroso 5",
                    Activo = true
                };

                //Password: Password1!

                var result = await _userManager.CreateAsync(profesor, Configs.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(profesor, Configs.Profesor);
                }


                var profesor2 = new Profesor
                {
                    UserName = "profesor2@ort.edu.ar",
                    Email = "profesor2@ort.edu.ar",
                    Nombre = "Lucas",
                    Apellido = "Howlett",
                    DNI = "10000002",
                    Telefono = "1112345678",
                    Direccion = "Calle Lobezno 1832",
                    Activo = true
                };

                //Password: Password1!

                result = await _userManager.CreateAsync(profesor2, Configs.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(profesor2, Configs.Profesor);
                }
            }

        }

        private async Task CrearEmpleados()
        {
            if (!_context.Empleados.Any())
            {
                var empleado = new Empleado
                {
                    UserName = "empleado@ort.edu.ar",
                    Email = "empleado@ort.edu.ar",
                    Nombre = "Carlos",
                    Apellido = "Javier",
                    DNI = "30000001",
                    Telefono = "1112345678",
                    Direccion = "Avenida centinela 123",
                    Activo = true
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
                    UserName = "alumno@ort.edu.ar",
                    Email = "alumno@ort.edu.ar",
                    Nombre = "Kevin",
                    Apellido = "Wagner",
                    DNI = "30000003",
                    Telefono = "11333444",
                    Direccion = "Callejon Oscuro 666",
                    Activo = true,
                    CarreraId = carrera1.Id
                };
                var result = await _userManager.CreateAsync(alumno, Configs.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(alumno, Configs.Alumno);
                }

                var alumno2 = new Alumno
                {
                    UserName = "alumno2@ort.edu.ar",
                    Email = "alumno2@ort.edu.ar",
                    Nombre = "Oriana",
                    Apellido = "Monroe",
                    DNI = "30000004",
                    Telefono = "11333555",
                    Direccion = "Cairo 75",
                    Activo = true,
                    CarreraId = carrera2.Id
                };

                result = await _userManager.CreateAsync(alumno2, Configs.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(alumno2, Configs.Alumno);
                }


                var alumno3 = new Alumno
                {
                    UserName = "alumno3@ort.edu.ar",
                    Email = "alumno3@ort.edu.ar",
                    Nombre = "Pedro",
                    Apellido = "Colosso",
                    DNI = "30000005",
                    Telefono = "11222455",
                    Direccion = "Av Madre Rusia 87",
                    Activo = true,
                    CarreraId = carrera1.Id
                };

                result = await _userManager.CreateAsync(alumno3, Configs.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(alumno3, Configs.Alumno);
                }


                var alumno4 = new Alumno
                {
                    UserName = "alumno4@ort.edu.ar",
                    Email = "alumno4@ort.edu.ar",
                    Nombre = "Anna Maria",
                    Apellido = "Rogue",
                    DNI = "30000006",
                    Telefono = "113334448",
                    Direccion = "Calle Sugah 23",
                    Activo = true,
                    CarreraId = carrera1.Id
                };

                result = await _userManager.CreateAsync(alumno4, Configs.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(alumno4, Configs.Alumno);
                }
            }
        }

        private async Task CrearMateriasCursadas()
        {
            if (!_context.MateriasCursadas.Any())
            {
                var materias = await _context.Materias.ToListAsync();
                var profesores = await _context.Profesores.ToListAsync();

                var cursadas = new List<MateriaCursada>();

                var cursada1 = new MateriaCursada
                {
                    MateriaId = materias.First(m => m.CodigoMateria == "FT1").Id,
                    CodigoCursada = CodigoCursada.A,
                    Anio = new DateTime(2025, 1, 1),
                    Cuatrimestre = 1,
                    Activo = true,
                    ProfesorId = profesores.First(p => p.Email == "profesor@ort.edu.ar").Id
                };
                cursada1.Materia = materias.First(m => m.CodigoMateria == "FT1");
                cursada1.GenerarNombre();
                cursadas.Add(cursada1);

                var cursada2 = new MateriaCursada
                {
                    MateriaId = materias.First(m => m.CodigoMateria == "BK1").Id,
                    CodigoCursada = CodigoCursada.A,
                    Anio = new DateTime(2025, 1, 1),
                    Cuatrimestre = 1,
                    Activo = true,
                    ProfesorId = profesores.First(p => p.Email == "profesor2@ort.edu.ar").Id
                };
                cursada2.Materia = materias.First(m => m.CodigoMateria == "BK1");
                cursada2.GenerarNombre();
                cursadas.Add(cursada2);

                var cursada3 = new MateriaCursada
                {
                    MateriaId = materias.First(m => m.CodigoMateria == "RVA1").Id,
                    CodigoCursada = CodigoCursada.A,
                    Anio = new DateTime(2025, 1, 1),
                    Cuatrimestre = 1,
                    Activo = true,
                    ProfesorId = profesores.First(p => p.Email == "profesor@ort.edu.ar").Id
                };
                cursada3.Materia = materias.First(m => m.CodigoMateria == "RVA1");
                cursada3.GenerarNombre();
                cursadas.Add(cursada3);

                var cursada4 = new MateriaCursada
                {
                    MateriaId = materias.First(m => m.CodigoMateria == "BD1").Id,
                    CodigoCursada = CodigoCursada.A,
                    Anio = new DateTime(2024, 1, 1),
                    Cuatrimestre = 2,
                    Activo = false,
                    ProfesorId = profesores.First(p => p.Email == "profesor2@ort.edu.ar").Id
                };
                cursada4.Materia = materias.First(m => m.CodigoMateria == "BD1");
                cursada4.GenerarNombre();
                cursadas.Add(cursada4);

                _context.MateriasCursadas.AddRange(cursadas);
                await _context.SaveChangesAsync();
            }
        }

        private async Task CrearInscripciones()
        {
            if (!_context.Inscripciones.Any())
            {
                var alumnos = await _context.Alumnos.ToListAsync();
                var cursadas = await _context.MateriasCursadas.ToListAsync();

                var inscripciones = new List<Inscripcion>();

                inscripciones.Add(new Inscripcion
                {
                    AlumnoId = alumnos.First(a => a.Email == "alumno@ort.edu.ar").Id,
                    MateriaCursadaId = cursadas.First(c => c.Nombre.Contains("FT1-2025-1C-A")).Id
                });

                inscripciones.Add(new Inscripcion
                {
                    AlumnoId = alumnos.First(a => a.Email == "alumno3@ort.edu.ar").Id,
                    MateriaCursadaId = cursadas.First(c => c.Nombre.Contains("FT1-2025-1C-A")).Id
                });

                inscripciones.Add(new Inscripcion
                {
                    AlumnoId = alumnos.First(a => a.Email == "alumno4@ort.edu.ar").Id,
                    MateriaCursadaId = cursadas.First(c => c.Nombre.Contains("RVA1-2025-1C-A")).Id
                });

                inscripciones.Add(new Inscripcion
                {
                    AlumnoId = alumnos.First(a => a.Email == "alumno2@ort.edu.ar").Id,
                    MateriaCursadaId = cursadas.First(c => c.Nombre.Contains("BK1-2025-1C-A")).Id
                });

                _context.Inscripciones.AddRange(inscripciones);
                await _context.SaveChangesAsync();
            }
        }

        private async Task CrearCalificaciones()
        {
            if (!_context.Calificaciones.Any())
            {
                var inscripciones = await _context.Inscripciones
                    .Include(i => i.MateriaCursada)
                    .ToListAsync();

                var calificaciones = new List<Calificacion>();

                foreach (var inscripcion in inscripciones)
                {
                    Nota nota = Nota.Pendiente;

                    if (inscripcion.AlumnoId % 2 == 0)
                    {
                        nota = Nota.Ocho; 
                    }
                    else if (inscripcion.AlumnoId % 3 == 0)
                    {
                        nota = Nota.Siete; 
                    }

                    calificaciones.Add(new Calificacion
                    {
                        AlumnoId = inscripcion.AlumnoId,
                        MateriaCursadaId = inscripcion.MateriaCursadaId,
                        Fecha = DateTime.Now,
                        Nota = nota,
                        ProfesorId = inscripcion.MateriaCursada.ProfesorId
                    });
                }

                _context.Calificaciones.AddRange(calificaciones);
                await _context.SaveChangesAsync();
            }
        }
        private async Task CrearRoles()
        {
            foreach (var rolName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(rolName))
                {
                    await _roleManager.CreateAsync(new IdentityRole<int> { Name = rolName });
                }
            }
        }
    }
}
