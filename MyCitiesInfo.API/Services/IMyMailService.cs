namespace MyCitiesInfo.API.Services
{
    public interface IMyMailService
    {
        void SendMail(string mailSubject, string mailMessage);
    }
}