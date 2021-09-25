using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using InmobiliariaAlbornoz.Data;
using InmobiliariaAlbornoz.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace InmobiliariaAlbornoz.Controllers
{
    [Authorize]
    public class InmueblesController : Controller
    {
        private RepoInmueble repo;
        private RepoPropietario repoPropietario;
        private RepoContrato repoContrato;

        public InmueblesController(IConfiguration config)
        {
            this.repo = new RepoInmueble(config);
            this.repoPropietario = new RepoPropietario(config);
            this.repoContrato = new RepoContrato(config);
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
                if (i.Id > 0)
                {
                    return View(i);
                }
                else
                {
                    TempData["msg"] = "No se encontró Inmueble. Intente nuevamente.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                TempData["msg"] = "Ocurrió un error. Intente nuevamente.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Inmueble/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.Propietarios = repoPropietario.All();
                ViewBag.Tipos = Inmueble.ObtenerTipos();
                ViewBag.Usos = Inmueble.ObtenerUsos();
                return View();
            }
            catch (Exception)
            {
                TempData["msg"] = "Ocurrió un error. Intente nuevamente.";
                return RedirectToAction(nameof(Index));
            }

        }

        // POST: Inmueble/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble i)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = repo.Put(i);
                    if (res > 0)
                    {
                        TempData["msg"] = "Inmueble cargado.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["msg"] = "No se cargó el inmueble. Intente nuevamente.";
                        ViewBag.Tipos = Inmueble.ObtenerTipos();
                        ViewBag.Usos = Inmueble.ObtenerUsos();
                        return View();
                    }
                }
                else
                {
                    TempData["msg"] = "Los datos ingresados no son válidos. Intente nuevamente.";
                    ViewBag.Tipos = Inmueble.ObtenerTipos();
                    ViewBag.Usos = Inmueble.ObtenerUsos();
                    return View();
                }
            }
            catch (Exception)
            {
                TempData["msg"] = "Ocurrió un error. Intente nuevamente";
                ViewBag.Tipos = Inmueble.ObtenerTipos();
                ViewBag.Usos = Inmueble.ObtenerUsos();
                return View();
            }
        }

        // GET: Inmueble/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var propietarios = repoPropietario.All();
                if (propietarios.Count == 0)
                {
                    TempData["msg"] = "No se pudo obtener la lista de propietarios. Intente nuevamente.";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Propietarios = propietarios;
                
                var i = repo.ById(id);
                if (i.Id > 0)
                {
                    var isTaken = repo.isTaken(i.Id);
                    if (isTaken)
                    {
                        ViewBag.IsTaken = true;
                    }
                    else
                    {
                        ViewBag.IsTaken = false;
                    }

                    ViewBag.Tipos = Inmueble.ObtenerTipos();
                    ViewBag.Usos = Inmueble.ObtenerUsos();
                    return View(i);
                }
                else
                {
                    TempData["msg"] = "No se encontró Inmueble. Intente nuevamente.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                TempData["msg"] = "Ocurrió un error. Intente nuevamente.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Inmueble/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inmueble i)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    i.Id = id;

                    var isTaken = repo.isTaken(id);

                    var res = repo.Edit(i, isTaken);
                    if (res > 0)
                    {
                        TempData["msg"] = "Cambios guardados.";
                        return RedirectToAction(nameof(Edit), new { id = id });
                    }
                    else
                    {
                        TempData["msg"] = "No se guardaron los cambios. Intente nuevamente.";
                        return RedirectToAction(nameof(Edit), new { id = id });
                    }
                }
                else
                {
                    TempData["msg"] = "Los datos ingresados no son válidos. Intente nuevamente.";
                    return RedirectToAction(nameof(Edit), new { id = id });
                }

            }
            catch(Exception)
            {
                TempData["msg"] = "Ocurrió un error. Intente nuevamente.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Inmueble/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var i = repo.ById(id);
                if (i.Id == 0)
                {
                    TempData["msg"] = "No se encontró el Inmueble.";
                    return RedirectToAction(nameof(Index));
                }

                return View(i);
            }
            catch (Exception e)
            {

                throw e;
            }
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
                    TempData["msg"] = "Registro de Inmueble borrado.";
                    return RedirectToAction(nameof(Index));
                }
                else if(res == 0)
                {
                    TempData["msg"] = "No se borró registro de Inmueble. Intente nuevamente";
                    return RedirectToAction(nameof(Delete), new { id = id });
                }
                else
                {
                    TempData["msg"] = "No se puede borrar un registro de Inmueble que tiene contratos registrados.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public ActionResult Availables()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetAvailablesByDates(IFormCollection form)
        {

            IList <Inmueble> inmuebles;
            
            try
            {
                DateTime desde = DateTime.Parse(form["desde"].ToString());
                DateTime hasta = DateTime.Parse(form["hasta"].ToString());
                inmuebles = repo.InmueblesAvailableByDates(desde, hasta);
                return Ok(inmuebles);
            }
            catch (Exception)
            {
                TempData["msg"] = "Ocurió un error. Intente nuevamente";
                return View("Availables");
            }

        }

        public ActionResult Contratos(int id)
        {
            try
            {
                var i = repo.ById(id);
                if (i.Id == 0)
                {
                    TempData["msg"] = "No se encontró el Inmueble.";
                    return RedirectToAction(nameof(Index));
                }

                IList<Contrato> contratos = repoContrato.AllByInmueble(i.Id);
                ViewBag.Inmueble = i;
                return View(contratos);
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}