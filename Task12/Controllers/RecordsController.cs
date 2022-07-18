using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Task12.Services;

namespace Task12.Controllers
{
    public class RecordsController : Controller
    {
        private readonly IRecordFinanceService _service;

        public RecordsController(IRecordFinanceService service)
        {
            _service = service;
        }


        [HttpGet("records")]
        public IActionResult GetAllRecords()
        {
            var json = _service.GetAllRecords();

            if (json != null)
            {
                return Json(json);
            }
            return NotFound(new { message = "Records not found" });
        }


        [HttpPost("records/add")]
        public IActionResult AddRecord([FromBody] JsonDocument newRecord)
        {
            return Json(_service.AddRecord(newRecord));
        }


        [HttpDelete("records/delete/{id}")]
        public IActionResult DeleteRecord(int id)
        {
            var json = _service.DeleteRecord(id);
            if (json != null)
            {
                return Json(json);
            }
            return NotFound(new { message = "Record not found" });
        }


        [HttpPut("records/change")]
        public IActionResult ChangeRecord([FromBody] JsonDocument newRecord)
        {
            var json = _service.UpdateRecord(newRecord);
            if (json != null)
            {
                return Json(json);
            }
            return NotFound(new { message = "Record not found" });
        }


        [HttpGet("/records/daily/list")]
        public IActionResult DayReport([FromQuery] string date)
        {
            var json = _service.CurrentDateReport(date);
            if (json != null)
            {
                return Json(json);
            }
            return NotFound(new { message = "Incorrect date, or records for this date not found" });
        }


        [HttpGet("/records/daily/total")]
        public IActionResult DayTotal([FromQuery] string date)
        {
            var json = _service.CurrentDateTotals(date);
            if (json != null)
            {
                return Json(json);
            }
            return NotFound(new { message = "Incorrect date, or records for this date not found" });
        }


        [HttpGet("/records/range/list")]
        public IActionResult RangeReport([FromQuery] string startDate, [FromQuery] string endDate)
        {
            var json = _service.RangeDateReport(startDate, endDate);
            if (json != null)
            {
                return Json(json);
            }
            return NotFound(new { message = "Incorrect dates, or records for this dates not found" });
        }


        [HttpGet("/records/range/total")]
        public IActionResult RangeTotal([FromQuery] string startDate, [FromQuery] string endDate)
        {
            var json = _service.RangeDateTotals(startDate, endDate);
            if (json != null)
            {
                return Json(json);
            }
            return NotFound(new { message = "Incorrect dates, or records for this dates not found" });
        }

    }
}
