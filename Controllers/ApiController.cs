using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DogApi.Models;
using System.Net;

namespace DogApi.Controllers
{
    [Route("[controller]")]
    public class ApiController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var files = System.IO.Directory.GetFiles("DogFiles", "*.json");
            List<Dog> dogs = new List<Dog>();
            foreach (var file in files)
            {
                dogs.Add(JsonConvert.DeserializeObject<Dog>(System.IO.File.ReadAllText(file)));
            }

            return dogs.Select(d => d.BreedName).ToArray();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            string src = "DogFiles/" + id + ".json";
            Dog dog = null;

            if (System.IO.File.Exists(src))
            {
                try
                {
                    dog = (JsonConvert.DeserializeObject<Dog>(System.IO.File.ReadAllText(src)));
                    return new ObjectResult(dog);
                }
                catch
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
            }

            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return null;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Dog dog)
        {
            if ((dog != null) && !String.IsNullOrEmpty(dog.BreedName) && !String.IsNullOrEmpty(dog.WikipediaUrl) && !String.IsNullOrEmpty(dog.Description))
            {
                string src = "DogFiles/" + dog.BreedName + ".json";

                if (!System.IO.File.Exists(src))
                {
                    try
                    {
                        System.IO.File.WriteAllText(src, (JsonConvert.SerializeObject(dog)));
                        return;
                    }
                    catch
                    {
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        return;
                    }
                }

                Response.StatusCode = (int)HttpStatusCode.Conflict;
                return;
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] Dog dog)
        {
            if ((dog != null) && !String.IsNullOrEmpty(dog.BreedName) && !String.IsNullOrEmpty(dog.WikipediaUrl) && !String.IsNullOrEmpty(dog.Description))
            {
                //Dog dog = new Dog { BreedName = breedName, WikipediaUrl = wikipediaUrl, Description = description };
                string src = "DogFiles/" + id + ".json";

                if (System.IO.File.Exists(src))
                {
                    try
                    {
                        System.IO.File.WriteAllText(src, (JsonConvert.SerializeObject(dog)));
                        return;
                    }
                    catch
                    {
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        return;
                    }
                }

                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return;
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            string src = "DogFiles/" + id + ".json";

            if (System.IO.File.Exists(src))
            {
                try
                {
                    System.IO.File.Delete(src);
                }
                catch
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
        }
    }
}
