using System;
using Markdig;
using Microsoft.AspNetCore.Components;
using static MudBlazor.CategoryTypes;

namespace judo_univ_rennes.Pages.AdminViews
{
	public  partial class EditMarkdown
	{
        protected string markdownValue = "## Table\n| Right | Left | Default | Center |\n|------:|:-----|---------|:------:|\n|   12  |  12  |    12   |    12  |\n|  123  |  123 |   123   |   123  |\n|    1  |    1 |     1   |     1  |\n+---------------+---------------+--------------------+\n| Fruit         | Price         | Advantages         |\n+===============+===============+====================+\n| Bananas       | $1.34         | - built-in wrapper |\n|               |               | - bright color     |\n+---------------+---------------+--------------------+\n| Oranges       | $2.10         | - cures scurvy     |\n|               |               | - tasty            |\n+---------------+---------------+--------------------+\n## Header Identifiers in HTML\n[Header identifiers in HTML](#header-identifiers-in-html-given)";

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

