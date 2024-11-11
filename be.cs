using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        policy => policy.AllowAnyOrigin()
                         .AllowAnyMethod()
                         .AllowAnyHeader());
});

var app = builder.Build();

app.UseCors("AllowAnyOrigin");


List<order> bd = new List<order>()
{
    new order(1, DateTime.Now, "Ноутбук", "Проблема с экраном", "Экран треснул", "Иван Иванов", "в ожидании")
};

app.MapGet("/", () => bd);

app.MapPost("/", (order newOrder) =>
{
    newOrder.Number = bd.Any() ? bd.Max(o => o.Number) + 1 : 1;
    newOrder.DateAdd = DateTime.Now;

    if (string.IsNullOrEmpty(newOrder.Status))
    {
        newOrder.Status = "в_ожидании";
    }

    bd.Add(newOrder);

    return Results.Created($"/{newOrder.Number}", newOrder);
});


app.MapPost("/{orderNumber}", (int orderNumber, OrderUpdate orderUpdate) =>
{
    var orderToUpdate = bd.FirstOrDefault(o => o.Number == orderNumber);

    if (orderToUpdate == null)
    {
        return Results.NotFound(new { Message = $"Заявка с номером {orderNumber} не найдена" });
    }

    if (!string.IsNullOrEmpty(orderUpdate.Status))
    {
        orderToUpdate.Status = orderUpdate.Status;
    }

    if (!string.IsNullOrEmpty(orderUpdate.ProblemDesc))
    {
        orderToUpdate.ProblemDesc = orderUpdate.ProblemDesc;
    }

    if (!string.IsNullOrEmpty(orderUpdate.Master))
    {
        orderToUpdate.Master = orderUpdate.Master;
    }

    return Results.Ok(orderToUpdate);
});


app.Run();

class order
{
    public int Number { get; set; }
    public DateTime DateAdd { get; set; }
    public string Device { get; set; }
    public string ProblemType { get; set; }
    public string ProblemDesc { get; set; }
    public string Client { get; set; }
    public string Status { get; set; } = "в_ожидании";
    public string Master { get; set; } = "Не назначен";

    public order(int number, DateTime dateAdd, string device, string problemType, string problemDesc, string client, string status)
    {
        Number = number;
        DateAdd = dateAdd;
        Device = device;
        ProblemType = problemType;
        ProblemDesc = problemDesc;
        Client = client;
        Status = status;
    }
}

public class OrderUpdate
{
    public string Status { get; set; }
    public string ProblemDesc { get; set; }
    public string Master { get; set; }
}