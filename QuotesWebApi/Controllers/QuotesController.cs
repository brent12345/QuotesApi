using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Cors;
using QuotesWebApi.Models;
using Microsoft.AspNet.Identity;
using WebApi.OutputCache.V2;

namespace QuotesWebApi.Controllers
{
    [Authorize]
    public class QuotesController : ApiController
    {
        ApplicationDbContext quotesDbContext = new ApplicationDbContext();
        // GET: api/Quotes
       [AllowAnonymous]
       [HttpGet]
       [CacheOutput(ClientTimeSpan = 60)]
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
        [Route("api/Quotes/MyQuotes")]
        public IHttpActionResult MyQuotes()
        {
            string userId = User.Identity.GetUserId();
            var quotes = quotesDbContext.Quotes.Where(q =>q.UserID==userId);
            return Ok(quotes);
        }

        [HttpGet]
        [Route("api/Quotes/PagingQuote/{pageNumber=1}/{pageSize=5}")]
        public IHttpActionResult PageingQuote(int pageNumber, int pageSize)
        {
            var quotes = quotesDbContext.Quotes.OrderBy(q => q.Id);
            return Ok(quotes.Skip((pageNumber - 1) * pageSize).Take(pageSize));
        }

        [HttpGet]
        [Route("api/Quotes/SearchQuote/{type=}")]
        public IHttpActionResult SearchQuote (string type)
        {
           var quotes = quotesDbContext.Quotes.Where(q => q.Type.StartsWith(type));
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
            string userid = User.Identity.GetUserId();
            quote.UserID = userid;
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
            string userid = User.Identity.GetUserId();
            var entity = quotesDbContext.Quotes.FirstOrDefault(q => q.Id == id);
          
            // checks to see if a bad request has been found
            if (entity == null)
            {
                return BadRequest("No record has been found for this ID");
            }
            if (userid != entity.UserID)
            {
                return BadRequest("You don't have rights to update this record....");
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
            string userid = User.Identity.GetUserId();

            var quote = quotesDbContext.Quotes.Find(id);
            if (quote == null)
            {
                return BadRequest("No ID has been found to delete!");
            }
            if (userid != quote.UserID)
            {
                return BadRequest("You don't have rights to delete this record....");
            }
            quotesDbContext.Quotes.Remove(quote);
            quotesDbContext.SaveChanges();
            return Ok("Record Deleted succcessfully...!");
        }
    }
}
