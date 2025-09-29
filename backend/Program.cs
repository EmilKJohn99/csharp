using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<HabitContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/habits", async (Habit habit, HabitContext db) =>
{
    db.Habits.Add(habit);
    await db.SaveChangesAsync();
    return Results.Created($"/habits/{habit.Id}", habit);
});

app.MapGet("/habits", async (HabitContext db) =>
    await db.Habits.Include(h => h.Logs).ToListAsync()
);

app.MapGet("/habits/{id}", async (int id, HabitContext db) =>
{
    var habit = await db.Habits.Include(h => h.Logs).FirstOrDefaultAsync(h => h.Id == id);
    return habit is not null ? Results.Ok(habit) : Results.NotFound();
});

app.MapPut("/habits/{id}", async (int id, Habit updatedHabit, HabitContext db) =>
{
    var habit = await db.Habits.FindAsync(id);
    if (habit is null) return Results.NotFound();

    habit.Name = updatedHabit.Name;
    habit.Description = updatedHabit.Description;
    habit.Frequency = updatedHabit.Frequency;
    habit.StartDate = updatedHabit.StartDate;
    habit.IsActive = updatedHabit.IsActive;

    await db.SaveChangesAsync();
    return Results.Ok(habit);
});

app.MapDelete("/habits/{id}", async (int id, HabitContext db) =>
{
    var habit = await db.Habits.FindAsync(id);
    if (habit is null) return Results.NotFound();

    db.Habits.Remove(habit);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapPost("/habitlogs", async (HabitLog log, HabitContext db) =>
{
    var habit = await db.Habits.FindAsync(log.HabitId);
    if (habit is null) return Results.NotFound("Habit not found");

    db.HabitLogs.Add(log);
    await db.SaveChangesAsync();
    return Results.Created($"/habitlogs/{log.Id}", log);
});

app.MapGet("/habitlogs/{habitId}", async (int habitId, HabitContext db) =>
{
    var logs = await db.HabitLogs
        .Where(l => l.HabitId == habitId)
        .ToListAsync();

    return logs.Any() ? Results.Ok(logs) : Results.NotFound();
});
app.MapGet("/habitlogs", async (HabitContext db) =>
    await db.HabitLogs.Include(l => l.Habit).ToListAsync()
);

app.MapDelete("/habitlogs/{id}", async (int id, HabitContext db) =>
{
    var log = await db.HabitLogs.FindAsync(id);
    if (log is null) return Results.NotFound();

    db.HabitLogs.Remove(log);
    await db.SaveChangesAsync();
    return Results.NoContent();
});




app.Run();


