using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Paas.Pioneer.Domain.Shared.Auth;
using Paas.Pioneer.Knife4jUI.Swagger;
using Paas.Pioneer.Middleware.Middleware.Extensions;
using Paas.Pioneer.Redis;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Paas.Pioneer
{
    [DependsOn(
		typeof(AbpAutofacModule),
		typeof(AbpAspNetCoreMvcModule),
		typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
		typeof(Knife4jUISwaggerModule),
		typeof(DomainSharedModule),
		typeof(RedisModule)
		)]
	public class PioneerModule : AbpModule
	{
		private const string DefaultCorsPolicyName = "Allow";

		public override void ConfigureServices(ServiceConfigurationContext context)
		{
			var hostingEnvironment = context.Services.GetHostingEnvironment();

			var configuration = context.Services.GetConfiguration();

			ConfigureAuthentication(context, configuration);
		}

		private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
		{
			context.Services.AddCors(options =>
			{
				options.AddDefaultPolicy(builder =>
				{
					builder
						.WithOrigins(
							configuration["App:CorsOrigins"]
								.Split(",", StringSplitOptions.RemoveEmptyEntries)
								.Select(o => o.RemovePostFix("/"))
								.ToArray()
						)
						.WithAbpExposedHeaders()
						.SetIsOriginAllowedToAllowWildcardSubdomains()
						.AllowAnyHeader()
						.AllowAnyMethod()
						.AllowCredentials();
				});
			});
		}

		public override void OnApplicationInitialization(ApplicationInitializationContext context)
		{
			var app = context.GetApplicationBuilder();
			var env = context.GetEnvironment();

			// 判断是否测试模式
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			// 生成全局唯一Id
			app.UseCorrelationId();

			//路由
			app.UseRouting();

			//跨域
			app.UseCors();

			// 验证
			app.UseAuthentication();

			// Jwt
			app.UseJwtTokenMiddleware();

			// 工作单元
			app.UseUnitOfWork();

			// 授权
			app.UseAuthorization();

			//全局日志中间件
			app.UseLoggerMiddleware();

			// 配置末端点
			app.UseConfiguredEndpoints(endpoints =>
			{
				endpoints.MapSwaggerUI();
			});
		}

		/// <summary>
		/// 配置验证模式
		/// </summary>
		/// <param name="context"></param>
		/// <param name="configuration"></param>
		private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
		{
			context.Services.AddAuthentication(options =>
			{
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = nameof(ResponseAuthenticationHandler); //401
				options.DefaultForbidScheme = nameof(ResponseAuthenticationHandler);    //403
			})
			.AddJwtBearer(options =>
			{
				options.Events = new JwtBearerEvents()
				{
					OnMessageReceived = context =>
					{
						var accessToken = context.Request.Query["access_token"];
						// If the request is for our hub...
						var path = context.HttpContext.Request.Path;
						if (!string.IsNullOrEmpty(accessToken) &&
							(path.StartsWithSegments("/chat-hub")))
						{
							// Read the token out of the query string
							context.Token = accessToken;
							context.HttpContext.Request.Headers["Authorization"] = "Bearer " + accessToken;
						}
						return Task.CompletedTask;
					}
				};
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = configuration["jwtconfig:issuer"],
					ValidAudience = configuration["jwtconfig:audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwtconfig:securitykey"])),
					ClockSkew = TimeSpan.Zero,
					RequireExpirationTime = true
				};
			})
			.AddScheme<AuthenticationSchemeOptions, ResponseAuthenticationHandler>(nameof(ResponseAuthenticationHandler), o => { });
		}
	}
}
