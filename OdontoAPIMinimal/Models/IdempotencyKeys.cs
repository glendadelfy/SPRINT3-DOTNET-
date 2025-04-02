﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OdontoAPIMinimal.Models
{
    [Table("IdempotencyKeys")] // Nome da tabela no banco de dados
    public class IdempotencyKey
    {
        [Key]
        [StringLength(50)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public int StatusCode { get; set; }

        [Required]
        public string ResponseBody { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
