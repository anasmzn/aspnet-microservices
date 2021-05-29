using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Catalog.Api.Entities;
using Catalog.Api.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> logger;

        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
        {
            this._repository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProductsAsync()
        {
            var products = await _repository.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            var product = await _repository.GetProduct(id);
            if (product == null)
            {
                logger.LogInformation($"product with id {id} not found");
                return NotFound();
            }
            return Ok(product);
        }

        [Route("[action]/{category}", Name = "GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            var products = await _repository.GetProductsByCategory(category);
            return Ok(products);
        }

        [Route("[action]/{name}", Name = "GetProductByName")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByName(string name)
        {
            var products = await _repository.GetProductsByName(name);
            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            await _repository.CreateProduct(product);
            return CreatedAtAction(nameof(CreateProduct), product);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            var success = await _repository.UpdateProduct(product);
            return success ? CreatedAtAction(nameof(CreateProduct), product) : base.BadRequest();
        }
    }
}
