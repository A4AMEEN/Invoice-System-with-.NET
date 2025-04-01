using Microsoft.AspNetCore.Mvc;
using myWebApi.Models;
using myWebApi.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace myWebApi.Controllers
{
    [ApiController]
    [Route("api/invoices")]
    public class InvoiceController : ControllerBase
    {
        private readonly MongoDbService _mongoDbService;

        public InvoiceController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        [HttpGet]

        public IActionResult GetInvoices()
        {
            Console.WriteLine("Came here");
            var invoices = new[]
            {
                new { Id = 1, Customer = "John Doe", Amount = 500 },
                new { Id = 2, Customer = "Jane Smith", Amount = 1500 }
            };

            return Ok(invoices);

        }

        [HttpGet("dicount-rates")]
        public IActionResult GetDiscountRates()
        {
            Console.WriteLine("Getting discount rates");
            var discountRates = new[]
            {
                new { MinAmount = 5000, DiscountPercent = 0.05 },
                new { MinAmount = 15000, DiscountPercent = 0.10 },
                new { MinAmount = 50000, DiscountPercent = 0.15 }
            };

            return Ok(discountRates);
        }

        [HttpGet("customers")]
        public IActionResult GetCustomers()
        {
            Console.WriteLine("Getting customers");
            var customers = new[]
            {
                new {
                    Id = "1",
                    Name = "Acme Corporation",
                    Email = "billing@acme.com",
                    Address = "123 Business Street, New York, NY 10001",
                },
                new {
                    Id = "2",
                    Name = "Tech Innovations Inc.",
                    Email = "finance@techinnovations.com",
                    Address = "456 Tech Park, San Francisco, CA 94105",
                },
                new {
                    Id = "3",
                    Name = "Global Enterprises",
                    Email = "contact@globalenterprises.com",
                    Address = "789 Corporate Blvd, Chicago, IL 60601",
                },
                new {
                    Id = "4",
                    Name = "NextGen Solutions",
                    Email = "info@nextgensolutions.com",
                    Address = "321 Future Lane, Austin, TX 73301",
                },
                new {
                    Id = "5",
                    Name = "Pioneer Tech",
                    Email = "support@pioneertech.com",
                    Address = "987 Innovation Drive, Seattle, WA 98101",
                }
            };

            return Ok(customers);
        }

        [HttpGet("products")]
        public IActionResult GetProducts()
        {
            Console.WriteLine("Getting products");
            var products = new[]
            {
                new { Id = "1", Name = "Laptop", BasePrice = 89999 },
                new { Id = "2", Name = "Smartphone", BasePrice = 15999 },
                new { Id = "3", Name = "Tablet", BasePrice = 14599 },
                new { Id = "4", Name = "Headphones", BasePrice = 5999 },
                new { Id = "5", Name = "Monitor", BasePrice = 25999 }
            };

            return Ok(products);
        }

        [HttpPost("save-invoice")]
        public async Task<IActionResult> SaveInvoice([FromBody] InvoiceDto invoice)
        {
            Console.WriteLine("Received Save Invoice Request.");

            if (invoice == null)
            {
                Console.WriteLine("Error: Invoice data is null.");
                return BadRequest("Invalid Invoice Data");
            }

            Console.WriteLine($"Saving Invoice: {invoice.CustomerName}");
            Console.WriteLine($"Invoice Items Count: {invoice.Items?.Count ?? 0}");

            try
            {
                await _mongoDbService.CreateAsync(invoice);
                return Ok(new { Message = "Invoice saved successfully", InvoiceId = invoice.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving invoice: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(string id, [FromBody] InvoiceDto invoice)
        {
            if (invoice == null)
            {
                return BadRequest("Invalid Invoice Data");
            }

            var existingInvoice = await _mongoDbService.GetAsync(id);
            if (existingInvoice == null)
            {
                return NotFound($"Invoice with ID {id} not found");
            }

            await _mongoDbService.UpdateAsync(id, invoice);
            return Ok(new { Message = "Invoice updated successfully" });

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(string id)
        {
            var invoice = await _mongoDbService.GetAsync(id);
            if (invoice == null)
            {
                return NotFound($"Invoice with ID {id} not found");
            }

            await _mongoDbService.RemoveAsync(id);
            return Ok(new { Message = "Invoice deleted successfully" });
        }
    }


}