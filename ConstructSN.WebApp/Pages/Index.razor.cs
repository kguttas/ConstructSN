using ConstructSN.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System;



namespace ConstructSN.WebApp.Pages
{
    [Route("/")]
    public partial class Index : ComponentBase
    {
        [Inject] IConfiguration? Configuration { get; set; }


        public string? Title { get; set; }
        public string? Description { get; set; }

        protected override Task OnInitializedAsync()
        {
            Title = "Inicio";
            Description = "Portal de Empresas Constructuras de Casas Prefabricadas";

            if (Configuration != null)
            {
                var conn = new DataConnection
                {
                    DatabaseName = Configuration["MongoDbSettings:DatabaseName"],
                    ConnectionString = Configuration["MongoDbSettings:ConnectionString"]
                };
            }

            return base.OnInitializedAsync();
        }
    }
}
