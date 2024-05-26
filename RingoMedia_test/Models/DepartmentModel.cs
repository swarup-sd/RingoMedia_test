namespace RingoMedia_test.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentLogo { get; set; }
        public int? ParentDepartmentId { get; set; }
        public Department ParentDepartment { get; set; }
        public ICollection<Department> SubDepartments { get; set; }
    }

}
