using System.Text.Json;
using Task12.Models;

namespace Task12.Services;

public interface IRecordFinanceService
{
    List<Record> AddRecord(JsonDocument record);
    List<Record> UpdateRecord(JsonDocument record);
    List<Record> DeleteRecord(int id);

    List<Record> RangeDateReport(string startDate, string endDate);
    TotalIncomeOrExpences RangeDateTotals(string startDate, string endDate);

    TotalIncomeOrExpences CurrentDateTotals(string date);
    List<Record> CurrentDateReport(string date);

    List<Record> GetAllRecords();
}

public class RecordFinanceService : IRecordFinanceService
{
    ApplicationContext db;
    public RecordFinanceService(ApplicationContext context)
    {
        db = context;
    }


    public List<Record> AddRecord(JsonDocument record)
    {
        Record? newRecord = JsonSerializer.Deserialize<Record?>(record);
        db.Records.Add(newRecord);
        db.SaveChanges();

        return GetAllRecords();
    }


    public List<Record> DeleteRecord(int id)
    {
        Record? record = db.Records.Find(id);

        if (record == null)
        {
            return null;
        }

        db.Records.Remove(record);
        db.SaveChanges();

        return GetAllRecords();
    }


    public List<Record> UpdateRecord(JsonDocument newRecord)
    {
        Record? newDbRecord = JsonSerializer.Deserialize<Record>(newRecord);
        Record? record = db.Records.Find(newDbRecord.Id);

        if (record == null)
        {
            return null;
        }

        record.Value = newDbRecord.Value;
        record.Description = newDbRecord.Description;
        record.TypeName = newDbRecord.TypeName;
        record.Date = newDbRecord.Date;
        db.Records.Update(record);
        db.SaveChanges();

        return GetAllRecords();
    }


    public List<Record> CurrentDateReport(string date)
    {
        bool res = DateTime.TryParse(date, out DateTime comparsionDate);
        if (!res)
        {
            return null;
        }

        List<Record> records = db.Records.Where(p => p.Date == comparsionDate).ToList();

        if (records.Count == 0)
        {
            return null;
        }

        return records;
    }


    public TotalIncomeOrExpences CurrentDateTotals(string date)
    {
        bool res = DateTime.TryParse(date, out DateTime comparsionDate);
        if (!res)
        {
            return null;
        }

        TotalIncomeOrExpences totalIncomeOrExpences = new TotalIncomeOrExpences();
        List<Record> dailyRecords = db.Records.Where(p => p.Date == comparsionDate).ToList();

        if (dailyRecords.Count == 0)
        {
            return null;
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


    public List<Record> GetAllRecords()
    {
        List<Record> records = db.Records.ToList();
        if (records.Count == 0)
        {
            return null;
        }

        return records;
    }


    public List<Record> RangeDateReport(string startDate, string endDate)
    {
        bool res = CheckDates(startDate, endDate);
        if (!res)
        {
            return null;
        }

        return db.Records.Where(p => p.Date >= DateTime.Parse(startDate) && p.Date <= DateTime.Parse(endDate)).ToList();
    }


    public TotalIncomeOrExpences RangeDateTotals(string startDate, string endDate)
    {

        bool res = CheckDates(startDate, endDate);
        if (!res)
        {
            return null;
        }

        TotalIncomeOrExpences totalIncomeOrExpences = new TotalIncomeOrExpences();
        List<Record> records = db.Records.Where(p => p.Date >= DateTime.Parse(startDate) && p.Date <= DateTime.Parse(endDate)).ToList();
        
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

    public bool CheckDates(string startDate, string endDate)
    {
        bool res1 = DateTime.TryParse(startDate, out DateTime comparsionDate1);
        bool res2 = DateTime.TryParse(endDate, out DateTime comparsionDate2);
        if (!res1 || !res2)
        {
            return false;
        }

        Record firstRecord = db.Records.First();
        DateTime firstDate = firstRecord.Date;
        List<Record> sortRecords = db.Records.OrderBy(p => p.Date).ToList();
        Record lastRecord = sortRecords.Last();
        DateTime lastDate = lastRecord.Date;

        if (comparsionDate1 < firstDate || comparsionDate2 > lastDate)
        {
            return false;
        }

        return true;
    }
}
