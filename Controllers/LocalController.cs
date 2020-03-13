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
    public class LocalController: Controller
    {
        private readonly Contexto _context;

        public LocalController(Contexto context) 
        {
            _context = context;    
        }


        public async Task<IActionResult> Index()
        {
            var listaLocal = await _context.Local.ToListAsync();
            return View(listaLocal);
        }
           


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Local Local)
        {
            if (ModelState.IsValid){
                _context.Add(Local);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(Local);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var Local = await _context.Local.SingleOrDefaultAsync(m => m.Id == id);
            if (Local == null)            
                return NotFound();
            
            return View(Local);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Local Local)
        {
            if (id != Local.Id)
                return NotFound();            

            if (ModelState.IsValid){
                try{
                    _context.Update(Local);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException){
                    if (!LocalExists(Local.Id))
                        return NotFound();
                    else
                        throw;                    
                }
                return RedirectToAction("Index");
            }
            return View(Local);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)            
                return NotFound();            

            var Local = await _context.Local.SingleOrDefaultAsync(m => m.Id == id);
            if (Local == null)            
                return NotFound();            

            return View(Local);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Local = await _context.Local.SingleOrDefaultAsync(m => m.Id == id);
            _context.Local.Remove(Local);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        private bool LocalExists(int id)
        {
            return _context.Local.Any(e => e.Id == id);
        }
    }
}