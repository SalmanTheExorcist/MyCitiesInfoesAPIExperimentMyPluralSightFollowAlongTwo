namespace MyCitiesInfo.API.Services
{
    public class MyCloudMailService: IMyMailService
    {
        private string _mailTo = string.Empty;
        private string _mailFrom = string.Empty;

        public MyCloudMailService(IConfiguration configuration)
        {
            _mailTo = configuration["myMailSettings:myMailToAddress"];
            _mailFrom = configuration["myMailSettings:myMailFromAddress"];
        }

        public void SendMail(string mailSubject, string mailMessage)
        {
            //--Sending mail output to console window.
            Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with {nameof(MyCloudMailService)}.");
            Console.WriteLine($"Subject: {mailSubject}");
            Console.WriteLine($"Message: {mailMessage}");



        }//--End-SendMail()
    }
}
