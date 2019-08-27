using System;
using static vecihi.helper.Const.Enums;

namespace vecihi.helper.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class FilterAttribute : Attribute
    {
        public SearchType SearchType { get; }
        public string DbName { get; }
        public bool Ignore { get; }

        public FilterAttribute(SearchType searchType = SearchType.Equal, string dbName = null, bool ignore = false)
        {
            SearchType = searchType;
            DbName = dbName;
            Ignore = ignore;
        }
    }
}