using AutoMapper;
using ArkFury.Entities.DTOs;
using ArkFury.Entities.ElasticSearch;
using ArkFury.Entities.Models;
using static ArkFury.Common.AppSettings;

namespace ArkFury.Core.Mapping
{
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Please try to keep these organized by region and also sorted alphabetically, else it will grow unwieldly quick!!!
        /// </summary>
        public MappingProfile()
        {
            #region SQL to DTO (Sort Alphabetic ASC)
            #endregion

            #region SQL to Elastic (Sort Alphabetic ASC)
            #endregion

            #region DTO to SQL (Sort Alphabetic ASC)

            #endregion

            #region DTO to Elastic (Sort Alphabetic ASC)

            #endregion

            #region Elastic to DTO (Sort Alphabetic ASC)

            #endregion

            #region DTO to DTO (Sort Alphabetic ASC)
            CreateMap<InventoryDTO, ItemDTO>();
            CreateMap<ItemDTO, InventoryDTO>();
            CreateMap<InventoryDTO, DinosaurDTO>();
            CreateMap<DinosaurDTO, InventoryDTO>();
            #endregion
        }
    }
}