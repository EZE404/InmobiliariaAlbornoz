using InmobiliariaAlbornoz.Data;
using InmobiliariaAlbornoz.Models;
using Microsoft.AspNetCore.Authorization;
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
        private RepoContrato repoContrato;
        public ContratosController(IConfiguration config)
        {
            this.repo = new RepoContrato(config);
            this.repoInmueble = new RepoInmueble(config);
            this.repoInquilino = new RepoInquilino(config);
            this.repoContrato = new RepoContrato(config);
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
                if (c.Id > 0)
                {
                    return View(c);
                }
                else
                {
                    TempData[""] = "No se encontró Contrato. Intente nuevamente.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                TempData["msg"] = "Ocurrió un error. Intente nuevamente.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ContratosController/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.Inmuebles = repoInmueble.AllValid();
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
                if (ModelState.IsValid)
                {
                    var datesAreOk = repoContrato.CheckDates(c.IdInmueble, c.Desde, c.Hasta);
                    var inmuebleIsOk = repoInmueble.CheckAvailability(c.IdInmueble);
                    var inquilinoIsOk = repoInquilino.Details(c.IdInquilino).Id > 0 ? true : false;

                    if (!datesAreOk)
                    {
                        TempData["msg"] = "El rango de fechas seleccionadas no está disponible";
                        return RedirectToAction(nameof(Create));
                    }

                    if (inmuebleIsOk && inquilinoIsOk && datesAreOk)
                    {
                        var res = repo.Put(c);
                        if (res > 0)
                        {
                            TempData["msg"] = "Contrato cargado";
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            TempData["msg"] = "No se cargó el contrato. Intente nuevamente.";
                            return RedirectToAction(nameof(Create));
                        }
                    }
                    else
                    {
                        TempData["msg"] = "Datos no válidos. Intente nuevamente.";
                        return RedirectToAction(nameof(Create));
                    }
                }
                else
                {
                    TempData["msg"] = "Datos no válidos. Intente nuevamente.";
                    return RedirectToAction(nameof(Create));
                }
            }
            catch(Exception e)
            {
                TempData["msg"] = "Ocurrió un error. Intente nuevamente.";
                return RedirectToAction(nameof(Create));
            }
        }

        // GET: ContratosController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var c = repo.Details(id);
                if (c.Id <= 0)
                {
                    TempData["msg"] = "No se encontró Contrato. Intente nuevamente.";
                    return RedirectToAction(nameof(Index));
                }

                if (!c.Valido)
                {
                    TempData["msg"] = "No se puede editar un Contrato invalidado.";
                    return RedirectToAction(nameof(Index));
                }

                var inmuebles = repoInmueble.All();
                if (inmuebles.Count == 0)
                {
                    TempData["msg"] = "No se pudo obtener lista de Inmuebles. Intente nuevamente.";
                    return RedirectToAction(nameof(Index));
                }
                
                var inquilinos = repoInquilino.All();
                if (inquilinos.Count == 0)
                {
                    TempData["msg"] = "No se pudo obtener lista de Inquilinos. Intente nuevamente.";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Inmuebles = inmuebles;
                ViewBag.Inquilinos = inquilinos;
                return View(c);
            }
            catch (Exception)
            {
                ViewData["msg"] = "Ocurrió un error. Intente nuevamente.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ContratosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contrato c)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["msg"] = "Los datos ingresados no son válidos. Intente nuevamente.";
                    return RedirectToAction(nameof(Edit), new { id = id });
                }

                c.Id = id;

                var datesAreOk = repoContrato.CheckDates(c.IdInmueble, c.Desde, c.Hasta, c.Id);
                if (!datesAreOk)
                {
                    TempData["msg"] = "El rango entre fechas ingresadas no está disponible.";
                    return RedirectToAction(nameof(Edit), new { id = id });
                }

                var res = repo.Edit(c);
                if (res > 0)
                {
                    TempData["msg"] = "Cambios guardados.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["msg"] = "No se guardaron cambios. Intente nuevamente.";
                    return RedirectToAction(nameof(Edit), new { id = id });
                }
            }
            catch(Exception)
            {
                TempData["msg"] = "Ocurrió un error. Intente nuevamente.";
                return RedirectToAction(nameof(Edit), new { id = id });
            }
        }

        // GET: ContratosController/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                Contrato c = repo.Details(id);
                if (c.Id > 0)
                {
                    return View(c);
                }
                else
                {
                    TempData["msg"] = "No se encontró Contrato. Intente nuevamente";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                TempData["msg"] = "Ocurrió un error. Intente nuevamente";
                return RedirectToAction(nameof(Index));
            }

        }

        // POST: ContratosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var res = repo.Delete(id);
                if (res > 0)
                {
                    TempData["msg"] = "Contrato eliminado de la base de datos.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    Exception e = new Exception("No se eliminó Contrato. Intente nuevamente.");
                    return RedirectToAction(nameof(Delete), new { id = id });
                }
            }
            catch(Exception)
            {
                TempData["msg"] = "Ocurrió un error. Intente nuevamente.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
