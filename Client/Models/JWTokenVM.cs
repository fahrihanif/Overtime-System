using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Models
{
    public class JWTokenVM
    {
        public string IdToken { get; set; }
        public string Message { get; set; }
    }
}
