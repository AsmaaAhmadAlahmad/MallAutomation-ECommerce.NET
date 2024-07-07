namespace AlhamraMallApi.ApiModels.EmployeeModels
{
    public class EmployeeForCreate
    {
        public string Name { get; set; }
        public double Salary { get; set; }
        public Guid DepartmentId { get; set; }
    }
}