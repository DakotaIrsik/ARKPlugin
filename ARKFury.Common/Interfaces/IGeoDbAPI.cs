using Refit;
using ArkFury.Common.LookUps;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArkFury.Common.Interfaces
{
    public interface IGeoDbAPI
    {

        [Get("/v1/geo/countries/US/regions/{state}/cities?minPopulation=25000&limit={size}")]
        Task<GeoDbResponse<IEnumerable<T>>> GetCities<T>([Header("X-RapidAPI-Key")] string authorization, string state, int size);
    }

    public class GeoDbResponse<T>
    {
        public T Data { get; set; }
    }
}
