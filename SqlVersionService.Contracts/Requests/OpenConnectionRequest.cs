using System.ComponentModel.DataAnnotations;

namespace SqlVersionService.Contracts.Requests
{
    public sealed class OpenConnectionRequest
    {
        [Required]
        public string ConnectionString { get; set; }
    }
}