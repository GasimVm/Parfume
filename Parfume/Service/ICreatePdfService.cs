using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Service
{
   public interface ICreatePdfService
    {
        public string CreateHTML(int orderId);
        public string CreateCSS();
        public void CreatePdf(string css,string html,int orderId );

      
    }
}
