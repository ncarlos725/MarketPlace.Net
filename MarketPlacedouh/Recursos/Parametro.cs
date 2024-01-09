namespace MarketPlacedouh.Recursos
{
    public class Parametro
    {
        public Parametro(string nombre, string valor)  // constructuor
        {
            Nombre = nombre;
            Valor = valor;
        } 
        public string Nombre { get; set; }
        public string Valor { get; set; }
    }
}
