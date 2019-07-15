﻿using Bootstrap.DataAccess;
using Longbow.Web.Mvc;
using System.Linq;

namespace Bootstrap.Admin.Query
{
    /// <summary>
    /// 
    /// </summary>
    public class QueryGroupOption : PaginationOption
    {
        /// <summary>
        /// 
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public QueryData<object> RetrieveData()
        {
            // int limit, int offset, string name, string price, string sort, string order
            var data = GroupHelper.Retrieves();
            if (!string.IsNullOrEmpty(GroupName))
            {
                data = data.Where(t => t.GroupName.Contains(GroupName));
            }
            if (!string.IsNullOrEmpty(Description))
            {
                data = data.Where(t => t.Description.Contains(Description));
            }
            var ret = new QueryData<object>();
            ret.total = data.Count();
            data = Order == "asc" ? data.OrderBy(t => t.GroupName) : data.OrderByDescending(t => t.GroupName);
            ret.rows = data.Skip(Offset).Take(Limit).Select(g => new { g.Id, g.GroupCode, g.GroupName, g.Description });
            return ret;
        }
    }
}
