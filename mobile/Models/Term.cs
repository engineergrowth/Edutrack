using System;

namespace mobile.Models
{
    public class Term : BaseModel
    {
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public override string DisplayDetails()
        {
            return $"Term: {Title}, Start: {StartDate:MM/dd/yyyy}, End: {EndDate:MM/dd/yyyy}";
        }
    }
}
