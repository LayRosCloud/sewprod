using StockAdmin.Models;

namespace StockAdmin.Scripts.Repositories.Interfaces;

public interface IRepositoryFactory
{
    public ICrud<AgeEntity> CreateAgeRepository();
    public IClothOperationRepository CreateClothOperationRepository();
    public IClothOperationPersonRepository CreateClothOperationPersonRepository();
    public IPackagesRepository CreatePackagesRepository();
    public IPartiesRepository CreatePartyRepository();
    public ICrud<ModelEntity> CreateModelRepository();
    public ICrud<MaterialEntity> CreateMaterialRepository();
    public ICrud<OperationEntity> CreateOperationRepository();
    public IDataCreator<PermissionEntity> CreatePermissionRepository();
    public IPerson CreatePersonRepository();
    public ICrud<PostEntity> CreatePostRepository();
    public IDataReader<LinkEntity> CreateLinkRepository();
    public ICrud<PriceEntity> CreatePriceRepository();
    public ISizesRepository CreateSizeRepository();
    public IModelPriceRepository CreateModelPriceRepository();
    public IModelOperationRepository CreateModelOperationRepository();
    public IDataReader<HistoryEntity> CreateHistoryRepository();
}