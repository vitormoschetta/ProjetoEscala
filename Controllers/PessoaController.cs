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
    public class PessoaController: Controller
    {
        private readonly Contexto _context;

        public PessoaController(Contexto context) 
        {
            _context = context;    
        }


        public async Task<IActionResult> AdicionarLocal(int localId, int pessoaId)
        {            
            PessoaLocal PessoaLocal = new PessoaLocal();
            PessoaLocal.PessoaId = pessoaId;
            PessoaLocal.LocalId = localId;

            _context.Add(PessoaLocal);
            await _context.SaveChangesAsync();

            return PartialView("_ListaLocal");
        }


        public async Task<IActionResult> RetirarLocal(int localId, int pessoaId)
        {            
            var pessoaLocal = await _context.PessoaLocal
                                .SingleOrDefaultAsync(p => p.PessoaId == pessoaId && p.LocalId == localId);

            if (pessoaLocal != null){
                 _context.PessoaLocal.Remove(pessoaLocal);
                await _context.SaveChangesAsync();                
            }
            return PartialView("_ListaLocal");
        }


        public async Task<IActionResult> Index()
        {            
            var listaPessoa = await _context.Pessoa.ToListAsync();
            return View(listaPessoa);
        }
           


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Ativo")] Pessoa pessoa)
        {
            if (ModelState.IsValid){
                _context.Add(pessoa);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pessoa);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var pessoa = await _context.Pessoa.SingleOrDefaultAsync(m => m.Id == id);
            if (pessoa == null)            
                return NotFound();

            //ViewBag.ListaLocal = await _context.Local.ToListAsync();
            //ViewBag.ListaPessoaLocal = await _context.PessoaLocal
                                        //.Where(p => p.PessoaId == pessoa.Id).ToListAsync();
            
            return View(pessoa);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Ativo")] Pessoa pessoa)
        {
            if (id != pessoa.Id)
                return NotFound();            

            if (ModelState.IsValid){
                try{
                    _context.Update(pessoa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException){
                    if (!PessoaExists(pessoa.Id))
                        return NotFound();
                    else
                        throw;                    
                }
                return RedirectToAction("Index");
            }
            return View(pessoa);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)            
                return NotFound();            

            var pessoa = await _context.Pessoa.SingleOrDefaultAsync(m => m.Id == id);
            if (pessoa == null)            
                return NotFound();            

            return View(pessoa);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pessoa = await _context.Pessoa.SingleOrDefaultAsync(m => m.Id == id);
            _context.Pessoa.Remove(pessoa);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        private bool PessoaExists(int id)
        {
            return _context.Pessoa.Any(e => e.Id == id);
        }
    
    }
}