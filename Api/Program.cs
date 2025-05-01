using Contacts.Application;
using Contacts.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var api = app.MapGroup("api");

var phoneNumbers = api.MapGroup("phone_numbers");

var createPhoneNumber = phoneNumbers.MapPost("", () => TypedResults.Ok());
var updatePhoneNumber = phoneNumbers.MapPut("{id:guid}", ([FromRoute] Guid id) => TypedResults.Ok());
var deletePhoneNumber = phoneNumbers.MapDelete("{id:guid}", ([FromRoute] Guid id) => TypedResults.Ok());
var getPhoneNumber = phoneNumbers.MapGet("{id:guid}", ([FromRoute] Guid id) => TypedResults.Ok());
var assignPhoneNumber = phoneNumbers.MapPut("{id:guid}/assign", ([FromRoute] Guid id) => TypedResults.Ok());
var retainPhoneNumber = phoneNumbers.MapPut("{id:guid}/retain", ([FromRoute] Guid id) => TypedResults.Ok());

app.Run();
