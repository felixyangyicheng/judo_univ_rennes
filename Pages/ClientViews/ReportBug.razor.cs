using System;
using Blazorise.RichTextEdit;

namespace judo_univ_rennes.Pages.ClientViews
{
	public partial class ReportBug
	{
        protected RichTextEdit richTextEditRef;
        protected bool readOnly;
        protected string contentAsHtml;
        protected string contentAsDeltaJson;
        protected string contentAsText;
        protected string savedContent;

        public async Task OnContentChanged()
        {
            contentAsHtml = await richTextEditRef.GetHtmlAsync();
            contentAsDeltaJson = await richTextEditRef.GetDeltaAsync();
            contentAsText = await richTextEditRef.GetTextAsync();
        }

        public async Task OnSave()
        {
            savedContent = await richTextEditRef.GetHtmlAsync();
            await richTextEditRef.ClearAsync();
        }
    }
}

