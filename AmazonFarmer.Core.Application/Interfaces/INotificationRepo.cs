using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Domain.Entities;

namespace AmazonFarmer.Core.Application.Interfaces // Defining namespace for the interface
{
    public interface INotificationRepo // Defining the interface for handling 
    {
        /// <summary>
        /// Retrieves the first active email notification of a specified type.
        /// </summary>
        /// <param name="type">The type of the email notification to retrieve.</param>
        /// <returns>
        /// A <see cref="NotificationDTO"/> object containing the title and body of the first matching active email notification,
        /// or <c>null</c> if no matching notification is found.
        /// </returns>
        /// <remarks>
        /// This method uses Entity Framework Core to asynchronously fetch the first email notification matching the specified
        /// type and active status. It projects the result into a <see cref="NotificationDTO"/> object, which includes the
        /// notification's title and body.
        /// </remarks>
        Task<NotificationDTO> getNotificationByENotificationBody(ENotificationBody type);
        Task<NotificationDTO> getNotificationByENotificationBody(ENotificationBody type, string languageCode);
        /// <summary>
        /// Retrieves a paginated list of device notifications for a given user.
        /// </summary>
        /// <param name="userID">The ID of the user for whom to retrieve notifications.</param>
        /// <param name="skip">The number of notifications to skip for pagination.</param>
        /// <param name="take">The number of notifications to take for pagination.</param>
        /// <returns>
        /// A list of <see cref="DeviceNotificationDTO"/> containing the notifications with their body and type ID.
        /// </returns>
        /// <remarks>
        /// This method fetches notifications for the specified user, including the associated device notifications.
        /// It includes related <see cref="tblDeviceNotifications"/> entities to provide complete notification details.
        /// The results are ordered by descending notification ID, and pagination is applied using the <paramref name="skip"/> and <paramref name="take"/> parameters.
        /// Notifications are projected into a <see cref="DeviceNotificationDTO"/> to include the body and type of each device notification.
        /// </remarks>
        Task<List<DeviceNotificationDTO>> getNotificationByUserID(string userID, int skip, int take);

        /// <summary>
        /// Retrieves a list of device notifications for a given user in a specified language.
        /// </summary>
        /// <param name="userID">The ID of the user for whom to retrieve notifications.</param>
        /// <param name="languageCode">The language code used to filter the device notification translations.</param>
        /// <param name="skip">Number of notifications to skip for pagination.</param>
        /// <param name="take">Number of notifications to take for pagination.</param>
        /// <returns>A list of <see cref="DeviceNotificationDTO"/> containing the notifications with their body (translated if available) and type ID.</returns>
        /// <remarks>
        /// This method fetches all notifications for the specified user, including the associated device notifications and their translations.
        /// It includes related <see cref="tblDeviceNotifications"/> and <see cref="tblDeviceNotificationTranslation"/> entities to provide complete notification details.
        /// The notifications are projected into a <see cref="DeviceNotificationDTO"/> to include the translated body text (if available) and the type of each device notification.
        /// If a translation for the specified language code is not found, the default body text of the device notification is used.
        /// </remarks>
        Task<List<DeviceNotificationDTO>> getNotificationByUserID(string userID, string languageCode, int skip, int take);
        IQueryable<tblNotification> queryableNotificationByUserID(string userID, string languageCode);
        IQueryable<tblNotification> queryableNotificationByUserID(string userID);
        Task<tblNotification> getNotificationByNotificationID(int NotificationID, string UserID);
        void markNotificationAsRead(tblNotification Notification);
        Task<tblDeviceNotifications?> getDeviceNotificationByType(EDeviceNotificationType type);
        Task<List<tblDeviceNotifications>> getDeviceNotificationByType(List<EDeviceNotificationType> type);
        void addDeviceNotification(tblNotification notification);
    }
}
