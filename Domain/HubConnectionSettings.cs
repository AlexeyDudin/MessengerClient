namespace Domain
{
    public class HubConnectionSettings
    {
        public HubConnectionSettings() { }
        public HubConnectionSettings(string subscribeUrl = "", string addFuncName = "", string editFuncName = "", string deleteFuncName = "")
        {
            SubscribeUrl = subscribeUrl;
            AddFuncName = addFuncName;
            EditFuncName = editFuncName;
            DeleteFuncName = deleteFuncName;
        }

        public string SubscribeUrl { get; set; } = string.Empty;
        public string AddFuncName { get; set; } = string.Empty;
        public string EditFuncName { get; set; } = string.Empty;
        public string DeleteFuncName { get; set; } = string.Empty;
    }
}
