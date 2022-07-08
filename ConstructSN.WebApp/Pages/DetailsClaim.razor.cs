using ConstructSN.DAL;
using ConstructSN.Shared.BusinessModel;
using ConstructSN.Shared;
using Microsoft.AspNetCore.Components;
using MongoDB.Driver;
using Syncfusion.Blazor.Lists.Internal;

namespace ConstructSN.WebApp.Pages
{
    [Route("/reclamos/detalle/{idClaim}")]
    public partial class DetailsClaim : ComponentBase
    {
        [Inject] NavigationManager? NavigationManager { get; set; }

        [Inject] IConfiguration? Configuration { get; set; }

        [Inject] IWebHostEnvironment? env { get; set; }

        [Parameter]
        public string? idClaim { get; set; }

        protected ClaimAgainstCompany claimsForm;

        public string? Title { get; set; }

        public string? Description { get; set; }

        DataConnection? DataConnection { get; set; }

        ClaimsRepository? repository { get; set; }

        List<FrameworkDetails> FrameworkData = new List<FrameworkDetails>();

        private string PathImages { get; set; }

        protected override Task OnInitializedAsync()
        {
            Title = "Detalles del Reclamo";
            Description = "Portal de Empresas Constructuras de Casas Prefabricadas";



            if (Configuration != null)
            {
                DataConnection = new DataConnection
                {
                    DatabaseName = Configuration["MongoDbSettings:DatabaseName"],
                    ConnectionString = Configuration["MongoDbSettings:ConnectionString"]
                };

                repository = new ClaimsRepository(DataConnection.ConnectionString, DataConnection.DatabaseName);

                
            }



            return base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()
        {

            if (!string.IsNullOrEmpty(idClaim) && claimsForm == null)
            {
                var builder = Builders<ClaimAgainstCompany>.Filter;
                var filter = builder.Where(_ => _._id == (new Guid(idClaim)));

                var result = await repository.Get(0, 1, filter);

                if (result != null && result.Count() > 0)
                {
                    claimsForm = result.FirstOrDefault();

                    PathImages = $"{env.WebRootPath}\\images\\{claimsForm._id.ToString()}";

                    // Get Images in folder

                    if (Directory.Exists(PathImages))
                    {
                        var images = Directory.GetFiles(PathImages);

                        foreach (var item in images)
                        {
                            FrameworkData.Add(new FrameworkDetails
                            {
                                ID = item.Split("\\").Last(),
                                Title = item.Split("\\").Last(),
                                Content = "",
                                ImageName = item.Split("\\").Last(),
                                URL = $"images\\{idClaim}\\{item.Split("\\").Last()}",
                            });
                        }
                    }
                }
            }

           await base.OnParametersSetAsync();
        }

        void Cancel()
        {
            NavigationManager.NavigateTo("/reclamos");
        }

       
        
        public class FrameworkDetails
        {
            public string ID { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public string ImageName { get; set; }
            public string URL { get; set; }
        }
    }

    
}
