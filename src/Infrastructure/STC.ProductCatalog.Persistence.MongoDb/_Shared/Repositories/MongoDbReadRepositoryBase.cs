using System.Linq.Expressions;
using MongoDB.Driver;
using STC.ProductCatalog.Domain._Shared.Paginations.Abstracts;
using STC.ProductCatalog.Domain._Shared.Paginations.Concretes;
using STC.ProductCatalog.Domain._Shared.Repositories;
using STC.ProductCatalog.Domain._Shared.Types.Abstracts;

namespace STC.ProductCatalog.Persistence.MongoDb._Shared.Repositories;

public abstract class MongoDbReadRepository<TDocument, TKey>(IMongoCollection<TDocument> collection)
    : IReadRepository<TDocument, TKey>
    where TDocument : ITable<TKey>
{
    protected readonly IMongoCollection<TDocument> Collection = collection;

    public Task<bool> AnyAsync(Expression<Func<TDocument, bool>> exp, CancellationToken cancellationToken) =>
        Collection.Find(exp).AnyAsync(cancellationToken: cancellationToken);

    public Task<long> CountAsync(CancellationToken cancellationToken, Expression<Func<TDocument, bool>>? exp = null) =>
        Collection.CountDocumentsAsync(filter: exp ?? (_ => true), cancellationToken: cancellationToken);

    public async Task<TDocument?> GetAsync(Expression<Func<TDocument, bool>> exp, CancellationToken cancellationToken)
        => await Collection.Find(exp).FirstOrDefaultAsync(cancellationToken: cancellationToken);

    public async Task<ICollection<TDocument>> GetAllAsync(CancellationToken cancellationToken,
        Expression<Func<TDocument, bool>>? exp = null) =>
        await Collection.Find(exp ?? (_ => true)).ToListAsync(cancellationToken: cancellationToken);

    public async Task<(ICollection<TDocument> Data, IPaginationOut Pagination)> GetAllAsync(IPaginationArguments pagination,
        CancellationToken cancellationToken, Expression<Func<TDocument, bool>>? exp = null)
    {
        long totalItemCount = await Collection.Find(exp).CountDocumentsAsync(cancellationToken: cancellationToken);
        if (totalItemCount is 0)
            return new(item1: [],
                item2: new PaginationOut(Page: 1, Count: 0, TotalCount: 0));

        var fluent = Collection.Find(exp);

        {
            {
                fluent = fluent.Skip(Convert.ToInt32((pagination.Page - 1) * pagination.ItemCountInThePage));

                if (pagination.ItemCountInThePage.HasValue)
                    fluent = fluent.Limit(Convert.ToInt32(pagination.ItemCountInThePage));
            }

            {
                var sortDefinitionBuilder = Builders<TDocument>.Sort;
                SortDefinition<TDocument>? sort = null;

                foreach (SortDefinition sortDefinition in pagination.Sorts)
                {
                    SortDefinition<TDocument> currentSort = sortDefinition.OrderBy == OrderBy.Ascending
                        ? sortDefinitionBuilder.Ascending(sortDefinition.Field)
                        : sortDefinitionBuilder.Descending(sortDefinition.Field);

                    sort = sort is null ? currentSort : sortDefinitionBuilder.Combine(sort, currentSort);
                }

                if (sort != null)
                    fluent = fluent.Sort(sort);
            }
        }

        ICollection<TDocument> data = await fluent.ToListAsync(cancellationToken: cancellationToken);

        return new(data,
            new PaginationOut(Page: pagination.Page,
                Count: pagination.ItemCountInThePage ?? data.Count,
                TotalCount: totalItemCount));
    }
}