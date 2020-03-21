namespace ProjetoEscala.Models
{
    public class Aviso
    {
        public int Id {get; set;}
        public string Mensagem {get; set;}
        public int EscalaId {get; set;}
        public  Escala Escala {get; set;}
    }
}