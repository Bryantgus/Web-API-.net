using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUDconSQL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDconSQL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.Customers)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    CustomersId = o.CustomersId,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount,
                    DateCreated = o.DateCreated,
                    DateUpdated = o.DateUpdated,
                    Customers = o.Customers != null ? new CustomerDto
                    {
                        Id = o.Customers.Id,
                        FirstName = o.Customers.FirstName,
                        LastName = o.Customers.LastName,
                        DateCreated = o.Customers.DateCreated,
                        DateUpdated = o.Customers.DateUpdated
                    } : null
                })
                .ToListAsync();

            return orders;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Customers)
                .Where(o => o.Id == id)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    CustomersId = o.CustomersId,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount,
                    DateCreated = o.DateCreated,
                    DateUpdated = o.DateUpdated,
                    Customers = o.Customers != null ? new CustomerDto
                    {
                        Id = o.Customers.Id,
                        FirstName = o.Customers.FirstName,
                        LastName = o.Customers.LastName,
                        DateCreated = o.Customers.DateCreated,
                        DateUpdated = o.Customers.DateUpdated
                    } : null
                })
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> PostOrder([FromBody] CreateOrderDto orderDto)
        {
            var customerExists = await _context.Customers.AnyAsync(c => c.Id == orderDto.CustomersId);
            if (!customerExists)
            {
                return BadRequest("El Customer especificado no existe");
            }

            var order = new Order
            {
                CustomersId = orderDto.CustomersId,
                Status = orderDto.Status,
                TotalAmount = orderDto.TotalAmount,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };
            
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            await _context.Entry(order).Reference(o => o.Customers).LoadAsync();

            var orderResponse = new OrderDto
            {
                Id = order.Id,
                CustomersId = order.CustomersId,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                DateCreated = order.DateCreated,
                DateUpdated = order.DateUpdated,
                Customers = order.Customers != null ? new CustomerDto
                {
                    Id = order.Customers.Id,
                    FirstName = order.Customers.FirstName,
                    LastName = order.Customers.LastName,
                    DateCreated = order.Customers.DateCreated,
                    DateUpdated = order.Customers.DateUpdated
                } : null
            };

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, orderResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, [FromBody] CreateOrderDto orderDto)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var customerExists = await _context.Customers.AnyAsync(c => c.Id == orderDto.CustomersId);
            if (!customerExists)
            {
                return BadRequest("El Customer especificado no existe");
            }

            order.CustomersId = orderDto.CustomersId;
            order.Status = orderDto.Status;
            order.TotalAmount = orderDto.TotalAmount;
            order.DateUpdated = DateTime.Now;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}