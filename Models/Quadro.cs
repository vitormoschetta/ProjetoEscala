using System.Collections.Generic;
using System;

namespace ProjetoEscala.Models
{
    public class Quadro
    {
        public int Id {get; set;}

        public int EventoId {get; set;}
        public Evento Evento {get; set;}

        public int EscalaId {get; set;}
        public Escala Escala {get; set;}

        public ICollection<PessoaQuadro> ListaPessoaQuadro {get; set;}

        public DateTime Data {get; set;}

    }
}