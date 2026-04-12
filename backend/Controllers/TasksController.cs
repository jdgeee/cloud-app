using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CloudBackend.Data;
using CloudBackend.Models;
using CloudBackend.DTOs;

namespace CloudBackend.Controllers;

[ApiController]
[Route("api/[controller]")] // Adres: http://localhost:8081/api/tasks
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;

    // Wstrzykiwanie zależności (Dependency Injection) kontekstu bazy danych
    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Pobiera listę wszystkich zadań z bazy Azure SQL.
    /// Metoda wykorzystuje mechanizm ponawiania (Retry Logic) w przypadku uśpienia bazy.
    /// </summary>
    /// <returns>Lista zadań w formacie TaskReadDto.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskReadDto>>> GetAll()
    {
    // Pobieramy encje z bazy danych
    var tasks = await _context.Tasks.ToListAsync();
    // Mapujemy każdą encję na obiekt DTO
    var tasksDto = tasks.Select(t => new TaskReadDto
    {
        Id = t.Id,
        Name = t.Name,
        IsCompleted = t.IsCompleted
    });
    return Ok(tasksDto);
    }

    /// <summary>
    /// Pobiera pojedyncze zadanie na podstawie jego identyfikatora.
    /// </summary>
    /// <param name="id">Unikalny identyfikator zadania.</param>
    /// <returns>Zadanie w formacie TaskReadDto lub 404 Not Found.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskReadDto>> GetById(int id)
    {
    var task = await _context.Tasks.FindAsync(id);
    if (task == null) return NotFound();  // Zwracamy DTO zamiast czystej encji
    return Ok(new TaskReadDto 
    { 
        Id = task.Id, 
        Name = task.Name, 
        IsCompleted = task.IsCompleted 
    });
    }
    

    /// <summary>
    /// Tworzy nowe zadanie i zapisuje je w bazie danych Azure SQL.
    /// Przyjmuje TaskCreateDto (kontrakt wejściowy) i zwraca TaskReadDto z nadanym przez bazę Id.
    /// </summary>
    /// <param name="taskDto">Dane nowego zadania (nazwa).</param>
    /// <returns>Utworzone zadanie wraz z adresem URL do jego zasobu (201 Created).</returns>
    [HttpPost]
    public async Task<ActionResult<TaskReadDto>> Create(TaskCreateDto taskDto)
    {
    // 1. Mapowanie DTO -> Entity
    // Przekształcamy to, co przyszło z sieci, na model bazy danych
    var newTask = new CloudTask
    {
        Name = taskDto.Name,
        IsCompleted = false // Domyślnie nowe zadanie nie jest gotowe
    };

    // 2. Zapis do bazy danych
    _context.Tasks.Add(newTask);
    await _context.SaveChangesAsync();

    // 3. Mapowanie Entity -> DTO (Zwrotka)
    // Zwracamy TaskReadDto, który zawiera już nadane przez bazę Id
    var readDto = new TaskReadDto
    {
        Id = newTask.Id,
        Name = newTask.Name,
        IsCompleted = newTask.IsCompleted
    };

    return CreatedAtAction(nameof(GetById), new { id = readDto.Id }, readDto);
    }
 


    /// <summary>
    /// Aktualizuje istniejące zadanie (np. oznacza jako ukończone).
    /// </summary>
    /// <param name="id">Identyfikator zadania do zaktualizowania.</param>
    /// <param name="task">Zaktualizowany obiekt zadania.</param>
    /// <returns>204 No Content w przypadku sukcesu lub 400 Bad Request przy niezgodności ID.</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, CloudTask task)
    {
        if (id != task.Id) return BadRequest("ID mismatch");
        _context.Entry(task).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent(); // Status 204 - operacja udana, brak danych do odesłania
    }

    /// <summary>
    /// Usuwa zadanie o podanym identyfikatorze z bazy danych.
    /// </summary>
    /// <param name="id">Identyfikator zadania do usunięcia.</param>
    /// <returns>204 No Content w przypadku sukcesu lub 404 Not Found gdy zadanie nie istnieje.</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}