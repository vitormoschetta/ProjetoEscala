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
    public class EventoController: Controller
    {
        private readonly Contexto _context;

        public EventoController(Contexto context) 
        {
            _context = context;    
        }


        public async Task<IActionResult> Index()
        {
            var listaEvento = await _context.Evento.ToListAsync();
            return View(listaEvento);
        }
           


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Evento Evento)
        {
            if (ModelState.IsValid){
                _context.Add(Evento);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(Evento);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var Evento = await _context.Evento.SingleOrDefaultAsync(m => m.Id == id);
            if (Evento == null)            
                return NotFound();
            
            return View(Evento);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Evento Evento)
        {
            if (id != Evento.Id)
                return NotFound();            

            if (ModelState.IsValid){
                try{
                    _context.Update(Evento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException){
                    if (!EventoExists(Evento.Id))
                        return NotFound();
                    else
                        throw;                    
                }
                return RedirectToAction("Index");
            }
            return View(Evento);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)            
                return NotFound();            

            var Evento = await _context.Evento.SingleOrDefaultAsync(m => m.Id == id);
            if (Evento == null)            
                return NotFound();            

            return View(Evento);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Evento = await _context.Evento.SingleOrDefaultAsync(m => m.Id == id);
            _context.Evento.Remove(Evento);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        private bool EventoExists(int id)
        {
            return _context.Evento.Any(e => e.Id == id);
        }
    }
}