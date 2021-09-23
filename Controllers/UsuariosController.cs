using InmobiliariaAlbornoz.Data;
using InmobiliariaAlbornoz.Models;
using InmobiliariaAlbornoz.ModelsAux;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InmobiliariaAlbornoz.Controllers
{
    public class UsuariosController : Controller
    {
        RepoUsuario repo;
        IConfiguration configuration;
        IWebHostEnvironment environment;

        public UsuariosController(IConfiguration config, IWebHostEnvironment environment)
        {
            this.repo = new RepoUsuario(config);
            this.configuration = config;
            this.environment = environment;

        }

        // GET: Usuarios
        [Authorize(Policy = "Administrador")]
        public ActionResult Index()
        {
            var usuarios = repo.ObtenerTodos();
            return View(usuarios);
        }

        // GET: Usuarios/Details/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Details(int id)
        {
            try
            {
                var u = repo.ObtenerPorId(id);
                if (u != null)
                {
                    return View(u);
                }
                else
                {
                    TempData["msg"] = "No se encontró el usuario";
                    return RedirectToAction(nameof(Index), new { id = id });
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        // GET: Usuarios/Create
        [Authorize(Policy = "Administrador")]
        public ActionResult Create()
        {
            try
            {
                ViewBag.Roles = Usuario.ObtenerRoles();
                return View();
            }
            catch (Exception e)
            {
                TempData["msg"] = "Ocurrió un error al procesar formulario. Intente nuevamente.";
                return RedirectToAction(nameof(Index));
            }

        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Create(Usuario u)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: u.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                u.Clave = hashed;
                //u.Rol = User.IsInRole("Administrador") ? u.Rol : (int)enRoles.Empleado;
                var nbreRnd = Guid.NewGuid();//posible nombre aleatorio
                int res = repo.Alta(u);
                if (u.AvatarFile != null && res > 0)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
                    string fileName = "avatar_" + u.Id + Path.GetExtension(u.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    u.Avatar = Path.Combine("Uploads", fileName);
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        u.AvatarFile.CopyTo(stream);
                    }
                    repo.Modificacion(u);
                }
                TempData["msg"] = "¡Usuario creado!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Roles = Usuario.ObtenerRoles();
                return View();
            }
        }

        // GET: Usuarios/Edit/5
        [Authorize]
        public ActionResult Perfil()
        {
            try
            {
                ViewData["Title"] = "Mi perfil";
                TempData["returnUrl"] = Request.Headers["referer"].FirstOrDefault();
                var u = repo.ObtenerPorEmail(User.Identity.Name);
                if (u != null)
                {
                    ViewBag.Roles = Usuario.ObtenerRoles();
                    return View("Edit", u);
                }
                else
                {
                    TempData["msg"] = "No se encontró usuario. Intente nuevamente.";
                    return RedirectToAction("Index", "Home");
                }

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        // GET: Usuarios/Edit/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Edit(int id)
        {
            try
            {
                ViewData["Title"] = "Editar usuario";
                TempData["returnUrl"] = Request.Headers["referer"].FirstOrDefault();
                var u = repo.ObtenerPorId(id);
                if (u != null)
                {
                    ViewBag.Roles = Usuario.ObtenerRoles();
                    return View(u);
                }
                else
                {
                    TempData["msg"] = "No se encontró el usuario. Intente nuevamente.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Usuario u)
        {
            var returnUrl = Request.Headers["referer"].FirstOrDefault();
            bool editAvatar = false;
            bool editRol = false;

            try
            {
                var sessionUser = repo.ObtenerPorEmail(User.Identity.Name);

                if (u.Id == 0) // significa que viene desde /Usuarios/Perfil y se quiere editar a sí mismo
                {
                    u.Id = sessionUser.Id; //Le asigno al binding el Id de la sesión
                }

                // Ahora busco el user que se intenta editar
                var userToEdit = repo.ObtenerPorId(u.Id);

                // Chequeo que el usuario exista (medio al pedo)
                if (userToEdit == null)
                {
                    TempData["msg"] = "No se pudo comprobar el usuario. Intente nuevamente.";
                    return RedirectToAction("Denied", "Home");
                }

                // Se chequea que si no es administrador, esté editando su perfil
                if (!User.IsInRole("Administrador"))
                {
                    // Si el NO empleado está intentando editar un user con un id
                    // distinto al propio, o si está intentando mandar un value para
                    // el atributo Rol, es pateado al page "Denied"
                    if (sessionUser.Id != u.Id || u.Rol > 0)
                        return RedirectToAction("Denied", "Home");
                }


                // TODO: Aplicar lógica para modificar usuario
                // Ojo que admin puede modificar Roles. Empleado NO.
                // Empleado solo modifica desde /Usuarios/Perfil

                if (u.Rol > 0)
                {
                    editRol = true; // Bandera para el repo
                }

                if (u.AvatarFile != null)
                {
                    string wwwPath = environment.WebRootPath; // ruta raíz del servidor
                    string pathUploads = Path.Combine(wwwPath, "Uploads");

                    if (!Directory.Exists(pathUploads))
                    {
                        Directory.CreateDirectory(pathUploads);
                    }

                    //Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir

                    string fileName = "avatar_" + u.Id + Path.GetExtension(u.AvatarFile.FileName);
                    string avatarFullPath = Path.Combine(pathUploads, fileName);

                    using (FileStream stream = new FileStream(avatarFullPath, FileMode.Create))
                    {
                        u.AvatarFile.CopyTo(stream);

                        // Si todo fue bien, revalidamos ruta en el usuario y marcamos bandera
                        u.Avatar = Path.Combine("Uploads", fileName);
                        editAvatar = true;
                    }
                }

                var res = repo.Update(u, editRol, editAvatar);

                if (res > 0)
                {
                    TempData["msg"] = "¡Usuario actualizado!";
                    return Redirect(returnUrl);
                }
                else
                {
                    TempData["msg"] = "No se actualizaron datos. Intente nuevamente.";
                    return Redirect(returnUrl);
                }


            }
            catch (Exception ex)
            {//colocar breakpoints en la siguiente línea por si algo falla
                throw ex;
            }
        }


        // GET: Usuarios/EditPass/{id}
        [Authorize]
        public ActionResult EditPass(int id)
        {
            try
            {
                var sessionUser = repo.ObtenerPorEmail(User.Identity.Name);

                // Si pasa lo siguiente, es admin o es un empleado
                // que quiere editar su propia contraseña
                if (!User.IsInRole("Administrador") && id != sessionUser.Id)
                {
                    return RedirectToAction("Denied", "Home");
                }

                var userToEdit = repo.ObtenerPorId(id);
                if (userToEdit != null)
                {
                    ViewBag.Usuario = userToEdit;
                    return View();
                }
                else
                {
                    TempData["msg"] = "No se encontró usuario. Intente Nuevamente";
                    return Redirect(Request.Headers["referer"].FirstOrDefault());
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        // POST: Usuarios/EditPass/{id}
        [HttpPost]
        [Authorize]
        public ActionResult EditPass(int id, UsuarioPassEdit p)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var sessionUser = repo.ObtenerPorEmail(User.Identity.Name);
                    var userToEdit = repo.ObtenerPorId(id);

                    if (userToEdit == null)
                    {
                        // El usuario a editar no existe, se termina la cosa
                        TempData["msg"] = "No se encontró el usuario. Intente Nuevamente.";
                        return Redirect(Request.Headers["referer"].FirstOrDefault());
                    }

                    if (!User.IsInRole("Administrador") && sessionUser.Id != id)
                    {
                        return RedirectToAction("Denied", "Home");
                    }

                    // Acá ya podemos dejar de revisar autorización y cambiar la contraseña
                    string oldPassHashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                           password: p.OldPass,
                           salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                           prf: KeyDerivationPrf.HMACSHA1,
                           iterationCount: 1000,
                           numBytesRequested: 256 / 8));

                    if (oldPassHashed != userToEdit.Clave)
                    {
                        TempData["msg"] = "La contraseña antigua no es válida";
                        return RedirectToAction(nameof(EditPass), new { id = id});
                    }

                    string newPassHashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                           password: p.NewPass,
                           salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                           prf: KeyDerivationPrf.HMACSHA1,
                           iterationCount: 1000,
                           numBytesRequested: 256 / 8));

                    var res = repo.UpdatePass(id, newPassHashed);

                    if (res > 0)
                    {
                        TempData["msg"] = "¡Contraseña actualizada!";
                        return Redirect(Request.Headers["referer"].FirstOrDefault());
                    }
                    else
                    {
                        TempData["msg"] = "No se pudo cambiar contraseña. Intente nuevamente.";
                        return RedirectToAction(nameof(EditPass), new { id = id});
                    }

                }
                else
                {
                    TempData["msg"] = "Los datos ingresados no son válidos. Intente nuevamente.";
                    return RedirectToAction(nameof(EditPass), new { id = id });
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        // GET: Usuarios/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                var u = repo.ObtenerPorId(id);
                if (u != null)
                {
                    return View(u);
                }
                else
                {
                    TempData["msg"] = "No se encontró el usuario";
                    return RedirectToAction(nameof(Delete), new { id = id });
                }
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        // POST: Usuarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var res = repo.Baja(id);
                if (res > 0)
                {
                    TempData["msg"] = "¡Usuario eliminado!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["msg"] = "No se eliminó al usuario. Intnte nuevamente.";
                    return RedirectToAction(nameof(Delete));
                }

            }
            catch(Exception e)
            {
                throw e;
            }
        }


        //[Authorize]
        //public IActionResult Avatar()
        //{
        //    var u = repo.ObtenerPorEmail(User.Identity.Name);
        //    string fileName = "avatar_" + u.Id + Path.GetExtension(u.Avatar);
        //    string wwwPath = environment.WebRootPath;
        //    string path = Path.Combine(wwwPath, "Uploads");
        //    string pathCompleto = Path.Combine(path, fileName);

        //    //leer el archivo
        //    byte[] fileBytes = System.IO.File.ReadAllBytes(pathCompleto);
        //    //devolverlo
        //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        //}


        // GET: Usuarios/Create
        //[Authorize]
        //public ActionResult Foto()
        //{
        //    try
        //    {
        //        var u = repo.ObtenerPorEmail(User.Identity.Name);
        //        var stream = System.IO.File.Open(
        //            Path.Combine(environment.WebRootPath, u.Avatar.Substring(1)),
        //            FileMode.Open,
        //            FileAccess.Read);
        //        var ext = Path.GetExtension(u.Avatar);
        //        return new FileStreamResult(stream, $"image/{ext.Substring(1)}");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        // GET: Usuarios/Create
        //[Authorize]
        //public ActionResult Datos()
        //{
        //    try
        //    {
        //        var u = repo.ObtenerPorEmail(User.Identity.Name);
        //        string buffer = "Nombre;Apellido;Email" + Environment.NewLine +
        //            $"{u.Nombre};{u.Apellido};{u.Email}";
        //        var stream = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(buffer));
        //        var res = new FileStreamResult(stream, "text/plain");
        //        res.FileDownloadName = "Datos.csv";
        //        return res;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //[AllowAnonymous]
        //// GET: Usuarios/Login/
        //public ActionResult LoginModal()
        //{
        //    return PartialView("_LoginModal", new LoginView());
        //}

        //[Authorize(Policy = "Administrador")]
        //public ActionResult TestLogin()
        //{
        //    return View();
        //}

        [AllowAnonymous]
        // GET: Usuarios/Login/
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            TempData["returnUrl"] = returnUrl;
            return View();
        }

        // POST: Usuarios/Login/
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginView login)
        {
            
            try
            {
                var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string) ? "/Home" : TempData["returnUrl"].ToString();
                if (ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: login.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));

                    var e = repo.ObtenerPorEmail(login.Usuario);

                    if (e == null || e.Clave != hashed)
                    {
                        ModelState.AddModelError("", "El email o la clave no son correctos");
                        TempData["returnUrl"] = returnUrl;
                        return View();
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, e.Email),
                        new Claim("FullName", e.Nombre + " " + e.Apellido),
                        new Claim(ClaimTypes.Role, e.RolNombre),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                    TempData.Remove("returnUrl");
                    //string retorno = HttpContext.Request.Headers["referer"].FirstOrDefault();
                    return Redirect(returnUrl);
                }
                ModelState.AddModelError("", "El email o la clave no son correctos");
                TempData["returnUrl"] = returnUrl;
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: /salir
        [Route("salir", Name = "logout")]
        public async Task<ActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(
    CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {

                throw e;
            }

        }
    }
}
