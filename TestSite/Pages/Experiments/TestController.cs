﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestSite.Pages.Experiments
{
    [Route("test/[controller]")]
    public class TestController : Controller
    {
        // GET: test/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET test/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST test/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT test/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE test/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}