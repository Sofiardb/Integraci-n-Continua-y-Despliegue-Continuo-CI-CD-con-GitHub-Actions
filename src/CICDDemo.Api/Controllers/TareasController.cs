using CICDDemo.Api.Models;
using CICDDemo.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CICDDemo.Api.Controllers;

/// <summary>
/// Controller para gestión de tareas
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TareasController : ControllerBase
{
    private readonly ITareaService _tareaService;

    public TareasController(ITareaService tareaService)
    {
        _tareaService = tareaService;
    }

    /// <summary>
    /// Obtiene todas las tareas
    /// </summary>
    [HttpGet]
    public ActionResult<IEnumerable<Tarea>> ObtenerTodas()
    {
        return Ok(_tareaService.ObtenerTodas());
    }

    /// <summary>
    /// Obtiene una tarea por su ID
    /// </summary>
    [HttpGet("{id}")]
    public ActionResult<Tarea> ObtenerPorId(int id)
    {
        var tarea = _tareaService.ObtenerPorId(id);
        if (tarea == null)
        {
            return NotFound(new { mensaje = $"No se encontró la tarea con ID {id}" });
        }
        return Ok(tarea);
    }

    /// <summary>
    /// Crea una nueva tarea
    /// </summary>
    [HttpPost]
    public ActionResult<Tarea> Crear([FromBody] Tarea tarea)
    {
        if (string.IsNullOrWhiteSpace(tarea.Titulo))
        {
            return BadRequest(new { mensaje = "El título es requerido" });
        }

        var nuevaTarea = _tareaService.Crear(tarea);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevaTarea.Id }, nuevaTarea);
    }

    /// <summary>
    /// Actualiza una tarea existente
    /// </summary>
    [HttpPut("{id}")]
    public ActionResult<Tarea> Actualizar(int id, [FromBody] Tarea tarea)
    {
        var tareaActualizada = _tareaService.Actualizar(id, tarea);
        if (tareaActualizada == null)
        {
            return NotFound(new { mensaje = $"No se encontró la tarea con ID {id}" });
        }
        return Ok(tareaActualizada);
    }

    /// <summary>
    /// Elimina una tarea
    /// </summary>
    [HttpDelete("{id}")]
    public ActionResult Eliminar(int id)
    {
        var eliminada = _tareaService.Eliminar(id);
        if (!eliminada)
        {
            return NotFound(new { mensaje = $"No se encontró la tarea con ID {id}" });
        }
        return NoContent();
    }

    /// <summary>
    /// Marca una tarea como completada
    /// </summary>
    [HttpPatch("{id}/completar")]
    public ActionResult<Tarea> MarcarComoCompletada(int id)
    {
        var tarea = _tareaService.MarcarComoCompletada(id);
        if (tarea == null)
        {
            return NotFound(new { mensaje = $"No se encontró la tarea con ID {id}" });
        }
        return Ok(tarea);
    }

    /// <summary>
    /// Obtiene tareas por prioridad
    /// </summary>
    [HttpGet("prioridad/{prioridad}")]
    public ActionResult<IEnumerable<Tarea>> ObtenerPorPrioridad(Prioridad prioridad)
    {
        return Ok(_tareaService.ObtenerPorPrioridad(prioridad));
    }

    /// <summary>
    /// Obtiene tareas pendientes
    /// </summary>
    [HttpGet("pendientes")]
    public ActionResult<IEnumerable<Tarea>> ObtenerPendientes()
    {
        return Ok(_tareaService.ObtenerPendientes());
    }

    /// <summary>
    /// Obtiene tareas completadas
    /// </summary>
    [HttpGet("completadas")]
    public ActionResult<IEnumerable<Tarea>> ObtenerCompletadas()
    {
        return Ok(_tareaService.ObtenerCompletadas());
    }
}
