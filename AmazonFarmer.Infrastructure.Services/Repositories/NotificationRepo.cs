using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    public class NotificationRepo : INotificationRepo
    {
        private AmazonFarmerContext _context;

        /// <summary>
        /// Constructor to initialize the NotificationRepo with the AmazonFarmerContext.
        /// </summary>
        /// <param name="context">The AmazonFarmerContext for database operations.</param>
        public NotificationRepo(AmazonFarmerContext context)
        {
            _context = context;
        }
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
        public async Task<NotificationDTO> getNotificationByENotificationBody(ENotificationBody type)
        {
            // Use Entity Framework's asynchronous method FirstOrDefaultAsync to retrieve the first notification matching the criteria
            return await _context.EmailNotificationTranslations
                .Include(x=>x.Notification)
                // Filter notifications based on the provided type and status
                .Where(x =>
                    x.Notification.Type == type && // Filter by notification type
                    x.Notification.Status == EActivityStatus.Active &&// Filter by notification status
                    x.LanguageCode == "EN"
                )
                // Project the filtered notifications into a new NotificationDTO object
                .Select(
                    x => new NotificationDTO
                    {
                        // Assign values from the EmailNotification entity to the properties of NotificationDTO
                        title = x.Title, // Assign notification title
                        body = x.Body, // Assign notification body
                        smsBody = x.SMSBody, // Assign notification body
                        fcmBody = x.FCMBody, // Assign notification body
                        deviceBody = x.DeviceBody, // Assign notification body
                    }
                )
                // Retrieve the first matching notification asynchronously
                .FirstOrDefaultAsync();
        }
        public async Task<NotificationDTO> getNotificationByENotificationBody(ENotificationBody type, string languageCode)
        {
            // Use Entity Framework's asynchronous method FirstOrDefaultAsync to retrieve the first notification matching the criteria
            return await _context.EmailNotificationTranslations
                .Include(x => x.Notification)
                // Filter notifications based on the provided type and status
                .Where(x =>
                    x.Notification.Type == type && // Filter by notification type
                    x.Notification.Status == EActivityStatus.Active &&// Filter by notification status
                    x.LanguageCode == languageCode
                )
                // Project the filtered notifications into a new NotificationDTO object
                .Select(
                    x => new NotificationDTO
                    {
                        // Assign values from the EmailNotification entity to the properties of NotificationDTO
                        title = x.Title, // Assign notification title
                        body = x.Body, // Assign notification body
                        smsBody = x.SMSBody, // Assign notification body
                        fcmBody = x.FCMBody, // Assign notification body
                        deviceBody = x.DeviceBody, // Assign notification body
                    }
                )
                // Retrieve the first matching notification asynchronously
                .FirstOrDefaultAsync();
        }
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
        public async Task<List<DeviceNotificationDTO>> getNotificationByUserID(string userID, int skip, int take)
        {
            return await _context.Notifications
                // Include related DeviceNotification entities
                .Include(x => x.Notification)
                // Order notifications by descending ID to get the latest notifications first
                .OrderByDescending(x => x.ID)
                // Apply pagination by skipping the specified number of records
                .Skip(skip)
                // Limit the number of records retrieved based on the 'take' parameter
                .Take(take)
                // Filter notifications by the specified UserID
                .Where(x => x.UserID == userID)
                // Project the filtered notifications into DeviceNotificationDTO objects
                .Select(s => new DeviceNotificationDTO
                {
                    // Assign the body of the device notification
                    //body = s.DeviceNotification.Body,
                    // Assign and cast the type of the device notification to an integer
                    //typeID = (int)s.DeviceNotification.Type
                })
                // Execute the query and convert the results to a list asynchronously
                .ToListAsync();
        }

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
        public async Task<List<DeviceNotificationDTO>> getNotificationByUserID(string userID, string languageCode, int skip, int take)
        {
            return await _context.Notifications
                // Include related DeviceNotification entities
                .Include(x => x.Notification)
                // Include related DeviceNotificationTranslations entities
                .ThenInclude(x => x.EmailNotificationTranslations)
                // Order notifications by ID in descending order
                .OrderByDescending(x => x.ID)
                // Skip the specified number of notifications for pagination
                .Skip(skip)
                // Take the specified number of notifications for pagination
                .Take(take)
                // Filter notifications by the specified UserID
                .Where(x => x.UserID == userID)
                // Project the results into DeviceNotificationDTO objects
                .Select(s => new DeviceNotificationDTO
                {
                    // Select the translated body text if available; otherwise, use the default body text
                    body = s.Notification.EmailNotificationTranslations
                                .Where(t => t.LanguageCode == languageCode)
                                .Select(t => t.DeviceBody)
                                .FirstOrDefault() ?? string.Empty,
                    // Select and cast the type of the device notification to an integer
                    typeID = (int)s.Notification.Type
                })
                // Execute the query and convert the results to a list asynchronously
                .ToListAsync();
        }
        public IQueryable<tblNotification> queryableNotificationByUserID(string userID, string languageCode)
        {
            return _context.Notifications
                // Include related DeviceNotification entities
                .Include(x => x.Notification)
                // Include related DeviceNotificationTranslations entities
                .ThenInclude(x => x.EmailNotificationTranslations.Where(x => x.LanguageCode == languageCode))
                .Include(x=>x.Warehouse).ThenInclude(x=>x.WarehouseTranslation.Where(x=>x.LanguageCode == languageCode))
                .Include(x=>x.Reason).ThenInclude(x=>x.ReasonTranslation.Where(x=>x.LanguageCode == languageCode))
                .Include(x=>x.User)
                // Filter notifications by the specified UserID
                .Where(x => x.UserID == userID);
        }
        public IQueryable<tblNotification> queryableNotificationByUserID(string userID)
        {
            return _context.Notifications
                // Include related DeviceNotification entities
                .Include(x => x.Notification).ThenInclude(x=>x.EmailNotificationTranslations)
                // Order notifications by descending ID to get the latest notifications first
                .OrderByDescending(x => x.ID);
        }
        public async Task<tblNotification> getNotificationByNotificationID(int NotificationID, string UserID)
        {
            return await _context.Notifications.Where(x => x.ID == NotificationID && x.UserID == UserID).FirstOrDefaultAsync();
        }
        public void markNotificationAsRead(tblNotification Notification)
        {
            _context.Notifications.Update(Notification);
        }
        public async Task<tblDeviceNotifications?> getDeviceNotificationByType(EDeviceNotificationType type)
        {
            return await _context.DeviceNotification.Include(x => x.DeviceNotificationTranslations).Where(x => x.Type == type).FirstOrDefaultAsync();
        }
        public async Task<List<tblDeviceNotifications>> getDeviceNotificationByType(List<EDeviceNotificationType> type)
        {
            return await _context.DeviceNotification.Include(x => x.DeviceNotificationTranslations).Where(x => type.Contains(x.Type)).ToListAsync();
        }
        public void addDeviceNotification(tblNotification notification)
        {
            _context.Notifications.Add(notification);
        }


    }
}
