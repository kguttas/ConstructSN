using ConstructSN.DAL;
using ConstructSN.DAL.Infrastructure;
using ConstructSN.Shared;
using ConstructSN.Shared.BusinessModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Navigations;
using System;


namespace ConstructSN.WebApp.Pages
{
    //[Route("/reclamos/detalle/{idClaim}")]
    [Route("/reclamos/ingresar")]
    public partial class AddClaim : ComponentBase
    {
        [Inject] NavigationManager? NavigationManager { get; set; }

        [Inject] IConfiguration? Configuration { get; set; }

        [Inject] IWebHostEnvironment? env { get; set; }

        //private EditContext editContext;

        //private CancellationTokenSource cancelation;

        //private bool displayProgress;
        
        //private int progressPercent;

        protected ClaimAgainstCompany claimsForm = new();

        public string? Title { get; set; }
        
        public string? Description { get; set; }

        DataConnection? DataConnection { get; set; }

        ClaimsRepository? repository { get; set; }

        protected bool IsVisibleModal { get; set; } = false;

        SfUploader? sfUploader { get; set; }


        protected override Task OnInitializedAsync()
        {
            //cancelation = new CancellationTokenSource();

            //editContext = new EditContext(claimsForm);

            Title = "Ingresar Reclamo";
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

        protected async Task SaveUser()
        {
            IsVisibleModal = true;

            var result = await repository.Upsert(claimsForm);

            if (UploadFiles != null)
            {
                var pathDirectory = $"{env.WebRootPath}\\images\\{claimsForm._id.ToString()}";
                
                if (!Directory.Exists(pathDirectory))
                {
                    Directory.CreateDirectory(pathDirectory);
                }

                for (int i = 0; i < UploadFiles.Count(); i++)
                {
                    var pathImage = Path.Combine(pathDirectory, $"{UploadFiles[i].FileInfo.Name}");

                    FileStream filestream = new FileStream(pathImage, FileMode.Create, FileAccess.Write);

                    UploadFiles[i].Stream.WriteTo(filestream);
                    filestream.Close();
                    UploadFiles[i].Stream.Close();
                }
            }

            Cancel();

            IsVisibleModal = false;
        }

        public void Cancel()
        {
            NavigationManager?.NavigateTo("/reclamos");
        }

        public List<UploadFiles>? UploadFiles { get; set; }

        protected void SfUpload_OnChange(UploadChangeEventArgs args)
        {
            UploadFiles = args.Files;

        }

        private void SuccessHandler(SuccessEventArgs args)
        {
            IsVisibleModal = true;

            base.StateHasChanged();
        }

        private void OnActionCompleteHandler(ActionCompleteEventArgs args)
        {
            IsVisibleModal = false;

            base.StateHasChanged();
        }

        private void FileSelectedHandler(SelectedEventArgs args)
        {
            // Here, you can customize your code.
            IsVisibleModal = true;

            base.StateHasChanged();
        }
    }
}
