using Business.Abstract;
using Entities.DTOs.CategoryDTOs;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpPost]
        public IActionResult Create(List<AddCategoryDTO> models)
        {
            _categoryService.Create(models);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id,[FromBody]List<UpdateCategoryDTO> models)
        {
            await _categoryService.Update(id, models);
            return Ok();
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateByLang(Guid id,string LangCode,UpdateCategoryDTO model)
        {
            await _categoryService.UpdateByLang(id,LangCode,model);

            return Ok();
        }
        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            _categoryService.Delete(id);
            return Ok();
        }
        [HttpGet("{id}")]
        public IActionResult GetByLang(Guid id)
        {
            string LangCode = Request.Headers.AcceptLanguage;
            if(LangCode !="az"||LangCode!="en-US"||LangCode!="ru-RU")
            {
               var res= _categoryService.GetByLang(id, "az");
                return Ok(res);
            }
            var result= _categoryService.GetByLang(id,LangCode);
            return Ok(result);
        }
        [HttpGet("[action]")]
        public IActionResult GetAll(Guid id)
        {
         
            var result=_categoryService.GetAllLanguages(id);
            return Ok(result);
        }
    }
}
