﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using judo_univ_rennes.Components.Dialogs;
using judo_univ_rennes.Contracts;
using judo_univ_rennes.Data;
using MongoDB.Driver.GridFS;
using MudBlazor;
using System.IO;

namespace judo_univ_rennes.Components.Libraries
{
    public partial class PdfLibrary
    {
        #region properties
        /// <summary>
        /// dependency injection service pdf
        /// </summary>
        [Inject] IPdfRepo _pdf { get; set; }
        /// <summary>
        /// dependency injection MudBlazor dialogservice
        /// </summary>
        [Inject] IDialogService DialogService { get; set; }
        /// <summary>
        /// dependency injection MudBlazor Snackbar
        /// </summary>
        [Inject] ISnackbar Snackbar { get; set; }
        /// <summary>
        /// projectname parameter (url path)
        /// </summary>
        [Parameter] public string? projectName { get; set; }
  
        [Parameter] public bool UploadCompleted { get; set; } 
        [Parameter] public EventCallback<bool> UploadCompletedChanged { get; set; }
        /// <summary>
        /// page loading status
        /// </summary>
        private bool loading { get; set; } = false;

        /// <summary>
        /// selected item in MudBlazor table
        /// </summary>
        private PdfModel selectedItem = null;
/// <summary>
/// PdfModel to be edited
/// </summary>
        private PdfModel elementBeforeEdit;
        /// <summary>
        /// Edit trigger
        /// </summary>
        private TableEditTrigger editTrigger = TableEditTrigger.RowClick;
        
        /// <summary>
        /// items in MudBlazor table
        /// </summary>
        protected IList<PdfModel> PdfModels { get; set; }

        #endregion
        #region methods

        //protected override async Task OnInitializedAsync()
        //{
        //    PdfModels = await _pdf.GetByProjectNameAsync(projectName);
            
        //}
        protected override async Task OnParametersSetAsync()
        {
            loading = true;

            PdfModels = await _pdf.GetAsync();
            //if (PdfCategory!=null)
            //{
            //    PdfModels = PdfModels.Where(x => x.Category == PdfCategory.ToString()).ToList();
            //}
            //StateHasChanged();

            loading = false;

            StateHasChanged();
            base.OnParametersSetAsync();
        }

        private async void DeleteFile(string fileName)
        {
            loading = true;
            var itemToRemove = PdfModels.Where(a => a.FileName == fileName).FirstOrDefault();
            PdfModel fileInfo = await _pdf.GetByNameAsync(fileName);
            Task deleteComplete = _pdf.RemoveAsync(fileInfo.Id);
            StateHasChanged();

            if (deleteComplete.IsCanceled)
            {
                Snackbar.Add($"{fileName} n'a pas été supprimé", Severity.Error);

            }
            else
            {
                PdfModels.Remove(itemToRemove);
                Snackbar.Add($"{fileName} supprimé", Severity.Warning);
            }
            loading = false;
            StateHasChanged();
        }

        private async Task UpdatePdf(PdfModel pdfModel)
        {
            var result=  _pdf.UpdateAsync(pdfModel.Id, pdfModel);
            StateHasChanged();

            if (result.IsCompleted)
            {

                Snackbar.Add("pdf mis à jour");
            }
            else
            {
                Snackbar.Add("erreur de MAJ", Severity.Error);

            }
        }

        private void BackupItem(object element)
        {
            elementBeforeEdit = new()
            {
                FileName = ((PdfModel)element).FileName,
                Category = ((PdfModel)element).Category
            };
            //AddEditionEvent($"RowEditPreview event: made a backup of Element {((PdfUploadModel)element).FileName}");
        }

        private void ItemHasBeenCommitted(object element)
        {
            //AddEditionEvent($"RowEditCommit event: Changes to Element {((PdfUploadModel)element).FileName} committed");
        }

        private void ResetItemToOriginalValues(object element)
        {
            ((PdfModel)element).FileName = elementBeforeEdit.FileName;
            ((PdfModel)element).Category = elementBeforeEdit.Category;
            //((PdfUploadModel)element).Position = elementBeforeEdit.Position;
           // AddEditionEvent($"RowEditCancel event: Editing of Element {((PdfUploadModel)element).FileName} canceled");
        }
        private async Task ShowFile(string fileName)
        {
            var parameters = new DialogParameters();
            parameters.Add("fileName", fileName);
            parameters.Add("projectName", projectName);
            parameters.Add("ButtonText", "Show");
            parameters.Add("Color", Color.Info);
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraExtraLarge, FullWidth = true };
            var result = await DialogService.Show<DialogPdf>("Document interactif", parameters, options).Result;

        }
        #endregion
    }
}
