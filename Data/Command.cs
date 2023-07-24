using BlazorPro.Spinkit;

namespace judo_univ_rennes.Data
{
    public class Command : BaseTextItem
    {
        public string Title { get; set; } 
        public string Type { get; set; } 
        public bool Closed { get; set; }
    }
}
