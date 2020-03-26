﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YodaIM.Models
{
    public enum FileType
    {
        Generic,
        Avatar
    };

    public class FileModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(255)] [Required]
        public string FileName { get; set; }

        [Required]
        public string ContentType { get; set; }

        [Required]
        public FileType Type { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid BinaryBlobId { get; set; }

        [JsonIgnore]
        public BinaryBlob BinaryBlob { get; set; }

        public virtual User User { get; set; }

        [JsonIgnore]
        public List<MessageAttachment> MessageAttachments { get; set; }

    }
}
