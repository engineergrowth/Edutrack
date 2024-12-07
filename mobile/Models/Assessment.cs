using System;

namespace mobile.Models
{
    public class Assessment : BaseModel
    {
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Type { get; set; }
        public bool NotificationEnabled { get; set; }

        public int CourseID { get; set; }

        public override string DisplayDetails()
        {
            return $"Assessment: {Title}, Type: {Type}, CourseID: {CourseID}";
        }
    }
}
