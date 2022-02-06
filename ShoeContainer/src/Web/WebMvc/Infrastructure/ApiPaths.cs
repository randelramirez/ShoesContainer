using System;

namespace ShoesOnContainers.Web.WebMvc.Infrastructure
{
    public  class ApiPaths
    {
        public static class Catalog
        {
            public static string GetAllCatalogItems(string baseUri, int page, int take, int? brand, int? type)
            {
                var filterQs = (typeHasValue: type.HasValue, brandHasValue: brand.HasValue) switch
                    {
                        (typeHasValue: true, brandHasValue: false) => $"/type/{type.Value.ToString()}",
                        (typeHasValue: false, brandHasValue: true) => $"brand/{brand.Value.ToString()}",
                        (typeHasValue: true, brandHasValue: true) => $"/type/{type.Value.ToString()}/brand/{brand.Value.ToString()}",
                        _ => ""
                    };

                return $"{baseUri}items{filterQs}?pageIndex={page.ToString()}&pageSize={take.ToString()}";
            }
            
            public static string GetAllCatalogItems2(string baseUri, int page, int take, int? brand, int? type)
            {
                var filterQs = "";

                if (brand.HasValue || type.HasValue)
                {
                    var brandQs = (brand.HasValue) ? brand.Value.ToString() : "null";
                    var typeQs = (type.HasValue) ? type.Value.ToString() : "null";
                    filterQs = $"/type/{typeQs}/brand/{brandQs}";
                }

                return $"{baseUri}items{filterQs}?pageIndex={page}&pageSize={take}";
            }

            public static string GetCatalogItem(string baseUri, int id)
            {

                return $"{baseUri}/items/{id}";
            }
            public static string GetAllBrands(string baseUri)
            {
                return $"{baseUri}catalogBrands";
            }

            public static string GetAllTypes(string baseUri)
            {
                return $"{baseUri}catalogTypes";
            }
        }
        
    }
}