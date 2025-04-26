namespace TradeApiCaller.RegistrationNumber
{
    public static class RegNumbersData
    {
        public static readonly Dictionary<RegistrationNumberType, List<string>> RegNumbers = new()
        {
            { RegistrationNumberType.VatPayer, new List<string>()
                {
                    "01781448","24269981","25087029","25876163","26687933","26809061","02300389","05340772","05718473",
                    "08949140","26555964","64626776","26852080","47252171","27177726","28570847","06375472","63025710",
                    "28972350","26194627","25835793","27329763","08798729","19361343","02110822","48400947","06040616",
                    "08978671","09896627","25617834","21412294","48028321","28779371","07387156"
                }
            },
            { RegistrationNumberType.VatPayerWithMultipleBankAccounts, new List<string>()
                {
                    "26856816","18941117","25785915","25523431","25818732","06340512","48395013","05421845","02343746",
                    "26835002","28540174"
                }
            },
            { RegistrationNumberType.VatPayerWithoutBankAccount, new List<string>()
                {
                    "03514790", "47593377"
                }
            },
            { RegistrationNumberType.NoVatPayer, new List<string>()
                {
                    "24655767","60448946","64631176","65348095","65349881","67007546","67984550","02476266","05929369",
                    "05923921","28788851","29452872","08940100","29100437","22610791","22853405","07892799"
                }
            },
            { RegistrationNumberType.WithoutAddress, new List<string>()
                {
                    "60432781","60446366","27042642","02496267","67106048","27042642","02496267"
                }
            }
        };
    }
}
