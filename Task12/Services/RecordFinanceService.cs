﻿using System.Text.Json;
using Task12.Models;
using Task12.Dto;
using Task12.ViewModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Task12.Services;

public interface IRecordFinanceService
{
    Task<List<RecordViewModel>> AddRecord(RecordCreateDto record);
    Task<List<RecordViewModel>> UpdateRecord(RecordUpdateDto record);
    Task<bool> DeleteRecord(int id);

    Task<List<RecordViewModel>> RangeDateReport(string startDate, string endDate);
    Task<TotalIncomeOrExpences> RangeDateTotals(string startDate, string endDate);

    Task<TotalIncomeOrExpences> CurrentDateTotals(string date);
    Task<List<RecordViewModel>> CurrentDateReport(string date);

    Task<List<RecordViewModel>> GetAllRecords();
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


    public async Task<List<RecordViewModel>> AddRecord(RecordCreateDto record)
    {
        Record? newRecord = new Record();

        newRecord.Value = record.Value;
        newRecord.TypeId = record.TypeId;
        newRecord.Description = record.Description;
        newRecord.Date = record.Date;

        await db.Records.AddAsync(newRecord);
        await db.SaveChangesAsync();

        return await GetAllRecords();
    }


    public async Task<bool> DeleteRecord(int id)
    {
        Record? record = await db.Records.FindAsync(id);

        if (record == null)
        {
            return false;
        }

        db.Records.Remove(record);
        await db.SaveChangesAsync();

        return true;

    }


    public async Task<List<RecordViewModel>> UpdateRecord(RecordUpdateDto newRecord)
    {
        Record? record = await db.Records.FindAsync(newRecord.Id);

        if (record == null)
        {
            return new List<RecordViewModel>();
        }

        record.Value = newRecord.Value;
        record.Description = newRecord.Description;
        record.TypeId = newRecord.TypeId;
        record.Date = newRecord.Date;

        db.Records.Update(record);
        await db.SaveChangesAsync();

        return await GetAllRecords();
    }


    public async Task<List<RecordViewModel>> CurrentDateReport(string date)
    {
        bool res = DateTime.TryParse(date, out DateTime comparsionDate);
        if (!res)
        {
            return new List<RecordViewModel>();
        }

        List<Record> records = await db.Records.Where(p => p.Date == comparsionDate).ToListAsync();

        if (records.Count == 0)
        {
            return new List<RecordViewModel>();
        }


        var mapper = new Mapper(config);
        List<RecordViewModel> recordVM = new List<RecordViewModel>();
        mapper.Map(records, recordVM);

        return recordVM;
    }


    public async Task<TotalIncomeOrExpences> CurrentDateTotals(string date)
    {
        bool res = DateTime.TryParse(date, out DateTime comparsionDate);
        if (!res)
        {
            return new TotalIncomeOrExpences();
        }

        TotalIncomeOrExpences totalIncomeOrExpences = new TotalIncomeOrExpences();
        List<Record> dailyRecords = await db.Records.Where(p => p.Date == comparsionDate).ToListAsync();

        if (dailyRecords.Count == 0)
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


    public async Task<List<RecordViewModel>> GetAllRecords()
    {
        List<Record> records = await db.Records.ToListAsync();
        if (records.Count == 0)
        {
            return new List<RecordViewModel>();
        }

        var mapper = new Mapper(config);
        List<RecordViewModel> recordVM = new List<RecordViewModel>();
        mapper.Map(records, recordVM);

        return recordVM;
    }


    public async Task<List<RecordViewModel>> RangeDateReport(string startDate, string endDate)
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


    public async Task<TotalIncomeOrExpences> RangeDateTotals(string startDate, string endDate)
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
