using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using StockAdmin.Models;

namespace StockAdmin.Scripts.Repositories;

internal class DataContext : DbContext
{
    private static DataContext? _instance;

    public static DataContext Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DataContext();
            }

            return _instance;
        }
    }
    
    public DbSet<ActionEntity> actions { get; set; } = null!;
    public DbSet<AgeEntity> ages { get; set; } = null!;
    public DbSet<ClothOperationEntity> clothoperations { get; set; } = null!;
    public DbSet<ClothOperationPersonEntity> clothOperationPersons { get; set; } = null!;
    public DbSet<HistoryEntity> histories { get; set; } = null!;
    public DbSet<LinkEntity> links { get; set; } = null!;
    public DbSet<MaterialEntity> materials { get; set; } = null!;
    public DbSet<ModelEntity> models { get; set; } = null!;
    public DbSet<ModelOperationEntity> modelOperations { get; set; } = null!;
    public DbSet<ModelPriceEntity> modelPrices { get; set; } = null!;
    public DbSet<OperationEntity> operations { get; set; } = null!;
    public DbSet<PackageEntity> packages { get; set; } = null!;
    public DbSet<PartyEntity> parties { get; set; } = null!;
    public DbSet<PermissionEntity> permissions { get; set; } = null!;
    public DbSet<PersonEntity> persons { get; set; } = null!;
    public DbSet<PostEntity> posts { get; set; } = null!;
    public DbSet<PriceEntity> prices { get; set; } = null!;
    public DbSet<SizeEntity> sizes { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL("User Id=admin;Password=551617;Host=127.0.0.1;Port=3306;Database=stock;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PackageEntity>().Ignore(x => x.Status).Ignore(x=>x.CompletedTasks);
        modelBuilder.Entity<HistoryEntity>()
            .HasOne<ActionEntity>(x => x.Action)
            .WithMany()
            .HasForeignKey(x => x.ActionId);
    }
}