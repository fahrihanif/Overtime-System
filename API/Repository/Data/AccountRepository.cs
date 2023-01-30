using API.Contexts;
using API.Models;
using API.ViewModels;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ThreadSafeRandomizer;

namespace API.Repository.Data
{
    public class AccountRepository : GeneralRepository<MyContext, Account, string>
    {
        private readonly MyContext _context;

        public AccountRepository(MyContext myContext) : base(myContext)
        {
            _context = myContext;
        }

        public int ChangePassord(ChangePasswordVM change)
        {
            var account = _context.Accounts.Select(a => new
            {
                NIK = a.NIK,
                Email = a.Email,
                Password = a.Password,
                OTP = a.OTP,
                IsUsed = a.IsUsed
            }).SingleOrDefault(e => e.Email == change.Email);

            if (account != null)
            {
                if (change.OTP == account.OTP)
                {
                    if (account.IsUsed == false)
                    {
                        if (change.NewPassword == change.ConfirmPassword)
                        {
                            base.Update(new Account
                            {
                                NIK = account.NIK,
                                Email = account.Email,
                                Password = HashPassword(change.NewPassword),
                                OTP = account.OTP,
                                IsUsed = true
                            });
                            return 4;
                        }
                        return 3;
                    }
                    return 2;
                }
                return 1;
            }
            return 0;
        }

        public int ForgotPassword(String email)
        {
            var account = _context.Accounts.Select(a => new
            {
                NIK = a.NIK,
                Email = a.Email,
                Password = a.Password
            }).SingleOrDefault(e => e.Email == email);

            if (account != null)
            {
                Account acc = new Account
                {
                    NIK = account.NIK,
                    Email = account.Email,
                    Password = account.Password,
                    OTP = ThreadSafeRandom.Instance.Next(100000, 999999),
                    IsUsed = false
                };
                Update(acc);

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Overtime System", "kaibee3333@gmail.com"));
                message.To.Add(MailboxAddress.Parse(email));
                message.Subject = $"Change Password OTP ({acc.OTP})";
                message.Body = new TextPart("Plain") { Text = $"Kode OTP : {acc.OTP}" };

                SmtpClient smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 465, true);
                smtp.Authenticate("kaibee3333@gmail.com", "Kaibe333");
                smtp.Send(message);
                smtp.Disconnect(true);

                return 1;
            }
            return 0;
        }

        public string userNIK(string email)
        {
            return _context.Accounts.SingleOrDefault(e => e.Email == email).NIK;
        }

        public List<string> UserRole(string email)
        {
            var checkNIK = _context.Accounts.SingleOrDefault(e => e.Email == email).NIK;

            var checkRole = _context.AccountRoles
                .Where(ar => ar.AccountNIK == checkNIK)
                .Join(_context.Roles, ar => ar.RoleId, r => r.Id, (ar, r) => r.Name)
                .ToList();

            return checkRole;
        }

        public int Login(LoginVM login)
        {
            var checkAccount = _context.Accounts.Select(a => new
            {
                Email = a.Email,
                Password = a.Password
            }).SingleOrDefault(e => e.Email == login.Email);

            if (checkAccount != null)
            {
                return ValidatePassword(login.Password, checkAccount.Password)
                    ? 2
                    : 1;
            }
            return 0;
        }

        public IEnumerable MasterEmployeeDataId(string id)
        {

            return _context.Employees
                .Join(_context.Accounts, e => e.NIK, a => a.NIK,
                (e, a) => new { e, a })
                .Join(_context.Jobs, ea => ea.e.JobId, j => j.Id,
                (ea, j) => new
                {
                    NIK = ea.e.NIK,
                    FullName = (ea.e.FirstName + " " + ea.e.LastName),
                    Phone = ea.e.Phone,
                    Gender = ea.e.Gender.ToString(),
                    Email = ea.a.Email,
                    Salary = ea.e.Salary,
                    JobTitle = j.Title,
                }).Where(w => w.NIK == id);
        }

        public IEnumerable MasterEmployeeData()
        {

            return _context.Employees
                .Join(_context.Accounts, e => e.NIK, a => a.NIK,
                (e, a) => new { e, a })
                .Join(_context.Jobs, ea => ea.e.JobId, j => j.Id,
                (ea, j) => new
                {
                    NIK = ea.e.NIK,
                    FullName = (ea.e.FirstName + " " + ea.e.LastName),
                    Phone = ea.e.Phone,
                    Gender = ea.e.Gender.ToString(),
                    Email = ea.a.Email,
                    Salary = ea.e.Salary,
                    JobTitle = j.Title,
                });
        }

        public int Register(RegisterVM register)
        {
            var Year = DateTime.Now.Year;
            var empCount = _context.Employees.OrderByDescending(e => e.NIK).FirstOrDefault();

            if (empCount == null)
            {
                register.NIK = Year + "00" + 1;
            }
            else
            {
                register.NIK = Convert.ToString(Convert.ToInt32(empCount.NIK) + 1);
            }

            if (CheckEmailPhone(register.Email, register.Phone))
            {
                Employee emp = new Employee
                {
                    NIK = register.NIK,
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Gender = register.Gender,
                    Phone = register.Phone,
                    Salary = register.Salary,
                    JobId = register.JobId
                };
                _context.Employees.Add(emp);

                _context.Accounts.Add(new Account
                {
                    NIK = emp.NIK,
                    Email = register.Email,
                    Password = HashPassword(register.Password)
                });
                _context.SaveChanges();

                _context.AccountRoles.Add(new AccountRole
                {
                    AccountNIK = emp.NIK,
                    RoleId = 1
                });
                _context.SaveChanges();

                return 1;
            }
            return 0;
        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
        }

        private static bool ValidatePassword(string password, string correctHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, correctHash);
        }

        private bool CheckEmailPhone(string email, string phone)
        {
            return _context.Employees.Where(e => e.Phone == phone).SingleOrDefault() == null && _context.Accounts.Where(a => a.Email == email).SingleOrDefault() == null;
        }
    }
}
