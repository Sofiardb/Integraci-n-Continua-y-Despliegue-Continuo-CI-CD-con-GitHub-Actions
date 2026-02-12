using CICDDemo.Api.Models;

namespace CICDDemo.Api.Services;

/// <summary>
/// Interface para el servicio de tareas
/// </summary>
public interface ITareaService
{
    IEnumerable<Tarea> ObtenerTodas();
    Tarea? ObtenerPorId(int id);
    Tarea Crear(Tarea tarea);
    Tarea? Actualizar(int id, Tarea tarea);
    bool Eliminar(int id);
    Tarea? MarcarComoCompletada(int id);
    IEnumerable<Tarea> ObtenerPorPrioridad(Prioridad prioridad);
    IEnumerable<Tarea> ObtenerPendientes();
    IEnumerable<Tarea> ObtenerCompletadas();
}
