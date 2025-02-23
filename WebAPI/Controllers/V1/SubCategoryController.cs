using Asp.Versioning;
using Business.Abstract;
using Entities.DTOs.SubCategoryDTOs;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryService _subCategoryService;

        public SubCategoryController(ISubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }
        [HttpPost]
        [MapToApiVersion("1.0")]
        public IActionResult Create(AddSubCategoryDTO model)
        {
            var result=_subCategoryService.Create(model);
            if(result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("{id}")]
        [MapToApiVersion("1.0")]
        public IActionResult Update([FromRoute]Guid id,UpdateSubCategoryDTO model)
        {
            var result = _subCategoryService.Update(id,model);
            if(result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpDelete("{id}")]
        [MapToApiVersion("1.0")]
        public IActionResult Delete([FromRoute]Guid id)
        {
            var result=_subCategoryService.Delete(id);
            if(result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        public IActionResult Get([FromRoute]Guid id)
        {
            var result= _subCategoryService.Get(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
    