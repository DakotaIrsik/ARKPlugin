using ArkFury.common.Services;
using ArkFury.Common;
using ArkFury.Common.Models;
using ArkFury.Common.Services;
using ArkFury.Entities.ElasticSearch;
using ArkFury.Entities.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkFury.Core.BusinessLogic
{
    public interface IInfoDomain
    {
        Task<ElasticInfo> AsObject();

        Task<string> AsString();
    }

    public class InfoDomain : BaseDomain, IInfoDomain
    {
        public InfoDomain(ArkShopContext context,
            IMapper mapper,
           IElasticSearchService elastic,
           ILogger logger,
           IHttpContextAccessor httpContextAccessor,
           IOptions<AppSettings> settings,
           ICacheService cache) : base(context, mapper, elastic, logger, httpContextAccessor, settings, cache)
        {
        }

        public async Task<ElasticInfo> AsObject()
        {
            var response = await _elastic.Search<ElasticInfo>(new PagingRequest(true), "info");
            return response.Data.SingleOrDefault();
        }

        public async Task<string> AsString()
        {
            var obj = await AsObject();
            var sb = new StringBuilder();

            sb.Append("General Rules");sb.AppendLine();
            foreach (var gr in obj.GeneralRules)
            {
                sb.Append(gr);
                sb.AppendLine();
            }
            sb.AppendLine();

            sb.Append("Server Rules"); sb.AppendLine();
            foreach (var sr in obj.ServerRules)
            {
                sb.Append(sr);
                sb.AppendLine();
            }
            sb.AppendLine();

            sb.Append("Building Rules"); sb.AppendLine();
            foreach (var br in obj.BuildingRules)
            {
                sb.Append(br);
                sb.AppendLine();
            }
            sb.AppendLine();

            sb.Append("Blocked Building Locations"); sb.AppendLine();
            foreach (var bbl in obj.BlockedBuildingLocations)
            {
                sb.Append(bbl);
                sb.AppendLine();
            }
            sb.AppendLine();

            sb.Append("Raiding Rules"); sb.AppendLine();
            foreach (var rr in obj.RaidingRules)
            {
                sb.Append(rr);
                sb.AppendLine();
            }
            sb.AppendLine();

            sb.Append("Dino Platform Rules"); sb.AppendLine();
            foreach (var dpr in obj.DinoPlatformRules)
            {
                sb.Append(dpr);
                sb.AppendLine();
            }
            sb.AppendLine();

            return sb.ToString();
        }
    }
}
