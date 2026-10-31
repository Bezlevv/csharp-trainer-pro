using EfCoreDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo.Data;

/// <summary>
/// Контекст базы данных (представляет сессию с БД).
/// </summary>
public sealed class AppDbContext : DbContext
{
    /// <summary>Набор задач в БД.</summary>
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    /// <summary>Создаёт контекст с переданными настройками.</summary>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}
