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

       
        
        public async Task<IActionResult> GerarEscala()
        {            
            var escala = HttpContext.Session.GetInt32("Escala_Mes");      
                      
            // Lista de Quadro pertencentes ao Mês selecionado (Session 'Escala_mes'):
            var listaQuadro = await _context.Quadro.Where(q => q.EscalaId == escala).ToListAsync();   
            // lista todas as pessoas
            var listaPessoa = await _context.Pessoa.ToListAsync();  
            //Pega nr total de pessoas:                               
            int totalPessoas = listaPessoa.Count;
            
            int indicePessoas = 0;
            int indiceQuadro =  0;
            //Inicia Laço: Quadro por Quadro do Mês selecionado            
            foreach (var quadro in listaQuadro){
                //Lista registros da tabela ItemQuadro do Quadro atual no laço:
                var listaItemQuadro = await _context.ItemQuadro.Where(p => p.QuadroId == quadro.Id).ToListAsync();
                //Laço com lista de registros da tabela ItemQuadro:                
                foreach (var itemQuadro in listaItemQuadro){    
                    /*Atualiza registro da tabela ItemQuadro com informaçõa da Pessoa, a partir da lista total de 
                    Pessoas. Quando toda a lista de pessoas é percorrida, o registro volta para o início, fazendo um
                    loop nas pessoas até que toda a escala esteja preenchida. Para isso usa-se as variáveis 'cont', 
                    que funciona como contador da lista, e a variável 'totalPessoas', que faz o controle da quantidade
                    de pessoas, informando a hora de retornar para o início se a lista atingir o total:          */                    

                    //Pega a lista de LOCAL que a pessoa atual da listaPessoa está configurada a ser escalada
                    var listaPessoaLocal = await _context.PessoaLocal.Where(p => p.PessoaId == listaPessoa[indicePessoas].Id).ToListAsync();
                    
                    //Verifica se na lista de LOCAL configurada para a pessoal atual da listaPessoa consta o Local do ItemQuadro Atual:
                    foreach (var pessoaLocal in listaPessoaLocal){                        
                        if (pessoaLocal.LocalId == itemQuadro.LocalId){
                            //Se "Sim" atualiza itemQuadro com a pessoa atual da listaPessoa:
                            itemQuadro.PessoaId = listaPessoa[indicePessoas].Id; 
                            _context.Update(itemQuadro);
                            await _context.SaveChangesAsync();
                        }                      
                    }                     
                    /*se a pessoa atual da listaPessoa não pode ser escalada no local do itemQuadro,
                    proceder novo loop com todas as pessoas até encontrar alguém:               */
                    if (itemQuadro.PessoaId == 0){      
                        //var indicePessoas02 = indicePessoas + 1;
                        for (int i = indicePessoas + 1; i != indicePessoas; i++)
                        {
                            //Quando alguem for encontrado: parar o laço com break
                            if (itemQuadro.PessoaId != 0){
                                break;
                            }
                            listaPessoaLocal = await _context.PessoaLocal.Where(p => p.PessoaId == listaPessoa[i].Id).ToListAsync();
                            foreach (var pessoaLocal in listaPessoaLocal){                        
                                if (pessoaLocal.LocalId == itemQuadro.LocalId){
                                    //Se "Sim" atualiza itemQuadro com a pessoa atual da listaPessoa:
                                    itemQuadro.PessoaId = listaPessoa[i].Id; 
                                    _context.Update(itemQuadro);
                                    await _context.SaveChangesAsync();
                                }                      
                            }


                            if(i >= totalPessoas)
                                i = 0;                 
                        }
                    }     

                    indicePessoas++;
                    //se 'cont' tiver o número total de pessoas, 'cont' volta para o valor zero.
                    if (indicePessoas >= totalPessoas)
                        indicePessoas = 0;

                }     
                indiceQuadro++;                          
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



    }
}   