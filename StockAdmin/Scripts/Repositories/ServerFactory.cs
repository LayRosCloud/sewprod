using StockAdmin.Models;
using StockAdmin.Scripts.Repositories.Interfaces;
using StockAdmin.Scripts.Repositories.Server;

namespace StockAdmin.Scripts.Repositories;

public class ServerFactory : IRepositoryFactory
{
    public ICrud<AgeEntity> CreateAgeRepository()
    {
        return new AgeRepository();
    }

    public IClothOperationRepository CreateClothOperationRepository()
    {
        return new ClothOperationRepository();
    }

    public IClothOperationPersonRepository CreateClothOperationPersonRepository()
    {
        return new ClothOperationPersonRepository();
    }

    public IPackagesRepository CreatePackagesRepository()
    {
        return new PackageRepository();
    }

    public IPartiesRepository CreatePartyRepository()
    {
        return new PartyRepository();
    }

    public ICrud<ModelEntity> CreateModelRepository()
    {
        return new ModelRepository();
    }

    public ICrud<MaterialEntity> CreateMaterialRepository()
    {
        return new MaterialRepository();
    }

    public ICrud<OperationEntity> CreateOperationRepository()
    {
        return new OperationRepository();
    }

    public IDataCreator<PermissionEntity> CreatePermissionRepository()
    {
        return new PermissionRepository();
    }

    public IPerson CreatePersonRepository()
    {
        return new PersonRepository();
    }

    public ICrud<PostEntity> CreatePostRepository()
    {
        return new PostRepository();
    }

    public IDataReader<LinkEntity> CreateLinkRepository()
    {
        return new LinkRepository();
    }

    public ICrud<PriceEntity> CreatePriceRepository()
    {
        return new PriceRepository();
    }

    public ISizesRepository CreateSizeRepository()
    {
        return new SizeRepository();
    }

    public ICrud<ModelPriceEntity> CreateModelPriceRepository()
    {
        return new ModelPriceRepository();
    }

    public ICrud<ModelOperationEntity> CreateModelOperationRepository()
    {
        return new ModelOperationRepository();
    }

    public IDataReader<HistoryEntity> CreateHistoryRepository()
    {
        return new HistoryRepository();
    }
}