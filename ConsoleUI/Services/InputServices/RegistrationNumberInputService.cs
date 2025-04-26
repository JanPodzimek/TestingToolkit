using ConsoleUI.Helpers;
using ConsoleUI.Interfaces;
using Serilog;
using TradeApiCaller.RegistrationNumber;

namespace ConsoleUI.Services.InputServices
{
    public class RegistrationNumberInputService(
        IMenuService<RegistrationNumberType> registrationNumberMenuService) : IInputService
    {
        public async Task Run(HttpClient httpClient)
        {
            string regNumber = string.Empty;
            int index = 0;
            bool isRegistered = false;
            Random rnd = new Random();

            var selectedMenuOption = registrationNumberMenuService.ShowMenu();

            List<string> regNumbers = RegNumbersData.RegNumbers[selectedMenuOption];
            //List<string> regNumbers = new List<string>() { "27082440", "86611321", "74247611", "05493684", "26555964", "70766606", "27082440", "26204967", "28476654", "05823919", "01418467", "27082440", "28476654", "64938336", "25655701", "06216609", "05677017", "27082440", "01283103", "75529921", "74088823", "27082440", "27082440", "28476654", "27082440", "28476654", "28476654", "28476654", "01418467", "88344975", "04617550", "27082440", "01392620", "87915367", "27082440", "60182156", "69355860", "26061741", "25870262", "24699829", "06769691", "71895817", "41399790", "25922246", "76643107", "05964857", "47706872", "24826022", "18530010", "73375136", "25624075", "87909944", "02953293", "25351117", "07117621", "03924424", "03233090", "27227201", "73305251", "01563653", "01516400", "29414938", "07829612", "27573541", "06516581", "07518358", "06183638", "05052025", "24184942", "28263731", "48421936", "05860601", "26128233", "06552901", "87601818", "06961801", "05798370", "05523869", "08532451", "27394701", "06640061", "09001697", "05355257", "25723804", "24795097", "27336930", "02879808", "25048023", "07649576", "64169375", "08351325", "28595378", "01991922", "28825632", "27068188", "08624186", "01844059", "05677017", "08830291", "27082440", "27184421" };

            Log.Logger.Information("Looking for available registration number...");

            do
            {
                index = rnd.Next(regNumbers.Count);
                regNumber = regNumbers[index];

                if (regNumbers.Count > 0)
                {
                    isRegistered = await RegNumberVerifier.CheckRegNumber(httpClient, regNumber);

                    if (isRegistered)
                    {
                        regNumbers.RemoveAt(index);
                    }
                }
            } while (isRegistered == true && regNumbers.Count > 0);

            if (isRegistered == false)
            {
                Console.WriteLine();
                Log.Logger.Information("========");
                Log.Logger.Information("{RegNumber} ", regNumber);
                Log.Logger.Information("========");
                Log.Logger.Information("");
                Log.Logger.Information("{numberOfAvailableRegNumbers} registration numbers still available (of this type)", regNumbers.Count);
            }
            else
            {
                Console.WriteLine();
                Log.Logger.Warning("=================================================");
                Log.Logger.Warning("No registration number of this type is available.");
                Log.Logger.Warning("=================================================");
            }

            InputHelper.End();
        }
    }
}
