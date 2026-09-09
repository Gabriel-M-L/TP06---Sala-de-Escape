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
            HttpContext.Session.SetString("Pistas", "0");
            HttpContext.Session.SetString("Pista", "0");
            HttpContext.Session.SetString("TiendaComodines", "");
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
        HttpContext.Session.SetString("Pistas", "0");
        HttpContext.Session.SetString("Pista", "0");
        HttpContext.Session.SetString("TiendaComodines", "");
        bd.GuardarUsuario(usuario);
        return RedirectToAction("IntroNivel1");
    }

    public IActionResult NivelActual()
    {
        BD bd = new BD();
        string usuario = HttpContext.Session.GetString("Usuario");
        int nivel = bd.ObtenerNivel(usuario);
        string idsNiveles;
        idsNiveles = bd.ObtenerIdsNivelesAleatorios(nivel);
        HttpContext.Session.SetString("IdsNivel", idsNiveles);
        return RedirectToAction($"Nivel{nivel}");
    }
    public IActionResult Tienda(string mensaje)
    {
        BD bd = new BD();
        string usuario = HttpContext.Session.GetString("Usuario");
        ViewBag.Plata = bd.ObtenerPlata(usuario);
        ViewBag.Comodin = bd.ObtenerComodin(usuario);
        string sessionComodines = HttpContext.Session.GetString("TiendaComodines");
        if (string.IsNullOrEmpty(sessionComodines))
        {
            var comodines = bd.ObtenerComodinAleatorio();
            ViewBag.Comodines = comodines;
            HttpContext.Session.SetString("TiendaComodines", string.Join(",", comodines.Select(c => c.Id)));
        }
        else
        {
            var ids = sessionComodines.Split(',', StringSplitOptions.RemoveEmptyEntries);
            var lista = new List<Comodin>();
            foreach (var s in ids)
            {
                if (int.TryParse(s, out int cid) && cid != bd.ObtenerComodin(usuario).Id) lista.Add(bd.ObtenerComodinPorId(cid));
            }
            ViewBag.Comodines = lista;
        }
        ViewBag.Mensaje = mensaje;
        return View();
    }
    public IActionResult PasarDeNivel()
    {
        BD bd = new BD();
        string usuario = HttpContext.Session.GetString("Usuario");
        bd.CambiarNivel(usuario);
        return RedirectToAction("NivelActual");
    }
    public IActionResult IntroNivel1()
    {
        return View();
    }

    public IActionResult SinIntentos()
    {
        BD bd = new BD();
        string usuario = HttpContext.Session.GetString("Usuario");
        ViewBag.Plata = bd.ObtenerPlata(usuario);
        return View();
    }

    [HttpPost]
    public IActionResult RevivirConfirmar()
    {
        BD bd = new BD();
        string usuario = HttpContext.Session.GetString("Usuario");
        int plata = bd.ObtenerPlata(usuario);
        if (plata > 0)
        {
            bd.cambiarPlata(usuario, -plata);
        }
        HttpContext.Session.SetString("Intentos", "8");
        HttpContext.Session.SetString("Completados", "0");
        return RedirectToAction("Nivel1");
    }

    public IActionResult Victoria()
    {
        return View();
    }
        public IActionResult Nivel1()
    {
        BD bd = new BD();
        conseguirPistas();
        ViewBag.Pistas = int.Parse(HttpContext.Session.GetString("Pistas"));
        HttpContext.Session.SetString("TiendaComodines", "");
        if (bd.ObtenerNivel(HttpContext.Session.GetString("Usuario")) != 1)
        {
            return RedirectToAction("NivelActual");
        }
        if(int. Parse(HttpContext.Session.GetString("Intentos")) <= 0)
        {

            return RedirectToAction("SinIntentos");
        }
        ViewBag.Nivel = bd.ObtenerDatosNivel(1, int.Parse(HttpContext.Session.GetString("IdsNivel").Split(',', StringSplitOptions.RemoveEmptyEntries)[int.Parse(HttpContext.Session.GetString("Intentos")) - 1]));
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
        conseguirPistas();
        ViewBag.Pistas = int.Parse(HttpContext.Session.GetString("Pistas"));
        HttpContext.Session.SetString("TiendaComodines", "");
        if (bd.ObtenerNivel(HttpContext.Session.GetString("Usuario")) != 2)
        {
            return RedirectToAction("NivelActual");
        }
        if(int. Parse(HttpContext.Session.GetString("Intentos")) <= 0)
        {

            return RedirectToAction("SinIntentos");
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
        conseguirPistas();
        ViewBag.Pistas = int.Parse(HttpContext.Session.GetString("Pistas"));
        HttpContext.Session.SetString("TiendaComodines", "");
        if (bd.ObtenerNivel(HttpContext.Session.GetString("Usuario")) != 3)
        {
            return RedirectToAction("NivelActual");
        }
        if(int. Parse(HttpContext.Session.GetString("Intentos")) <= 0)
        {

            return RedirectToAction("SinIntentos");
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
        conseguirPistas();
        ViewBag.Pistas = int.Parse(HttpContext.Session.GetString("Pistas"));
        HttpContext.Session.SetString("TiendaComodines", "");
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
            return View("Victoria");
        }
        int numero = int.Parse(HttpContext.Session.GetString("IdsNivel").Split(',', StringSplitOptions.RemoveEmptyEntries)[random.Next(0, 3)]);
        ViewBag.Nivel = ConstruirNivel4(numero);
        return View();
    }
    public Nivel ConstruirNivel4(int id)
    {
        BD bd = new BD();
        string pregunta = "";
        string consigna = "";
        string respuesta = "";
        string pista = "";
        List<Carta> cartas = new List<Carta>();
        string imagesHtml = "<div class='consigna-html-4'>";
        Random random = new Random();
        cartas = GenerarCarta(5);
        foreach (Carta carta in cartas)
        {
            imagesHtml += $"<img src='{carta.Imagen}' alt='{carta.Palo} {carta.Numero}' style='height:200px; margin-right: 10px; width: 150px;'>";
        }
        imagesHtml += "</div>";
        imagesHtml += "<span class='d-block mt-2 text-muted'>Jimbo considera que estos simbolos te pueden servir:</span><span class='d-block mt-2 tag-suit'>♥ ♠ ♦ ♣</span>";
        switch (id)
        {
            case 1:
                pregunta = bd.ObtenerPregunta(1);
                consigna = $"<p>{pregunta}</p> {imagesHtml}";
                respuesta = cartas[1].Numero + cartas[1].Palo;
                pista = "<p>la carta que necesitas es la segunda de la lista</p>";
                break;
            case 2:
                pregunta = bd.ObtenerPregunta(2);
                consigna = $"<p>{pregunta}</p> {imagesHtml}";
                respuesta = cartas[2].Numero + cartas[2].Palo;
                pista = $"<p>la carta que necesitas es de {cartas[2].Palo}</p>";
                break;
            case 3:
                int ran = random.Next(0, 5);
                pregunta = bd.ObtenerPregunta(3);
                consigna = $"<p>Qué carta estaba en la posición {ran + 1}?</p> {imagesHtml}";
                respuesta = cartas[ran].Numero + cartas[ran].Palo;
                pista = $"<p>la carta que necesitas es de {cartas[ran].Numero}</p>";
                break;
            case 4:
                pregunta = bd.ObtenerPregunta(4);
                consigna = $"<p>{pregunta}</p> {imagesHtml}";
                int numero = 0;
                foreach (Carta carta in cartas)
                {
                    if(carta.Palo == "♥")
                    {
                        numero ++;
                    }
                }
                respuesta = numero.ToString();
                pista = $"<p>la cantidad de cartas de corazones es mas o menos {numero + random.Next(-1, 2)}</p>";
                break;
        }
        return new Nivel { Desafio = consigna, Respuesta = respuesta, Pista = pista };
    }
    public List<Carta> GenerarCarta(int cantidad)
    {
        List<Carta> cartas = new List<Carta>();
        Random random = new Random();
        string[] palos = { "♥", "♦", "♣", "♠" };
        string[] palos2 = { "Corazones", "Diamantes", "Treboles", "Picas" };
        string[] numeros = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
        

        for (int i = 0; i < cantidad; i++)
        {
            int numeroRandom = random.Next(palos.Length);
            string palo = palos[numeroRandom];
            string palo2 = palos2[numeroRandom];
            string numero = numeros[random.Next(numeros.Length)];
            string imagen = $"/imagenes/cartas/{numero}_{palo2}.png"; 
            cartas.Add(new Carta { Palo = palo, Numero = numero, Imagen = imagen });
        }

        return cartas;
    }
    public void conseguirPistas()
    {
        BD bd = new BD();
        string? usuario = HttpContext.Session.GetString("Usuario");
        if (string.IsNullOrEmpty(usuario))
        {
            return;
        }
        if (bd.ObtenerComodin(usuario) != null)
        {
            if (bd.ObtenerComodin(usuario).Id == 1)
            {
                int pistas = ObtenerEnteroDeSesion("Pista") + 1;
                HttpContext.Session.SetString("Pista", pistas.ToString());
            }
            else if (bd.ObtenerNivel(usuario)%2 == 1 && bd.ObtenerComodin(usuario).Id == 2)
            {
                int pistas = ObtenerEnteroDeSesion("Pista") + 2;
                HttpContext.Session.SetString("Pista", pistas.ToString());
            }
            else if (bd.ObtenerNivel(usuario)%2 == 0 && bd.ObtenerComodin(usuario).Id == 3)
            {
                int pistas = ObtenerEnteroDeSesion("Pista") + 2;
                HttpContext.Session.SetString("Pista", pistas.ToString());
            }
        }  
    }

    private int ObtenerEnteroDeSesion(string key, int valorPorDefecto = 0)
    {
        string? valor = HttpContext.Session.GetString(key);
        return int.TryParse(valor, out int resultado) ? resultado : valorPorDefecto;
    }

    [HttpPost]
    public IActionResult VerificarRespuesta(string respuesta, int id, int pistasUsadas)
    {
        BD bd = new BD();
        int intentos;
        int nivel = bd.ObtenerNivel(HttpContext.Session.GetString("Usuario"));
        Nivel datosNivel = bd.ObtenerDatosNivel(nivel, id);
        HttpContext.Session.SetString("Pistas", (int.Parse(HttpContext.Session.GetString("Pistas")) - pistasUsadas).ToString());
        if (respuesta.ToUpper() == datosNivel.Respuesta)
        {
            int completados = int.Parse(HttpContext.Session.GetString("Completados")) + 1;
            HttpContext.Session.SetString("Completados", completados.ToString());
        }
        else if (bd.ObtenerComodin(HttpContext.Session.GetString("Usuario")) != null && bd.ObtenerComodin(HttpContext.Session.GetString("Usuario")).Id == 4)
        {
            intentos = int.Parse(HttpContext.Session.GetString("Intentos")) + 1;
        }
        intentos = int.Parse(HttpContext.Session.GetString("Intentos")) - 1;
        HttpContext.Session.SetString("Intentos", intentos.ToString());
        return RedirectToAction($"Nivel{nivel}");
    }
    public IActionResult VerificarRespuesta4(string respuesta, string respuestaCorrecta, int pistasUsadas)
    {
        BD bd = new BD();
        int intentos;
        int nivel = bd.ObtenerNivel(HttpContext.Session.GetString("Usuario"));
        HttpContext.Session.SetString("Pistas", (int.Parse(HttpContext.Session.GetString("Pistas")) - pistasUsadas).ToString());
        if (respuesta.ToUpper() == respuestaCorrecta.ToUpper())
        {
            int completados = int.Parse(HttpContext.Session.GetString("Completados")) + 1;
            HttpContext.Session.SetString("Completados", completados.ToString());
        }
        else if (bd.ObtenerComodin(HttpContext.Session.GetString("Usuario")) != null && bd.ObtenerComodin(HttpContext.Session.GetString("Usuario")).Id == 4)
        {
            intentos = int.Parse(HttpContext.Session.GetString("Intentos")) + 1;
        }
        intentos = int.Parse(HttpContext.Session.GetString("Intentos")) - 1;
        HttpContext.Session.SetString("Intentos", intentos.ToString());
        return RedirectToAction($"Nivel{nivel}");
    }
    public IActionResult ComprarComodin(int id)
    {
        BD bd = new BD();
        string usuario = HttpContext.Session.GetString("Usuario");
        Comodin comodin = bd.ObtenerComodinPorId(id);
        string Nmensaje;
        int plata = bd.ObtenerPlata(usuario);
        if (plata >= comodin.Precio)
        {
            bd.cambiarPlata(usuario, -comodin.Precio);
            bd.CambiarComodin(usuario, id);
            Nmensaje = "Comodín comprado con éxito.";
        }
        else
        {
            Nmensaje = "No tenes suficiente plata para comprar este comodín.";
        }
        return RedirectToAction("Tienda", new { mensaje = Nmensaje });
    }

    
    public IActionResult CerrarSesion()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
    public IActionResult FinalizarPartida()
    {
        BD bd = new BD();
        string usuario = HttpContext.Session.GetString("Usuario");
        bd.FinalizarPartida(usuario);
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
