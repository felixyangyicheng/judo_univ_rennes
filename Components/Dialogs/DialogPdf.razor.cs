using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using judo_univ_rennes.Configurations;
using MudBlazor;
using Swashbuckle.SwaggerUi.CustomAssets;
using System.Net.Http.Headers;
using judo_univ_rennes.Contracts;
using judo_univ_rennes.Data;
using Consul;
using Microsoft.Extensions.Hosting;

namespace judo_univ_rennes.Components.Dialogs
{
    public partial class DialogPdf
    {
        #region Properties

        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        //[Parameter] public string projectName { get; set; }

        [Parameter] public string fileName { get; set; }
        [Parameter] public string ButtonText { get; set; }
        [Parameter] public Color Color { get; set; }
        //[Inject] HttpClient HttpClient { get; set; }
        [Inject] IConfiguration _config { get; set; }
        [Inject] IPdfRepo _pdfRepo{ get; set; }
        [Inject] ILocalStorageService _localStorage { get; set; }




        public string pdfUrl { get; set; } = "";
        public PdfModel pdf { get; set; }
        private bool Loading = false;
        #endregion
        #region methods

        protected override async Task OnParametersSetAsync()
        {
            Loading = true;
            pdf = await _pdfRepo.GetByNameAsync(fileName);
            if (pdf != null)
            {
                pdfUrl = $"http://localhost:8080/api/pdf/{fileName}";

                //pdfUrl = $"data:application/pdf;base64,{Convert.ToBase64String(pdf.Content)}";
            }
            await base.OnParametersSetAsync();            
            Loading = false;
        }


        private void Ok()
        {
            MudDialog.Close(DialogResult.Ok(true));
        }
        #endregion
    }
}
