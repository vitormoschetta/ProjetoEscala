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
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace ProjetoEscala.Controllers
{
    public class ItemQuadroController: Controller
    {
        private readonly Contexto _context;
        private readonly Conexao _conexao;

        public ItemQuadroController(Contexto context) 
        {
            _context = context;  
            _conexao = new Conexao();

        }

        public async Task<IActionResult> Create(int Id)
        {
            ViewBag.QuadroId = Id;
            ViewBag.Pessoa = await _context.Pessoa.Where(p => p.Ativo == 2).ToListAsync();       
            ViewBag.Local = await _context.Local.ToListAsync();           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSave([Bind("Id,PessoaId,LocalId,QuadroId")] ItemQuadro itemQuadro)
        {
            if (ModelState.IsValid){
                _context.Add(itemQuadro);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Quadro");
            }
            return View(itemQuadro);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            ViewBag.ItemQuadro = await _context.ItemQuadro.SingleOrDefaultAsync(m => m.Id == id);

            if (ViewBag.ItemQuadro == null)            
                return NotFound();
            
            ViewBag.Pessoa = await _context.Pessoa.ToListAsync();           
            ViewBag.Local = await _context.Local.ToListAsync();         

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, [Bind("Id,PessoaId,LocalId,QuadroId")] ItemQuadro itemQuadro)
        {
            if (Id != itemQuadro.Id)
                return NotFound();            

            if (ModelState.IsValid){
                try{
                    _context.Update(itemQuadro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException){
                    if (!ItemQuadroExists(itemQuadro.Id))
                        return NotFound();
                    else
                        throw;                    
                }
                return RedirectToAction("Index", "Quadro");
            }
            return View(itemQuadro);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)            
                return NotFound();            

            var ItemQuadro = await _context.ItemQuadro.SingleOrDefaultAsync(m => m.Id == id);
            if (ItemQuadro == null)            
                return NotFound();            

            return View(ItemQuadro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ItemQuadro = await _context.ItemQuadro.SingleOrDefaultAsync(m => m.Id == id);
            _context.ItemQuadro.Remove(ItemQuadro);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Quadro");
        }

        private bool ItemQuadroExists(int id)
        {
            return _context.ItemQuadro.Any(e => e.Id == id);
        }


        public async Task<IActionResult> LocalGeral()
        {          
            ViewBag.Local = await _context.Local.ToListAsync();           
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> LocalGeralCreate(int LocalId, int quantidade)
        {            
            var escala = HttpContext.Session.GetInt32("Escala_Mes");      
                      
            var listaQuadro = await _context.Quadro
                .Where(q => q.EscalaId == escala).ToListAsync();            

            foreach (var item in listaQuadro){
                for (int i = 0; i < quantidade; i++){
                    ItemQuadro ItemQuadro = new ItemQuadro();
                    ItemQuadro.QuadroId = item.Id;
                    ItemQuadro.LocalId = LocalId;

                    _context.Add(ItemQuadro);
                    await _context.SaveChangesAsync();
                }                
            }            
            return RedirectToAction("Index", "Quadro");            
           
        }

       
        public async Task<IActionResult> GerarEscala()
        {   // Id da escala selecionada:         
            var escala = HttpContext.Session.GetInt32("Escala_Mes");          
            // Lista de quadros/dias contidos na escala selecionada: 
            var listaQuadroId = _conexao.ListaQuadroIdPorEscala(Convert.ToInt32(escala));                    
            // Lista o Id dos locais diferentes/agrupados da tabela 'PessoaLocal' => configuracao do local que cada pessoa pode ser escalada
            IList<int> listaLocaisDiferentesNaEscala = _conexao.ListaLocalIdAgrupado(Convert.ToInt32(escala));
            // Obs: Cada local contido na lista acima, terá uma lista de Pessoas pre configuradas para ser escalada para este local especifico
            // Contador de quantos loops terei que dar nos quadros desta escala:            
            int quantidadeLoop = listaLocaisDiferentesNaEscala.Count;                        
            
            // Bloco de instancias => Para não sobrecarregar novas instancias em memória durante o loop
            IList<int> listaPessoaId = new List<int>();               
            int indicePessoas = new int();  
            int quantidadePessoas = new int();                        
            
            // Iniciando o loop de controle de local específico a preencher
            for (int i = 0; i < quantidadeLoop; i++){
                listaPessoaId.Clear(); // A cada loop é necessário limpar a lista do loop anterior
                // lista de pessoas cujo local configurado é igual ao local atual no loop (listaLocaisDiferentesNaEscala[i]):
                listaPessoaId = _conexao.ListaPessoaIdDeLocalEspecifico(listaLocaisDiferentesNaEscala[i]);
                // Contador de quantidade de pessoas para controlle do loop:
                quantidadePessoas = listaPessoaId.Count;   
                // Controlador do loop de pessoas => interage com 'quantidadePessoas':
                indicePessoas = 0;

                // Loop de controle de Quadro 
                foreach (var quadroId in listaQuadroId){
                    var listaItemQuadro = await _context.ItemQuadro.Where(iq => iq.QuadroId == quadroId).ToListAsync();
                    // Loop de controle de itemQuadro
                    foreach (var itemQuadro in listaItemQuadro){
                        if (itemQuadro.LocalId == listaLocaisDiferentesNaEscala[i]){
                            //Se o 'itemQuadro' tiver o 'LocalId' igual o 'listaLocaisDiferentesNaEscala' em loop[i]:
                            itemQuadro.PessoaId = listaPessoaId[indicePessoas];
                            _context.Update(itemQuadro);
                            await _context.SaveChangesAsync();   

                            indicePessoas++;
                            //quando atingir a ultima pessoa da listaPessoaLocalEspecifi, o indice volta pra zero:
                            if (indicePessoas >= quantidadePessoas)
                                indicePessoas = 0;     
                        }                                                
                    }
                }            
            }
            return RedirectToAction("Index", "Quadro");  
        }



        public async Task<IActionResult> LimparPessoas()
        {
            var escala = HttpContext.Session.GetInt32("Escala_Mes");
            var listaQuadro = await _context.Quadro
                .Where(q => q.EscalaId == escala).ToListAsync();

            foreach (var quadro in listaQuadro){
                var listaItemQuadro = await _context.ItemQuadro.Where(i => i.QuadroId == quadro.Id).ToListAsync();
                foreach (var itemQuadro in listaItemQuadro){
                    itemQuadro.PessoaId = 0;
                    _context.Update(itemQuadro);
                    await _context.SaveChangesAsync();     
                }
            }
                    
            return RedirectToAction("Index", "Quadro");
        }

        public async Task<IActionResult> LimparLocal()
        {
            var escala = HttpContext.Session.GetInt32("Escala_Mes");
            var listaQuadro = await _context.Quadro
                .Where(q => q.EscalaId == escala).ToListAsync();

            foreach (var quadro in listaQuadro){
                var listaItemQuadro = await _context.ItemQuadro.Where(i => i.QuadroId == quadro.Id).ToListAsync();
                foreach (var itemQuadro in listaItemQuadro){
                    _context.ItemQuadro.Remove(itemQuadro);
                    await _context.SaveChangesAsync();
                }
            }
                    
            return RedirectToAction("Index", "Quadro");
        }
        
        

        public async Task<IActionResult> LimparEscala()
        {
            var escala = HttpContext.Session.GetInt32("Escala_Mes");  
            var listaQuadro = await _context.Quadro
                .Where(q => q.EscalaId == escala).ToListAsync();

            foreach (var quadro in listaQuadro){               
                _context.Quadro.Remove(quadro);
                await _context.SaveChangesAsync();
            }                 
            return RedirectToAction("Index", "Quadro");     
        }


        /*
        public async Task<IActionResult> LimparTudoAJAX(int escalaId)
        {            
            var listaQuadro = await _context.Quadro
                .Where(q => q.EscalaId == escalaId).ToListAsync();

            foreach (var quadro in listaQuadro){               
                _context.Quadro.Remove(quadro);
                await _context.SaveChangesAsync();
            }                 
            return RedirectToAction("Index", "Quadro");     
        }
        */


    }
}   