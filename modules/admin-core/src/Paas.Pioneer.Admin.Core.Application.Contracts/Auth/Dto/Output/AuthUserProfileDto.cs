﻿namespace Paas.Pioneer.Admin.Core.Application.Contracts.Auth.Dto.Output
{
    public class AuthUserProfileDto
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }
    }
}