using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nancy.Json;
using SitioPersona.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SitioPersona.Controllers
{
    public class PersonaUsuarioController : Controller
    {
        private readonly ILogger<PersonaUsuarioController> _logger;
        private readonly IConfiguration _Configure;
        private readonly string apiBaseUrl;

        public PersonaUsuarioController(ILogger<PersonaUsuarioController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _Configure = configuration;

            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");
        }

        public IActionResult IndexPersona()
        {
            IEnumerable<Persona> personas = null;
            IEnumerable<Usuario> usuarios = null;
            IEnumerable<PersonaUsuario> personaUsuarios = null;
            IList<PersonaUsuario> listapersonaUsuarios = new List<PersonaUsuario>();
            PersonaUsuario personaUsuario;

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(apiBaseUrl);
                    //HTTP GET
                    var responseTask = client.GetAsync("Registro");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<IList<Persona>>();
                        readTask.Wait();

                        personas = readTask.Result;

                        var responseUsuario = client.GetAsync("Usuario");
                        responseUsuario.Wait();
                        var resultU = responseUsuario.Result;
                        if (resultU.IsSuccessStatusCode)
                        {
                            var readRespuestaUsuarios = resultU.Content.ReadAsAsync<IList<Usuario>>();
                            readRespuestaUsuarios.Wait();

                            usuarios = readRespuestaUsuarios.Result;

                            foreach (Persona persona in personas)
                            {
                                foreach (Usuario usuario in usuarios)
                                {
                                    if (persona.Identificador == usuario.FkPersona)
                                    {
                                        personaUsuario = new PersonaUsuario();
                                        personaUsuario.Identificador = persona.Identificador;
                                        personaUsuario.Nombres = persona.Nombres;
                                        personaUsuario.Apellidos = persona.Apellidos;
                                        personaUsuario.NumeroIdentificacion = persona.NumeroIdentificacion;
                                        personaUsuario.Email = persona.Email;
                                        personaUsuario.TipoIdentificacion = persona.TipoIdentificacion;
                                        personaUsuario.FechaCreacion = persona.FechaCreacion;

                                        personaUsuario.IdUsuario = usuario.IdUsuario;
                                        personaUsuario.Usuario1 = usuario.Usuario1;
                                        personaUsuario.Pass = usuario.Pass;
                                        personaUsuario.FechaCreacionUsuario = usuario.FechaCreacion;
                                        listapersonaUsuarios.Add(personaUsuario);
                                    }

                                }

                            }

                            personaUsuarios = listapersonaUsuarios;
                        }

                    }
                    else //web api sent error response 
                    {
                        //log response status here..

                        personaUsuarios = Enumerable.Empty<PersonaUsuario>();

                        ModelState.AddModelError(string.Empty, "Se presentaron errores al cargar la lista de Personas");
                    }
                }
                catch (Exception)
                {

                    personaUsuarios = Enumerable.Empty<PersonaUsuario>();

                    ModelState.AddModelError(string.Empty, "No Hubo conexión con el servidor remoto, contacte al administrador");
                }
                
            }
            return View(personaUsuarios);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult create(PersonaUsuario personaUsuario)
        {
            if (!string.IsNullOrEmpty(personaUsuario.Nombres))
            {
                using (var client = new HttpClient())
                {
                    try
                    {
                        client.BaseAddress = new Uri(apiBaseUrl);

                        Persona persona = new Persona();
                        persona.Nombres = personaUsuario.Nombres;
                        persona.Apellidos = personaUsuario.Apellidos;
                        persona.Email = personaUsuario.Email;
                        persona.NumeroIdentificacion = personaUsuario.NumeroIdentificacion;
                        persona.TipoIdentificacion = personaUsuario.TipoIdentificacion;
                        persona.FechaCreacion = DateTime.Now;

                        //HTTP POST
                        var postTask = client.PostAsJsonAsync<Persona>("Registro", persona);
                        postTask.Wait();

                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var resultadoPersona = result.Content.ReadAsAsync<Persona>();
                            Persona resultadop = resultadoPersona.Result;
                            int id = resultadop.Identificador;

                            Usuario usuario = new Usuario();
                            usuario.Usuario1 = personaUsuario.Usuario1;
                            usuario.Pass = personaUsuario.Pass;
                            usuario.FechaCreacion = DateTime.Now;
                            usuario.FkPersona = id;

                            var postUsuario = client.PostAsJsonAsync<Usuario>("Usuario", usuario);
                            postUsuario.Wait();

                            var resultadoPostUsuario = postUsuario.Result;

                            if (resultadoPostUsuario.IsSuccessStatusCode)
                            {
                                return RedirectToAction("Index");
                            }
                        }
                    }
                    catch (Exception)
                    {

                        ModelState.AddModelError(string.Empty, "No hubo comunicación con el servidor remoto, comuniquese con el administrador");

                        return View("Create", personaUsuario);
                    }
                    
                }
            }
            
            ModelState.AddModelError(string.Empty, "Se presentaron errores al registrar");

            return View("Create", personaUsuario);
        }

       

    }
}
