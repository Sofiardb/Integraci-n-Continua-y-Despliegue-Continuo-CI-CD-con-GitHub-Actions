using CICDDemo.Api.Models;

namespace CICDDemo.Api.Services;

/// <summary>
/// Implementación del servicio de tareas (usando almacenamiento en memoria)
/// </summary>
public class TareaService : ITareaService
{
    private static readonly List<Tarea> _tareas = new();
    private static int _nextId = 1;

    public IEnumerable<Tarea> ObtenerTodas()
    {
        return _tareas.ToList();
    }

    public Tarea? ObtenerPorId(int id)
    {
        return _tareas.FirstOrDefault(t => t.Id == id);
    }

    public Tarea Crear(Tarea tarea)
    {
        tarea.Id = _nextId++;
        tarea.FechaCreacion = DateTime.UtcNow;
        tarea.Completada = false;
        _tareas.Add(tarea);
        return tarea;
    }

    public Tarea? Actualizar(int id, Tarea tareaActualizada)
    {
        var tarea = _tareas.FirstOrDefault(t => t.Id == id);
        if (tarea == null) return null;

        tarea.Titulo = tareaActualizada.Titulo;
        tarea.Descripcion = tareaActualizada.Descripcion;
        tarea.Prioridad = tareaActualizada.Prioridad;
        
        return tarea;
    }

    public bool Eliminar(int id)
    {
        var tarea = _tareas.FirstOrDefault(t => t.Id == id);
        if (tarea == null) return false;
        
        return _tareas.Remove(tarea);
    }

    public Tarea? MarcarComoCompletada(int id)
    {
        var tarea = _tareas.FirstOrDefault(t => t.Id == id);
        if (tarea == null) return null;

        tarea.Completada = true;
        tarea.FechaCompletada = DateTime.UtcNow;
        return tarea;
    }

    public IEnumerable<Tarea> ObtenerPorPrioridad(Prioridad prioridad)
    {
        return _tareas.Where(t => t.Prioridad == prioridad).ToList();
    }

    public IEnumerable<Tarea> ObtenerPendientes()
    {
        return _tareas.Where(t => !t.Completada).ToList();
    }

    public IEnumerable<Tarea> ObtenerCompletadas()
    {
        return _tareas.Where(t => t.Completada).ToList();
    }
}
