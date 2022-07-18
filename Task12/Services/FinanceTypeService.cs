using System.Text.Json;
using Task12.Models;

namespace Task12.Services;

public interface IFinanceTypeService
{
    List<FinanceType> GetFinanceTypes();
    List<FinanceType> GetEpencesTypes();
    List<FinanceType> GetIncomeTypes();

    List<FinanceType> AddFinanceType(JsonDocument financeType);

    List<FinanceType> DeleteFinanceType(int id);

    List<FinanceType> UpdateFinanceType(JsonDocument financeType);
}

public class FinanceTypeService : IFinanceTypeService
{
    ApplicationContext db;
    public FinanceTypeService(ApplicationContext context)
    {
        db = context;
    }


    public List<FinanceType> AddFinanceType(JsonDocument newFinanceType)
    {
        FinanceType? financeType = JsonSerializer.Deserialize<FinanceType>(newFinanceType);

        db.Types.Add(financeType);
        db.SaveChanges();

        return GetFinanceTypes();
    }


    public List<FinanceType> UpdateFinanceType(JsonDocument newFinanceTypeJson)
    {
        FinanceType? newFinanceType = JsonSerializer.Deserialize<FinanceType>(newFinanceTypeJson);

        FinanceType? financeType = db.Types.Find(newFinanceType.Id);

        if (financeType == null)
        {
            return null;
        }

        financeType.OperationType = newFinanceType.OperationType;
        financeType.TypeName = newFinanceType.TypeName;
        db.Types.Update(financeType);
        db.SaveChanges();

        return GetFinanceTypes();
    }


    public List<FinanceType> DeleteFinanceType(int id)
    {
        FinanceType? financeType = db.Types.Find(id);

        if (financeType == null)
        {
            return null;
        }

        db.Types.Remove(financeType);
        db.SaveChanges();

        return GetFinanceTypes();
    }


    public List<FinanceType> GetEpencesTypes()
    {
        List<FinanceType> financeTypes = db.Types.Where(p => p.OperationType == false).ToList();

        if (financeTypes.Count == 0)
        {
            return null;
        }

        return financeTypes;
    }

    public List<FinanceType> GetIncomeTypes()
    {
        List<FinanceType> financeTypes = db.Types.Where(p => p.OperationType == true).ToList();

        if (financeTypes.Count == 0)
        {
            return null;
        }

        return financeTypes;
    } 

    public List<FinanceType> GetFinanceTypes()
    {
        List<FinanceType> financeTypes = db.Types.ToList();

        if (financeTypes.Count == 0)
        {
            return null;
        }

        return financeTypes;
    }

}
