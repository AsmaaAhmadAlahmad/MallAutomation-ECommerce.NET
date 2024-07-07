namespace AlhamraMallApi.ApiModels.EmployeeModels
{
    public class EmployeeForUpdate
    {
        public string Name { get; set; }
        public double Salary { get; set; }
        public Guid DepartmentId { get; set; }
    }
}