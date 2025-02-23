using Asp.Versioning;
using Business.Abstract;
using Entities.DTOs.BrandDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        public readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }
        [HttpPost]
        [MapToApiVersion("1.0")]
        public IActionResult Create( AddBrandDTO model)
        {
            var result = _brandService.Create(model);
            if (result.Success)
            
                return Ok(result);
            
            return BadRequest(result);
        }
        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        public IActionResult Get(Guid id) {
            var result = _brandService.GetBrands(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("{id}")]
        [MapToApiVersion("1.0")]
        public IActionResult Update(Guid id, UpdateBrandDTO model)
        {
            model.Id = id;
            var result = _brandService.Update(id, model);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPatch("{id}")]
        [MapToApiVersion("1.0")]
        public IActionResult SoftDelete(Guid id)
        {
            var result = _brandService.SoftDelete(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
