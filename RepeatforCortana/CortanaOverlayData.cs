namespace RepeatforCortana
{
    class CortanaOverlayData
    {
        public CortanaOverlayData(string Title, string Message)
        {
            this._title = Title;
            this._message = Message;
        }

        private string _title { get; set; }
        private string _message { get; set; }
    }
}
