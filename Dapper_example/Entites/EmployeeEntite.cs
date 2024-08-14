namespace Dapper_example.Entites
{
    public class EmployeeEntite
    {
        protected EmployeeEntite() { }

        public string Fullname { get; set; }
        public int Id { get; set; }
        public DateTime Birthdate { get; set; }
        public double Salary { get; set; }
        public string Position { get; set; }
        public bool IsActive { get; set; }

        public EmployeeEntite(string fullname, int id, DateTime birthdate, double salary, string position)
        {
            Fullname = fullname;
            Id = id;
            Birthdate = birthdate;
            Salary = salary;
            Position = position;
            IsActive = true;
        }
    }
}
