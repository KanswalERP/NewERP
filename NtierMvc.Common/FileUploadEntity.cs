﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtierMvc.Common
{
    public class FileUploadEntity
    {
        public byte[] FileByteArray { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string ContentType { get; set; }
    }
}
