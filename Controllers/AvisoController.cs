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
    public class AvisoController:Controller
    {   
        private readonly Contexto _context;

        public AvisoController(Contexto context) 
        {
            _context = context;    
        }
    
         public async Task<IActionResult> Index()
        {
            var listaAviso = await _context.Aviso.ToListAsync();
            return View(listaAviso);
        }
           


        public IActionResult Create()
        {
            
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Mensagem,EscalaId")] Aviso Aviso)
        {
            if (ModelState.IsValid){
                _context.Add(Aviso);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(Aviso);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var Aviso = await _context.Aviso.SingleOrDefaultAsync(m => m.Id == id);
            if (Aviso == null)            
                return NotFound();
            
            return View(Aviso);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Mensagem,EscalaId")] Aviso Aviso)
        {
            if (id != Aviso.Id)
                return NotFound();            

            if (ModelState.IsValid){
                try{
                    _context.Update(Aviso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException){
                    if (!AvisoExists(Aviso.Id))
                        return NotFound();
                    else
                        throw;                    
                }
                return RedirectToAction("Index");
            }
            return View(Aviso);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)            
                return NotFound();            

            var Aviso = await _context.Aviso.SingleOrDefaultAsync(m => m.Id == id);
            if (Aviso == null)            
                return NotFound();            

            return View(Aviso);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Aviso = await _context.Aviso.SingleOrDefaultAsync(m => m.Id == id);
            _context.Aviso.Remove(Aviso);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        private bool AvisoExists(int id)
        {
            return _context.Aviso.Any(e => e.Id == id);
        }
    }
}