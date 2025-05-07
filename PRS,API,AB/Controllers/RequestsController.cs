using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRSBackendAB.models;
using PRSBackendAB.Models;


namespace PRSBackendAB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController(PrsDbContext context) : ControllerBase
    {
        private readonly PrsDbContext _context = context;

        // GET: Get All
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
            return await _context.Requests.ToListAsync();
        }

        // GET: Get by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        // PUT: update 

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {

            if (id != request.ID)
            {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: add a request

        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest([FromBody] RequestForm requestForm)
        {
           
            var userExists = await _context.Users.AnyAsync(u => u.ID == requestForm.UserID);
            if (!userExists)
            {
                return BadRequest("Invalid UserID. The specified user does not exist.");
            }

          
            string nextRequestNumber = getNextRequestNumber();

            
            var request = new Request
            {
                UserID = requestForm.UserID,
                Description = requestForm.Description,
                Justification = requestForm.Justification,
                DateNeeded = requestForm.DateNeeded,
                DeliveryMode  ="Pickup",
                RequestNumber = nextRequestNumber 
            };

          
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

         
            return CreatedAtAction("GetRequest", new { id = request.ID }, request);
        }

        // DELETE: Deleted request
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.ID == id);
        }
        [HttpPut("submit-review/{id}")] //submit for review auto approve 50 or less // Find the request by ID // Update the status based on the total// Update the submitted date to the current date
        public async Task<IActionResult> SubmitRequestForReview(int id)
        {

            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }


            if (request.Total <= 50)
            {
                request.Status = "APPROVED";
            }
            else
            {
                request.Status = "REVIEW";
            }


            request.SubmittedDate = DateTime.Now;


            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(request);
        }
        [HttpGet("list-review/{userId}")] //review by user id
        public async Task<ActionResult<IEnumerable<Request>>> GetRequestsForReview(int userId)
        {

            var requests = await _context.Requests
                                         .Where(r => r.Status == "REVIEW" && r.UserID != userId)
                                         .ToListAsync();


            return Ok(requests);
        }
        [HttpPut("approve/{id}")]// approv
        public async Task<IActionResult> ApproveRequest(int id)
        {
            // Find the request by ID
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            // Set the status to "APPROVED"
            request.Status = "APPROVED";

            // Save changes to the database
            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Return the updated request
            return Ok(request);
        }
        [HttpPut("reject/{id}")]// reject
        public async Task<IActionResult> RejectRequest(int id)
        {
            // Find the request by ID
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            // Set the status to "APPROVED"
            request.Status = "REJECTED";

            // Save changes to the database
            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Return the updated request
            return Ok(request);
        }
        private string getNextRequestNumber()
        {
            // requestNumber format: RYYMMDD####

            string requestNbr = "R";

            // Add YYMMDD string
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            requestNbr += today.ToString("yyMMdd");

            // Get maximum request number from db
            string? maxReqNbr = _context.Requests
                                        .Where(r => r.RequestNumber.StartsWith(requestNbr))
                                        .Max(r => r.RequestNumber);

            string reqNbr;
            if (!string.IsNullOrEmpty(maxReqNbr))
            {
                // Extract last 4 characters and increment
                string tempNbr = maxReqNbr.Substring(7);
                if (int.TryParse(tempNbr, out int nbr))
                {
                    nbr++;
                    reqNbr = nbr.ToString().PadLeft(4, '0');
                }
                else
                {
                    reqNbr = "0001";
                }
            }
            else
            {
                reqNbr = "0001";
            }

            requestNbr += reqNbr;
            return requestNbr;
        }
    }
}