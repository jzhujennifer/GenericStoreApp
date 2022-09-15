using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace GenericStoreApp.Models
{
    public class FakeProduct
    {
        public int id { get; set; }
        public string? title { get; set; }

        public decimal price { get; set; }
        public string? description { get; set; }
        public string? category { get; set; }
        public string? image { get; set; }

    }
}