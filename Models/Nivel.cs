namespace TP06_Martinez_Loufer.Models;

public class Nivel
{
    public int Id { get; set; }
    public string Desafio { get; set; }
    public string Respuesta { get; set; }
    public string Pista { get; set; }
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
                cartas = GenerarCarta(5);
                imagesHtml = "<div style=\"display:flex;gap:8px;align-items:center;\">"
                    + string.Join("", cartas.Select(c => $"<img src=\"images/{c.Imagen}\" alt=\"{c.Palo} {c.Numero}\" style=\"height:100px;\">"))
                    + "</div>";
                consigna = pregunta + " " + imagesHtml;
                int numero = 0;
                foreach (Carta carta in cartas)
                {
                    if(carta.Palo == "Corazones")
                    {
                        numero ++;
                    }
                }
                respuesta = numero.ToString();
                pista = $"la cantidad de cartas de corazones es mas o menos {numero + random.Next(-1, 2)}";
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
}