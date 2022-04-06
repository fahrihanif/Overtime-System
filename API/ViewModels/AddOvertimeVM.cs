using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class AddOvertimeVM
    {
        public DateTime SubmitDate { get; set; }
        public Status Status { get; set; }
        public int Paid { get; set; }
        public Types Type { get; set; }
        public List<EmployeeOvertime> Detail { get; set; }
    }
}
