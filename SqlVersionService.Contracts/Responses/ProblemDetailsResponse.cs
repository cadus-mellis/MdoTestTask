using System;
using System.Collections.Generic;
using System.Text;

namespace SqlVersionService.Contracts.Responses
{
    public sealed class ProblemDetailsResponse
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int? Status { get; set; }
        public string Detail { get; set; }
        public string Instance { get; set; }
    }
}
