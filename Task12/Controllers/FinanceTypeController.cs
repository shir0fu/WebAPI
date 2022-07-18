using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Task12.Services;

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
        public IActionResult GetFinanceTypes()
        {
            var json = _service.GetFinanceTypes();

            if (json != null)
            {
                return Json(json);
            }
            return NotFound(new { message = "Missing any finance types" });
        }


        [HttpGet("types/expences")]
        public IActionResult GetEpencesTypes()
        {
            var json = _service.GetEpencesTypes();

            if (json != null)
            {
                return Json(json);
            }
            return NotFound(new { message = "Missing expences types" });
        }


        [HttpGet("types/income")]
        public IActionResult GetIncomeTypes()
        {
            var json = _service.GetIncomeTypes();

            if (json != null)
            {
                return Json(json);
            }
            return NotFound(new { message = "Missing income types" });
        }


        [HttpPost("types/add")]
        public IActionResult AddType([FromBody] JsonDocument newFinanceType)
        {
            return Json(_service.AddFinanceType(newFinanceType));
        }


        [HttpDelete("types/delete/{id}")]
        public IActionResult DeleteFinancetype(int id)
        {
            var json = _service.DeleteFinanceType(id);
            if (json != null)
            {
                return Json(json);
            }
            return NotFound(new { message = "Type not found" });
        }


        [HttpPut("types/change")]
        public IActionResult ChangeFinanceType([FromBody] JsonDocument newFinanceType)
        {
            var json = _service.UpdateFinanceType(newFinanceType);
            if (json != null)
            {
                return Json(json);
            }
            return NotFound(new { message = "Type not found" });
        }

    }
}