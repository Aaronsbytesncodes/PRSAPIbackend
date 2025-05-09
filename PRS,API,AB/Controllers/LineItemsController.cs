using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRSBackendAB.models;

namespace PRSBackendAB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineItemsController(PrsDbContext context) : ControllerBase
    {
        private readonly PrsDbContext _context = context;

        // GET: Retrieve a line item by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<LineItem>> GetLineItem(int id)
        {
            var lineItem = await _context.LineItems.FindAsync(id);

            if (lineItem == null)
            {
                return NotFound();
            }

            return lineItem;
        }

        // PUT: Update a line item and recalculate the associated request's total
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLineItem(int id, LineItem lineItem)
        {
            if (id != lineItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(lineItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await RecalculateRequestTotal(lineItem.RequestID); // Update the request total
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LineItemExists(id))
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

        // POST: Add a new line item and recalculate the associated request's total
        [HttpPost]
        public async Task<ActionResult<LineItem>> PostLineItem([FromBody] LineItem lineItem)
        {
            _context.LineItems.Add(lineItem);
            await _context.SaveChangesAsync();

            // Update the request total after adding the line item
            await RecalculateRequestTotal(lineItem.RequestID);

            return CreatedAtAction("GetLineItem", new { id = lineItem.Id }, lineItem);
        }

        // DELETE: Remove a line item and recalculate the associated request's total
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLineItem(int id)
        {
            var lineItem = await _context.LineItems.FindAsync(id);
            if (lineItem == null)
            {
                return NotFound();
            }

            _context.LineItems.Remove(lineItem);
            await RecalculateRequestTotal(lineItem.RequestID); // Update the request total
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: Retrieve all line items for a specific request
        [HttpGet("lines-for-req/{reqId}")]
        public async Task<ActionResult<IEnumerable<LineItem>>> GetLineItemsForRequest(int reqId)
        {
            var lineItems = await _context.LineItems
                                          .Where(li => li.RequestID == reqId)
                                          .ToListAsync();

            if (lineItems == null || !lineItems.Any())
            {
                return NotFound();
            }

            return Ok(lineItems);
        }

        // Helper method: Check if a line item exists by ID
        private bool LineItemExists(int id)
        {
            return _context.LineItems.Any(e => e.Id == id);
        }

        // Helper method: Recalculate the total for a specific request
        private async Task RecalculateRequestTotal(int requestId)
        {
            var request = await _context.Requests.FindAsync(requestId);
            if (request == null) return;

            var total = await _context.LineItems
                                      .Where(li => li.RequestID == requestId)
                                      .Join(_context.Products,
                                            li => li.ProductID,
                                            p => p.ID,
                                            (li, p) => li.Quantity * p.Price)
                                      .SumAsync();

            request.Total = total;
            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
