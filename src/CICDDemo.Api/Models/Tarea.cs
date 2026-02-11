namespace CICDDemo.Api.Models;

/// <summary>
/// Representa una tarea en el sistema
/// </summary>
public class Tarea
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Completada { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaCompletada { get; set; }
    public Prioridad Prioridad { get; set; }
}

public enum Prioridad
{
    Baja = 0,
    Media = 1,
    Alta = 2,
    Urgente = 3
}
