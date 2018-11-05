using QuotesApi.Data;
using QuotesApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace QuotesApi.Controllers
{
    
    public class QuotesController : ApiController
    {
        QuotesDbContext quotesDbContext = new QuotesDbContext();
        // GET: api/Quotes
       
       [HttpGet]
        public IHttpActionResult LoadQuotes(string sort)
        {
            IQueryable<Quote> quotes;

            switch (sort)
            {
                case "desc":
                    quotes = quotesDbContext.Quotes.OrderByDescending(q => q.CreatedAt);
                    break;
                case "asc":
                    quotes = quotesDbContext.Quotes.OrderBy(q => q.CreatedAt);
                    break;
                default:
                    quotes = quotesDbContext.Quotes;
                    break;
            }
            //var quote =  quotesDbContext.Quotes;
            return Ok(quotes);
     
        }
        [HttpGet]
        [Route("api/Quotes/Test/{id}")]
        public int Test(int id)
        {
            return id;
        }
        // GET: api/Quotes/5
        [HttpGet]
        public IHttpActionResult LoadQuote(int id)
        {
            var quote = quotesDbContext.Quotes.Find(id);
            if (quote == null)
            {
                return NotFound();
            }
            return Ok(quote);
        }

        // POST: api/Quotes
        public IHttpActionResult Post([FromBody]Quote quote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            quotesDbContext.Quotes.Add(quote);
            quotesDbContext.SaveChanges();
            return StatusCode(HttpStatusCode.Created);
        }

        // PUT: api/Quotes/5
        public IHttpActionResult Put(int id, [FromBody]Quote quote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = quotesDbContext.Quotes.FirstOrDefault(q => q.Id == id);
            // checks to see if a bad request has been found
            if (entity == null)
            {
                return BadRequest("No record has been found for this ID");
            }
            entity.Title = quote.Title;
            entity.Author = quote.Author;
            entity.Description = quote.Description;

            quotesDbContext.SaveChanges();
            return Ok("Record updated succcessfully...!");
        }

        // DELETE: api/Quotes/5
        public IHttpActionResult Delete(int id)
        {
            var quote = quotesDbContext.Quotes.Find(id);
            if (quote == null)
            {
                return BadRequest("No ID has been found to delete!");
            }
            quotesDbContext.Quotes.Remove(quote);
            quotesDbContext.SaveChanges();
            return Ok("Record Deleted succcessfully...!");
        }
    }
}
