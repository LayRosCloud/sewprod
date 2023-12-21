using NPOI.XWPF.UserModel;

namespace StockAdmin.Scripts.Exports;

public interface ITableBuilder<TEntity>
{
    void FillHeaders();
    void FillBody(TEntity item, int index = 0);
    
}