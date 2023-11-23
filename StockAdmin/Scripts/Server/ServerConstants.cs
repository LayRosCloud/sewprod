using StockAdmin.Models;

namespace StockAdmin.Scripts.Server;

public abstract class ServerConstants
{
    public const string ServerAddress = "http://localhost:5000";
    public static AuthEntity Token = new();
    public static string Login = "";
    public static string Password = "";

    public const string FieldId = "id";
    
    public abstract class Package
    {
        public const string FieldPartyId = "partyId";
        public const string FieldSizeId = "sizeId";
        public const string FieldPersonId = "personId";
        public const string FieldMaterialId = "materialId";
        public const string FieldCount = "count";
        public const string FieldIsEnded = "isEnded";
        public const string FieldIsRepeat = "isRepeat";
        public const string FieldIsUpdated = "isUpdated";
        public const string FieldUid = "uid";
        public const string FieldSize = "size";
        public const string FieldParty = "party";
        public const string FieldPerson = "person";
        public const string FieldMaterial = "material";
        public const string FieldClothOperations = "clothOperations";
        public const string FieldCreatedAt = "createdAt";
    }
    
    public abstract class Person
    {
        public const string FieldEmail = "email";
        public const string FieldPassword = "password";
        public const string FieldLastName = "lastName";
        public const string FieldFirstName = "firstName";
        public const string FieldPatronymic = "patronymic";
        public const string FieldBirthDay = "birthDay";
        public const string FieldUid = "uid";
        public const string FieldPosts = "posts";
    }
    
    public abstract class Post
    {
        public const string FieldName = "name";
        public const string FieldDescription = "description";
    }
    
    public abstract class Price
    {
        public const string FieldNumber = "number";
        public const string FieldDate = "date";
    }

    public abstract class Operation
    {
        public const string FieldName = "name";
        public const string FieldDescription = "description";
        public const string FieldUid = "uid";
        public const string FieldPercent = "percent";
    }

    public abstract class Size
    {
        public const string FieldNumber = "number";
        public const string FieldAgeId = "ageId";
        public const string FieldAge = "age";
    }
    
    public abstract class Age
    {
        public const string FieldName = "name";
        public const string FieldDescription = "description";
    }
    
    public abstract class Auth
    {
        public const string FieldToken = "token";
        public const string FieldPosts = "posts";
    }
    
    public abstract class Permission
    {
        public const string FieldPersonId = "personId";
        public const string FieldPostId = "postId";
    }
    
    public abstract class Party
    {
        public const string FieldModelId = "modelId";
        public const string FieldPersonId = "personId";
        public const string FieldDateStart = "dateStart";
        public const string FieldDateEnd = "dateEnd";
        public const string FieldCutNumber = "cutNumber";
        public const string FieldPerson = "person";
        public const string FieldModel = "model";
        public const string FieldPriceId = "priceId";
        public const string FieldPrice = "price";
        public const string FieldPackages = "packages";
    }
    
    public abstract class Model
    {
        public const string FieldTitle = "title";
        public const string FieldCodeVendor = "codeVendor";
        public const string FieldPrice = "prices";
        public const string FieldOperations = "operations";
    }
    
    public abstract class Material
    {
        public const string FieldName = "name";
        public const string FieldDescription = "description";
        public const string FieldUid = "uid";
    }
    
    public abstract class Link
    {
        public const string FieldRel = "rel";
        public const string FieldHref = "href";
    }
    
    public abstract class History
    {
        public const string FieldPersonId = "personId";
        public const string FieldActionId = "actionId";
        public const string FieldTableName = "tableName";
        public const string FieldValue = "value";
        public const string FieldCreatedAt = "createdAt";
        public const string FieldPerson = "person";
        public const string FieldAction = "action";
    }
    
    public abstract class ClothOperationPerson
    {
        public const string FieldPersonId = "personId";
        public const string FieldClothOperationId = "clothOperationId";
        public const string FieldDateStart = "dateStart";
        public const string FieldIsEnded = "isEnded";
        public const string FieldPerson = "person";
    }
    
    public abstract class ClothOperation
    {
        public const string FieldOperationId = "operationId";
        public const string FieldPackageId = "packageId";
        public const string FieldPriceId = "priceId";
        public const string FieldIsEnded = "isEnded";
        public const string FieldOperation = "operation";
        public const string FieldPrice = "price";
        public const string FieldClothOperationPersons = "clothOperationPersons";
    }
    
    public abstract class ModelOperation
    {
        public const string FieldOperationId = "operationId";
        public const string FieldModelId = "modelId";
    }
    
    public abstract class ModelPrice
    {
        public const string FieldModelId = "modelId";
        public const string FieldPriceId = "priceId";
    }
}