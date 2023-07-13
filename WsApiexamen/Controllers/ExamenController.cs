using Microsoft.AspNetCore.Mvc;
using WsApiexamen.Models;

namespace WsApiexamen.Controllers
{


    public class ReturnMessage
    {
        public bool ReturnStatus { get; set; }
        public string? ReturnMsg { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class ExamenController : Controller
    {
        private readonly BdiExamenContext _context;
        public ExamenController(BdiExamenContext context) {
            _context = context;

        }

        [HttpPost("AgregarExamen")]

        public async Task<ActionResult<ReturnMessage>> AgregarExamen(int idExamen, string Nombre, string Descripcion)
        {
            ReturnMessage returnMessage = new ReturnMessage();
            Examen nuevoExamen = new Examen();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    nuevoExamen.IdExamen = idExamen;
                    nuevoExamen.Nombre = Nombre;
                    nuevoExamen.Descripcion = Descripcion;

                    _context.TblExamen.Add(nuevoExamen);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    returnMessage.ReturnStatus = true;
                    return returnMessage;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    returnMessage.ReturnStatus = false;
                    returnMessage.ReturnMsg = ex.Message;
                    return returnMessage;
                }
            }
        }

        [HttpPut("ActualizarExamen")]

        public async Task<ActionResult<ReturnMessage>> ActualizarExamen(int idExamen, string Nombre, string Descripcion)
        {
            ReturnMessage returnMessage = new ReturnMessage();
            Examen nuevoExamen = new Examen();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var examen = await _context.TblExamen.FindAsync(idExamen);
                    if (examen == null)
                    {
                        throw new Exception("Examen no encontrado.");
                    }

                    examen.Nombre = Nombre;
                    examen.Descripcion = Descripcion;

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    returnMessage.ReturnStatus = true;
                    return returnMessage;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    returnMessage.ReturnStatus = false;
                    returnMessage.ReturnMsg = ex.Message;
                    return returnMessage;
                }
            }
        }

        [HttpDelete("EliminarExamen")]

        public async Task<ActionResult<ReturnMessage>> EliminarExamen(int idExamen)
        {
            ReturnMessage returnMessage = new ReturnMessage();
            using (var transaction = _context.Database.BeginTransaction())
            {

                try
                {
                    var examen = await _context.TblExamen.FindAsync(idExamen);
                    if (examen == null)
                    {
                        throw new Exception("Examen no encontrado.");
                    }

                    _context.TblExamen.Remove(examen);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    returnMessage.ReturnStatus = true;
                    return returnMessage;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    returnMessage.ReturnStatus = false;
                    returnMessage.ReturnMsg = ex.Message;
                    return returnMessage;
                }
            }
        }

        [HttpGet("ConsultarExamen")]
        public async Task<ActionResult<List<Examen>>> ConsultarExamen(string Nombre, string Descripcion)
        {
                var examenes = await _context.TblExamen.ToListAsync();

                if (!string.IsNullOrEmpty(Nombre) && !string.IsNullOrEmpty(Descripcion))
                {
                    examenes = examenes.Where(e => e.Nombre == Nombre && e.Descripcion == Descripcion).ToList();
                }

                return Ok(examenes);
        }  

    }
}
