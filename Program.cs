using NotesApi.Repositories;
using NotesApi.Services;
using NotesApi.Models;
using NotesApi.Dtos;
using Microsoft.EntityFrameworkCore;
using NotesApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<WeatherService>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<NoteService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddValidation();
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", (WeatherService service) =>
{
    return service.GetMessage();
});

//Notes endpoints start here
app.MapGet("/notes", async (NoteService noteService) =>
{
    var notes = await noteService.GetAll();

    var response = notes.Select(note => new NoteResponseDto
    {
        Id = note.Id,
        Title = note.Title,
        Content = note.Content
    });


    return Results.Ok(response);
});

app.MapPost("/notes", async (CreateNoteDto dto, NoteService noteService) =>
{
    var note = new Note
    {
        Title = dto.Title,
        Content = dto.Content,
        UserId = dto.UserId
    };

    var created = await noteService.Create(note);

    if (!created)
    {
        return Results.BadRequest(new
        {
            message = $"User with ID {dto.UserId} does not exist."
        });
    }

    var response = new NoteResponseDto
    {
        Id = note.Id,
        Title = note.Title,
        Content = note.Content,
        UserId = note.UserId
    };

    return Results.Created($"/notes/{note.Id}", response);
});

app.MapGet("/notes/{id}", async (int id, NoteService noteService) =>
{
    var note = await noteService.GetById(id);

    if (note is null)
    {
        return Results.NotFound();
    }

    var response = new NoteResponseDto
    {
        Id = note.Id,
        Title = note.Title,
        Content = note.Content
    };

    return Results.Ok(response);
});

app.MapPut("/notes/{id}", async (int id, UpdateNoteDto dto, NoteService noteService) =>
{
    var note = await noteService.Update(id, dto);
    if (note is null)
    {
        return Results.NotFound();
    }

    var response = new NoteResponseDto
    {
        Id = note.Id,
        Title = note.Title,
        Content = note.Content

    };

    return Results.Ok(response);
});

app.MapDelete("/notes/{id}", async (int id, NoteService noteService) =>
{

    var deleted = await noteService.Delete(id);

    if (!deleted)
    {
        return Results.NotFound();
    }

    return Results.NoContent(); //No content means "Your request succeeded, but there's nothing I need to send back."
});

//User Endpoints Start here

app.MapPost("/users", async (CreateUserDto dto, UserService userService) =>
{
    var user = new User
    {
        Name = dto.Name,
        Email = dto.Email
    };

    await userService.Create(user);

    var response = new UserResponseDto
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email
    };
    return Results.Created($"/users/{user.Id}", response);
});

app.MapGet("/users", async (UserService userService) =>
{
    var users = await userService.GetAll();

    var response = users.Select(user => new UserResponseDto
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email
    });

    return Results.Ok(response);
});

app.MapGet("/users/{id}", async (int id, UserService userService) =>
{
    var user = await userService.GetById(id);

    if (user is null)
    {
        return Results.NotFound();
    }

    var response = new UserResponseDto
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
        Notes = user.Notes.Select(note => new NoteResponseDto
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            UserId = note.UserId
        }).ToList()

    };

    return Results.Ok(response);

});

// Thsi endpoint creates a note that belongs to a specific user
app.MapPost("/users/{id}/notes", async (int id, CreateNoteDto dto, NoteService noteService) =>
{

    var note = new Note
    {
        Title = dto.Title,
        Content = dto.Content,
        UserId = id

    };


    var created = await noteService.Create(note);

    if (!created)
    {
        return Results.NotFound(new
        {
            message = $"User with ID {id} does not exist."
        });
    }

    var response = new NoteResponseDto
    {
        Title = note.Title,
        Content = note.Content,
        Id = note.Id,
        UserId = note.UserId
    };

    return Results.Created($"/users/{note.Id}", response);

});


//This Route retrieves all the notes that belong to a certain user
app.MapGet("/users/{id}/notes", async (int id, UserService userService) =>
{
    var user = await userService.GetById(id);

    if (user is null)
    {
        return Results.NotFound();
    }

    var response = user.Notes.Select(note => new NoteResponseDto
    {
        Id = note.Id,
        Title = note.Title,
        Content = note.Content,
        UserId = note.UserId

    });

    return Results.Ok(response);

});

app.MapPut("/users/{id}", async (int id, UpdateUserDto dto, UserService userService) =>
{
    var user = await userService.Update(id, dto);

    if (user is null)
    {
        return Results.NotFound();
    }

    var response = new UserResponseDto
    {
        Name = user.Name,
        Email = user.Email,
        Notes = user.Notes.Select(note => new NoteResponseDto
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            UserId = note.UserId
        }).ToList()

    };


    return Results.Ok(response);
});

app.MapDelete("/users/{id}", async (int id, UserService userService) =>
{
    var deleted = await userService.Delete(id);

    if (!deleted)
    {
        return Results.NotFound();
    }

    return Results.NoContent();
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
