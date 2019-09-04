﻿using Longbow.Web.Mvc;
using PetaPoco;
using System;
using System.Collections.Generic;

namespace Bootstrap.DataAccess
{
    /// <summary>
    /// 登录用户信息实体类
    /// </summary>
    [TableName("LoginLogs")]
    public class LoginUser
    {
        /// <summary>
        /// 获得/设置 Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 获得/设置 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 获得/设置 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 获得/设置 登录IP地址
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 获得/设置 登录浏览器
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        /// 获得/设置 登录操作系统
        /// </summary>
        public string OS { get; set; }

        /// <summary>
        /// 获得/设置 登录地点
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 获得/设置 登录是否成功
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 获得/设置 用户 UserAgent
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 保存登录用户数据
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual bool Log(LoginUser user)
        {
            var db = DbManager.Create();
            db.Save(user);
            return true;
        }

        /// <summary>
        /// 获得登录用户的分页数据
        /// </summary>
        /// <param name="po"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public virtual Page<LoginUser> RetrieveByPages(PaginationOption po, DateTime? startTime, DateTime? endTime, string ip)
        {
            var sql = new Sql("select * from LoginLogs");
            if (startTime.HasValue) sql.Where("LoginTime >= @0", startTime.Value);
            if (endTime.HasValue) sql.Where("LoginTime < @0", endTime.Value.AddDays(1));
            if (!string.IsNullOrEmpty(ip)) sql.Where("ip = @0", ip);
            sql.OrderBy($"{po.Sort} {po.Order}");
            return DbManager.Create().Page<LoginUser>(po.PageIndex, po.Limit, sql);
        }

        /// <summary>
        /// 获取所有登录数据
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<LoginUser> RetrieveAll(DateTime? startTime, DateTime? endTime, string ip)
        {
            var sql = new Sql("select UserName, LoginTime, Ip, Browser, OS, City, Result from LoginLogs");
            if (startTime.HasValue) sql.Where("LoginTime >= @0", startTime.Value);
            if (endTime.HasValue) sql.Where("LoginTime < @0", endTime.Value.AddDays(1));
            if (!string.IsNullOrEmpty(ip)) sql.Where("ip = @0", ip);
            sql.OrderBy($"LoginTime");
            return DbManager.Create().Fetch<LoginUser>(sql);
        }
    }
}