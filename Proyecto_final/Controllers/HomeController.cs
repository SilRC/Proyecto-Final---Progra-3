using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_final.Models;
using System.Diagnostics;
using System.Linq;

namespace Proyecto_final.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProyectoFinalDbContext _context;

        private readonly ILogger<HomeController> _logger;

        private readonly IHttpContextAccessor _httpContextAccessor;


        public HomeController(ProyectoFinalDbContext context, ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // Buscar el usuario en la base de datos por nombre de usuario y contraseña
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.NombreUsuario == model.NombreUsuario && u.Contrasena == model.Contrasena);

                if (usuario != null) //si encuentra usuario en bdd
                {
                    if (model.NombreUsuario == "admin")
                    {
                        _httpContextAccessor.HttpContext.Session.SetString("admin", "True"); //variable de sesión true
                    }
                    else
                    {
                        _httpContextAccessor.HttpContext.Session.SetString("admin", "False");
                    }

                    _httpContextAccessor.HttpContext.Session.SetString("name", usuario.NombreUsuario);
                    _httpContextAccessor.HttpContext.Session.SetInt32("UserId", usuario.UserId);

                    // Usuario autenticado, redirige a la página de inicio
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Usuario no encontrado, agrega un error al modelo
                    ModelState.AddModelError(string.Empty, "Nombre de usuario o contraseña incorrectos.");
                }
            }

            // Si el modelo no es válido o la autenticación falló, vuelve a la vista de inicio de sesión con errores
            return View();
        }

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                
                _context.Add(usuario);
                await _context.SaveChangesAsync();

                // Redirige a la página de inicio de sesión
                return RedirectToAction("Login", "Home");
            }

            // Si el modelo no es válido, vuelve a la vista de registro
            return View();
        }

        
        public IActionResult Informes()
        {
            var usuarios = _context.Usuarios.ToList(); // Obtener la lista de usuarios

            ViewBag.Usuarios = new SelectList(usuarios, "UserId", "NombreUsuario");

            return View();
        }

        [HttpPost]
        public IActionResult GenerarInforme(Informes model)
        {
            if (ModelState.IsValid)
            {
                var usuario = _context.Usuarios.FirstOrDefault(u => u.UserId == model.UserId);

                if (usuario != null)
                {
                    // Obtener cuentas del usuario
                    var cuentas = _context.Cuentas
                        .Where(c => c.UserId == usuario.UserId)
                        .ToList();

                    // Obtener ingresos del usuario
                    var ingresos = _context.Ingresos
                        .Include(i => i.Cuenta)
                        .Where(i => cuentas.Select(c => c.CuentaId).ToList().Contains((int)i.CuentaId))
                        .ToList();

                    // Obtener gastos del usuario
                    var gastos = _context.Gastos
                        .Include(g => g.Cuenta)
                        .Where(g => cuentas.Select(c => c.CuentaId).ToList().Contains((int)g.CuentaId))
                        .ToList();

                    var informacionCuentas = $"Número de cuentas: {cuentas.Count} <br><br>";
                    foreach (var cuenta in cuentas)
                    {
                        informacionCuentas += $"Cuenta ID: {cuenta.CuentaId}, Nombre: {cuenta.NombreCuenta}, Saldo Inicial: {cuenta.SaldoInicial} <br>";
                    }


                    // Calcular el total de ingresos y gastos
                    decimal totalIngresos = ingresos.Sum(i => i.Monto);
                    decimal totalGastos = gastos.Sum(g => g.Monto);

                    // Calcular el estado (ingresos - gastos)
                    decimal estado = totalIngresos - totalGastos;

                    // Simular la construcción de la información para mostrar en la vista
                    var resultados = new ResultadoInforme
                    {
                        InformacionCuentas = informacionCuentas,
                        InformacionIngresos = $"Total de ingresos: {totalIngresos:C}",
                        InformacionGastos = $"Total de gastos: {totalGastos:C}",
                        InformacionEstado = $"Estado: {estado:C}"
                    };

                    // Devolver la vista parcial con los resultados
                    return PartialView("ResultadoInforme", resultados);
                }

                else
                {
                    // Manejar el caso en que no se encuentre el usuario.
                    ModelState.AddModelError("UserId", "Usuario no encontrado");
                }
            }

            // Si el modelo no es válido, devuelve la vista con errores.
            return View("Informes", model);
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
