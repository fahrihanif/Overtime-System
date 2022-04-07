using API.Contexts;
using API.Models;
using API.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class OvertimeRepository : GeneralRepository<MyContext, Overtime, int>
    {
        private readonly MyContext _context;
        public OvertimeRepository(MyContext myContext) : base(myContext)
        {
            _context = myContext;
        }

        public IEnumerable ListOvertime()
        {
            var x = _context.EmployeeOvertimes.Join(_context.Overtimes, eo => eo.OvertimeId, o => o.Id, (eo, o) => new
            {
                NIK = eo.EmployeeId,
                Submit = o.SubmitDate.ToString("dd/MMM/yyyy"),
                Total = (eo.EndOvertime - eo.StartOvertime).TotalMinutes,
                Paid = o.Paid,
                Type = o.Type.ToString(),
                Status = o.Status.ToString()
            }).ToList();

            return x.GroupBy(g => g.NIK).Select(s => new
            {
                NIK = s.Key,
                Submit = s.Select(s => s.Submit).First(),
                Total = s.Sum(s => s.Total),
                Paid = s.Sum(s => s.Paid),
                Type = s.Select(s => s.Type).First(),
                Status = s.Select(s => s.Status).First(),
            });
        }
        public IEnumerable RemainingOvertime()
        {
            var x = _context.EmployeeOvertimes.Select(s => new
            {
                NIK = s.EmployeeId,
                Total = (s.EndOvertime - s.StartOvertime).TotalMinutes
            }).ToList();

            return x.GroupBy(g => g.NIK).Select(s => new
            {
                NIK = s.Key,
                Remaining = 2400 - s.Sum(s => s.Total)
            });
        }

        public int Request(AddOvertimeVM overtime)
        {
            if (overtime != null)
            {
                var o = new Overtime
                {
                    SubmitDate = overtime.SubmitDate,
                    Status = overtime.Status,
                    Paid = overtime.Paid,
                    Type = overtime.Type
                };
                _context.Overtimes.Add(o);
                _context.SaveChanges();

                foreach (var item in overtime.Detail)
                {
                    var eo = new EmployeeOvertime
                    {
                        StartOvertime = item.StartOvertime,
                        EndOvertime = item.EndOvertime,
                        Description = item.Description,
                        OvertimeId = o.Id,
                        EmployeeId = item.EmployeeId
                    };
                    _context.EmployeeOvertimes.Add(eo);
                    _context.SaveChanges();
                }

                return 1;
            }
            
            return 0;
        }
    }
}
