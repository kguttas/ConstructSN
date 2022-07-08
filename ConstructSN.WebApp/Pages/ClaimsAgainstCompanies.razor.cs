using ConstructSN.DAL;
using ConstructSN.Shared;
using ConstructSN.Shared.BusinessModel;
using ConstructSN.Shared.InfrastructureModel;
using Microsoft.AspNetCore.Components;
using MongoDB.Driver;
using Syncfusion.Blazor.Navigations;
using static System.Net.WebRequestMethods;

namespace ConstructSN.WebApp.Pages
{
    [Route("/reclamos")]
    public partial class ClaimsAgainstCompanies : ComponentBase
    {
        [Inject] IConfiguration? Configuration { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }

       

        protected ResultForPager<ClaimAgainstCompany> claimsList = new();

        protected string SearchString { get; set; } = string.Empty;

        SfPager? Pager = new SfPager();
        public int pageSize { get; set; }
        public int SkipValue;
        public int TakeValue = 5;
        public int TotalItemsCount { get; set; } = 0;

        DataConnection? DataConnection { get; set; }

        ClaimsRepository? repository { get; set; }

        protected override Task OnInitializedAsync()
        {
            Title = "Reclamos";
            Description = "Reclamos en Contra de Empresas Constructoras de Casas Prefabricadas";

            if (Configuration != null)
            {
                DataConnection = new DataConnection
                {
                    DatabaseName = Configuration["MongoDbSettings:DatabaseName"],
                    ConnectionString = Configuration["MongoDbSettings:ConnectionString"]
                };

                repository = new ClaimsRepository(DataConnection.ConnectionString, DataConnection.DatabaseName);

                GetUser();
            }

            return base.OnInitializedAsync();
        }

        protected async Task FilterClaims()
        {
            if (Pager != null)
            {
                Pager.CurrentPage = 1;

                SkipValue = (Pager.CurrentPage * Pager.PageSize) - Pager.PageSize;
                TakeValue = Pager.PageSize;
            }

            await GetUser();
        }
        public async Task ResetSearch()
        {
            SearchString = string.Empty;
            await GetUser();
        }

        protected async Task GetUser()
        {
            if (repository != null)
            {
                var builder = Builders<ClaimAgainstCompany>.Filter;
                var filter = builder.And(
                   builder.Or(
                       builder.Or(
                           builder.Regex(_ => _.NameCompany, new MongoDB.Bson.BsonRegularExpression("/.*" + SearchString + ".*/i")),
                           builder.Where(_ => string.IsNullOrEmpty(SearchString))
                       ),
                       builder.Or(
                           builder.Regex(_ => _.ProblemDescription, new MongoDB.Bson.BsonRegularExpression("/.*" + SearchString + ".*/i")),
                           builder.Where(_ => string.IsNullOrEmpty(SearchString))
                       )
                   )
               );


                var dataList = await repository.Get(SkipValue, TakeValue, filter);
                var countList = await repository.CountDocuments(filter);

                claimsList.Items = dataList;
                claimsList.CountItems = countList;
                TotalItemsCount = (int)countList;
            }
            
            base.StateHasChanged();
        }

        public async Task ClickPager(PageClickEventArgs args)
        {
            if (Pager != null)
            {
                SkipValue = (args.CurrentPage * Pager.PageSize) - Pager.PageSize;
                TakeValue = Pager.PageSize;
            }

            await GetUser();
        }
    }
}
