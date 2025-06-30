using ArkFury.Common.Models;
using System;

namespace ArkFury.Entities.DTOs
{
    public class BaseDTO : PagingRequest
    {
        public string Id { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
