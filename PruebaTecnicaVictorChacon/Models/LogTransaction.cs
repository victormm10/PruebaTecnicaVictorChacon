using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class LogTransaction
    {
        public string Id { get; set; }

        public TransactionType IdTipoTransaccion { get; set; }

        public string UserId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Descripcion { get {
                //var fecha = FechaRegistro.ToString("dd/MM/yyyy HH:mm:ss");
                return $"{Evento} - {UserId}";
            } }

        public string Evento { get
            {
                var tmp = string.Empty;
                var opt = ((int)IdTipoTransaccion);
                switch (opt) {
                    case 1:
                        tmp = "Consultar lista de usuarios";
                        break;
                    case 2:
                        tmp = "Consulta de publicacion de usuario";
                        break;
                    case 3:
                        tmp = "Consultar album de usuario";
                        break;
                    case 4:
                        tmp = "Consultar lista de fotos de usuario";
                        break;
                    default:
                        tmp = string.Empty;
                        break;
                }
                return tmp;
            } }
    }

    public enum TransactionType
    {
        UserList = 1,
        Publicaciones = 2,
        Albums = 3,
        Fotos = 4
    }
}
