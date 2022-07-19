using System.Text.Json;
using Task12.Models;
using Task12.Dto;
using Task12.ViewModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Task12.Services;

public interface IRecordFinanceService
{
    Task<List<RecordViewModel>> AddRecordAsync(RecordCreateDto record);
    Task<bool> UpdateRecordAsync(RecordUpdateDto record);
    Task<bool> DeleteRecordAsync(int id);

    Task<List<RecordViewModel>> RangeDateReportAsync(string startDate, string endDate);
    Task<TotalIncomeOrExpences> RangeDateTotalsAsync(string startDate, string endDate);

    Task<TotalIncomeOrExpences> CurrentDateTotalsAsync(string date);
    Task<List<RecordViewModel>> CurrentDateReportAsync(string date);

    Task<List<RecordViewModel>> GetAllRecordsAsync();
}

public class RecordFinanceService : IRecordFinanceService
{

    MapperConfiguration config = new MapperConfiguration(cfg =>
    cfg.CreateMap<Record, RecordViewModel>()
    );

    ApplicationContext db;
    public RecordFinanceService(ApplicationContext context)
    {
        db = context;
    }


    public async Task<List<RecordViewModel>> AddRecordAsync(RecordCreateDto record)
    {
        Record? newRecord = new Record();

        newRecord.Value = record.Value;
        newRecord.TypeId = record.TypeId;
        newRecord.Description = record.Description;
        newRecord.Date = record.Date;

        await db.Records.AddAsync(newRecord);
        await db.SaveChangesAsync();

        return await GetAllRecordsAsync();
    }


    public async Task<bool> DeleteRecordAsync(int id)
    {
        Record? record = await db.Records.FindAsync(id);

        if (record is null)
        {
            return false;
        }

        db.Records.Remove(record);
        await db.SaveChangesAsync();

        return true;

    }


    public async Task<bool> UpdateRecordAsync(RecordUpdateDto newRecord)
    {
        Record? record = await db.Records.FindAsync(newRecord.Id);

        if (record is null)
        {
            return false;
        }

        record.Value = newRecord.Value;
        record.Description = newRecord.Description;
        record.TypeId = newRecord.TypeId;
        record.Date = newRecord.Date;

        db.Records.Update(record);
        await db.SaveChangesAsync();

        return true;
    }


    public async Task<List<RecordViewModel>> CurrentDateReportAsync(string date)
    {
        bool res = DateTime.TryParse(date, out DateTime comparsionDate);
        if (!res)
        {
            return new List<RecordViewModel>();
        }

        List<Record> records = await db.Records.Where(p => p.Date == comparsionDate).ToListAsync();

        if (!records.Any())
        {
            return new List<RecordViewModel>();
        }


        var mapper = new Mapper(config);
        List<RecordViewModel> recordVM = new List<RecordViewModel>();
        mapper.Map(records, recordVM);

        return recordVM;
    }


    public async Task<TotalIncomeOrExpences> CurrentDateTotalsAsync(string date)
    {
        bool res = DateTime.TryParse(date, out DateTime comparsionDate);
        if (!res)
        {
            return new TotalIncomeOrExpences();
        }

        TotalIncomeOrExpences totalIncomeOrExpences = new TotalIncomeOrExpences();
        List<Record> dailyRecords = await db.Records.Where(p => p.Date == comparsionDate).ToListAsync();

        if (!dailyRecords.Any())
        {
            return new TotalIncomeOrExpences();
        }

        foreach (Record record in dailyRecords)
        {
            if (record.Value < 0)
            {
                totalIncomeOrExpences.ExpenseValue += record.Value;
            }
            else
            {
                totalIncomeOrExpences.IncomeValue += record.Value;
            }
        }

        return totalIncomeOrExpences;
    }


    public async Task<List<RecordViewModel>> GetAllRecordsAsync()
    {
        List<Record> records = await db.Records.ToListAsync();
        if (!records.Any())
        {
            return new List<RecordViewModel>();
        }

        var mapper = new Mapper(config);
        List<RecordViewModel> recordVM = new List<RecordViewModel>();
        mapper.Map(records, recordVM);

        return recordVM;
    }


    public async Task<List<RecordViewModel>> RangeDateReportAsync(string startDate, string endDate)
    {
        bool res = await CheckDates(startDate, endDate);
        if (!res)
        {
            return new List<RecordViewModel>();
        }

        List<Record> records = await db.Records.Where(p => p.Date >= DateTime.Parse(startDate) && p.Date <= DateTime.Parse(endDate)).ToListAsync();

        var mapper = new Mapper(config);
        List<RecordViewModel> recordVM = new List<RecordViewModel>();
        mapper.Map(records, recordVM);

        return recordVM;
    }


    public async Task<TotalIncomeOrExpences> RangeDateTotalsAsync(string startDate, string endDate)
    {

        bool res = await CheckDates(startDate, endDate);
        if (!res)
        {
            return new TotalIncomeOrExpences();
        }

        TotalIncomeOrExpences totalIncomeOrExpences = new TotalIncomeOrExpences();
        List<Record> records = await db.Records.Where(p => p.Date >= DateTime.Parse(startDate) && p.Date <= DateTime.Parse(endDate)).ToListAsync();
        
        foreach (Record record in records)
        {
            if (record.Value < 0)
            {
                totalIncomeOrExpences.ExpenseValue += record.Value;
            }
            else
            {
                totalIncomeOrExpences.IncomeValue += record.Value;
            }
        }

        return totalIncomeOrExpences;
    }

    public async Task<bool> CheckDates(string startDate, string endDate)
    {
        bool res1 = DateTime.TryParse(startDate, out DateTime comparsionDate1);
        bool res2 = DateTime.TryParse(endDate, out DateTime comparsionDate2);
        if (!res1 || !res2)
        {
            return false;
        }

        Record firstRecord = await db.Records.FirstAsync();
        DateTime firstDate = firstRecord.Date;
        List<Record> sortRecords = await db.Records.OrderBy(p => p.Date).ToListAsync();
        Record lastRecord = sortRecords.Last();
        DateTime lastDate = lastRecord.Date;

        if (comparsionDate1 < firstDate || comparsionDate2 > lastDate)
        {
            return false;
        }

        return true;
    }
}
