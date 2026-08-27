using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TP06_Martinez_Loufer.Models;

namespace TP06_Martinez_Loufer.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Juego()
    {
        return View();
    }

    public IActionResult Historia()
    {
        return View();
    }

    public IActionResult Integrantes()
    {
        return View();
    }

    public IActionResult Continuar()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Jugar(string usuario, bool continuar)
    {
        BD bd = new BD();
        if (continuar)
        {
            if(!bd.RecuperarUsuario(usuario))
            {
                ViewBag.Mensaje = "Usuario inexistente o con partida finalizada.";
                return View("Continuar");
            }
            HttpContext.Session.SetString("Usuario", usuario);
            HttpContext.Session.SetString("Intentos", "8");
            HttpContext.Session.SetString("Completados", "0");
            return RedirectToAction("NivelActual");
        }
        if (bd.BuscarUsuario(usuario))
        {
            ViewBag.Mensaje = "Usuario ya existente.";
            return View("Juego");
        }
        HttpContext.Session.SetString("Usuario", usuario);
        HttpContext.Session.SetString("Intentos", "8");
        HttpContext.Session.SetString("Completados", "0");
        bd.GuardarUsuario(usuario);
        return RedirectToAction("Nivel1");
    }

    public IActionResult NivelActual()
    {
        BD bd = new BD();
        string usuario = HttpContext.Session.GetString("usuario");
        int nivel = bd.ObtenerNivel(usuario);
        return RedirectToAction($"Nivel{nivel}");
    }
    public IActionResult Tienda()
    {
        BD bd = new BD();
        ViewBag.Plata = bd.ObtenerPlata(HttpContext.Session.GetString("Usuario"));
        ViewBag.Comodin = bd.ObtenerComodin(HttpContext.Session.GetString("Usuario"));
        return View();
    }
    public IActionResult PasarDeNivel()
    {
        BD bd = new BD();
        string usuario = HttpContext.Session.GetString("Usuario");
        bd.CambiarNivel(usuario);
        int nivel = bd.ObtenerNivel(usuario);
        string idsNiveles = bd.ObtenerIdsNivelesAleatorios(nivel);
        HttpContext.Session.SetString("IdsNivel", idsNiveles);
        return RedirectToAction($"Nivel{nivel}");
    }
    public IActionResult Nivel1()
    {
        BD bd = new BD();
        if(int. Parse(HttpContext.Session.GetString("Intentos")) <= 0)
        {
            bd.cambiarPlata(HttpContext.Session.GetString("Usuario"), 0);
            HttpContext.Session.SetString("Intentos", "8");
            HttpContext.Session.SetString("Completados", "0");
            return RedirectToAction("NivelActual");
        }
        ViewBag.Nivel = bd.ObtenerDatosNivel(int.Parse(HttpContext.Session.GetString("IdsNivel").Split(',')[int.Parse(HttpContext.Session.GetString("Intentos")) - 1]));
        if(int.Parse(HttpContext.Session.GetString("Completados")) >= 3)
        {
            bd.cambiarPlata(HttpContext.Session.GetString("Usuario"), int.Parse(HttpContext.Session.GetString("Intentos")));
            HttpContext.Session.SetString("Intentos", "8");
            HttpContext.Session.SetString("Completados", "0");
            return RedirectToAction("Tienda");
        }
        return View();
    }
    public IActionResult Nivel2()
    {
        BD bd = new BD();
        if(int. Parse(HttpContext.Session.GetString("Intentos")) <= 0)
        {
            bd.cambiarPlata(HttpContext.Session.GetString("Usuario"), 0);
            HttpContext.Session.SetString("Intentos", "8");
            HttpContext.Session.SetString("Completados", "0");
            return RedirectToAction("NivelActual");
        }
        ViewBag.Nivel = bd.ObtenerDatosNivel(int.Parse(HttpContext.Session.GetString("IdsNivel").Split(',')[int.Parse(HttpContext.Session.GetString("Intentos")) - 1]));
        if(int.Parse(HttpContext.Session.GetString("Completados")) >= 3)
        {
            bd.cambiarPlata(HttpContext.Session.GetString("Usuario"), int.Parse(HttpContext.Session.GetString("Intentos")));
            HttpContext.Session.SetString("Intentos", "8");
            HttpContext.Session.SetString("Completados", "0");
            return RedirectToAction("Tienda");
        }
        return View();
    }
    public IActionResult Nivel3()
    {
        BD bd = new BD();
        if(int. Parse(HttpContext.Session.GetString("Intentos")) <= 0)
        {
            bd.cambiarPlata(HttpContext.Session.GetString("Usuario"), 0);
            HttpContext.Session.SetString("Intentos", "8");
            HttpContext.Session.SetString("Completados", "0");
            return RedirectToAction("NivelActual");
        }
        ViewBag.Nivel = bd.ObtenerDatosNivel(int.Parse(HttpContext.Session.GetString("IdsNivel").Split(',')[int.Parse(HttpContext.Session.GetString("Intentos")) - 1]));
        if(int.Parse(HttpContext.Session.GetString("Completados")) >= 3)
        {
            bd.cambiarPlata(HttpContext.Session.GetString("Usuario"), int.Parse(HttpContext.Session.GetString("Intentos")));
            HttpContext.Session.SetString("Intentos", "8");
            HttpContext.Session.SetString("Completados", "0");
            return RedirectToAction("Tienda");
        }
        return View();
    }
    public IActionResult Nivel4()
    {
        BD bd = new BD();
        if(int. Parse(HttpContext.Session.GetString("Intentos")) <= 0)
        {
            bd.cambiarPlata(HttpContext.Session.GetString("Usuario"), 0);
            HttpContext.Session.SetString("Intentos", "8");
            HttpContext.Session.SetString("Completados", "0");
            return RedirectToAction("NivelActual");
        }
        if(int.Parse(HttpContext.Session.GetString("Completados")) >= 3)
        {
            bd.cambiarPlata(HttpContext.Session.GetString("Usuario"), int.Parse(HttpContext.Session.GetString("Intentos")));
            HttpContext.Session.SetString("Intentos", "8");
            HttpContext.Session.SetString("Completados", "0");
            return RedirectToAction("Tienda");
        }
        return View();
    }
    public IActionResult Nivel5()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
