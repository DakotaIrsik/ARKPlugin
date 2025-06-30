using ArkFury.Entities.DTOs;
using Nest;
using System;

namespace ArkFury.Entities.ElasticSearch
{
    public class ElasticServer : BaseElastic
    {
      public ElasticConfiguration Configuration { get; set; }

    }
}
