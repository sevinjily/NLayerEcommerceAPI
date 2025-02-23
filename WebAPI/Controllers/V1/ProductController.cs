using Asp.Versioning;
using BenchmarkDotNet.Attributes;
using Business.Abstract;
using Entities.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace WebAPI.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Create(AddProductDTO product)
        {
            var result=await _productService.CreateAsync(product);
            return Ok(result);

            }
        [HttpPut("{id}/[action]")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Update(Guid id, UpdateProductDTO product)
        {
            var result = await _productService.UpdateAsync(id, product);
            if(result.Success)
            
                return Ok(result);
            return BadRequest(result);

        }
        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetById(Guid id)
        {
            var lang=Thread.CurrentThread.CurrentCulture.Name;
            var result = _productService.GetById(id, lang);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
