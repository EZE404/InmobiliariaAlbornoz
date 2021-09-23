using InmobiliariaAlbornoz.Data;
using InmobiliariaAlbornoz.Models;
using InmobiliariaAlbornoz.ModelsAux;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlbornoz.Controllers
{
    public class PagosController : Controller
    {
        RepoPago repo;
        RepoContrato repoContrato;
        RepoInquilino repoInquilino;

        public PagosController(IConfiguration config)
        {
            repo = new RepoPago(config);
            repoContrato = new RepoContrato(config);
            repoInquilino = new RepoInquilino(config);
        }

        // GET: PagosController
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

        // GET: PagosController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var p = repo.Details(id);
                return View(p);

            }
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
                return View();
            }
        }

        // GET: PagosController/Create
        public ActionResult Create(int id)
        {
            try
            {
                if (id > 0) // Si es mayor a 0, viene desde Contrato (/pagos/create/idContrato)
                {
                    Contrato c = repoContrato.Details(id);
                    if (c.Id == 0)
                    {
                        TempData["msg"] = "No se encontró el Contrato. Intente nuevamente.";
                        return View();
                    }

                    if (!c.Valido)
                    {
                        TempData["msg"] = "No se puede cargar un pago a un contrato invalidado.";
                        return View();
                    }

                    ViewBag.Contrato = c;
                    return View();
                }

                return View(); // Sino, es que viene de "Crear nuevo" en Index de Pagos (/pagos/create)
            }
            catch (Exception)
            {
                TempData["msg"] = "Ocurrió un error. Intente nuevamente.";
                return View();
            }

        }

        // GET: PagosController/Inquilino/{dni}
        public ActionResult Inquilino(string dni)
        {
            PagoCreate pc = new PagoCreate();
            try
            {
                if (dni != null)
                {
                    Inquilino i = repoInquilino.Details(dni);
                    IList<Contrato> c = repoContrato.AllByInquilino(i.Id, true);

                    pc.Inquilino = i;
                    pc.Contratos = c;
                }

                return Ok(pc); // { "Inquilino": { "Nombre":"Pepito", ... }, "Contratos":[{"IdINmueble": 8}, {}, {}] }
            }
            catch (Exception)
            {
                TempData["msg"] = "Ocurió un error. Intente nuevamente";
                return RedirectToAction(nameof(Create));
            }

        }

        // POST: PagosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, Pago p)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    if (id > 0 && p.IdContrato == 0)
                    {
                        p.Id = 0;
                        p.IdContrato = id;
                    }

                    // TODO: Rescatar IdContrato y volver a chequear que realmente esté vigente
                    Contrato c = repoContrato.Details(p.IdContrato);

                    if (c.Valido)
                    {
                        // TODO: SI Contrato está vigente, Enviar p al repo.
                        var res = repo.Put(p);
                        if (res > 0)
                        {
                            TempData["msg"] = "Pago cargado";
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            TempData["msg"] = "Pago no cargado. Intente de nuevo.";
                            return RedirectToAction(nameof(Create));
                        }
                    }
                    else
                    {
                        TempData["msg"] = "El contrato indicado no está vigente o fue cancelado.";
                        return RedirectToAction(nameof(Create));
                    }
                }
                else
                {
                    TempData["msg"] = "Datos Inválidos. Intente nuevamente";
                    return RedirectToAction(nameof(Create));
                }

            }
            catch
            {
                TempData["msg"] = "Ocurrió un error. Intente nuevamente";
                return View();
            }
        }

        // GET: PagosController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                Pago p = repo.Details(id);
                if (p.Id == 0)
                {
                    TempData["msg"] = "No se encontró el Pago. Intente nuevamente";
                    return RedirectToAction(nameof(Index));
                }

                if (p.Anulado)
                {
                    TempData["msg"] = "No se puede editar un pago anulado.";
                    return RedirectToAction(nameof(Index));
                }

                return View(p);
            }
            catch (Exception)
            {
                TempData["msg"] = "Ocurrió un error. Intente nuevamente.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: PagosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Pago p)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var oldP = repo.Details(id);
                    if (oldP.Id == 0)
                    {
                        TempData["msg"] = "No se encontró el Pago. Intente nuevamente.";
                        return RedirectToAction(nameof(Index));
                    }

                    if (oldP.Anulado)
                    {
                        TempData["msg"] = "No se puede editar un pago anulado.";
                        return RedirectToAction(nameof(Index));
                    }

                    var res = repo.Edit(p);
                    if (res > 0)
                    {
                        TempData["msg"] = "Pago editado.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["msg"] = "Cambios no guardados. Intente Nuevamente";
                        return RedirectToAction(nameof(Edit), new { id = id });
                    }
                }
                else
                {
                    TempData["msg"] = "Datos de pago no válidos. Intente Nuevamente.";
                    return RedirectToAction(nameof(Edit), new { id = id});
                }
            }
            catch (Exception)
            {
                TempData["msg"] = "Ocurrió un error. Intente nuevamente.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: PagosController/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                Pago p = repo.Details(id);
                if (p.Id > 0)
                {
                    return View(p);
                }
                else
                {
                    TempData["msg"] = "El Pago " + id + " no existe en la base de datos.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {
                TempData["msg"] = "Ocurrió un error. Intente nuevamente.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: PagosController/Delete/5
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
                    TempData["msg"] = "Pago eliminado.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["msg"] = "Pago no encontrado. Vuelva a Intentar.";
                    return RedirectToAction(nameof(Delete), new { id = id });
                }
            }
            catch
            {
                TempData["msg"] = "Ocurrió un error. Vuelva a intentar.";
                return RedirectToAction(nameof(Delete), new { id = id });
            }
        }
    }
}
