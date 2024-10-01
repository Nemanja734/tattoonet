using System;
using System.Linq.Expressions;

namespace Core.Interfaces;

public interface ISpecification<T>
{
    Expression<Func<T, bool>>? Criteria {get;}
    Expression<Func<T, object>>? OrderBy {get;}
    Expression<Func<T, object>>? OrderByDescending {get;}
    List<Expression<Func<T, object>>> Includes {get;}       // ep. 193 eager loading for delivery method
    List<string> IncludeStrings {get;}       // For ThenInclude
    bool IsDistinct{get;}
    int Take {get;}
    int Skip {get;}
    bool IsPagingEnabled {get;}

    // Used for pagination
    IQueryable<T> ApplyCriteria(IQueryable<T> query);
}

// Additional Interface for getting Brands or Types
public interface ISpecification<T, TResult> : ISpecification<T>
{
    Expression<Func<T, TResult>>? Select {get;}
}