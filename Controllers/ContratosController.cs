using InmobiliariaAlbornoz.Data;
using InmobiliariaAlbornoz.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlbornoz.Controllers
{
    public class ContratosController : Controller
    {
        private RepoContrato repo;
        private RepoInmueble repoInmueble;
        private RepoInquilino repoInquilino;
        public ContratosController(IConfiguration config)
        {
            this.repo = new RepoContrato(config);
            this.repoInmueble = new RepoInmueble(config);
            this.repoInquilino = new RepoInquilino(config);
        }

        // GET: ContratosController
        public ActionResult Index()
        {
            try
            {
                var lista = repo.All();
                return View(lista);
            }
            catch (Exception e)
            {
                ViewData["Error"] = "Hubo un error al solicitar la lista de contratos: " + e.Message;
                return View();
            }
            
        }

        // GET: ContratosController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var c = repo.Details(id);
                return View(c);

            }
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
                return View();
            }
        }

        // GET: ContratosController/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.Inmuebles = repoInmueble.All();
                ViewBag.Inquilinos = repoInquilino.All();

                return View();
            }
            catch (Exception e)
            {
                ViewData["Error"] = "Ocurrió un error al recuperar Inmuebles e/o Inquilinos: " + e.Message;
                return View();
                
            }
        }

        // POST: ContratosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato c)
        {
            try
            {
                var res = repo.Put(c);
                if (res > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    Exception e = new Exception("No se cargó el contrato. Intente de nuevo");
                    throw e;
                }
                
            }
            catch(Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction(nameof(Create));
            }
        }

        // GET: ContratosController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var c = repo.Details(id);
                ViewBag.Inmuebles = repoInmueble.All();
                ViewBag.Inquilinos = repoInquilino.All();
                return View(c);
            }
            catch (Exception e)
            {
                ViewData["Error"] = "Ocurrió un error al recuperar inmuebles e/o inquilinos: " + e.Message;
                return View();

            }

        }

        // POST: ContratosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contrato c)
        {
            try
            {
                c.Id = id;
                var res = repo.Edit(c);
                if (res > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    Exception e = new Exception("No se guardaron cambios. Intente nuevamente.");
                    throw e;
                }
            }
            catch(Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction(nameof(Edit), id);
            }
        }

        // GET: ContratosController/Delete/5
        public ActionResult Delete(int id)
        {
            //FALTA UN TRY CATCH?
            Contrato c = repo.Details(id);
            return View(c);
        }

        // POST: ContratosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var res = repo.Delete(id);
                if (res > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    Exception e = new Exception("Fallo al guardar cambios. Intente nuevamente.");
                    throw e;
                }
            }
            catch(Exception e)
            {
                ViewData["Error"] = e.Message;
                return View();
            }
        }
    }
}
