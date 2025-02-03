using Business.Abstract;
using Entities.DTOs.SubCategoryDTOs;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryService _subCategoryService;

        public SubCategoryController(ISubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }
        [HttpPost]
        public IActionResult Create(AddSubCategoryDTO model)
        {
            var result=_subCategoryService.Create(model);
            if(result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute]Guid id,UpdateSubCategoryDTO model)
        {
            var result = _subCategoryService.Update(id,model);
            if(result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute]Guid id)
        {
            var result=_subCategoryService.Delete(id);
            if(result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute]Guid id)
        {
            var result= _subCategoryService.Get(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
    