using Newtonsoft.Json.Linq;
using ProductsDatabaseAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProductsDatabaseAPI.Controllers
{
    public class ProductsController : ApiController
    {
        private string connStr = Properties.Settings.Default.connectionString;

        // GET: api/Products
        [Route("api/Products")]
        public IEnumerable<Product> Get()
        {
            List<Product> listOfProducts = new List<Product>();
            string queryString = "SELECT * FROM Products";

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //converter o registo da BD para Product
                        Product p = new Product
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"],
                            Category = (reader["Category"] == DBNull.Value) ? "" : (string)reader["Category"], 
                            Price = (reader["Price"] == DBNull.Value) ? 0 : Convert.ToDecimal(reader["Price"])
                        };

                        listOfProducts.Add(p);
                    }
                }
            }

            return listOfProducts;
        }

        // GET: api/Products/5
        [Route("api/Products/{ID:int}")]
        public IHttpActionResult Get(int id)
        {
            string queryString = "SELECT * FROM Products WHERE Id = @idProd";

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@idProd", id);

                command.Connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        //converter o registo da BD para Product
                        Product p = new Product
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"],
                            Category = (reader["Category"] == DBNull.Value) ? "" : (string)reader["Category"],
                            Price = (reader["Price"] == DBNull.Value) ? 0 : Convert.ToDecimal(reader["Price"])
                        };

                        return Ok(p);
                    }
                }
            }

            return NotFound();
        }

        // POST: api/Products
        [Route("api/Products")]
        public IHttpActionResult Post([FromBody]Product value)
        {
            string queryString = "INSERT INTO Products VALUES (@name, @cat, @price)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.AddWithValue("@name", value.Name);
                    command.Parameters.AddWithValue("@cat", value.Category);
                    command.Parameters.AddWithValue("@price", value.Price);

                    try
                    {
                        command.Connection.Open();

                        int result = command.ExecuteNonQuery();

                        if(result == 0)
                        {
                            return NotFound();
                        }

                        return Ok();
                    }
                    catch (Exception)
                    {
                        return InternalServerError();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
           
        }

        // PUT: api/Products/5
        [Route("api/Products/{ID}")]
        public IHttpActionResult Put(int id, [FromBody]Product value)
        {
            string queryString = "UPDATE Products SET Name=@name, Category=@cat, Price=@price " + "WHERE Id=@idProd";

            try
            {
                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.AddWithValue("@name", value.Name);
                    command.Parameters.AddWithValue("@cat", value.Category);
                    command.Parameters.AddWithValue("@price", value.Price);
                    command.Parameters.AddWithValue("@idProd", id);

                    try
                    {
                        command.Connection.Open();

                        int result = command.ExecuteNonQuery();

                        if (result == 0)
                        {
                            return NotFound();
                        }

                        return Ok();
                    }
                    catch (Exception)
                    {
                        return InternalServerError();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // DELETE: api/Products/5
        [Route("api/Products/{ID}")]
        public IHttpActionResult Delete(int id)
        {
            string queryString = "DELETE FROM Products WHERE Id=@idProd";

            try
            {
                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.AddWithValue("@idProd", id);

                    try
                    {
                        command.Connection.Open();

                        int result = command.ExecuteNonQuery();

                        if (result == 0)
                        {
                            return NotFound();
                        }

                        return Ok();
                    }
                    catch (Exception)
                    {
                        return InternalServerError();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}