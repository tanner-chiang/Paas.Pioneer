﻿using System;

namespace Paas.Pioneer.Admin.Core.Application.Contracts.Api.Dto.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public partial class ApiUpdateInput
    {
        /// <summary>
        /// 接口Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 所属模块
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 接口地址
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 接口提交方法
        /// </summary>
        public string HttpMethods { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        public bool Enabled { get; set; }
    }
}