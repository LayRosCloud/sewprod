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
    public DbSet<ClothOperationEntity> clothOperations { get; set; } = null!;
    public DbSet<ClothOperationPersonEntity> clothOperationsPersons { get; set; } = null!;
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
        OnModelPackageCreated(modelBuilder);

        OnModelSizeCreated(modelBuilder);

        OnModelHistoryCreated(modelBuilder);

        OnModelPersonCreated(modelBuilder);

        OnModelPartyCreated(modelBuilder);

        OnModelPriceCreated(modelBuilder);
        
        OnModelOperationCreated(modelBuilder);

        OnModelCreated(modelBuilder);

        OnModelClothOperationPersonCreated(modelBuilder);

        OnModelClothOperationCreated(modelBuilder);
    }
    
    private void OnModelPersonCreated(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PersonEntity>()
            .HasMany(x => x.Posts)
            .WithMany(x => x.Persons)
            .UsingEntity<PermissionEntity>(
                x => x.HasOne<PostEntity>()
                    .WithMany()
                    .HasForeignKey(per => per.PostId),
                x => x.HasOne<PersonEntity>()
                    .WithMany()
                    .HasForeignKey(per => per.PersonId)
            );
    }

    private void OnModelCreated(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ModelEntity>()
            .HasMany(x => x.Operations)
            .WithMany(x => x.Models)
            .UsingEntity<ModelOperationEntity>(
                x => x.HasOne<OperationEntity>()
                    .WithMany()
                    .HasForeignKey(per => per.OperationId),
                x => x.HasOne<ModelEntity>()
                    .WithMany()
                    .HasForeignKey(per => per.ModelId)
            );
        
        modelBuilder.Entity<ModelEntity>()
            .HasMany(x => x.Prices)
            .WithMany(x => x.Models)
            .UsingEntity<ModelPriceEntity>(
                x => x.HasOne<PriceEntity>()
                    .WithMany()
                    .HasForeignKey(per => per.PriceId),
                x => x.HasOne<ModelEntity>()
                    .WithMany()
                    .HasForeignKey(per => per.ModelId)
            );
    }

    private void OnModelPartyCreated(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PartyEntity>()
            .HasOne(x => x.Person)
            .WithMany()
            .HasForeignKey(x => x.PersonId);

        modelBuilder.Entity<PartyEntity>()
            .HasOne(x => x.Model)
            .WithMany()
            .HasForeignKey(x => x.ModelId);
        
        modelBuilder.Entity<PartyEntity>()
            .HasOne(x => x.Price)
            .WithMany()
            .HasForeignKey(x => x.PriceId);
        
    }
    
    private void OnModelPriceCreated(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PriceEntity>()
            .Ignore(x => x.ModelPrice);

    }
    
    private void OnModelClothOperationPersonCreated(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClothOperationPersonEntity>()
            .HasOne(x => x.Person)
            .WithMany()
            .HasForeignKey(x => x.PersonId);
    }
    
    private void OnModelClothOperationCreated(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClothOperationEntity>()
            .Ignore(x => x.OperationTask);
        
        modelBuilder.Entity<ClothOperationEntity>()
            .HasMany(x => x.ClothOperationPersons)
            .WithOne(x => x.ClothOperation)
            .HasForeignKey(x => x.ClothOperationId);
        
        modelBuilder.Entity<ClothOperationEntity>()
            .HasOne(x => x.Operation)
            .WithMany()
            .HasForeignKey(x => x.OperationId);
        
        modelBuilder.Entity<ClothOperationEntity>()
            .HasOne(x => x.Price)
            .WithMany()
            .HasForeignKey(x => x.PriceId);

    }
    
    private void OnModelOperationCreated(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OperationEntity>()
            .Ignore(x => x.ModelOperation);
    }
    
    private void OnModelPackageCreated(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PackageEntity>()
            .Ignore(x => x.Status)
            .Ignore(x=>x.Party)
            .HasMany(x => x.ClothOperations)
            .WithOne()
            .HasForeignKey(x => x.PackageId);

        modelBuilder.Entity<PackageEntity>()
            .HasOne(x => x.Material)
            .WithMany()
            .HasForeignKey(x => x.MaterialId);
        
        modelBuilder.Entity<PackageEntity>()
            .HasOne(x => x.Person)
            .WithMany()
            .HasForeignKey(x => x.PersonId);
        
        modelBuilder.Entity<PackageEntity>()
            .HasOne(x => x.Size)
            .WithMany()
            .HasForeignKey(x => x.SizeId);
    }
    
    private void OnModelSizeCreated(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SizeEntity>()
            .Ignore(x => x.FullName)
            .HasOne(x => x.Age)
            .WithMany()
            .HasForeignKey(x => x.AgeId);
    }

    private void OnModelHistoryCreated(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HistoryEntity>()
            .HasOne(x => x.Action)
            .WithMany()
            .HasForeignKey(x => x.ActionId);

        modelBuilder.Entity<HistoryEntity>()
            .HasOne(x => x.Person)
            .WithMany()
            .HasForeignKey(x => x.PersonId);
    }
}