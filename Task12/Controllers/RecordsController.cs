using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Task12.Services;
using Task12.Dto;
using Task12.ViewModels;

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
        public async Task<IActionResult> GetAllRecords()
        {
            List<RecordViewModel> json = await _service.GetAllRecords();

            if (json.Count != 0)
            {
                return Ok(json);
            }
            return NotFound(new { message = "Records not found" });
        }


        [HttpPost("records/add")]
        public async Task<IActionResult> AddRecord([FromBody] RecordCreateDto newRecord)
        {
            return Ok(await _service.AddRecord(newRecord));
        }


        [HttpDelete("records/delete/{id}")]
        public async Task<IActionResult> DeleteRecord(int id)
        {
            bool res = await _service.DeleteRecord(id);
            if (res)
            {
                return Ok(new { message = "Record deleted" });
            }
            return NotFound(new { message = "Delete failed" });
        }


        [HttpPut("records/change")]
        public async Task<IActionResult> ChangeRecord([FromBody] RecordUpdateDto newRecord)
        {
            List<RecordViewModel> json = await _service.UpdateRecord(newRecord);
            if (json.Count != 0)
            {
                return Ok(json);
            }
            return NotFound(new { message = "Record not found" });
        }


        [HttpGet("/records/daily/list")]
        public async Task<IActionResult> DayReport([FromQuery] string date)
        {
            List<RecordViewModel> json = await _service.CurrentDateReport(date);
            if (json.Count != 0)
            {
                return Ok(json);
            }
            return NotFound(new { message = "Incorrect date, or records for this date not found" });
        }


        [HttpGet("/records/daily/total")]
        public async Task<IActionResult> DayTotal([FromQuery] string date)
        {
            var json = await _service.CurrentDateTotals(date);
            if (json != null)
            {
                return Ok(json);
            }
            return NotFound(new { message = "Incorrect date, or records for this date not found" });
        }


        [HttpGet("/records/range/list")]
        public async Task<IActionResult> RangeReport([FromQuery] string startDate, [FromQuery] string endDate)
        {
            List<RecordViewModel> json = await _service.RangeDateReport(startDate, endDate);
            if (json.Count != 0)
            {
                return Ok(json);
            }
            return NotFound(new { message = "Incorrect dates, or records for this dates not found" });
        }


        [HttpGet("/records/range/total")]
        public async Task<IActionResult> RangeTotal([FromQuery] string startDate, [FromQuery] string endDate)
        {
            var json = await _service.RangeDateTotals(startDate, endDate);
            if (json != null)
            {
                return Ok(json);
            }
            return NotFound(new { message = "Incorrect dates, or records for this dates not found" });
        }

    }
}
