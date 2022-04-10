using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class DetailOvertimeVM
    {
        public string NIK { get; set; }
        public string FullName { get; set; }
        public DateTime SubmitDate { get; set; }
        public int Salary { get; set; }
        public int Paid { get; set; }
        public Types Type { get; set; }
        public List<EmployeeOvertime> List { get; set; }
    }
}
