namespace UD.Core.Middlewares
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlingMiddleware> logger;
        private readonly IWebHostEnvironment env;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IWebHostEnvironment env)
        {
            this.next = next;
            this.logger = logger;
            this.env = env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try { await this.next(context); }
            catch (Exception ex)
            {
                if (this.logger.IsEnabled(LogLevel.Error)) { this.logger.LogError(ex, "Unhandled exception occurred. TraceId: {TraceId}, Path: {Path}", context.TraceIdentifier, context.Request.Path); }
                await HandleExceptionAsync(context, ex);
            }
        }
        private static readonly JsonSerializerOptions jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/problem+json";
            ProblemDetails problemDetails;
            if (this.env.IsDevelopment())
            {
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "An unexpected error occurred",
                    Detail = exception.Message,
                    Instance = context.Request.Path,
                    Extensions =
                    {
                        ["traceId"] = context.TraceIdentifier,
                        ["stackTrace"] = exception.ToString()
                    }
                };
            }
            else
            {
                problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occurred. Please try again later.",
                    Instance = context.Request.Path,
                    Extensions = { ["traceId"] = context.TraceIdentifier }
                };
            }
            if (exception is KeyNotFoundException || exception is ArgumentException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Bad Request";
                problemDetails.Detail = exception.Message;
            }
            else if (exception is UnauthorizedAccessException)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                problemDetails.Status = StatusCodes.Status401Unauthorized;
                problemDetails.Title = "Unauthorized";
            }
            else if (exception is OperationCanceledException)
            {
                context.Response.StatusCode = StatusCodes.Status499ClientClosedRequest;
                problemDetails.Status = StatusCodes.Status499ClientClosedRequest;
                problemDetails.Title = "Request Cancelled";
            }
            else { context.Response.StatusCode = StatusCodes.Status500InternalServerError; }
            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, jsonOptions));
        }
        /*
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddProblemDetails();
        var app = builder.Build();
        if (app.Environment.IsDevelopment()) { app.UseDeveloperExceptionPage(); }
        else { app.UseMiddleware<ExceptionHandlingMiddleware>(); }
        app.UseHttpsRedirection();
        app.UseRouting();
        app.MapControllers();
        app.Run();
        */
    }
}