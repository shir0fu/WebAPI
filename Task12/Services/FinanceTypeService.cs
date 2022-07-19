using System.Text.Json;
using Task12.Models;
using Task12.Dto;
using Task12.ViewModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Task12.Services;

public interface IFinanceTypeService
{
    Task<List<FinanceTypeViewModel>> GetListTypesAsync();
    Task<List<FinanceTypeViewModel>> GetExpencesTypesAsync();
    Task<List<FinanceTypeViewModel>> GetIncomeTypesAsync();

    Task<List<FinanceTypeViewModel>> AddFinanceTypeAsync(FinanceTypeCreateDto financeType);

    Task<bool> DeleteFinanceTypeAsync(int id);

    Task<bool> UpdateFinanceTypeAsync(FinanceTypeUpdateDto financeType);
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


    public async Task<List<FinanceTypeViewModel>> AddFinanceTypeAsync(FinanceTypeCreateDto newFinanceType)
    {

        FinanceType? financeType = new FinanceType();

        financeType.TypeName = newFinanceType.TypeName;
        financeType.OperationType = newFinanceType.OperationType;

        await db.Types.AddAsync(financeType);
        await db.SaveChangesAsync();

        return await GetListTypesAsync();
    }


    public async Task<bool> UpdateFinanceTypeAsync(FinanceTypeUpdateDto newFinanceType)
    {

        FinanceType? financeType = await db.Types.FindAsync(newFinanceType.Id);

        if (financeType is null)
        {
            return false;
        }

        financeType.OperationType = newFinanceType.OperationType;
        financeType.TypeName = newFinanceType.TypeName;

        db.Types.Update(financeType);
        await db.SaveChangesAsync();

        return true;
    }


    public async Task<bool> DeleteFinanceTypeAsync(int id)
    {
        FinanceType? financeType = await db.Types.FindAsync(id);

        if (financeType is null)
        {
            return false;
        }

        List<Record> records = await db.Records.Where(p => p.TypeId == id).ToListAsync();
        if (records.Any())
        {
            return false;
        }

        db.Types.Remove(financeType);
        await db.SaveChangesAsync();

        return true;
    }


    public async Task<List<FinanceTypeViewModel>> GetExpencesTypesAsync()
    {
        List<FinanceType> financeTypes = await db.Types.Where(p => !p.OperationType).ToListAsync();

        if (!financeTypes.Any())
        {
            return new List<FinanceTypeViewModel>();
        }

        var mapper = new Mapper(config);
        List<FinanceTypeViewModel> financeTypesVM = new List<FinanceTypeViewModel>();
        mapper.Map(financeTypes, financeTypesVM);

        return financeTypesVM;
    }

    public async Task<List<FinanceTypeViewModel>> GetIncomeTypesAsync()
    {
        List<FinanceType> financeTypes = await db.Types.Where(p => p.OperationType == true).ToListAsync();

        if (!financeTypes.Any())
        {
            return new List<FinanceTypeViewModel>();
        }

        var mapper = new Mapper(config);
        List<FinanceTypeViewModel> financeTypesVM = new List<FinanceTypeViewModel>();
        mapper.Map(financeTypes, financeTypesVM);

        return financeTypesVM;
    } 

    public async Task<List<FinanceTypeViewModel>> GetListTypesAsync()
    {
        List<FinanceType> financeTypes = await db.Types.ToListAsync();

        if (!financeTypes.Any())
        {
            return new List<FinanceTypeViewModel>();
        }

        var mapper = new Mapper(config);
        List<FinanceTypeViewModel> financeTypesVM = new List<FinanceTypeViewModel>();
        mapper.Map(financeTypes, financeTypesVM);

        return financeTypesVM;
    }

}
