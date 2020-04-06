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

        public ItemQuadroController(Contexto context) 
        {
            _context = context;    
        }

        public async Task<IActionResult> Create(int Id)
        {
            ViewBag.QuadroId = Id;
            ViewBag.Pessoa = await _context.Pessoa.ToListAsync();           
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
        public async Task<IActionResult> LocalGeralCreate(int LocalId)
        {            
            var escala = HttpContext.Session.GetInt32("Escala_Mes");      
                      
            var listaQuadro = await _context.Quadro
                .Where(q => q.EscalaId == escala).ToListAsync();            

            foreach (var item in listaQuadro){
                ItemQuadro ItemQuadro = new ItemQuadro();
                ItemQuadro.QuadroId = item.Id;
                ItemQuadro.LocalId = LocalId;

                _context.Add(ItemQuadro);
                await _context.SaveChangesAsync();
            }
            
     
            return RedirectToAction("Index", "Quadro");            
           
        }

        [HttpPost]
        public async Task<IActionResult> LimparLocal()
        {            
            var escala = HttpContext.Session.GetInt32("Escala_Mes");      
                      
            var listaQuadro = await _context.Quadro
                .Where(q => q.EscalaId == escala).ToListAsync();

            //var vinculo = _context.Quadro.FromSqlRaw("select top 1 id from quadro where escalaId = " + escala);            
            //var listaItemQuadro = await _context.ItemQuadro.Where(p => p.QuadroId == vinculo.Id).ToListAsync();   
            var listaItemQuadro = await _context.ItemQuadro.ToListAsync();

            var ItemQuadro = await _context.ItemQuadro.SingleOrDefaultAsync(p => p.Id == 10);
                  
            foreach (var item in listaQuadro){
                foreach (var item02 in listaItemQuadro){
                    if (item.Id == item02.QuadroId){                        
                        ItemQuadro = await _context.ItemQuadro.SingleOrDefaultAsync(p => p.QuadroId == item.Id);
                        _context.ItemQuadro.Remove(ItemQuadro);
                        await _context.SaveChangesAsync();
                    }
                }
            }            
     
            return RedirectToAction("Index", "Quadro");            
           
        }


        
        public async Task<IActionResult> GerarEscala()
        {            
            var escala = HttpContext.Session.GetInt32("Escala_Mes");      
                      
            // Lista de Quadro pertencentes ao Mês selecionado (Session 'Escala_mes'):
            var listaQuadro = await _context.Quadro.Where(q => q.EscalaId == escala).ToListAsync();   

            // lista todas as pessoas
            var listaPessoa = await _context.Pessoa.ToListAsync();  
            //Pega nr total de pessoas:                               
            var totalPessoas = listaPessoa.Count;
            
            var cont = 0;
            //Inicia Laço: Quadro por Quadro do Mês selecionado            
            foreach (var quadro in listaQuadro){
                //Lista registros da tabela ItemQuadro do Quadro atual no laço:
                var listaItemQuadro = await _context.ItemQuadro.Where(p => p.QuadroId == quadro.Id).ToListAsync();
                //Laço com lista de registros da tabela ItemQuadro:                
                foreach (var ItemQuadro in listaItemQuadro){    
                    /*Atualiza registro da tabela ItemQuadro com informaçõa da Pessoa, a partir da lista total de 
                    Pessoas. Quando toda a lista de pessoas é percorrida, o registro volta para o início, fazendo um
                    loop nas pessoas até que toda a escala esteja preenchida. Para isso usa-se as variáveis 'cont', 
                    que funciona como contador da lista, e a variável 'totalPessoas', que faz o controle da quantidade
                    de pessoas, informando a hora de retornar para o início se a lista atingir o total:          */
                    ItemQuadro.PessoaId = listaPessoa[cont].Id;                    
                    _context.Update(ItemQuadro);
                    await _context.SaveChangesAsync();

                    cont = cont + 1;
                    //se 'cont' tiver o número total de pessoas, 'cont' volta para o valor zero.
                    if (cont >= totalPessoas)
                        cont = 0;

                    //cont = cont >= totalPessoas ? 0 : cont + 1;
                }
               
            }            
            
            return RedirectToAction("Index", "Quadro");            
           
        }


    }
}   