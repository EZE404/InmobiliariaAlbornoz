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
    [Authorize]
    public class PagosController : Controller
    {
        private RepoPago repo;
        private RepoContrato repoContrato;
        private RepoInquilino repoInquilino;
        private RepoDev repoDev;

        public PagosController(IConfiguration config)
        {
            this.repo = new RepoPago(config);
            this.repoContrato = new RepoContrato(config);
            this.repoInquilino = new RepoInquilino(config);
            this.repoDev = new RepoDev(config);
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
                try
                {
                    string user = "Anónimo";
                    if (!String.IsNullOrEmpty(User.Identity.Name))
                    {
                        user = User.Identity.Name;
                    }
                    var res = repoDev.SaveException("Pagos", "Index:GET", e.Message, user);
                    TempData["msg"] = "Ocurrió un error. Intente nuevamente. ID_ERROR: " + res;
                    return Redirect(Request.Headers["referer"].FirstOrDefault());
                }
                catch (Exception)
                {
                    throw;
                }
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
                try
                {
                    string user = "Anónimo";
                    if (!String.IsNullOrEmpty(User.Identity.Name))
                    {
                        user = User.Identity.Name;
                    }
                    var res = repoDev.SaveException("Pagos", "Details:GET", e.Message, user);
                    TempData["msg"] = "Ocurrió un error. Intente nuevamente. ID_ERROR: " + res;
                    return Redirect(Request.Headers["referer"].FirstOrDefault());
                }
                catch (Exception)
                {
                    throw;
                }
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
            catch (Exception e)
            {
                try
                {
                    string user = "Anónimo";
                    if (!String.IsNullOrEmpty(User.Identity.Name))
                    {
                        user = User.Identity.Name;
                    }
                    var res = repoDev.SaveException("Pagos", "Create:GET", e.Message, user);
                    TempData["msg"] = "Ocurrió un error. Intente nuevamente. ID_ERROR: " + res;
                    return Redirect(Request.Headers["referer"].FirstOrDefault());
                }
                catch (Exception)
                {
                    throw;
                }
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
            catch (Exception e)
            {
                try
                {
                    string user = "Anónimo";
                    if (!String.IsNullOrEmpty(User.Identity.Name))
                    {
                        user = User.Identity.Name;
                    }
                    var res = repoDev.SaveException("Pagos", "Inquilino:GET", e.Message, user);
                    TempData["msg"] = "Ocurrió un error. Intente nuevamente. ID_ERROR: " + res;
                    return Redirect(Request.Headers["referer"].FirstOrDefault());
                }
                catch (Exception)
                {
                    throw;
                }
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
            catch (Exception e)
            {
                try
                {
                    string user = "Anónimo";
                    if (!String.IsNullOrEmpty(User.Identity.Name))
                    {
                        user = User.Identity.Name;
                    }
                    var res = repoDev.SaveException("Pagos", "Create:POST", e.Message, user);
                    TempData["msg"] = "Ocurrió un error. Intente nuevamente. ID_ERROR: " + res;
                    return Redirect(Request.Headers["referer"].FirstOrDefault());
                }
                catch (Exception)
                {
                    throw;
                }
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
            catch (Exception e)
            {
                try
                {
                    string user = "Anónimo";
                    if (!String.IsNullOrEmpty(User.Identity.Name))
                    {
                        user = User.Identity.Name;
                    }
                    var res = repoDev.SaveException("Pagos", "Edit:GET", e.Message, user);
                    TempData["msg"] = "Ocurrió un error. Intente nuevamente. ID_ERROR: " + res;
                    return Redirect(Request.Headers["referer"].FirstOrDefault());
                }
                catch (Exception)
                {
                    throw;
                }
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
            catch (Exception e)
            {
                try
                {
                    string user = "Anónimo";
                    if (!String.IsNullOrEmpty(User.Identity.Name))
                    {
                        user = User.Identity.Name;
                    }
                    var res = repoDev.SaveException("Pagos", "Edit:POST", e.Message, user);
                    TempData["msg"] = "Ocurrió un error. Intente nuevamente. ID_ERROR: " + res;
                    return Redirect(Request.Headers["referer"].FirstOrDefault());
                }
                catch (Exception)
                {
                    throw;
                }
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
            catch (Exception e)
            {
                try
                {
                    string user = "Anónimo";
                    if (!String.IsNullOrEmpty(User.Identity.Name))
                    {
                        user = User.Identity.Name;
                    }
                    var res = repoDev.SaveException("Pagos", "Delete:GET", e.Message, user);
                    TempData["msg"] = "Ocurrió un error. Intente nuevamente. ID_ERROR: " + res;
                    return Redirect(Request.Headers["referer"].FirstOrDefault());
                }
                catch (Exception)
                {
                    throw;
                }
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
            catch (Exception e)
            {
                try
                {
                    string user = "Anónimo";
                    if (!String.IsNullOrEmpty(User.Identity.Name))
                    {
                        user = User.Identity.Name;
                    }
                    var res = repoDev.SaveException("Pagos", "Delete:POST", e.Message, user);
                    TempData["msg"] = "Ocurrió un error. Intente nuevamente. ID_ERROR: " + res;
                    return Redirect(Request.Headers["referer"].FirstOrDefault());
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
