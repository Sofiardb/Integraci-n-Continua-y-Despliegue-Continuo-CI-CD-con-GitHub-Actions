using CICDDemo.Api.Models;
using CICDDemo.Api.Services;
using Xunit;

namespace CICDDemo.Api.Tests.Services;

/// <summary>
/// Tests unitarios para el servicio de tareas
/// </summary>
public class TareaServiceTests
{
    private readonly TareaService _tareaService;

    public TareaServiceTests()
    {
        _tareaService = new TareaService();
    }

    #region Crear Tests

    [Fact]
    public void Crear_TareaValida_RetornaTareaConId()
    {
        // Arrange
        var tarea = new Tarea
        {
            Titulo = "Nueva Tarea",
            Descripcion = "Descripción de la tarea",
            Prioridad = Prioridad.Media
        };

        // Act
        var resultado = _tareaService.Crear(tarea);

        // Assert
        Assert.NotNull(resultado);
        Assert.True(resultado.Id > 0);
        Assert.Equal("Nueva Tarea", resultado.Titulo);
        Assert.False(resultado.Completada);
        Assert.NotEqual(default, resultado.FechaCreacion);
    }

    [Fact]
    public void Crear_MultiplesTareas_AsignaIdsUnicos()
    {
        // Arrange
        var tarea1 = new Tarea { Titulo = "Tarea 1" };
        var tarea2 = new Tarea { Titulo = "Tarea 2" };

        // Act
        var resultado1 = _tareaService.Crear(tarea1);
        var resultado2 = _tareaService.Crear(tarea2);

        // Assert
        Assert.NotEqual(resultado1.Id, resultado2.Id);
    }

    #endregion

    #region ObtenerPorId Tests

    [Fact]
    public void ObtenerPorId_TareaExistente_RetornaTarea()
    {
        // Arrange
        var tarea = new Tarea { Titulo = "Tarea Test" };
        var tareaCreada = _tareaService.Crear(tarea);

        // Act
        var resultado = _tareaService.ObtenerPorId(tareaCreada.Id);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(tareaCreada.Id, resultado.Id);
    }

    [Fact]
    public void ObtenerPorId_TareaNoExistente_RetornaNull()
    {
        // Act
        var resultado = _tareaService.ObtenerPorId(99999);

        // Assert
        Assert.Null(resultado);
    }

    #endregion

    #region Actualizar Tests

    [Fact]
    public void Actualizar_TareaExistente_ActualizaDatos()
    {
        // Arrange
        var tarea = new Tarea { Titulo = "Tarea Original", Prioridad = Prioridad.Baja };
        var tareaCreada = _tareaService.Crear(tarea);
        var tareaActualizada = new Tarea
        {
            Titulo = "Tarea Actualizada",
            Descripcion = "Nueva descripción",
            Prioridad = Prioridad.Alta
        };

        // Act
        var resultado = _tareaService.Actualizar(tareaCreada.Id, tareaActualizada);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("Tarea Actualizada", resultado.Titulo);
        Assert.Equal("Nueva descripción", resultado.Descripcion);
        Assert.Equal(Prioridad.Alta, resultado.Prioridad);
    }

    [Fact]
    public void Actualizar_TareaNoExistente_RetornaNull()
    {
        // Arrange
        var tareaActualizada = new Tarea { Titulo = "No existe" };

        // Act
        var resultado = _tareaService.Actualizar(99999, tareaActualizada);

        // Assert
        Assert.Null(resultado);
    }

    #endregion

    #region Eliminar Tests

    [Fact]
    public void Eliminar_TareaExistente_RetornaTrue()
    {
        // Arrange
        var tarea = new Tarea { Titulo = "Tarea a eliminar" };
        var tareaCreada = _tareaService.Crear(tarea);

        // Act
        var resultado = _tareaService.Eliminar(tareaCreada.Id);

        // Assert
        Assert.True(resultado);
        Assert.Null(_tareaService.ObtenerPorId(tareaCreada.Id));
    }

    [Fact]
    public void Eliminar_TareaNoExistente_RetornaFalse()
    {
        // Act
        var resultado = _tareaService.Eliminar(99999);

        // Assert
        Assert.False(resultado);
    }

    #endregion

    #region MarcarComoCompletada Tests

    [Fact]
    public void MarcarComoCompletada_TareaExistente_MarcaComoCompletada()
    {
        // Arrange
        var tarea = new Tarea { Titulo = "Tarea pendiente" };
        var tareaCreada = _tareaService.Crear(tarea);

        // Act
        var resultado = _tareaService.MarcarComoCompletada(tareaCreada.Id);

        // Assert
        Assert.NotNull(resultado);
        Assert.True(resultado.Completada);
        Assert.NotNull(resultado.FechaCompletada);
    }

    [Fact]
    public void MarcarComoCompletada_TareaNoExistente_RetornaNull()
    {
        // Act
        var resultado = _tareaService.MarcarComoCompletada(99999);

        // Assert
        Assert.Null(resultado);
    }

    #endregion

    #region Filtros Tests

    [Fact]
    public void ObtenerPorPrioridad_FiltraCorrectamente()
    {
        // Arrange
        _tareaService.Crear(new Tarea { Titulo = "Baja 1", Prioridad = Prioridad.Baja });
        _tareaService.Crear(new Tarea { Titulo = "Alta 1", Prioridad = Prioridad.Alta });
        _tareaService.Crear(new Tarea { Titulo = "Alta 2", Prioridad = Prioridad.Alta });

        // Act
        var tareasAltas = _tareaService.ObtenerPorPrioridad(Prioridad.Alta);

        // Assert
        Assert.All(tareasAltas, t => Assert.Equal(Prioridad.Alta, t.Prioridad));
    }

    [Fact]
    public void ObtenerPendientes_RetornaSoloTareasPendientes()
    {
        // Arrange
        var tarea1 = _tareaService.Crear(new Tarea { Titulo = "Pendiente" });
        var tarea2 = _tareaService.Crear(new Tarea { Titulo = "Completada" });
        _tareaService.MarcarComoCompletada(tarea2.Id);

        // Act
        var pendientes = _tareaService.ObtenerPendientes();

        // Assert
        Assert.All(pendientes, t => Assert.False(t.Completada));
    }

    [Fact]
    public void ObtenerCompletadas_RetornaSoloTareasCompletadas()
    {
        // Arrange
        var tarea1 = _tareaService.Crear(new Tarea { Titulo = "Pendiente" });
        var tarea2 = _tareaService.Crear(new Tarea { Titulo = "Completada" });
        _tareaService.MarcarComoCompletada(tarea2.Id);

        // Act
        var completadas = _tareaService.ObtenerCompletadas();

        // Assert
        Assert.All(completadas, t => Assert.True(t.Completada));
    }

    #endregion
}

