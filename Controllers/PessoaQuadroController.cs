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

            var pessoaQuadro = await _context.PessoaQuadro.SingleOrDefaultAsync(m => m.Id == id);

            if (pessoaQuadro == null)            
                return NotFound();
            
            ViewBag.Pessoas = await _context.Pessoa.ToListAsync();           
            ViewBag.Local = await _context.Local.ToListAsync();     
            ViewBag.QuadroId = pessoaQuadro.QuadroId;      

            return View(pessoaQuadro);
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


    }
}