using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Parfume.DAL;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Parfume.Service
{
    public class CreatePdfService:ICreatePdfService
    {
        private readonly ParfumeContext _context;
        private readonly IHostingEnvironment _env;
     
        public CreatePdfService(ParfumeContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public string CreateHTML(int orderId )
        {
            var orderDb = _context.Orders.Where(c => c.Id == orderId).Include(c=>c.Customer).FirstOrDefault();
            var customerName = orderDb.Customer.Name;
            var customersurname = orderDb.Customer.Surname;
            var customerFathername = orderDb.Customer.FatherName;
            var customerFincode = orderDb.Customer.Fincode;
            var customerBaseNumber = orderDb.Customer.BaseNumber;
            var customerAddress = orderDb.Customer.Address;
            var customerWorkAddress = orderDb.Customer.WorkAddress;
            
            var orderInfoName = orderDb.Name;
            var orderInfoPrice = orderDb.Price;
            var orderInfoAmount = orderDb.Amount;
            var orderInfoQuantity = orderDb.Quantity;
            var orderInfoTotal = orderDb.TotalPrice;
            var FirstPrice = orderDb.FirstPrice;

            var orderInfoMounth = orderDb.MonthPrice;
            var date = DateTime.Now.ToString("dd/MM/yyyy");





            string htmlCustomer = $@"<div class=""contairner"">"+
                $@"<h3 class=""mb-3 text-center"">Müştəri məlumatları:</h3>"+
                $@"<div class=""row text-center"">"+
                 $@"<div class=""col-2"">"+
            $@"<p>Adı,soyadı,ata adı {customerName},{customersurname},{customerFathername}</p></div>"+
                 $@"<div class=""col-2"">"+
            $@"<p>Fin kod: {customerFincode}</p></div>"+
                 $@"<div class=""col-2"">"+
            $@"<p>Nömrəsi: {customerBaseNumber}</p></div>"+
                 $@"<div class=""col-2"">"+
            $@"<p>Ünvanı: {customerAddress}</p></div>"+
                 $@"<div class=""col-2"">"+
            $@"<p>İş yeri {customerWorkAddress}</p></div>"+
        $@"</div></div>";



            string htmlOrder = $@"<div class=""contairner"">"+
                $@"<h3 class=""mb-3 text-center"">Mal məlumatları:</h3>"+
                $@"<div class=""row text-center"">"+
                 $@"<div class=""col-2"">"+
            $@"<p>Malın adı: {orderInfoName}</p></div>"+
                 $@"<div class=""col-2"">"+
            $@"<p>Miqdarı {orderInfoQuantity}</p></div>"+
                 $@"<div class=""col-2"">"+
            $@"<p>Sayı: {orderInfoAmount}</p></div>" +
                 $@"<div class=""col-2"">"+
            $@"<p>Qiyməti: {orderInfoPrice}</p></div>" +
                 $@"<div class=""col-2"">"+
            $@"<p>Total qiymət: {orderInfoTotal}</p></div>"+
                 $@"<div class=""col-2"">"+
            $@"<p>Aylıq ödəniş: {orderInfoMounth}</p></div>"+
            $@"<p>İlkin ödəniş: {FirstPrice}</p></div>" +
            $@"<p>Tarix: {date}</p>"+
        $@"</div></div>";

            return htmlCustomer + htmlOrder;
        }

        public string CreateCSS()
        {
            var example_css = @".contairner { font-family: Arial, Helvetica, sans-serif; }";
            return example_css;

        }

        public void CreatePdf(string css, string html, int orderId)
        {
            Byte[] bytes;
            using (var ms = new MemoryStream())
            {
                //Create an iTextSharp Document which is an abstraction of a PDF but **NOT** a PDF
                using (var doc = new iTextSharp.text.Document(PageSize.A4, 40, 20, 40, 80))
                {

                    //Create a writer that's bound to our PDF abstraction and our stream
                    using (var writer = PdfWriter.GetInstance(doc, ms))
                    {
                        //Open the document for writing
                        doc.Open();

                        doc.NewPage();
                        doc.NewPage();
                        doc.NewPage();
                        doc.NewPage();
                        doc.Add(new Chunk(""));
                        //Our sample HTML and CSS

                        //Below we convert the strings into UTF8 byte array and wrap those in MemoryStreams
                        using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(css)))
                        {
                            using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(html)))
                            {
                                //Parse the HTML
                                iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, msCss);
                            }
                        }
                        doc.Close();
                    }
                }
                bytes = ms.ToArray();
            }

            
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+$"{"//"+orderId.ToString()+ "müqavilə.pdf"}";
            System.IO.File.WriteAllBytes(path, bytes);
           
        }

        
    }
}
