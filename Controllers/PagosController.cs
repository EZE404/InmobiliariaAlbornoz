using InmobiliariaAlbornoz.Data;
using InmobiliariaAlbornoz.Models;
using InmobiliariaAlbornoz.ModelsAux;
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
            if (id > 0)
            {
                Contrato c = repoContrato.Details(id);

                if (c.Id > 0)
                {
                    ViewBag.Contrato = c;
                    return View();
                }
            }
            
            return View();
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
                    IList<Contrato> c = repoContrato.AllByInquilino(i.Id);

                    pc.Inquilino = i;
                    pc.Contratos = c;
                }

                return Ok(pc); // { "Inquilino": { "Nombre":"Pepito", ... }, "Contratos":[{"IdINmueble": 8}, {}, {}] }
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        // POST: PagosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PagosController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PagosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PagosController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PagosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
