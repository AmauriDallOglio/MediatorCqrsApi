using System.ComponentModel.DataAnnotations;

namespace MediatorCqrsApi.Aplicacao.DML.Notification
{
    public interface INotificationContexto
    {
        IReadOnlyCollection<Notification> Notifications { get; }
        bool HasNotifications { get; }

        void AddNotification(string key, string message);
        void AddNotification(Notification notification);
        void AddNotifications(IList<Notification> notifications);
        void AddNotifications(ICollection<Notification> notifications);
        void AddNotifications(ValidationResult validationResult);
    }
}
