using System;

namespace Domain.Dtos
{
    public class HttpResultDto
    {
        public object value { get; set; }
        public int statusCode { get; set; }
        public string? contentType { get; set; }
    }
}
