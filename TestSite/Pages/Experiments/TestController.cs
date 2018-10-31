using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TestSite.Pages.Experiments
{
    public class TestController : Controller
    {
        public string Index()
        {
            return "This is just a test";
        }

        public string Test2()
        {
            return "This is test2!";
        }
    }
}