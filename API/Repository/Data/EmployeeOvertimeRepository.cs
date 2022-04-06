using API.Contexts;
using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class EmployeeOvertimeRepository : GeneralRepository<MyContext, EmployeeOvertime, int>
    {
        public EmployeeOvertimeRepository(MyContext myContext) : base(myContext)
        {
        }
    }
}
