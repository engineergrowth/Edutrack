using SQLite;

namespace mobile.Models
{
    public abstract class BaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public virtual string DisplayDetails()
        {
            return $"ID: {Id}";
        }
    }
}
