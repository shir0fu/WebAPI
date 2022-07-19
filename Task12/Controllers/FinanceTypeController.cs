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
        public async Task<IActionResult> GetFinanceTypes()
        {
            List<FinanceTypeViewModel> json = await _service.GetFinanceTypes();

            if (json.Count != 0)
            {
                return Ok(json);
            }
            return NotFound(new { message = "Missing any finance types" });
        }


        [HttpGet("types/expences")]
        public async Task<IActionResult> GetExpencesTypes()
        {
            List<FinanceTypeViewModel> json = await _service.GetExpencesTypes();

            if (json.Count != 0)
            {
                return Ok(json);
            }
            return NotFound(new { message = "Missing expences types" });
        }


        [HttpGet("types/income")]
        public async Task<IActionResult> GetIncomeTypes()
        {
            List<FinanceTypeViewModel> json = await _service.GetIncomeTypes();

            if (json.Count != 0)
            {
                return Ok(json);
            }
            return NotFound(new { message = "Missing income types" });
        }


        [HttpPost("types/add")]
        public async Task<IActionResult> AddType([FromBody] FinanceTypeCreateDto newFinanceType)
        {
            return Ok(await _service.AddFinanceType(newFinanceType));
        }


        [HttpDelete("types/delete/{id}")]
        public async Task<IActionResult> DeleteFinancetype([FromRoute]int id)
        {
            bool res = await _service.DeleteFinanceType(id);
            if (res)
            {
                return Ok(new { message = "Deleted succesfully" });
            }
            return NotFound(new { message = "Delete failed" });
        }


        [HttpPut("types/change")]
        public async Task<IActionResult> ChangeFinanceType([FromBody] FinanceTypeUpdateDto newFinanceType)
        {
            List<FinanceTypeViewModel> json = await _service.UpdateFinanceType(newFinanceType);
            if (json.Count != 0)
            {
                return Ok(json);
            }
            return NotFound(new { message = "Type not found" });
        }

    }
}