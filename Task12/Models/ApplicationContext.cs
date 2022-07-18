using Microsoft.EntityFrameworkCore;

namespace Task12.Models;

public class ApplicationContext : DbContext
{
    public DbSet<FinanceType> Types { get; set; } = null!;
    public DbSet<Record> Records { get; set; } = null!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();

        if (Records.Count() == 0)
        {
            Record record1 = new Record() { Value = -40, Description = "Siplo", TypeName = "Food", Date = new DateTime(2022, 6, 20) };
            Record record2 = new Record() { Value = -12.9, Description = "ATB", TypeName = "Food", Date = new DateTime(2022, 6, 20) };
            Record record3 = new Record() { Value = -500, Description = "Fuel", TypeName = "Car", Date = new DateTime(2022, 6, 20) };
            Record record4 = new Record() { Value = -800, Description = "Siplo", TypeName = "Food", Date = new DateTime(2022, 6, 25) };
            Record record5 = new Record() { Value = 5000, Description = "Salary", TypeName = "Work", Date = new DateTime(2022, 6, 30) };
            Record record6 = new Record() { Value = -20, Description = "Cake", TypeName = "Food", Date = new DateTime(2022, 6, 30) };
            Record record7 = new Record() { Value = -50, Description = "Beer", TypeName = "Food", Date = new DateTime(2022, 6, 30) };
            Record record8 = new Record() { Value = -50, Description = "Gas", TypeName = "Car", Date = new DateTime(2022, 7, 15) };
            Record record9 = new Record() { Value = -600, Description = "Engine", TypeName = "Car", Date = new DateTime(2022, 7, 18) };
            Record record10 = new Record() { Value = 200, Description = "Streer play", TypeName = "Hobbie", Date = new DateTime(2022, 7, 20) };
            Record record11 = new Record() { Value = -80, Description = "Tavria", TypeName = "Food", Date = new DateTime(2022, 7, 21) };
            Record record12 = new Record() {  Value = -30, Description = "ATB", TypeName = "Food", Date = new DateTime(2022, 7, 25) };
            Records.AddRange(record1, record2, record3, record4, record5, record6, record7, record8, record9, record10, record11, record12);
            SaveChanges();
        }
        
        if (Types.Count() == 0)
        {
            FinanceType f1 = new FinanceType { OperationType = false, TypeName = "Food" };
            FinanceType f2 = new FinanceType { OperationType = false, TypeName = "Car" };
            FinanceType f3 = new FinanceType { OperationType = true, TypeName = "Hobbie" };
            FinanceType f4 = new FinanceType { OperationType = true, TypeName = "Work" };
            Types.AddRange(f1, f2, f3, f4);
            SaveChanges();
        }
    }
}

