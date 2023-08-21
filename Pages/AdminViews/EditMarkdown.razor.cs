using System;
using Markdig;
using Microsoft.AspNetCore.Components;
using static MudBlazor.CategoryTypes;

namespace judo_univ_rennes.Pages.AdminViews
{
	public  partial class EditMarkdown
	{
        protected string markdownValue = "# EasyMDE \n Go ahead, play around with the editor! Be sure to check out **bold**, *italic*";

        protected string markdownHtml;
        protected MarkupString DisplayContent;

  
        protected override void OnInitialized()
        {
            markdownHtml = Markdown.ToHtml(markdownValue ?? string.Empty);

            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

            DisplayContent = (MarkupString)Markdown.ToHtml(markdownValue, pipeline);
            base.OnInitialized();
        }

        protected Task OnMarkdownValueChanged(string value)
        {
            markdownValue = value;

            markdownHtml = Markdown.ToHtml(markdownValue ?? string.Empty);
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

            DisplayContent = (MarkupString)Markdown.ToHtml(markdownValue, pipeline);

            return Task.CompletedTask;
        }
    }
}

