using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class ValuteGetterService
    {
        private readonly IRequestService requestService;
        private string responce;

        public ValuteGetterService(IRequestService requestService)
        {
            this.requestService = requestService;
        }
        public async Task<bool> ReloadAsync()
        {
            try
            {
                var stream = requestService.ExecuteUrl("https://www.cbr.ru/currency_base/daily/");
                var parser = new HtmlParser();

                var document = await parser.ParseDocumentAsync(stream);

                valutesToRub.Clear();

                foreach (IHtmlTableRowElement element in document.QuerySelectorAll("table")[0].Children[0].Children.Skip(1))
                {

                    valutesToRub.Add(element.Cells[1].InnerHtml, double.Parse(element.Cells[4].InnerHtml));
                }
                stream.Close();
                return true;
            }
            catch(Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }
        public string Message { get; set; }
        public double GetValute(string valute, double value = 1)
        {
            if(valutesToRub.ContainsKey(valute))
                return value / valutesToRub[valute];
            return 0;
        }
        public double GetUSDValue(double value)
        {
            return GetValute("USD", value);
        }
        public double GetEuroValue(double value)
        {
            return GetValute("EUR", value);
        }

        Dictionary<string, double> valutesToRub = new Dictionary<string, double>();
    }
}
