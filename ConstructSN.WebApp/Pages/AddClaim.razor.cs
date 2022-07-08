using ConstructSN.DAL;
using ConstructSN.DAL.Infrastructure;
using ConstructSN.Shared;
using ConstructSN.Shared.BusinessModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using System;


namespace ConstructSN.WebApp.Pages
{
    //[Route("/reclamos/detalle/{idClaim}")]
    [Route("/reclamos/ingresar")]
    public partial class AddClaim : ComponentBase, IDisposable
    {
        [Inject] NavigationManager? NavigationManager { get; set; }

        [Inject] IConfiguration? Configuration { get; set; }

        [Inject] IWebHostEnvironment? env { get; set; }

       // private EditContext editContext;

        private CancellationTokenSource cancelation;

        private bool displayProgress;
        
        private int progressPercent;

        protected ClaimAgainstCompany claimsForm = new();

        public string? Title { get; set; }
        
        public string? Description { get; set; }

        DataConnection? DataConnection { get; set; }

        ClaimsRepository? repository { get; set; }

        protected override Task OnInitializedAsync()
        {
            cancelation = new CancellationTokenSource();

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
            var result = await repository.Upsert(claimsForm);

            for (int i = 0; i < TotalPictures; i++)
            {
                var pathDirectory = $"{env.WebRootPath}\\images\\{claimsForm._id.ToString()}";
                var pathImage = Path.Combine(pathDirectory, $"{claimsForm.Pictures[i].Name}");

                if (!Directory.Exists(pathDirectory))
                {
                    Directory.CreateDirectory(pathDirectory);
                }

                using var file = File.OpenWrite(pathImage);
                using var stream = claimsForm.Pictures[i].OpenReadStream(968435456);

                var buffer = new byte[4 * 1096];
                int bytesRead;
                double totalRead = 0;

                displayProgress = true;

                while ((bytesRead = await stream.ReadAsync(buffer, cancelation.Token)) != 0)
                {
                    totalRead += bytesRead;
                    await file.WriteAsync(buffer, cancelation.Token);

                    progressPercent = (int)((totalRead / claimsForm.Pictures[i].Size) * 100);
                    StateHasChanged();
                }

                displayProgress = false;
            }

            Cancel();
        }

        public void Cancel()
        {
            NavigationManager?.NavigateTo("/reclamos");
        }

        private IList<string> imageDataUrls = new List<string>();

        private int TotalPictures { get; set; }

        private async Task LoadFiles(InputFileChangeEventArgs e)
        {
            claimsForm.Pictures = e.GetMultipleFiles().ToArray();

            var format = "image/png";
            TotalPictures = e.GetMultipleFiles().Count();
            foreach (var imageFile in e.GetMultipleFiles())
            {
                var resizedImageFile = await imageFile.RequestImageFileAsync(format, 512, 512);
                var buffer = new byte[resizedImageFile.Size];
                await resizedImageFile.OpenReadStream().ReadAsync(buffer);
                var imageDataUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                imageDataUrls.Add(imageDataUrl);
                claimsForm.ImageDataUrls = imageDataUrls;
            }
            
            //editContext.NotifyFieldChanged(FieldIdentifier.Create(() => claimsForm.Pictures));
        }

        public void Dispose()
        {
            cancelation.Cancel();
        }

    }
}
