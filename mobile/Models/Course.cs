using System;

namespace mobile.Models
{
    public class Course : BaseModel
    {
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Course title cannot be empty.");
                _title = value;
            }
        }

        public int TermID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }

        // Instructor Information
        public string InstructorName { get; set; }
        public string InstructorEmail { get; set; }
        public string InstructorPhone { get; set; }

        public string Notes { get; set; }
        public bool EnableNotifications { get; set; }

        public override string DisplayDetails()
        {
            return $"Course: {Title}, Instructor: {InstructorName}, Status: {Status}";
        }
    }
}
