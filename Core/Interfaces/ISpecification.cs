using System;
using System.Linq.Expressions;

namespace Core.Interfaces;

public interface ISpecification<T>
{
    Expression<Func<T, bool>>? Criteria {get;}
    Expression<Func<T, object>>? OrderBy {get;}
    Expression<Func<T, object>>? OrderByDescending {get;}
    bool IsDistinct{get;}
}

// Additional Interface for getting Brands or Types
public interface ISpecification<T, TResult> : ISpecification<T>
{
    Expression<Func<T, TResult>>? Select {get;}
}