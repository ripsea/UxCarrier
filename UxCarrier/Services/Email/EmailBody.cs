namespace UxCarrier.Services.Email
{
    public class EmailBody
    {
        private readonly IViewRenderService _viewRenderService;
        public EmailBody(IViewRenderService viewRenderService)
        {
            _viewRenderService = viewRenderService;
        }

        public string VerifyCode { get; set; }

        public string TemplateItem { get; set; }

        public enum EmailTemplate
        {
            VerifyCode = 0,
        }

        public void SetTemplateItem(string item)
        {
            TemplateItem = item;
        }

        public string GetTemplateView()
        {
            if (string.IsNullOrEmpty(TemplateItem))
            {
                return string.Empty;
            }

            //return @$"~/Views/Email/{TemplateItem}.cshtml";
            return @$"~/Views/Home/{TemplateItem}.cshtml";
        }

        public async Task<string> GetViewRenderString()
        {
            var aaa = GetTemplateView();
            return await _viewRenderService.RenderToStringAsync(
                viewName: GetTemplateView(),
                model: this);
        }
    }
}
