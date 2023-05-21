using Blazored.LocalStorage;
using judo_univ_rennes.Contracts;
using judo_univ_rennes.Data;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace judo_univ_rennes.Components.Tabs
{
    public partial class PdfTab
    {
        #region Properties

        [Parameter] public string fileName { get; set; }

        [Inject] IConfiguration _config { get; set; }
        [Inject] IPdfRepo _pdfRepo { get; set; }
        [Inject] ILocalStorageService _localStorage { get; set; }
        private bool loading { get; set; } = false;

        private string pdfLink { get; set; } = "";
        public PdfModel pdf { get; set; }


        #endregion
        #region methods
        protected override async Task OnParametersSetAsync()
        {
            loading = true;
            pdf = await _pdfRepo.GetByNameAsync(fileName);
            if (pdf != null)
            {
#if DEBUG
                var getBase = _config.GetSection("BaseAddress").GetRequiredSection("dev");
#else
                var getBase = _config.GetSection("BaseAddress").GetRequiredSection("prod");
#endif
                string protocole = getBase.GetRequiredSection("Protocole").Value;
                string host = getBase.GetRequiredSection("Host").Value;
                string port = getBase.GetRequiredSection("Port").Value;
                pdfLink = $"{protocole}{host}{port}/api/pdf/{fileName}";
            }
            await base.OnParametersSetAsync();
            loading = false;
        }


        #endregion
    }
}
