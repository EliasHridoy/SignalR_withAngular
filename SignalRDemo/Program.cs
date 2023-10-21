using Microsoft.AspNetCore.Builder;
using SignalRDemo.Hub;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSignalR();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(o =>
        {
            o.AddPolicy("CORSPolicy", builder =>
            {
                builder
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        }
    );

var app = builder.Build();
app.UseCors("CORSPolicy");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthorization();
app.UseRouting();
app.MapHub<MessageHub>("/offers");
//app.UseEndpoints(endpoints => {
//    endpoints.MapControllers();
//    endpoints.MapHub<MessageHub>("/offers");
//});

app.UseHttpsRedirection();


app.MapControllers();

app.Run();
