namespace AzureAD.B2C.BuildTask.Helpers
{
    using AzureAD.B2C.BuildTask.Commons;
    using AzureAD.B2C.BuildTask.Modles;
    using Newtonsoft.Json;
    using System.Linq;
    using System.Net.Http;

    public class KeysetHelper
    {
        private readonly GraphClientHelper _graphHelper;

        private readonly string TOKEN_SGNING_KEY_CONTAINER = "B2C_1A_TokenSigningKeyContainer";
        private readonly string TOKEN_ENCRYPTION_KEY_CONTAINER = "B2C_1A_TokenEncryptionKeyContainer";

        private readonly string KEYSET_API_ENDPOINT = "/trustFramework/keySets";

        public KeysetHelper(GraphClientHelper graphClientHelper)
        {
            _graphHelper = graphClientHelper;
        }

        private GenerateKey GenerateKeyModel(string use, string kty)
        {
            return new GenerateKey
            {
                Use = use,
                Kty = kty
            };
        }

        private void CreateAndGenerateKeyset(string keysetId, bool isSignature)
        {
            Common.RaiseConsoleMessage(LogType.DEBUG, $"Keyset : Creating {keysetId}", true);
            #region CreateKeyset
            //Create Key set
            var createModel = new KeySet { Id = keysetId.Split('_')[2] };
            var content = JsonConvert.SerializeObject(createModel);
            _graphHelper.SendGraphPostRequest(HttpMethod.Post, KEYSET_API_ENDPOINT, content, isJson: true).GetAwaiter().GetResult();
            Common.RaiseConsoleMessage(LogType.DEBUG, $"Keyset : Successfully Created {keysetId}", true);
            #endregion
            #region GenerateKeyset
            //Generate Key Set
            Common.RaiseConsoleMessage(LogType.DEBUG, $"Keyset : Generating Keyset value ->  {keysetId}", true);
            var usePropValue = isSignature ? "sig" : "enc";
            var postContent = JsonConvert.SerializeObject(GenerateKeyModel(usePropValue, "RSA"));
            _graphHelper.SendGraphPostRequest(HttpMethod.Post, $"{KEYSET_API_ENDPOINT}/{keysetId}/generateKey", postContent, isJson: true).GetAwaiter().GetResult();
            Common.RaiseConsoleMessage(LogType.DEBUG, $"Keyset : Successfully Generated Keyset value ->  {keysetId}", true);
            #endregion
        }

        public void HandleKeysets()
        {
            Common.RaiseConsoleMessage(LogType.DEBUG, $"Keyset : Create and Generate Keyset", false);
            Common.RaiseConsoleMessage(LogType.DEBUG, $"Keyset : Fetching Keyset", true);
            var keygenresp = _graphHelper.SendGraphPostRequest(HttpMethod.Get, KEYSET_API_ENDPOINT, null, false).GetAwaiter().GetResult();
            var deserializedKeysetModel = JsonConvert.DeserializeObject<OdataModel<KeySet>>(keygenresp);
            if (deserializedKeysetModel?.Values?.Count > 0)
            {
                Common.RaiseConsoleMessage(LogType.DEBUG, $"Keyset : Found  {deserializedKeysetModel?.Values?.Count} Keyset", true);
                if (!deserializedKeysetModel.Values.Any(x => x.Id == TOKEN_SGNING_KEY_CONTAINER))
                {
                    Common.RaiseConsoleMessage(LogType.DEBUG, $"Keyset : Found  {TOKEN_SGNING_KEY_CONTAINER} Not Found", true);
                    CreateAndGenerateKeyset(TOKEN_SGNING_KEY_CONTAINER, isSignature: true);
                }
                else if (!deserializedKeysetModel.Values.Any(x => x.Id == TOKEN_ENCRYPTION_KEY_CONTAINER))
                {
                    Common.RaiseConsoleMessage(LogType.DEBUG, $"Keyset : Found  {TOKEN_ENCRYPTION_KEY_CONTAINER} Not Found", true);
                    CreateAndGenerateKeyset(TOKEN_ENCRYPTION_KEY_CONTAINER, isSignature: false);
                }
            }
            else
            {
                Common.RaiseConsoleMessage(LogType.DEBUG, $"Keyset : Keysets Not Found", true);
                CreateAndGenerateKeyset(TOKEN_SGNING_KEY_CONTAINER, isSignature: true);
                CreateAndGenerateKeyset(TOKEN_ENCRYPTION_KEY_CONTAINER, isSignature: false);
            }
        }
    }
}
