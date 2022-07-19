using System.Text.Json;
using Task12.Models;
using Task12.Dto;
using Task12.ViewModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Task12.Services;

public interface IFinanceTypeService
{
    Task<List<FinanceTypeViewModel>> GetFinanceTypes();
    Task<List<FinanceTypeViewModel>> GetExpencesTypes();
    Task<List<FinanceTypeViewModel>> GetIncomeTypes();

    Task<List<FinanceTypeViewModel>> AddFinanceType(FinanceTypeCreateDto financeType);

    Task<bool> DeleteFinanceType(int id);

    Task<List<FinanceTypeViewModel>> UpdateFinanceType(FinanceTypeUpdateDto financeType);
}

public class FinanceTypeService : IFinanceTypeService
{
    MapperConfiguration config = new MapperConfiguration(cfg =>
    cfg.CreateMap<FinanceType, FinanceTypeViewModel>()
    );

    ApplicationContext db;
    public FinanceTypeService(ApplicationContext context)
    {
        db = context;
    }


    public async Task<List<FinanceTypeViewModel>> AddFinanceType(FinanceTypeCreateDto newFinanceType)
    {

        FinanceType? financeType = new FinanceType();

        financeType.TypeName = newFinanceType.TypeName;
        financeType.OperationType = newFinanceType.OperationType;

        await db.Types.AddAsync(financeType);
        await db.SaveChangesAsync();

        return await GetFinanceTypes();
    }


    public async Task<List<FinanceTypeViewModel>> UpdateFinanceType(FinanceTypeUpdateDto newFinanceType)
    {

        FinanceType? financeType = await db.Types.FindAsync(newFinanceType.Id);

        if (financeType == null)
        {
            return new List<FinanceTypeViewModel>();
        }

        financeType.OperationType = newFinanceType.OperationType;
        financeType.TypeName = newFinanceType.TypeName;

        db.Types.Update(financeType);
        await db.SaveChangesAsync();

        return await GetFinanceTypes();
    }


    public async Task<bool> DeleteFinanceType(int id)
    {
        FinanceType? financeType = await db.Types.FindAsync(id);

        if (financeType == null)
        {
            return false;
        }

        List<Record> records = await db.Records.Where(p => p.TypeId == id).ToListAsync();
        if (records.Count != 0)
        {
            return false;
        }

        db.Types.Remove(financeType);
        await db.SaveChangesAsync();

        return true;
    }


    public async Task<List<FinanceTypeViewModel>> GetExpencesTypes()
    {
        List<FinanceType> financeTypes = await db.Types.Where(p => !p.OperationType).ToListAsync();

        if (financeTypes.Count == 0)
        {
            return new List<FinanceTypeViewModel>();
        }

        var mapper = new Mapper(config);
        List<FinanceTypeViewModel> financeTypesVM = new List<FinanceTypeViewModel>();
        mapper.Map(financeTypes, financeTypesVM);

        return financeTypesVM;
    }

    public async Task<List<FinanceTypeViewModel>> GetIncomeTypes()
    {
        List<FinanceType> financeTypes = await db.Types.Where(p => p.OperationType == true).ToListAsync();

        if (financeTypes.Count == 0)
        {
            return new List<FinanceTypeViewModel>();
        }

        var mapper = new Mapper(config);
        List<FinanceTypeViewModel> financeTypesVM = new List<FinanceTypeViewModel>();
        mapper.Map(financeTypes, financeTypesVM);

        return financeTypesVM;
    } 

    public async Task<List<FinanceTypeViewModel>> GetFinanceTypes()
    {
        List<FinanceType> financeTypes = await db.Types.ToListAsync();

        if (financeTypes.Count == 0)
        {
            return new List<FinanceTypeViewModel>();
        }

        var mapper = new Mapper(config);
        List<FinanceTypeViewModel> financeTypesVM = new List<FinanceTypeViewModel>();
        mapper.Map(financeTypes, financeTypesVM);

        return financeTypesVM;
    }

}
