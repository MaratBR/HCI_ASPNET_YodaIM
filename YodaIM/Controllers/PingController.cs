using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace YodaIM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        private static string[] YODA_QUOTES =
        {
            "Do or do not. There is no try",
            "You must unlearn what you have learned.",
            "Named must be your fear before banish it you can.",
            "Fear is the path to the dark side. Fear leads to anger. Anger leads to hate. Hate leads to suffering.",
            "That is why you fail.",
            "The greatest teacher, failure is.",
            "Pass on what you have learned.",
            "May the Force be with you",
            "Once you start down the dark path, forever will it dominate your destiny. Consume you, it will.",
            "Patience you must have my young Padawan.",
            "In a dark place we find ourselves, and a little more knowledge lights our way.",
            "Truly wonderful the mind of a child is.",
            "When you look at the dark side, careful you must be. For the dark side looks back."
        };
        private readonly Random rnd = new Random();

        [HttpGet]
        public ActionResult Ping()
        {
            string saying = YODA_QUOTES[rnd.Next(0, YODA_QUOTES.Length - 1)];
            return Ok("pong\n" + saying);
        }
    }
}