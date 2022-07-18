global using Microsoft.EntityFrameworkCore;
global using MinimalApiProject;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//Method to Get All SuperHeroes
async Task<List<SuperHero>> GetAllSuperHeroes(DataContext _context) =>
    await _context.SuperHeroes.ToListAsync();
//GetAll Minimal API
app.MapGet("/SuperHero", async (DataContext _context) =>
await _context.SuperHeroes.ToListAsync());
//Get By Id Minimal API
app.MapGet("/SuperHero/{id}", async (DataContext _context, int id) => 
await _context.SuperHeroes.FindAsync(id) is SuperHero hero ? Results.Ok(hero):Results.NotFound("Sorry No SuperHero Found!!!"));
//Insert Minimal API
app.MapPost("/SuperHero", async (DataContext _context, SuperHero hero) =>
{
    _context.SuperHeroes.Add(hero);
    await _context.SaveChangesAsync();
    return Results.Ok(await GetAllSuperHeroes(_context));
});
//Update by Id Minimal API
app.MapPut("/SuperHero/{id}", async (DataContext _context, SuperHero hero, int id) =>
{
    var dbHero = await _context.SuperHeroes.FindAsync(id);
    if (dbHero == null)
        return Results.NotFound("No SuperHero Found!!!");
    dbHero.FirstName = hero.FirstName;
    dbHero.LastName = hero.LastName;
    dbHero.SuperHeroName = hero.SuperHeroName;
    await _context.SaveChangesAsync();
    return Results.Ok(await GetAllSuperHeroes(_context));
});
//Delete By Id Minimal API
app.MapDelete("/SuperHero/{id}", async (DataContext _context, int id) =>
{
    var dbHero=await _context.SuperHeroes.FindAsync(id);
    if (dbHero == null)
        return Results.NotFound("Not Record Found!!!");
    _context.SuperHeroes.Remove(dbHero);
    await _context.SaveChangesAsync();
    return Results.Ok(await GetAllSuperHeroes(_context));
});
app.Run(); 