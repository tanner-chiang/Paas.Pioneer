﻿using Paas.Pioneer.Admin.Core.Application.Contracts.Personnel.Organization.Dto.Input;
using System;

namespace Paas.Pioneer.Admin.Core.Application.Contracts.Personnel.Organization.Dto.Output
{
    public class OrganizationGetOutput 
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 主管
        /// </summary>
        public Guid? PrimaryUserId { get; set; }

        /// <summary>
        /// 员工人数
        /// </summary>
        public int EmployeeCount { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
		public bool Enabled { get; set; }
    }
}