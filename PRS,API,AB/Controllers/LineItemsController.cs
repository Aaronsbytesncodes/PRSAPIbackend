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

  
      
        [HttpGet("{id}")]// get by ID
        public async Task<ActionResult<LineItem>> GetLineItem(int id)
        {
            var lineItem = await _context.LineItems.FindAsync(id);

            if (lineItem == null)
            {
                return NotFound();
            }

            return lineItem;
        }

        // PUT: update and recalctotal
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLineItem( int id, LineItem lineItem)
        {
            if (id != lineItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(lineItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await RecalculateRequestTotal(lineItem.RequestID); // Recalculate total after update
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

        // POST: add new line item and recalctotal
        [HttpPost]
        public async Task<ActionResult<LineItem>> PostLineItem([FromBody] LineItem lineItem)
        {
            _context.LineItems.Add(lineItem);
            await _context.SaveChangesAsync();

            // Recalculate the total for the associated request
            await RecalculateRequestTotal(lineItem.RequestID);

            return CreatedAtAction("GetLineItem", new { id = lineItem.Id }, lineItem);
        }


        // DELETE: delete line item

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLineItem(int id)
        {
            var lineItem = await _context.LineItems.FindAsync(id);
            if (lineItem == null)
            {
                
                return NotFound();
            }
            await RecalculateRequestTotal(lineItem.RequestID);
            _context.LineItems.Remove(lineItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LineItemExists(int id)
        {
            return _context.LineItems.Any(e => e.Id == id);
        }
            
            [HttpGet("lines-for-req/{reqId}")]// Get lineitems for request
        public async Task<ActionResult<IEnumerable<LineItem>>> GetLineItemsForRequest(int reqId)
        {
           
            var lineItems = await _context.LineItems
                                          .Where(li => li.RequestID == reqId)
                                          .ToListAsync();

            // Check if no LineItems were found
            if (lineItems == null || !lineItems.Any())
            {
                return NotFound();
            }

            return Ok(lineItems);
        }
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
