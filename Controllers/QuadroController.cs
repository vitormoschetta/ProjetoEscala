using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using ProjetoEscala.Context;
using ProjetoEscala.Models;


namespace ProjetoEscala.Controllers
{
    public class QuadroController: Controller
    {
        private readonly Contexto _context;

        public QuadroController(Contexto context) 
        {
            _context = context;    
        }


        public async Task<IActionResult> Print()
        {
            
            return View();
        }

        public async Task<IActionResult> Index()
        {                        
            ViewBag.Local = await _context.Local.ToListAsync();
            ViewBag.Pessoa = await _context.Pessoa.ToListAsync();
            ViewBag.Escala = await _context.Escala.ToListAsync();
            ViewBag.Evento = await _context.Evento.ToListAsync();             
            ViewBag.PessoaQuadro = await _context.PessoaQuadro.ToListAsync(); 
    
            var escalaId = HttpContext.Session.GetInt32("Escala_Mes");  

            if (escalaId != 0){
                ViewBag.ListaQuadro = await _context.Quadro
                    .Where(q => q.EscalaId == escalaId)
                    .OrderBy(q => q.Data)
                    .ToListAsync();
            }
            else{                
                ViewBag.ListaQuadro = await _context.Quadro                    
                .ToListAsync();
            }                                
            
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> QuadroPorEscala(int escalaId)
        {            
            ViewBag.Local = await _context.Local.ToListAsync();
            ViewBag.Pessoa = await _context.Pessoa.ToListAsync();
            ViewBag.Escala = await _context.Escala.ToListAsync();
            ViewBag.Evento = await _context.Evento.ToListAsync();   
            ViewBag.PessoaQuadro = await _context.PessoaQuadro.ToListAsync();           

            if(escalaId != 0){
                HttpContext.Session.SetInt32("Escala_Mes", escalaId);  

                ViewBag.ListaQuadro = await _context.Quadro
                    .Where(q => q.EscalaId == escalaId)
                    .OrderBy(q => q.Data)
                    .ToListAsync();
            }
            else{
                ViewBag.ListaQuadro = await _context.Quadro                    
                    .ToListAsync();
            }

            return PartialView("_TabelaQuadro");

        }


        public async Task<IActionResult> Create()
        {
            ViewBag.Escala = await _context.Escala.ToListAsync();
            ViewBag.Evento = await _context.Evento.ToListAsync();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Data,EventoId,EscalaId")] Quadro Quadro)
        {
            if (ModelState.IsValid){
                _context.Add(Quadro);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(Quadro);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var Quadro = await _context.Quadro.SingleOrDefaultAsync(m => m.Id == id);
            ViewBag.Evento = await _context.Evento.ToListAsync();

            if (Quadro == null)            
                return NotFound();

            return View(Quadro);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Data,EventoId,EscalaId")] Quadro Quadro)
        {
            if (id != Quadro.Id)
                return NotFound();            

            if (ModelState.IsValid){
                try{
                    _context.Update(Quadro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException){
                    if (!QuadroExists(Quadro.Id))
                        return NotFound();
                    else
                        throw;                    
                }
                return RedirectToAction("Index");
            }
            return View(Quadro);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)            
                return NotFound();            

            var Quadro = await _context.Quadro.SingleOrDefaultAsync(m => m.Id == id);
            if (Quadro == null)            
                return NotFound();            

            return View(Quadro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Quadro = await _context.Quadro.SingleOrDefaultAsync(m => m.Id == id);
            _context.Quadro.Remove(Quadro);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        private bool QuadroExists(int id)
        {
            return _context.Quadro.Any(e => e.Id == id);
        }

        
    }
}