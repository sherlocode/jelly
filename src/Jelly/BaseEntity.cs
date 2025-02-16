using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jelly
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid SystemId { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid UpdatedBy { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
        [MaxLength(20)]
        public string StatusCode { get; set; }
    }
}
