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
    public class PessoaQuadroController: Controller
    {
        private readonly Contexto _context;

        public PessoaQuadroController(Contexto context) 
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
        public async Task<IActionResult> CreateSave([Bind("Id,PessoaId,LocalId,QuadroId")] PessoaQuadro pessoaQuadro)
        {
            if (ModelState.IsValid){
                _context.Add(pessoaQuadro);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Quadro");
            }
            return View(pessoaQuadro);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            ViewBag.PessoaQuadro = await _context.PessoaQuadro.SingleOrDefaultAsync(m => m.Id == id);

            if (ViewBag.PessoaQuadro == null)            
                return NotFound();
            
            ViewBag.Pessoa = await _context.Pessoa.ToListAsync();           
            ViewBag.Local = await _context.Local.ToListAsync();         

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, [Bind("Id,PessoaId,LocalId,QuadroId")] PessoaQuadro pessoaQuadro)
        {
            if (Id != pessoaQuadro.Id)
                return NotFound();            

            if (ModelState.IsValid){
                try{
                    _context.Update(pessoaQuadro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException){
                    if (!PessoaQuadroExists(pessoaQuadro.Id))
                        return NotFound();
                    else
                        throw;                    
                }
                return RedirectToAction("Index", "Quadro");
            }
            return View(pessoaQuadro);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)            
                return NotFound();            

            var pessoaQuadro = await _context.PessoaQuadro.SingleOrDefaultAsync(m => m.Id == id);
            if (pessoaQuadro == null)            
                return NotFound();            

            return View(pessoaQuadro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pessoaQuadro = await _context.PessoaQuadro.SingleOrDefaultAsync(m => m.Id == id);
            _context.PessoaQuadro.Remove(pessoaQuadro);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Quadro");
        }

        private bool PessoaQuadroExists(int id)
        {
            return _context.PessoaQuadro.Any(e => e.Id == id);
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
                PessoaQuadro pessoaQuadro = new PessoaQuadro();
                pessoaQuadro.QuadroId = item.Id;
                pessoaQuadro.LocalId = LocalId;

                _context.Add(pessoaQuadro);
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
            //var listaPessoaQuadro = await _context.PessoaQuadro.Where(p => p.QuadroId == vinculo.Id).ToListAsync();   
            var listaPessoaQuadro = await _context.PessoaQuadro.ToListAsync();

            var pessoaQuadro = await _context.PessoaQuadro.SingleOrDefaultAsync(p => p.Id == 10);
                  
            foreach (var item in listaQuadro){
                foreach (var item02 in listaPessoaQuadro){
                    if (item.Id == item02.QuadroId){                        
                        pessoaQuadro = await _context.PessoaQuadro.SingleOrDefaultAsync(p => p.QuadroId == item.Id);
                        _context.PessoaQuadro.Remove(pessoaQuadro);
                        await _context.SaveChangesAsync();
                    }
                }
            }            
     
            return RedirectToAction("Index", "Quadro");            
           
        }


        
        public async Task<IActionResult> GerarEscala()
        {            
            HttpContext.Session.SetInt32("teste", 10);
            var escala = HttpContext.Session.GetInt32("Escala_Mes");      
                      
            var listaQuadro = await _context.Quadro
                .Where(q => q.EscalaId == escala).ToListAsync();   

            var listaPessoa = await _context.Pessoa.ToListAsync();                     
            var cont = 0;
            var controle = listaPessoa.Count;

            foreach (var quadro in listaQuadro){
                var listaPessoaQuadro = await _context.PessoaQuadro.Where(p => p.QuadroId == quadro.Id).ToListAsync();
                foreach (var pessoaQuadro in listaPessoaQuadro){                      
                    pessoaQuadro.PessoaId = listaPessoa[cont].Id;                    
                    _context.Update(pessoaQuadro);
                    await _context.SaveChangesAsync();

                    cont = cont + 1;
                    if (cont >= controle)
                        cont = 0;
                }
               
            }            
            
            return RedirectToAction("Index", "Quadro");            
           
        }


    }
}   