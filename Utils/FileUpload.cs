﻿using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TryNetCore.Utils
{
    public class FileUpload
    {
        // .Net Core'da File Upload
        public async static Task<String> ImageUpload(string hostrootpath, IFormFile file, string oldimagepath = null)
        {
            try
            {
                if (file != null)
                {
                    if (file.Length > 0)
                    {
                        var imageExtension = Path.GetExtension(file.FileName);

                        if (imageExtension == ".jpg" || imageExtension == ".png" || imageExtension == ".jpeg"
                            ||
                         imageExtension == ".JPG" || imageExtension == ".PNG" || imageExtension == ".JPEG"
                            ||
                        imageExtension == "svg" || imageExtension == "SVG")
                        {
                           
                            var randomFileName = Guid.NewGuid().ToString();
                            var filename = Path.ChangeExtension(randomFileName, ".jpg");
                            var path = Path.Combine(hostrootpath,"wwwroot","uploadimages", filename);
                          
                            if (oldimagepath != null) 
                            {
                                 System.IO.File.Delete(Path.Combine(hostrootpath, "wwwroot", oldimagepath.Remove(0,1)));
                            }
                
                            using (var stream = new FileStream(path, FileMode.OpenOrCreate))
                            {
                                await file.CopyToAsync(stream);
                            }
                            //FileInfo filecompress = new FileInfo(path);
                            //var optimizer = new ImageOptimizer();
                            //bool isSuccess = optimizer.LosslessCompress(filecompress.FullName);
                          

                            //filecompress.Refresh();
                            return $"/uploadimages/{filename}";
                        }
                        return "Uzantı Hatası";
                    }
                    return "Boyut Hatası";
                }
                return "Null Hatası";
            }
            catch (Exception e)
            {
                var a = e.Message;
                return "Exception Hatası";
            }
        }
    }
}
