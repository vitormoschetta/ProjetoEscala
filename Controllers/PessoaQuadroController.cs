using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using ProjetoEscala.Context;
using ProjetoEscala.Models;
using Microsoft.EntityFrameworkCore;

namespace ProjetoEscala.Controllers
{
    public class PessoaQuadroController: Controller
    {
        private readonly Contexto _context;

        public PessoaQuadroController(Contexto context) 
        {
            _context = context;    
        }
    }
}