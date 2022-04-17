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

        public DetailOvertimeVM DetailOvertime(string id, DateTime date)
        {
            var x = _context.Employees
                .Join(_context.EmployeeOvertimes, e => e.NIK, eo => eo.EmployeeId, (e, eo) => new { e, eo})
                .Join(_context.Overtimes, eeo => eeo.eo.OvertimeId, o => o.Id, (eeo, o) => new { 
                    NIK = eeo.eo.EmployeeId,
                    FullName = (eeo.e.FirstName +" "+ eeo.e.LastName),
                    SubmitDate = o.SubmitDate,
                    Salary = eeo.e.Salary,
                    Paid = o.Paid,
                    Type = o.Type,
                    OvertimeId = o.Id
                }).FirstOrDefault(s => s.NIK == id && s.SubmitDate == date);

            return new DetailOvertimeVM { 
                NIK = x.NIK,
                FullName = x.FullName,
                SubmitDate = x.SubmitDate,
                Salary = x.Salary,
                Paid = x.Paid,
                Type = x.Type,
                List = _context.EmployeeOvertimes.Where(w => w.EmployeeId == x.NIK && w.OvertimeId == x.OvertimeId).ToList()
            };
        }

        public IEnumerable ListOvertimeById(string id)
        {
            var x = _context.EmployeeOvertimes.Where(w => w.EmployeeId == id).Join(_context.Overtimes, eo => eo.OvertimeId, o => o.Id, (eo, o) => new
            {
                NIK = eo.EmployeeId,
                Submit = o.SubmitDate,
                Total = (eo.EndOvertime - eo.StartOvertime).TotalHours,
                Paid = o.Paid,
                Type = o.Type.ToString(),
                Status = o.Status.ToString()
            }).ToList();

            return x.GroupBy(g => new { g.NIK, g.Submit }).Select(s => new
            {
                NIK = s.Key.NIK,
                Submit = s.Select(s => s.Submit).First(),
                Total = s.Sum(s => s.Total),
                Paid = s.Min(s => s.Paid),
                Type = s.Select(s => s.Type).First(),
                Status = s.Select(s => s.Status).First(),
            });
        }

        public IEnumerable ListFinanceOvertime()
        {
            var x = _context.EmployeeOvertimes.Join(_context.Overtimes, eo => eo.OvertimeId, o => o.Id, (eo, o) => new
            {
                NIK = eo.EmployeeId,
                Submit = o.SubmitDate,
                Total = (eo.EndOvertime - eo.StartOvertime).TotalHours,
                Paid = o.Paid,
                Type = o.Type.ToString(),
                Status = o.Status.ToString()
            }).ToList();

            return x.GroupBy(g => new { g.NIK, g.Submit }).Select(s => new
            {
                NIK = s.Key.NIK,
                Submit = s.Select(s => s.Submit).First(),
                Total = s.Sum(s => s.Total),
                Paid = s.Min(s => s.Paid),
                Type = s.Select(s => s.Type).First(),
                Status = s.Select(s => s.Status).First(),
            }).Where(w => !(w.Status == "Pending"));
        }

        public IEnumerable ListOvertime()
        {
            var x = _context.EmployeeOvertimes.Join(_context.Overtimes, eo => eo.OvertimeId, o => o.Id, (eo, o) => new
            {
                NIK = eo.EmployeeId,
                Submit = o.SubmitDate,
                Total = (eo.EndOvertime - eo.StartOvertime).TotalHours,
                Paid = o.Paid,
                Type = o.Type.ToString(),
                Status = o.Status.ToString()
            }).ToList();

            return x.GroupBy(g => new { g.NIK, g.Submit }).Select(s => new
            {
                NIK = s.Key.NIK,
                Submit = s.Select(s => s.Submit).First(),
                Total = s.Sum(s => s.Total),
                Paid = s.Min(s => s.Paid),
                Type = s.Select(s => s.Type).First(),
                Status = s.Select(s => s.Status).First(),
            });
        }
        public IEnumerable RemainingOvertime(string id)
        {
            return _context.Overtimes.Where(w => w.Type == Types.Weekday).Join(_context.EmployeeOvertimes, o => o.Id, eo => eo.OvertimeId, (o, eo) => new
            {
                NIK = eo.EmployeeId,
                Total = (eo.EndOvertime - eo.StartOvertime).TotalHours
            }).ToList().GroupBy(g => g.NIK).Select(s => new
            {
                NIK = s.Key,
                Remaining = 40 - s.Sum(s => s.Total)
            }).Where(w => w.NIK == id);
        }

        public int Request(AddOvertimeVM overtime)
        {
            var x = _context.EmployeeOvertimes.Join(_context.Overtimes, eo => eo.OvertimeId, o => o.Id, (eo, o) => new
            {
                NIK = eo.EmployeeId,
                Submit = o.SubmitDate
            }).ToList();

            if (overtime != null)
            {
                if ((x.FirstOrDefault(g => g.NIK == overtime.Detail[0].EmployeeId && g.Submit == overtime.SubmitDate)) == null)
                {
                    var today = overtime.SubmitDate.DayOfWeek;
                    var salary = _context.Employees.Where(w => w.NIK == overtime.Detail[0].EmployeeId).Select(s => s.Salary).Single();
                    var overtimePerHour = salary * 1 / 173;
                    var _total = 0.0;
                    var paid = 0;
                    Types type = 0;

                    foreach (var item in overtime.Detail)
                    {
                        _total += Convert.ToInt32((item.EndOvertime - item.StartOvertime).TotalHours);
                    }

                    int total = (int)Math.Round(_total, MidpointRounding.AwayFromZero);

                    if (today == DayOfWeek.Sunday || today == DayOfWeek.Saturday)
                    {
                        type = Types.Weekend;

                        for (int i = 0; i < total; i++)
                        {
                            if (i < 8)
                            {
                                paid += 2 * overtimePerHour;
                            } else if (i == 8)
                            {
                                paid += 3 * overtimePerHour;
                            } else
                            {
                                paid += 4 * overtimePerHour;
                            }
                        }
                    } else
                    {
                        type = Types.Weekday;

                        for (int i = 0; i < total; i++)
                        {
                            if (i < 1)
                            {
                                paid += Convert.ToInt32(1.5 * overtimePerHour);
                            } else
                            {
                                paid += 2 * overtimePerHour;
                            }
                        }
                    }

                    var o = new Overtime
                    {
                        SubmitDate = overtime.SubmitDate,
                        Status = Status.Pending,
                        Paid = paid,
                        Type = type
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
                    return 2;
                }
                return 1;
            }
            return 0;
        }
    }
}
