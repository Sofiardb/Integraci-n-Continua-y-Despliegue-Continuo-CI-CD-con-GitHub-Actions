using CICDDemo.Api.Controllers;
using CICDDemo.Api.Models;
using CICDDemo.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CICDDemo.Api.Tests.Controllers;

/// <summary>
/// Tests unitarios para el controlador de tareas
/// </summary>
public class TareasControllerTests
{
    private readonly Mock<ITareaService> _mockTareaService;
    private readonly TareasController _controller;

    public TareasControllerTests()
    {
        _mockTareaService = new Mock<ITareaService>();
        _controller = new TareasController(_mockTareaService.Object);
    }

    #region ObtenerTodas Tests

    [Fact]
    public void ObtenerTodas_RetornaOkConListaDeTareas()
    {
        // Arrange
        var tareas = new List<Tarea>
        {
            new Tarea { Id = 1, Titulo = "Tarea 1" },
            new Tarea { Id = 2, Titulo = "Tarea 2" }
        };
        _mockTareaService.Setup(s => s.ObtenerTodas()).Returns(tareas);

        // Act
        var result = _controller.ObtenerTodas();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var tareasResult = Assert.IsAssignableFrom<IEnumerable<Tarea>>(okResult.Value);
        Assert.Equal(2, tareasResult.Count());
    }

    #endregion

    #region ObtenerPorId Tests

    [Fact]
    public void ObtenerPorId_TareaExistente_RetornaOk()
    {
        // Arrange
        var tarea = new Tarea { Id = 1, Titulo = "Tarea Test" };
        _mockTareaService.Setup(s => s.ObtenerPorId(1)).Returns(tarea);

        // Act
        var result = _controller.ObtenerPorId(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var tareaResult = Assert.IsType<Tarea>(okResult.Value);
        Assert.Equal(1, tareaResult.Id);
    }

    [Fact]
    public void ObtenerPorId_TareaNoExistente_RetornaNotFound()
    {
        // Arrange
        _mockTareaService.Setup(s => s.ObtenerPorId(99)).Returns((Tarea?)null);

        // Act
        var result = _controller.ObtenerPorId(99);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    #endregion

    #region Crear Tests

    [Fact]
    public void Crear_TareaValida_RetornaCreatedAtAction()
    {
        // Arrange
        var tarea = new Tarea { Titulo = "Nueva Tarea" };
        var tareaCreada = new Tarea { Id = 1, Titulo = "Nueva Tarea" };
        _mockTareaService.Setup(s => s.Crear(It.IsAny<Tarea>())).Returns(tareaCreada);

        // Act
        var result = _controller.Crear(tarea);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var tareaResult = Assert.IsType<Tarea>(createdResult.Value);
        Assert.Equal(1, tareaResult.Id);
    }

    [Fact]
    public void Crear_TareaSinTitulo_RetornaBadRequest()
    {
        // Arrange
        var tarea = new Tarea { Titulo = "" };

        // Act
        var result = _controller.Crear(tarea);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public void Crear_TareaConTituloNull_RetornaBadRequest()
    {
        // Arrange
        var tarea = new Tarea { Titulo = null! };

        // Act
        var result = _controller.Crear(tarea);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    #endregion

    #region Actualizar Tests

    [Fact]
    public void Actualizar_TareaExistente_RetornaOk()
    {
        // Arrange
        var tareaActualizada = new Tarea { Titulo = "Tarea Actualizada" };
        _mockTareaService.Setup(s => s.Actualizar(1, It.IsAny<Tarea>())).Returns(tareaActualizada);

        // Act
        var result = _controller.Actualizar(1, tareaActualizada);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var tareaResult = Assert.IsType<Tarea>(okResult.Value);
        Assert.Equal("Tarea Actualizada", tareaResult.Titulo);
    }

    [Fact]
    public void Actualizar_TareaNoExistente_RetornaNotFound()
    {
        // Arrange
        _mockTareaService.Setup(s => s.Actualizar(99, It.IsAny<Tarea>())).Returns((Tarea?)null);

        // Act
        var result = _controller.Actualizar(99, new Tarea { Titulo = "Test" });

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    #endregion

    #region Eliminar Tests

    [Fact]
    public void Eliminar_TareaExistente_RetornaNoContent()
    {
        // Arrange
        _mockTareaService.Setup(s => s.Eliminar(1)).Returns(true);

        // Act
        var result = _controller.Eliminar(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void Eliminar_TareaNoExistente_RetornaNotFound()
    {
        // Arrange
        _mockTareaService.Setup(s => s.Eliminar(99)).Returns(false);

        // Act
        var result = _controller.Eliminar(99);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    #endregion

    #region MarcarComoCompletada Tests

    [Fact]
    public void MarcarComoCompletada_TareaExistente_RetornaOk()
    {
        // Arrange
        var tareaCompletada = new Tarea { Id = 1, Titulo = "Tarea", Completada = true };
        _mockTareaService.Setup(s => s.MarcarComoCompletada(1)).Returns(tareaCompletada);

        // Act
        var result = _controller.MarcarComoCompletada(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var tareaResult = Assert.IsType<Tarea>(okResult.Value);
        Assert.True(tareaResult.Completada);
    }

    [Fact]
    public void MarcarComoCompletada_TareaNoExistente_RetornaNotFound()
    {
        // Arrange
        _mockTareaService.Setup(s => s.MarcarComoCompletada(99)).Returns((Tarea?)null);

        // Act
        var result = _controller.MarcarComoCompletada(99);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    #endregion

    #region Filtros Tests

    [Fact]
    public void ObtenerPorPrioridad_RetornaOkConTareas()
    {
        // Arrange
        var tareas = new List<Tarea> { new Tarea { Id = 1, Prioridad = Prioridad.Alta } };
        _mockTareaService.Setup(s => s.ObtenerPorPrioridad(Prioridad.Alta)).Returns(tareas);

        // Act
        var result = _controller.ObtenerPorPrioridad(Prioridad.Alta);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public void ObtenerPendientes_RetornaOkConTareas()
    {
        // Arrange
        var tareas = new List<Tarea> { new Tarea { Id = 1, Completada = false } };
        _mockTareaService.Setup(s => s.ObtenerPendientes()).Returns(tareas);

        // Act
        var result = _controller.ObtenerPendientes();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public void ObtenerCompletadas_RetornaOkConTareas()
    {
        // Arrange
        var tareas = new List<Tarea> { new Tarea { Id = 1, Completada = true } };
        _mockTareaService.Setup(s => s.ObtenerCompletadas()).Returns(tareas);

        // Act
        var result = _controller.ObtenerCompletadas();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
    }

    #endregion
}

