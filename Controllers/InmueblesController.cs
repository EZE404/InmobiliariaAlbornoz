using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using InmobiliariaAlbornoz.Data;
using InmobiliariaAlbornoz.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace InmobiliariaAlbornoz.Controllers
{
    public class InmueblesController : Controller
    {
        private RepoInmueble repo;
        private RepoPropietario repoPropietario;

        public InmueblesController(IConfiguration config)
        {
            this.repo = new RepoInmueble(config);
            this.repoPropietario = new RepoPropietario(config);
        }

        // GET: Inmueble
        public ActionResult Index()
        {
            try
            {
                var lista = repo.All();
                return View(lista);
            }
            catch (Exception e)
            {
                ViewData["Error"] = "Hubo un error al solicitar la lista de inmuebles: " + e.Message;
                return View();
            }
        }

        // GET: Inmueble/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var i = repo.ById(id);
                return View(i);
            }
            catch (Exception ex)
            {
                 // TODO
                Debug.WriteLine(ex.ToString());
                return View();
            }
        }

        // GET: Inmueble/Create
        public ActionResult Create()
        {
            ViewBag.Propietarios = repoPropietario.All();
            return View();
        }

        // POST: Inmueble/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble i)
        {
            try
            {
                var res = repo.Put(i);
                if (res > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    Exception e = new Exception("No se cargó el inmueble. Intente de nuevo");
                    throw e;
                }

            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction(nameof(Create));
            }
        }

        // GET: Inmueble/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var propietarios = repoPropietario.All();
                ViewBag.Propietarios = propietarios;
                var i = repo.ById(id);
                return View(i);            
            }
            catch (Exception ex)
            {
                 // TODO
                Debug.WriteLine(ex.ToString());
                return View();

            }
        }

        // POST: Inmueble/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inmueble i)
        {
            try
            {
                // TODO: Add update logic here
                i.Id = id;
                var res = repo.Edit(i);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return View();
            }
        }

        // GET: Inmueble/Delete/5
        public ActionResult Delete(int id)
        {
            var i = repo.ById(id);
            return View(i);
        }

        // POST: Inmueble/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var res = repo.Delete(id);
                if (res > 0)
                {
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    Exception e = new Exception("Ocurrió un error al eliminar el registro. Intente nuevamente.");
                    throw e;
                }
            }
            catch(Exception e)
            {
                // CONSULTAR CÓMO HACER RECURSIVIDAD CON LA MISMA VISTA Y PASAR LOS PARÁMETROS QUE ESPERA
                ViewData["Error"] = e.Message;
                return RedirectToAction(nameof(Index));

            }
        }
    }
}