using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace ProductRegistry.Api;

public static class DiContainer
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddDotEmilu()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(s =>
            {
                s.UseAllOfToExtendReferenceSchemas();
                s.SupportNonNullableReferenceTypes();
            })
            .ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            })
            .Configure<JsonOptions>(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            })
            .AddHttpContextAccessor()
            .AddSerilog((sp, lc) => lc
                .ReadFrom.Configuration(configuration)
                .ReadFrom.Services(sp));

    public static IApplicationBuilder UseSerilogMiddleware(this IApplicationBuilder app)
        => app.UseSerilogRequestLogging(options =>
        {
            options.MessageTemplate =
                "[{UserIdentity}] HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.00} ms";
            options.GetLevel = AssignHttpLevel;
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set("UserIdentity",
                    httpContext.User.Identity?.Name?.ToLowerInvariant() ?? "Anonymous");
            };

            return;

            static LogEventLevel AssignHttpLevel(HttpContext httpContext, double elapsed, Exception? exception)
            {
                #region Check Exception

                if (exception is not null)
                    return LogEventLevel.Error;

                #endregion

                #region Check Elapsed Time

                const int maxElapsedSeconds = 60;
                var elapsedSeconds = elapsed / 1000;
                if (elapsedSeconds > maxElapsedSeconds)
                    return LogEventLevel.Warning;

                #endregion

                #region Check Status Code

                return httpContext.Response.StatusCode switch
                {
                    >= 200 and < 300 => LogEventLevel.Information,
                    >= 400 and < 500 => LogEventLevel.Warning,
                    >= 500 => LogEventLevel.Error,
                    _ => LogEventLevel.Debug
                };

                #endregion
            }
        });
}