﻿using System;

namespace Paas.Pioneer.Admin.Core.Application.Contracts.DictionaryType.Dto.Output
{
    /// <summary>
    /// 数据字典类型导出
    /// </summary>
    public partial class DictionaryTypeDataOutput
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 字典类型Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 字典名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 字典编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
		public bool Enabled { get; set; } = true;

        /// <summary>
        /// 排序
        /// </summary>
		public int Sort { get; set; }
    }
}