namespace UserProcessor
{
    public static class CreateUserRequestBody
    {
        public static string GetRegistrationBody(string login, string password)
        {
            return $@"
            {{
                ""inOrder"": false,
                ""anonymOrder"": false,
                ""login"": ""{login}"",
                ""password"": ""{password}"",
                ""fName"": null,
                ""fISIC"": null,
                ""fStreet"": null,
                ""fCity"": null,
                ""fZip"": null,
                ""fPhone"": ""+420 123 123 123"",
                ""fEmail"": null,
                ""cBic"": null,
                ""cIban"": null,
                ""bankAccountOwnerName"": null,
                ""cIco"": null,
                ""cDic"": null,
                ""cIcDph"": null,
                ""cBankAccount"": null,
                ""cBankCode"": null,
                ""cSpecificSymbol"": null,
                ""cInternalNumber"": null,
                ""dName"": null,
                ""dFirm"": null,
                ""dStreet"": null,
                ""dCity"": null,
                ""dZip"": null,
                ""dNote"": null,
                ""dPhone"": null,
                ""iNewsNoNews"": false,
                ""segments"": [],
                ""smsCode"": null,
                ""eduId"": null,
                ""cSodexoId"": null,
                ""emailHash"": """",
                ""isRegistration"": true,
                ""validateTaxIdentificationNumber"": null
            }}";
        }
    }
}
