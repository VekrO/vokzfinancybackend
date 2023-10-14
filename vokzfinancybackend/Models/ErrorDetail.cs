using System.Text.Json;

namespace VokzFinancy.Models {

    public class ErrorDetail {
        
        public string? Message { get; set; }
        public int? StatusCode { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

    }

}