using System;
using Blazorise.RichTextEdit;
using Microsoft.AspNetCore.Components;

namespace judo_univ_rennes.Pages.ClientViews
{
	public partial class ReportBug
	{
        protected RichTextEdit richTextEditRef;
        protected bool readOnly;
        protected string contentAsHtml=" Signaler un bug...";

        protected MarkupString DisplayContent;
        protected string contentAsDeltaJson;
        protected string contentAsText;
        protected string savedContent;


        protected override Task OnInitializedAsync( )
        {
            DisplayContent = (MarkupString)contentAsHtml;

            return base.OnInitializedAsync();
        }
        public async Task OnContentChanged()
        {
            contentAsHtml = await richTextEditRef.GetHtmlAsync();
            DisplayContent =(MarkupString) contentAsHtml;
            contentAsDeltaJson = await richTextEditRef.GetDeltaAsync();
            contentAsText = await richTextEditRef.GetTextAsync();
            StateHasChanged();
        }

        public async Task OnSave()
        {
            savedContent = await richTextEditRef.GetHtmlAsync();
            await richTextEditRef.ClearAsync();
        }
    }
}

