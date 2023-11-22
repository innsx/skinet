using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public interface ISpecification<T>
    {
        // Filtering Specifications Criteria Expression
         Expression<Func<T, bool>> Criteria {get;}


         // EAGER LOADING objects Specification Expression  
         List<Expression<Func<T, object>>> Includes {get;}


        // Sorting Specifications Expressions
        Expression<Func<T, object>> OrderBy {get;}
        Expression<Func<T, object>> OrderByDescending {get;}


        //PAGINATION SPECIFICATIONS
        int Take {get;}
        int Skip {get;}
        bool IsPagingEnabled {get;}
    }
}