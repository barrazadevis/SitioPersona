using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SitioPersona.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SitioPersona.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IConfiguration _Configure;
        private readonly string apiBaseUrl;

        public LoginController(ILogger<LoginController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _Configure = configuration;

            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");
        }
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult login(string usuario, string pass)
        {
            if (!string.IsNullOrEmpty(usuario))
            {
                using (var client = new HttpClient())
                {
                    try
                    {
                        client.BaseAddress = new Uri(apiBaseUrl);

                        Usuario objUsuario = new Usuario();
                        PersonaUsuario personaUsuario = new PersonaUsuario();

                        objUsuario.Usuario1 = usuario;
                        objUsuario.Pass = pass;

                        var postTask = client.PostAsJsonAsync<Usuario>("Login", objUsuario);
                        postTask.Wait();

                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var resultadoUsuario = result.Content.ReadAsAsync<Usuario>();
                            if (resultadoUsuario.Result != null)
                            {
                                Usuario resultadop = resultadoUsuario.Result;
                                if (resultadop.IdUsuario > 0)
                                {
                                    if (resultadop.FkPersona != null)
                                    {
                                        int id = (int)resultadop.FkPersona;
                                        var responseUsuario = client.GetAsync($"Registro/{id}");
                                        responseUsuario.Wait();
                                        var resultU = responseUsuario.Result;

                                        if (resultU.IsSuccessStatusCode)
                                        {
                                            var readPersona = resultU.Content.ReadAsAsync<Persona>();
                                            if (readPersona.Result != null)
                                            {
                                                Persona persona = readPersona.Result;
                                                personaUsuario.Identificador = persona.Identificador;
                                                personaUsuario.Nombres = persona.Nombres;
                                                personaUsuario.Apellidos = persona.Apellidos;
                                                personaUsuario.NumeroIdentificacion = persona.NumeroIdentificacion;
                                                personaUsuario.TipoIdentificacion = persona.TipoIdentificacion;
                                                personaUsuario.Email = persona.Email;
                                                personaUsuario.FechaCreacion = persona.FechaCreacion;
                                                personaUsuario.IdentificacionCompleta = persona.IdentificacionCompleta;
                                                personaUsuario.NombreCompleto = persona.NombreCompleto;

                                                personaUsuario.IdUsuario = resultadop.IdUsuario;
                                                personaUsuario.Usuario1 = resultadop.Usuario1;
                                                personaUsuario.FechaCreacionUsuario = resultadop.FechaCreacion;
                                                personaUsuario.FkPersona = resultadop.FkPersona;

                                                return View("Index", personaUsuario);
                                            }
                                            ModelState.AddModelError(string.Empty, "No se pudo confirmar el usuario");
                                            return View("Login");
                                        }
                                        ModelState.AddModelError(string.Empty, "No se pudo confirmar el usuario");
                                        return View("Login");
                                    }
                                    ModelState.AddModelError(string.Empty, "Se presentaron errores");
                                    return View("Login");
                                }
                                ModelState.AddModelError(string.Empty, "El usuario no se encuentra registrado");
                                return View("Login");
                            }
                            ModelState.AddModelError(string.Empty, "El usuario no se encuentra registrado");
                            return View("Login");

                        }
                    }
                    catch (Exception)
                    {

                        ModelState.AddModelError(string.Empty, "No hubo comunicación con el servidor remoto, contacte al administrador");

                        return View("Login", usuario);
                    }
                    
                }
            }

            ModelState.AddModelError(string.Empty, "Se presentaron errores al ingresar");

            return View("Login", usuario);
        }
    }
}

