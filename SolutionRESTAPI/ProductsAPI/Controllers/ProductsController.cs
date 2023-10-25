using ProductsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProductsAPI.Controllers
{
    public class ProductsController : ApiController
    {
        List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1},
            new Product { Id = 2, Name = "Yo-Yo", Category = "Toys", Price = 3.75M},
            new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M},
        };

        // GET: api/Products
        [Route("api/Products")]
        public IEnumerable<Product> GetAllProducts()
        {
            return products;
        }

        // GET: api/Products/5
        [Route("api/Products/{id}")]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            Product product = products.FirstOrDefault(X => X.Id == id);

            if (product == null)
                return NotFound();
            
            return Ok(product);
        }

        // GET: api/Products/Toys
        [Route("api/Products/{category}")]
        public IHttpActionResult Get(string category)
        {
            var product = products.Where(x => x.Equals(category)).ToList();

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // POST: api/Products
        [Route("api/Products")]
        public IHttpActionResult Post([FromBody]string value)
        {
            return Ok();
        }

        // PUT: api/Products/5
        [Route("api/Products/{id:int}")]
        public IHttpActionResult Put(int id, [FromBody]string value)
        {
            return Ok();
        }

        // DELETE: api/Products/5
        [Route("api/Products/{id}")]
        public IHttpActionResult Delete(int id)
        {
            return NotFound();
        }
    }
}
