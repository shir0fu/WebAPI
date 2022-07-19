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
        public async Task<IActionResult> GetAllRecordsAsync()
        {
            List<RecordViewModel> list = await _service.GetAllRecordsAsync();

            if (list.Any())
            {
                return Ok(list);
            }
            return NotFound(new { message = "Records not found" });
        }


        [HttpPost("records/add")]
        public async Task<IActionResult> AddRecordAsync([FromBody] RecordCreateDto newRecord)
        {
            return Ok(await _service.AddRecordAsync(newRecord));
        }


        [HttpDelete("records/delete/{id}")]
        public async Task<IActionResult> DeleteRecordAsync(int id)
        {
            bool res = await _service.DeleteRecordAsync(id);
            if (res)
            {
                return Ok();
            }
            return BadRequest(new { message = "Delete failed" });
        }


        [HttpPut("records/change")]
        public async Task<IActionResult> ChangeRecordAsync([FromBody] RecordUpdateDto newRecord)
        {
            bool res = await _service.UpdateRecordAsync(newRecord);
            if (res)
            {
                return Ok();
            }
            return BadRequest(new { message = "Record not found" });
        }


        [HttpGet("/records/daily/list")]
        public async Task<IActionResult> DayReportAsync([FromQuery] string date)
        {
            List<RecordViewModel> list = await _service.CurrentDateReportAsync(date);
            if (list.Any())
            {
                return Ok(list);
            }
            return NotFound(new { message = "Incorrect date, or records for this date not found" });
        }


        [HttpGet("/records/daily/total")]
        public async Task<IActionResult> DayTotalAsync([FromQuery] string date)
        {
            var total = await _service.CurrentDateTotalsAsync(date);
            if (total != null)
            {
                return Ok(total);
            }
            return NotFound(new { message = "Incorrect date, or records for this date not found" });
        }


        [HttpGet("/records/range/list")]
        public async Task<IActionResult> RangeReportAsync([FromQuery] string startDate, [FromQuery] string endDate)
        {
            List<RecordViewModel> list = await _service.RangeDateReportAsync(startDate, endDate);
            if (list.Any())
            {
                return Ok(list);
            }
            return NotFound(new { message = "Incorrect dates, or records for this dates not found" });
        }


        [HttpGet("/records/range/total")]
        public async Task<IActionResult> RangeTotalAsync([FromQuery] string startDate, [FromQuery] string endDate)
        {
            var total = await _service.RangeDateTotalsAsync(startDate, endDate);
            if (total != null)
            {
                return Ok(total);
            }
            return NotFound(new { message = "Incorrect dates, or records for this dates not found" });
        }

    }
}
