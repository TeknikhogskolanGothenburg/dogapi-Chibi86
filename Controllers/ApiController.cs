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
        // GET api/
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var files = System.IO.Directory.GetFiles("DogFiles", "*.json");
            List<Dog> dogs = new List<Dog>();
            foreach (string file in files)
            {
                Dog dog = JsonConvert.DeserializeObject<Dog>(System.IO.File.ReadAllText(file));
                dogs.Add(dog);
            }

            return dogs.Select(d => d.Id).ToArray();
        }

        // GET api/[id]
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

        // POST api/
        [HttpPost]
        public void Post([FromBody]Dog dog)
        {
            if ((dog != null) && !String.IsNullOrEmpty(dog.BreedName) && !String.IsNullOrEmpty(dog.WikipediaUrl) && !String.IsNullOrEmpty(dog.Description))
            {
                dog.Id = dog.BreedName;

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

        // PUT api/[id]
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] Dog dog)
        {
            if ((dog != null) && !String.IsNullOrEmpty(dog.BreedName) && !String.IsNullOrEmpty(dog.WikipediaUrl) && !String.IsNullOrEmpty(dog.Description))
            {
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

        // DELETE api/
        [HttpDelete]
        public void Delete()
        {
            var files = System.IO.Directory.GetFiles("DogFiles", "*.json");

            foreach (string file in files)
            {
                try
                {
                    System.IO.File.Delete(file);
                }
                catch
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
            }
        }

        // DELETE api/[id]
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
