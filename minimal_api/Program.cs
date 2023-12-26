using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

var hotels = new List<Hotel>();

app.MapGet("/", () => "hello world");
app.MapGet("/hotels", () => hotels);
app.MapGet("/hotels/{id}", (int id) => hotels.FirstOrDefault(h => h.Id == id));
app.MapPost("/hotels", (Hotel hotel) => hotels.Add(hotel));
app.MapPut("/hotels", (Hotel hotel) =>
{
    var index = hotels.FindIndex(h => h.Id == hotel.Id);
    if (index < 0)
    {
        throw new Exception("Not Found");
    }
    hotels[index] = hotel;
});
app.MapDelete("/hotels/{id}", (int id) =>
{
    var index = hotels.FindIndex(h => h.Id == id);
    if (index < 0)
    {
        throw new Exception("Not Found");
    }
    hotels.RemoveAt(index);
});

app.Run();

public record Hotel
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

[JsonSerializable(typeof(List<Hotel>))]
internal partial class AppJsonSerializerContext : JsonSerializerContext;