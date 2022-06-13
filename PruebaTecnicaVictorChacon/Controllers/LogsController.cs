using API.Models;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private IHostingEnvironment _hostingEnvironment;
        private static HttpClient client = new HttpClient();
        private LogsDBContext dbLogs;

        public LogsController(IHostingEnvironment env)
        {
            _hostingEnvironment = env;
            dbLogs = new LogsDBContext(_hostingEnvironment);

            if (client.BaseAddress == null)
            {
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }



        // Obtener la lista de usuarios
        private async Task<List<Usuarios>> GetUserListAsync()
        {
            var lista = new List<Usuarios>();
            HttpResponseMessage response = await client.GetAsync("users");
            if (response.IsSuccessStatusCode)
            {
                lista = JsonConvert.DeserializeObject<List<Usuarios>>(await response.Content.ReadAsStringAsync());
            }
            return lista;
        }

        private async Task<Usuarios> GetUserInfoAsync(int id)
        {
            var usuario = new Usuarios();
            HttpResponseMessage response = await client.GetAsync($"users/{id}");
            if (response.IsSuccessStatusCode)
            {
                usuario = JsonConvert.DeserializeObject<Usuarios>(await response.Content.ReadAsStringAsync());
            }
            return usuario;
        }

        [HttpGet]
        [Route("GetUserList")]
        public async Task<List<Usuarios>> GetUserList()
        {
            var lista = await GetUserListAsync();

            //agregar el log de la transaccion
            dbLogs.TransactionLogs.Add(new TransactionLogs { 
                Id = Guid.NewGuid().ToString(),
                UserId = "Lista general",
                Tipo = 1,
                FechaRegistro = DateTime.Now
            });
            dbLogs.SaveChanges();

            return lista;
        }

        [HttpGet]
        [Route("GetUserInfo/{id}")]
        public async Task<Usuarios> GetUserInfo(int id)
        {
            var usuario = await GetUserInfoAsync(id);

            return usuario;
        }

        //lista de publicaciones
        private async Task<List<Publicacion>> GetUserPostsAsync(int id)
        {
            var lista = new List<Publicacion>();
            HttpResponseMessage response = await client.GetAsync($"posts?userId={id}");
            if (response.IsSuccessStatusCode)
            {
                lista = JsonConvert.DeserializeObject<List<Publicacion>>(await response.Content.ReadAsStringAsync());
            }
            return lista;
        }

        [HttpGet]
        [Route("GetUserPosts/{id}")]
        public async Task<List<Publicacion>> GetUserPosts(int id)
        {
            var lista = await GetUserPostsAsync(id);
            var usuario = await GetUserInfoAsync(id);

            //agregar el log de la transaccion
            dbLogs.TransactionLogs.Add(new TransactionLogs
            {
                Id = Guid.NewGuid().ToString(),
                UserId = $"Usuario : {usuario.name}",
                Tipo = 2,
                FechaRegistro = DateTime.Now
            });
            dbLogs.SaveChanges();

            return lista;
        }

        //obtener las fotos de usuarios

        private async Task<List<Album>> GetUserAlbumsAsync(int id)
        {
            var lista = new List<Album>();
            HttpResponseMessage response = await client.GetAsync($"albums?userId={id}");
            if (response.IsSuccessStatusCode)
            {
                lista = JsonConvert.DeserializeObject<List<Album>>(await response.Content.ReadAsStringAsync());
            }
            return lista;
        }

        [HttpGet]
        [Route("GetUserAlbums/{id}")]
        public async Task<List<Album>> GetUserAlbums(int id)
        {
            var lista = await GetUserAlbumsAsync(id);
            var usuario = await GetUserInfoAsync(id);

            //agregar el log de la transaccion
            dbLogs.TransactionLogs.Add(new TransactionLogs
            {
                Id = Guid.NewGuid().ToString(),
                UserId = $"Usuario : {usuario.name}",
                Tipo = 3,
                FechaRegistro = DateTime.Now
            });
            dbLogs.SaveChanges();

            return lista;
        }

        private async Task<List<Foto>> GetUserPhotoAsync(int id)
        {
            var lista = new List<Foto>();
            HttpResponseMessage response = await client.GetAsync($"photos?albumId={id}");
            if (response.IsSuccessStatusCode)
            {
                lista = JsonConvert.DeserializeObject<List<Foto>>(await response.Content.ReadAsStringAsync());
            }
            return lista;
        }

        [HttpGet]
        [Route("GetUserPhoto/{id}")]
        public async Task<List<Foto>> GetUserPhoto(int id)
        {
            var lista = await GetUserPhotoAsync(id);

            //agregar el log de la transaccion
            dbLogs.TransactionLogs.Add(new TransactionLogs
            {
                Id = Guid.NewGuid().ToString(),
                UserId = $"IdAlbum : {id}",
                Tipo = 4,
                FechaRegistro = DateTime.Now
            });
            dbLogs.SaveChanges();

            return lista;
        }

        //logs de transacciones
        [HttpGet]
        [Route("ClearTransactionLogs")]
        public string ClearTransactionLogs()
        {
            var lista = dbLogs.TransactionLogs.ToList();
            dbLogs.TransactionLogs.RemoveRange(lista);
            dbLogs.SaveChanges();

            return dbLogs.TransactionLogs.Any() ? "No se pudo limpiar el Log de Transaccines..." : "Log de Transacciones Eliminado!";
        }

        [HttpGet]
        [Route("getTransactionLogs")]
        public List<LogTransaction> getTransactionLogs()
        {
            var lista = dbLogs.TransactionLogs.OrderByDescending(o => o.FechaRegistro).Select(a=> new LogTransaction { 
                UserId = a.UserId,
                Id = a.Id,
                IdTipoTransaccion = (TransactionType)a.Tipo,
                FechaRegistro = a.FechaRegistro
            }).ToList();
            return lista;
        }

    }
}
