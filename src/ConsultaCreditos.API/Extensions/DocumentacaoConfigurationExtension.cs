namespace ConsultaCreditos.API.Extensions
{
    public static class DocumentacaoConfigurationExtension
    {
        public static void AddOpenApiCustomService(this IServiceCollection services)
        {
            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Info = new()
                    {
                        Title = "API de Consulta de Créditos Constituídos",
                        Version = "v1",
                        Description = "API RESTful para integração e consulta de créditos constituídos com processamento assíncrono via Azure Service Bus",
                        Contact = new()
                        {
                            Name = "Equipe de Desenvolvimento",
                            Email = "contato@example.com"
                        }
                    };
                    return Task.CompletedTask;
                });
            });
        }

        public static void SwaggerCustomConfig(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "API de Consulta de Créditos v1");
                    options.DocumentTitle = "API de Consulta de Créditos - Documentação";
                    options.RoutePrefix = "swagger";
                    options.DisplayRequestDuration();
                    options.EnableTryItOutByDefault();
                });

                app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
            }
        }
    }
}
