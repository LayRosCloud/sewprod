namespace StockAdmin.Scripts.Exports.TableBuilders.Interfaces;

public interface ITableBuilder<TEntity>
{
    void FillHeaders();
    void FillBody(TEntity item, int position = 0);
    
}