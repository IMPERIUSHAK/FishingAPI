using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace listip{


    public class IP{

        public int Id {get; set;}
        public string? Adress{get;  set;}

    }

}