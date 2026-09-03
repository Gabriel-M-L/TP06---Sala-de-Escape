using System.Diagnostics;
using System.IO.Pipes;
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
        return RedirectToAction("NivelActual");
    }

    public IActionResult NivelActual()
    {
        BD bd = new BD();
        string usuario = HttpContext.Session.GetString("Usuario");
        int nivel = bd.ObtenerNivel(usuario);
        string idsNiveles = bd.ObtenerIdsNivelesAleatorios(nivel);
        HttpContext.Session.SetString("IdsNivel", idsNiveles);
        if(nivel == 4)
        {
            return RedirectToAction("ConstruirNivel4");
        }
        return RedirectToAction($"Nivel{nivel}");
    }
    public IActionResult Tienda()
    {
        BD bd = new BD();
        ViewBag.Plata = bd.ObtenerPlata(HttpContext.Session.GetString("Usuario"));
        ViewBag.Comodin = bd.ObtenerComodin(HttpContext.Session.GetString("Usuario"));
        ViewBag.Comodines = bd.ObtenerComodinAleatorio();
        return View();
    }
    public IActionResult PasarDeNivel()
    {
        BD bd = new BD();
        string usuario = HttpContext.Session.GetString("Usuario");
        bd.CambiarNivel(usuario);
        return RedirectToAction($"NivelActual");
    }
    public IActionResult Nivel1()
    {
        BD bd = new BD();
        if (bd.ObtenerNivel(HttpContext.Session.GetString("Usuario")) != 1)
        {
            return RedirectToAction("NivelActual");
        }
        if(int. Parse(HttpContext.Session.GetString("Intentos")) <= 0)
        {
            bd.cambiarPlata(HttpContext.Session.GetString("Usuario"), 0);
            HttpContext.Session.SetString("Intentos", "8");
            HttpContext.Session.SetString("Completados", "0");
            return RedirectToAction("NivelActual");
        }
        ViewBag.Nivel = bd.ObtenerDatosNivel(1, int.Parse(HttpContext.Session.GetString("IdsNivel").Split(',')[int.Parse(HttpContext.Session.GetString("Intentos")) - 1]));
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
        if (bd.ObtenerNivel(HttpContext.Session.GetString("Usuario")) != 2)
        {
            return RedirectToAction("NivelActual");
        }
        if(int. Parse(HttpContext.Session.GetString("Intentos")) <= 0)
        {
            bd.cambiarPlata(HttpContext.Session.GetString("Usuario"), 0);
            HttpContext.Session.SetString("Intentos", "8");
            HttpContext.Session.SetString("Completados", "0");
            return RedirectToAction("NivelActual");
        }
        ViewBag.Nivel = bd.ObtenerDatosNivel(2, int.Parse(HttpContext.Session.GetString("IdsNivel").Split(',')[int.Parse(HttpContext.Session.GetString("Intentos")) - 1]));
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
        if (bd.ObtenerNivel(HttpContext.Session.GetString("Usuario")) != 3)
        {
            return RedirectToAction("NivelActual");
        }
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
        ViewBag.Nivel = bd.ObtenerDatosNivel(3, int.Parse(HttpContext.Session.GetString("IdsNivel").Split(',')[int.Parse(HttpContext.Session.GetString("Intentos")) - 1]));
        return View();
    }
    public IActionResult Nivel4()
    {
        Random random = new Random(); 
        BD bd = new BD();
        if (bd.ObtenerNivel(HttpContext.Session.GetString("Usuario")) != 4)
        {
            return RedirectToAction("NivelActual");
        }
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
        ViewBag.Nivel = ConstruirNivel4(int.Parse(HttpContext.Session.GetString("IdsNivel").Split(',')[random.Next(0, 3)]));
        return View();
    }
    public IActionResult Nivel5()
    {
        return View();
    }

    [HttpPost]
    public IActionResult VerificarRespuesta(string respuesta, int id)
    {
        BD bd = new BD();
        int nivel = bd.ObtenerNivel(HttpContext.Session.GetString("Usuario"));
        Nivel datosNivel = bd.ObtenerDatosNivel(nivel, id);
        if (respuesta == datosNivel.Respuesta)
        {
            int completados = int.Parse(HttpContext.Session.GetString("Completados")) + 1;
            HttpContext.Session.SetString("Completados", completados.ToString());
        }
        int intentos = int.Parse(HttpContext.Session.GetString("Intentos")) - 1;
        HttpContext.Session.SetString("Intentos", intentos.ToString());
        return RedirectToAction($"Nivel{nivel}");
    }

    public IActionResult ComprarComodin(int id)
    {
        BD bd = new BD();
        string usuario = HttpContext.Session.GetString("Usuario");
        Comodin comodin = bd.ObtenerComodinPorId(id);
        int plata = bd.ObtenerPlata(usuario);
        if (plata >= comodin.Precio)
        {
            bd.cambiarPlata(usuario, -comodin.Precio);
            bd.CambiarComodin(usuario, id);
            ViewBag.Mensaje = "Comodín comprado con éxito.";
        }
        else
        {
            ViewBag.Mensaje = "No tenes suficiente plata para comprar este comodín.";
        }
        return RedirectToAction("Tienda");
    }

    public Nivel ConstruirNivel4(int id)
    {
        BD bd = new BD();
        string pregunta = "";
        string consigna = "";
        string respuesta = "";
        string pista = "";
        List<Carta> cartas = new List<Carta>();
        string imagesHtml = "";
        Random random = new Random();
        switch (id)
        {
            case 1:
                pregunta = bd.ObtenerPregunta(1);
                cartas = GenerarCarta(5);
                imagesHtml = "<div style=\"display:flex;gap:8px;align-items:center;\">"
                    + string.Join("", cartas.Select(c => $"<img src=\"images/{c.Imagen}\" alt=\"{c.Palo} {c.Numero}\" style=\"height:100px;\">"))
                    + "</div>";
                consigna = pregunta + " " + imagesHtml;
                respuesta = cartas[1].Palo + cartas[1].Numero;
                pista = "la carta que necesitas es la segunda de la lista";
                break;
            case 2:
                pregunta = bd.ObtenerPregunta(2);
                cartas = GenerarCarta(5);
                imagesHtml = "<div style=\"display:flex;gap:8px;align-items:center;\">"
                    + string.Join("", cartas.Select(c => $"<img src=\"images/{c.Imagen}\" alt=\"{c.Palo} {c.Numero}\" style=\"height:100px;\">"))
                    + "</div>";
                consigna = pregunta + " " + imagesHtml;
                respuesta = cartas[2].Palo + cartas[2].Numero;
                pista = $"la carta que necesitas es de {cartas[2].Palo}";
                break;
            case 3:
                int ran = random.Next(0, 5);
                pregunta = bd.ObtenerPregunta(3);
                cartas = GenerarCarta(5);
                imagesHtml = "<div style=\"display:flex;gap:8px;align-items:center;\">"
                    + string.Join("", cartas.Select(c => $"<img src=\"images/{c.Imagen}\" alt=\"{c.Palo} {c.Numero}\" style=\"height:100px;\">"))
                    + "</div>";
                consigna = "Qué carta estaba en la posición " + (ran + 1) + " " + imagesHtml;
                respuesta = cartas[ran].Palo + cartas[ran].Numero;
                pista = $"la carta que necesitas es de {cartas[ran].Numero}";
                break;
            case 4:
                pregunta = bd.ObtenerPregunta(4);
                break;
        }
        
        return new Nivel { Desafio = consigna, Respuesta = respuesta, Pista = pista };
    }

    public List<Carta> GenerarCarta(int cantidad)
    {
        List<Carta> cartas = new List<Carta>();
        Random random = new Random();
        string[] palos = { "Corazones", "Diamantes", "Tréboles", "Picas" };
        string[] numeros = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

        for (int i = 0; i < cantidad; i++)
        {
            string palo = palos[random.Next(palos.Length)];
            string numero = numeros[random.Next(numeros.Length)];
            string imagen = $"{palo}_{numero}.png"; 
            cartas.Add(new Carta { Palo = palo, Numero = numero, Imagen = imagen });
        }

        return cartas;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
