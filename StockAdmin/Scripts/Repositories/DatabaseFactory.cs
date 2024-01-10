using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Database;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Repositories.Server;

namespace StockAdmin.Scripts.Repositories;

public class DatabaseFactory : IRepositoryFactory
{
    public ICrud<AgeEntity> CreateAgeRepository()
    {
        return new DbAgeRepository();
    }

    public IClothOperationRepository CreateClothOperationRepository()
    {
        return new DbClothOperationRepository();
    }

    public IClothOperationPersonRepository CreateClothOperationPersonRepository()
    {
        return new DbClothOperationPersonRepository();
    }

    public IPackagesRepository CreatePackagesRepository()
    {
        return new DbPackageRepository();
    }

    public IPartiesRepository CreatePartyRepository()
    {
        return new DbPartyRepository();
    }

    public ICrud<ModelEntity> CreateModelRepository()
    {
        return new DbModelRepository();
    }

    public ICrud<MaterialEntity> CreateMaterialRepository()
    {
        return new DbMaterialRepository();
    }

    public ICrud<OperationEntity> CreateOperationRepository()
    {
        return new DbOperationRepository();
    }

    public IDataCreator<PermissionEntity> CreatePermissionRepository()
    {
        return new DbPermissionRepository();
    }

    public IPerson CreatePersonRepository()
    {
        return new DbPersonRepository();
    }

    public ICrud<PostEntity> CreatePostRepository()
    {
        return new DbPostRepository();
    }

    public IDataReader<LinkEntity> CreateLinkRepository()
    {
        return new DbLinkRepository();
    }

    public ICrud<PriceEntity> CreatePriceRepository()
    {
        return new DbPriceRepository();
    }

    public ISizesRepository CreateSizeRepository()
    {
        return new DbSizeRepository();
    }

    public ICrud<ModelPriceEntity> CreateModelPriceRepository()
    {
        return new DbModelPriceRepository();
    }

    public ICrud<ModelOperationEntity> CreateModelOperationRepository()
    {
        return new DbModelOperationRepository();
    }

    public IDataReader<HistoryEntity> CreateHistoryRepository()
    {
        return new DbHistoryRepository();
    }
}