using AutoMapper;
using Internal.Application.Common.Helpers;

namespace Internal.Application.Mapper
{
    public class PagedListConverter<T, U> : ITypeConverter<PagedList<T>, PagedList<U>>
    {
        public PagedList<U> Convert(PagedList<T> source, PagedList<U> destination, ResolutionContext context)
        {
            return new PagedList<U>(context.Mapper.Map<List<U>>(source.Items), 
                source.Page, 
                source.PageSize, 
                source.TotalCount);
        }
    }
}
