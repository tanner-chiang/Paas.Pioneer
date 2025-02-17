﻿using Paas.Pioneer.Admin.Core.Application.Contracts.Dictionary;
using Paas.Pioneer.Admin.Core.Application.Contracts.Dictionary.Dto.Input;
using Paas.Pioneer.Admin.Core.Application.Contracts.Dictionary.Dto.Output;
using Paas.Pioneer.Admin.Core.Domain.Dictionary;
using Paas.Pioneer.Domain.Shared.Dto.Input;
using Paas.Pioneer.AutoWrapper;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Paas.Pioneer.Domain.Shared.Dto.Output;
using System.Collections.Generic;

namespace Paas.Pioneer.Admin.Core.Application.Dictionary
{
    public class DictionaryService : ApplicationService, IDictionaryService
    {
        private readonly IDictionaryRepository _dictionaryRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DictionaryService(IDictionaryRepository dictionaryRepository)
        {
            _dictionaryRepository = dictionaryRepository;
        }

        /// <summary>
        /// 获取一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GetDictionaryOutput> GetAsync(Guid id)
        {
            var result = await _dictionaryRepository.GetAsync(p => p.Id == id, x => new GetDictionaryOutput()
            {
                Id = x.Id,
                Code = x.Code,
                Description = x.Description,
                DictionaryTypeId = x.DictionaryTypeId,
                Enabled = x.Enabled,
                Name = x.Name,
                Value = x.Value,
            });
            return result;
        }

        public Task<IEnumerable<GetDictionaryOutput>> GetListAsync(IEnumerable<Guid> ids)
        {
            return _dictionaryRepository.GetListAsync(expression: x => ids.Contains(x.Id), selector: x => new GetDictionaryOutput()
            {
                Id = x.Id,
                Code = x.Code,
                Description = x.Description,
                DictionaryTypeId = x.DictionaryTypeId,
                Enabled = x.Enabled,
                Name = x.Name,
                Value = x.Value,
            });
        }

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<Page<DictionaryPageListOutput>> GetPageListAsync(PageInput<DictionaryInput> input)
        {
            var data = await _dictionaryRepository.GetPageListAsync(input);
            return data;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task AddAsync(DictionaryAddInput input)
        {
            if (await _dictionaryRepository.AnyAsync(x => x.DictionaryTypeId == input.DictionaryTypeId && x.Code == input.Code))
            {
                throw new BusinessException("字典编码已存在！");
            }
            var dictionary = ObjectMapper.Map<DictionaryAddInput, Ad_DictionaryEntity>(input);
            await _dictionaryRepository.InsertAsync(dictionary);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateAsync(DictionaryUpdateInput input)
        {
            var entity = await _dictionaryRepository.GetAsync(input.Id);
            if (entity?.Id == Guid.Empty)
            {
                throw new BusinessException("数据字典不存在！");
            }
            if (await _dictionaryRepository.AnyAsync(x => x.Id != input.Id && x.DictionaryTypeId == input.DictionaryTypeId && x.Code == input.Code))
            {
                throw new BusinessException("字典编码已存在！");
            }
            ObjectMapper.Map(input, entity);
            await _dictionaryRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Guid id)
        {
            await _dictionaryRepository.DeleteAsync(m => m.Id == id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task BatchSoftDeleteAsync(Guid[] ids)
        {
            await _dictionaryRepository.DeleteAsync(x => ids.Contains(x.Id));
        }
    }
}