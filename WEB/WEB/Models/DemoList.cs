namespace WEB.Models
{
    public class DemoList
    {
        public DemoList(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }

}
