﻿using Lazy.SlideCaptcha.Core.Validator;
using System.ComponentModel.DataAnnotations;

namespace Paas.Pioneer.Admin.Core.Application.Contracts.Auth.Dto.Input
{
    /// <summary>
    /// 登录信息
    /// </summary>
    public class AuthLoginInput
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage = "用户名不能为空！")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空！")]
        public string Password { get; set; }

        /// <summary>
        /// 密码键
        /// </summary>
        public string PasswordKey { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        //[Required(ErrorMessage = "验证码不能为空！")]
        public string VerifyCode { get; set; }

        /// <summary>
        /// 验证码键
        /// </summary>
        public string VerifyCodeKey { get; set; }

        /// <summary>
        /// 验证数据
        /// </summary>
        public CaptchaInput Captcha { get; set; }
    }

    /// <summary>
    /// 滑动验证码
    /// </summary>
    public class CaptchaInput
    {
        public string Id { get; set; }
        public SlideTrack track { get; set; }
    }
}