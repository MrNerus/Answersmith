var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";



// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowSpecificOrigin",
//         builder => builder
//             .AllowAnyOrigin()
//             .AllowAnyHeader()
//             .AllowAnyMethod()
//             // .WithOrigins("http://localhost:4200") // specify your front-end origin
//             // .WithOrigins("*") // specify your front-end origin
//             // .AllowAnyHeader()
//             // .AllowAnyMethod()
//             );
// });

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder => { 
                        builder.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();


// app.UseCors("AllowSpecificOrigin");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();
app.MapControllers();
app.Run();





// var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddControllers();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// // Configure CORS
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowSpecificOrigin",
//         builder => builder
//             .AllowAnyOrigin()
//             .AllowAnyHeader()
//             .AllowAnyMethod()
//     );
// });

// var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

// // Ensure `UseRouting` comes before `UseCors`
// app.UseRouting();

// // Place `UseCors` between `UseRouting` and `UseEndpoints`
// app.UseCors("AllowSpecificOrigin");

// app.UseAuthorization();

// app.MapControllers();
// app.Run();