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
    public class EscalaController: Controller
    {   
        private readonly Contexto _context;

        public EscalaController(Contexto context) 
        {
            _context = context;    
        }

        public async Task<IActionResult> Index()
        {
            var listaEscala = await _context.Escala.ToListAsync();
            return View(listaEscala);
        }
           


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Escala Escala)
        {
            if (ModelState.IsValid){
                _context.Add(Escala);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(Escala);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var Escala = await _context.Escala.SingleOrDefaultAsync(m => m.Id == id);
            if (Escala == null)            
                return NotFound();
            
            return View(Escala);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Escala Escala)
        {
            if (id != Escala.Id)
                return NotFound();            

            if (ModelState.IsValid){
                try{
                    _context.Update(Escala);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException){
                    if (!EscalaExists(Escala.Id))
                        return NotFound();
                    else
                        throw;                    
                }
                return RedirectToAction("Index");
            }
            return View(Escala);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)            
                return NotFound();            

            var Escala = await _context.Escala.SingleOrDefaultAsync(m => m.Id == id);
            if (Escala == null)            
                return NotFound();            

            return View(Escala);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Escala = await _context.Escala.SingleOrDefaultAsync(m => m.Id == id);
            _context.Escala.Remove(Escala);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        private bool EscalaExists(int id)
        {
            return _context.Escala.Any(e => e.Id == id);
        }
    
    }
}