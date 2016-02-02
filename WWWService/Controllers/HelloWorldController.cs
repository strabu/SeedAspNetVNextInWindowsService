using System;
using Microsoft.AspNet.Mvc;

public class HelloWorldController
    {
        [Route("time")]
        public DateTime GetTime()
        {
            return DateTime.Now;
        }
    }