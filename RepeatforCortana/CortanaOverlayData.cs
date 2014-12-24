namespace RepeatforCortana
{
    class CortanaOverlayData
    {
        public CortanaOverlayData(string Title, string Message)
        {
            this._title = Title;
        }
        private string _title { get; set; }
        private string _message { get; set; }
    }
}
