using DAL;
using DAL.Dto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using word = Microsoft.Office.Interop.Word;

namespace BL
{
    public class WordService
    {
        public string Message { get; private set; }
        public void ShowOrderContract(OrderDto orderDto, ValuteGetterService valuteGetter, string path)
        {
            Dictionary<string, string> stubs = new Dictionary<string, string>();


            DateTime endDate = orderDto.TourDto.StartDate.AddDays(orderDto.TourDto.DaysCount);
            StringBuilder insStr = new StringBuilder();
            StringBuilder placementsStr = new StringBuilder();
            

            stubs.Add("[Номер_заказа]", orderDto.Id.ToString());
            stubs.Add("[День]", orderDto.CreationDate.ToString("dd"));
            stubs.Add("[Месяц]", orderDto.CreationDate.ToString("MMMM"));
            stubs.Add("[Год]", orderDto.CreationDate.ToString("yyyy"));
            stubs.Add("[ФИО]", orderDto.ClientDto.Name);
            stubs.Add("[Дата_с]", orderDto.TourDto.StartDate.ToString("dd.MM.yyyy"));
            stubs.Add("[Дата_по]", endDate.ToString("dd.MM.yyyy"));
            stubs.Add("[Кол-во_дней]", orderDto.TourDto.DaysCount.ToString());
            stubs.Add("[Кол-во_туристов]", orderDto.PeopleCount.ToString());

            placementsStr.AppendLine("Туристы:");

            foreach (var pl in orderDto.PlacementDtos)
            {
                string doc = !pl.IsChildBefore14 ? pl.Pasport : pl.BirthDoc;
                string docName = !pl.IsChildBefore14 ? "п-т" : "св-во. рожд.";
                placementsStr.AppendLine($"{pl.Fio}, {docName}: {doc}, к-та №: {pl.CabinDto.Id}, п: {pl.CabinDto.Deck}");
            }


            if (orderDto.HasIns)
            {
                insStr.AppendLine("Страховка: ");
                foreach (var ins in orderDto.InsuranceDtos)
                {
                    insStr.AppendLine(ins.Name);
                }
                stubs.Add("[Страховка]", insStr.ToString());
            }
            else 
                stubs.Add("[Страховка]", "нет");


            stubs.Add("[Туристы]", placementsStr.ToString());

            stubs.Add("[Общая_стоимость]", orderDto.FullCost.ToString("0.##"));
            stubs.Add("[Общая_стоимостьUSD]", valuteGetter.GetUSDValue(orderDto.FullCost).ToString("0.##"));
            stubs.Add("[Общая_стоимостьEUR]", valuteGetter.GetEuroValue(orderDto.FullCost).ToString("0.##"));

            ShowDocument(stubs, path);

        }
        public const int CUT_PART = 8;
        private bool ShowDocument(Dictionary<string, string> stubs, string path)
        {
            var wordApp = new word.Application();
            try
            {
                wordApp.Visible = false;


                var newDoc = wordApp.Documents.Add();
                newDoc.Content.InsertFile(path);
                //newDoc.Content.Font.Name = "Times New Roman";
                //Console.WriteLine(newDoc.Content.PageSetup.LeftMargin);
                //newDoc.Content.PageSetup.LeftMargin = 56.7f;
                //newDoc.Content.PageSetup.RightMargin = 56.7f;

                foreach ( var stub in stubs )
                {
                    //if (stub.Value?.Length > 30)
                    //{
                    //    int cuttedL = stub.Value.Length / CUT_PART;

                    //    int ost = stub.Value.Length % CUT_PART;

                    //    string part = stub.Value.Substring(0, cuttedL + ost);
                    //    ReplaceWordStub($"{stub}1", part, newDoc);

                    //    for (int i = 1; i < CUT_PART; i++)
                    //    {
                    //        part = stub.Value.Substring(i * cuttedL, cuttedL);
                    //        ReplaceWordStub($"{stub}{i + 1}", part, newDoc);
                    //    }
                    //}
                    //else
                    //{
                        ReplaceWordStub(stub.Key, stub.Value, newDoc);
                    //}
                }
                wordApp.Visible = true;
            }
            catch ( Exception ex )
            {
                Message = ex.Message;
                return false;
            }
            finally
            {
                wordApp.Quit();
            }
            return true;
        }
        private void ReplaceWordStub(string stub, string value, word.Document document)
        {
            var range = document.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: stub, ReplaceWith: value);
        }
    }
}
