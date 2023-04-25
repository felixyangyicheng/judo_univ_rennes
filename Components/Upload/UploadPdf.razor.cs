using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using judo_univ_rennes.Components.Dialogs;
using judo_univ_rennes.Configurations;
using judo_univ_rennes.Contracts;
using judo_univ_rennes.Data;

using MudBlazor;
using Tewr.Blazor.FileReader;


namespace judo_univ_rennes.Components.Upload
{
    public partial class UploadPdf
    {
        [Inject] IPdfRepo _pdf { get; set; }
        [Inject] IFileReaderService fileReaderService { get; set; }
        [Inject] NavigationManager _navi { get; set; }
        [Inject] IDialogService DialogService { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }
        [Inject] IJSRuntime _jsRuntime { get; set; }
        [Inject] public IConfiguration _config { get; set; }
        public IEnumerable<IFileReference> files { get; set; }
        [Parameter]
        public EventCallback<bool> OnUploadCompleted { get; set; }
        [Parameter] public string SelectedProject { get; set; }
        [Parameter] public int SelectedProjectId { get; set; }

        public bool uploadCompleted { get; set; } = true;
        private int timecounter { get; set; } = 30;
        private string protocole;
        private string host;
        private string port;
        private string url;
        private static System.Timers.Timer aTimer;
        public string SelectedCategory  { get; set; }

        protected class PdfUploadModel : PdfModel
        {
            public int Progress { get; set; }
            public bool Uploaded { get; set; } = false;
            public bool Deleted { get; set; }
        }
        private ElementReference inputTypeFileElement;

        protected List<PdfUploadModel> PdfUploadModels { get; set; } = new List<PdfUploadModel>();


        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }
        public async Task ReadFile()
        {
            files = await fileReaderService.CreateReference(inputTypeFileElement).EnumerateFilesAsync();
            foreach (var file in files)
            {
                await UploadFile(file);
                //StateHasChanged();
            }
            uploadCompleted = true;
            StateHasChanged();

            //if (UpdateStatus)
            //{
            //    Snackbar.Add($"{SelectedProject} mis à jour", Severity.Success);

            //}
            //else{
            //    Snackbar.Add($"{SelectedProject} erreur de mise à jour", Severity.Error);

            //}
            // StartTimer();
        }

        private async Task UploadFile(IFileReference file)
        {
            var uploadElement = new PdfUploadModel();
            uploadElement.Progress = 0;

            PdfUploadModels.Add(uploadElement);

            await using var fileStream = await file.OpenReadAsync();
            var buffer = new byte[1024 * 512];
            var finalBuffer = new byte[fileStream.Length];
            int count;
            int totalCount = 0;
            #region

            while ((count = await fileStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {
                Buffer.BlockCopy(buffer, 0, finalBuffer, totalCount, count);
                totalCount += count;
                uploadElement.Progress = (int)(totalCount * 100.0 / fileStream.Length);
                //uploadElement.progress = "Téléchargement de fichers vers le server  " + (int)(totalCount * 100.0 / fileStream.Length) + "%";
                StateHasChanged();
            }
            uploadElement.Category = SelectedCategory;
            uploadElement.FileName = file.ReadFileInfoAsync().Result.Name;
            uploadElement.DateOfUpdate = DateTime.UtcNow;
            uploadElement.Content = finalBuffer;

            var PdfDto = new PdfModel
            {
                DateOfUpdate=uploadElement.DateOfUpdate,
                FileName = uploadElement.FileName,
                Category=uploadElement.Category,
                Content = uploadElement.Content,
            };

            var isExistOld = await _pdf.GetByNameAsync(PdfDto.FileName);

            if (isExistOld != null)
            {
                PdfDto.Id= isExistOld.Id;
                var result = _pdf.UpdateAsync(isExistOld.Id, PdfDto);
                StateHasChanged();
                if (result.Result)
                {
                    uploadElement.Uploaded = true;
                    uploadCompleted = true;
                    StateHasChanged();
                    uploadCompleted = !uploadCompleted;
                    StateHasChanged();
          

                }

            }
            else
            {

                var result = await _pdf.CreateAsync(PdfDto);
                if (!string.IsNullOrEmpty(result.ToString()))
                {
                    uploadElement.Uploaded = true;
                    uploadCompleted = true;
                    StateHasChanged();
                    uploadCompleted = !uploadCompleted;
                    StateHasChanged();
       

                }
            }


            #endregion
            //StateHasChanged();
            //uploadCompleted = !uploadCompleted;
            //StateHasChanged();

        }
        private void DeleteAllFiles()
        {
            var count = PdfUploadModels.Count();
            while (PdfUploadModels.Count() != 0)
            {
                string id = PdfUploadModels[0].Id;
                string filename = PdfUploadModels[0].FileName;
                var itemToRemove = PdfUploadModels.Where(a => a.FileName == filename).FirstOrDefault();
                PdfUploadModels.Remove(itemToRemove);

            }

            InvokeAsync(StateHasChanged);

        }

        private async void DeleteFile(string fileName)
        {
            var itemToRemove = PdfUploadModels.Where(a => a.FileName == fileName).FirstOrDefault();
            itemToRemove.Deleted = false;


            PdfModel PdfModel = await _pdf.GetByNameAsync(fileName);
            itemToRemove.Deleted = _pdf.RemoveAsync(PdfModel.Id).IsCompletedSuccessfully;


            PdfUploadModels.Remove(itemToRemove);
            StateHasChanged();
            Snackbar.Add($"{fileName} supprimé", Severity.Warning);

            uploadCompleted = true;
            StateHasChanged();
            uploadCompleted = !uploadCompleted;

            StateHasChanged();
        }


        private async Task ShowFile(string fileName)
        {
            var parameters = new DialogParameters();
            parameters.Add("fileName", fileName);
            parameters.Add("projectName", SelectedProject);

            parameters.Add("ButtonText", "Show");
            parameters.Add("Color", Color.Info);
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraExtraLarge, };
            var result = await DialogService.Show<DialogPdf>("Documents PDF", parameters, options).Result;

        }


    }
}
