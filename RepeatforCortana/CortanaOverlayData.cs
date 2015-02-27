namespace RepeatforCortana
{
    class CortanaOverlayData
    {
        public CortanaOverlayData(string Title, string Message)
        {
            this._title = Title;
            this._message = Message;
        }

        public string _title { get; set; }
        public string _message { get; set; }
    }
}
