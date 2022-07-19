using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Task12.Services;
using Task12.ViewModels;
using Task12.Dto;

namespace Task12.Controllers
{
    public class FinanceTypeController : Controller
    {
        private readonly IFinanceTypeService _service;

        public FinanceTypeController(IFinanceTypeService service)
        {
            _service = service;
        }


        [HttpGet("types")]
        public async Task<IActionResult> GetFinanceTypesAsync()
        {
            List<FinanceTypeViewModel> list = await _service.GetListTypesAsync();

            if (list.Any())
            {
                return Ok(list);
            }
            return NotFound(new { message = "Missing any finance types" });
        }


        [HttpGet("types/expences")]
        public async Task<IActionResult> GetExpencesTypesAsync()
        {
            List<FinanceTypeViewModel> list = await _service.GetExpencesTypesAsync();

            if (list.Any())
            {
                return Ok(list);
            }
            return NotFound(new { message = "Missing expences types" });
        }


        [HttpGet("types/income")]
        public async Task<IActionResult> GetIncomeTypesAsync()
        {
            List<FinanceTypeViewModel> list = await _service.GetIncomeTypesAsync();

            if (list.Any())
            {
                return Ok(list);
            }
            return NotFound(new { message = "Missing income types" });
        }


        [HttpPost("types/add")]
        public async Task<IActionResult> AddTypeAsync([FromBody] FinanceTypeCreateDto newFinanceType)
        {
            return Ok(await _service.AddFinanceTypeAsync(newFinanceType));
        }


        [HttpDelete("types/delete/{id}")]
        public async Task<IActionResult> DeleteFinancetypeAsync([FromRoute]int id)
        {
            bool res = await _service.DeleteFinanceTypeAsync(id);
            if (res)
            {
                return Ok();
            }
            return BadRequest(new { message = "Delete failed" });
        }


        [HttpPut("types/change")]
        public async Task<IActionResult> ChangeFinanceTypeAsync([FromBody] FinanceTypeUpdateDto newFinanceType)
        {
            bool res = await _service.UpdateFinanceTypeAsync(newFinanceType);
            if (res)
            {
                return Ok();
            }
            return BadRequest(new { message = "Type not found" });
        }

    }
}