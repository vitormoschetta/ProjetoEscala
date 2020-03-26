namespace ProjetoEscala.Models
{
    public class PessoaLocal
    {
        public int Id {get; set;}
        public int PessoaId {get; set;}
        public Pessoa Pessoa {get; set;}
        public int LocalId {get; set;}
        public Local Local {get; set;}
    }
}