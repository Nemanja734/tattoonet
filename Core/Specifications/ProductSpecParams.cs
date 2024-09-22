using System;

namespace Core.Specifications;

public class ProductSpecParams
{
    // set maximum pageSize the client can select
    private const int MaxPageSize = 50;
    public int PageIndex {get;set;} = 1;

    // command propfull
    private int _pageSize = 6;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
    

    private List<string> _brands = [];
    public List<string> Brands
    {
        get => _brands;
        set
        {
            // Split List _brands into a List separated with commas
            _brands = value.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }
    
    private List<string> _types = [];
    public List<string> Types
    {
        get => _types;     // types = boards, gloves
        set
        {
            // Split List _types into a List separated with commas
            _types = value.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }

    public string? Sort {get; set;}

    private string? _search;
    public string Search
    {
        get => _search ?? "";
        set => _search = value.ToLower();
    }
    
    
}
