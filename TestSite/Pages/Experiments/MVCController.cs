using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestSite.Pages.Experiments
{
    [Route("test/[controller]/[action]")]
    public class MVCController : Controller
    {
        public int Test (int id, int id2)
        {
            return id2;
        }
    }
}