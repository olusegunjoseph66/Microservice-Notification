using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Notifications.Application.Constants;
using Notifications.Application.DTOs.Events;
using Notifications.Application.DTOs.Filters;
using Notifications.Application.DTOs.Sortings;
using Notifications.Application.Enums;
using Notifications.Application.Interfaces.Services;
using Notifications.Application.ViewModels.QueryFilters;
using Notifications.Application.ViewModels.Requests;
using Notifications.Application.ViewModels.Responses;
using Notifications.Infrastructure.QueryObjects;
using Shared.Data.Extensions;
using Shared.Data.Models;
using Shared.Data.Repository;
using Shared.ExternalServices.Interfaces;
using Shared.Utilities.DTO.Pagination;
using Shared.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Infrastructure.Services
{
    public class NotificationsService : BaseService, INotificationsService
    {
        private readonly IAsyncRepository<Shared.Data.Models.Notification> _repository;
        private readonly IAsyncRepository<Shared.Data.Models.Unsubscribe> _unsubsribeRepository;
        private readonly IAsyncRepository<Shared.Data.Models.UserNotification> _userNotificationRepository;
        private readonly IAsyncRepository<Shared.Data.Models.User> _userRepository;


        public readonly IDmsAzureMessageBus _messageBus;
        public NotificationsService(IAuthenticatedUserService authenticatedUserService, IAsyncRepository<Shared.Data.Models.Unsubscribe> unsubsribeRepository, IAsyncRepository<Shared.Data.Models.Notification> repository, IAsyncRepository<Shared.Data.Models.UserNotification> userNotificationRepository , IAsyncRepository<Shared.Data.Models.User> userRepository, IDmsAzureMessageBus messageBus) : base(authenticatedUserService)
        {
            _unsubsribeRepository = unsubsribeRepository;
            _repository = repository;
            _messageBus= messageBus;
            _userNotificationRepository = userNotificationRepository;
            _userRepository = userRepository;
        }

        public async Task<NotificationSaveResponse> SaveNotificationSettings(IEnumerable<NoticationSettingsSaveRequest> noticationSettings, CancellationToken cancellationToken)
        {
            var userId = _authenticatedUserService.UserId;
            if(userId < 1)
            {
                userId = 2;
            }
            var response = new NotificationSaveResponse();
            try
            {
                if (!noticationSettings.Any())
                {
                    response.StatusCode = ErrorCodes.SERVER_ERROR_CODE;
                    response.Status = Status.DEFAULT_FAILURE;
                    response.Message = ErrorMessages.EMPTY_LIST_OF_NOTIFICATIONS_SETTINGS;
                    return response;
                }
                else
                {
                    var unsubsubscribes = await _unsubsribeRepository.Table.Where(p => p.UserId == _authenticatedUserService.UserId).ToListAsync();
                    if (unsubsubscribes.Any())
                    {
                        await _unsubsribeRepository.DeleteRange(unsubsubscribes);
                    }
                    foreach (var notification in noticationSettings)
                    {
                        if (notification.Subscribed == false)
                        {
                            await _unsubsribeRepository.AddAsync(new Shared.Data.Models.Unsubscribe { DateUnsubscribed = DateTime.Now, NotificationId = notification.NotificationId, UserId = userId });
                        }
                    }

                    //var messageObject = new NotificationPublishMessage
                    //{
                    //    DateCreated = DateTime.UtcNow,
                    //    UserId = _authenticatedUserService.UserId

                    //};
                    //await _messageBus.PublishMessage(messageObject, EventMessages.NOTIFICATION_SETTINGS_UPDATED);

                    response.StatusCode = SuccessCodes.DEFAULT_SUCCESS_CODE;
                    response.Status = Status.DEFAULT_SUCCESS;
                    response.Message = SuccessMessages.NOTIFICATIONS_SETTINGS_UPDATED_SUCCESSFULLY;
                   
                }
            }
            catch
            {
                response.StatusCode = ErrorCodes.SERVER_ERROR_CODE;
                response.Status = Status.DEFAULT_FAILURE;
                response.Message = ErrorMessages.SERVER_ERROR;
                return response;
            }


            return await Task.FromResult(response);

        }

        public async Task<MyNotificationSettingsResponse> GetMyNotificationSettings(CancellationToken cancellationToken)
        {
            var response = new MyNotificationSettingsResponse();

            try
            {
                response.StatusCode = SuccessCodes.DEFAULT_SUCCESS_CODE;
                response.Status = Status.DEFAULT_SUCCESS;
                response.Message = SuccessMessages.NOTIFICATIONS_SETTINGS_FETCH_SUCCESS;
                response.Data.NotificationSettings = new List<PersonalNotificationResponseItem>();
                var personalSettings1=_unsubsribeRepository.Table.Where(p=>p.UserId==_authenticatedUserService.UserId).ToList();
                if (personalSettings1.Any())
                {
                    foreach (var personalSetting in personalSettings1)
                    {
                        var notificationName = "";
                        if (personalSetting.NotificationId != null)
                        {
                            int notId = Convert.ToInt32(personalSetting.NotificationId);
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                            Shared.Data.Models.Notification notification = _repository.Table.FirstOrDefault(p => p.Id == notId);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                            if (notification != null)
                            {
                                notificationName = notification.Name;
                            }
                        }
                        response.Data.NotificationSettings.Add(new PersonalNotificationResponseItem { Name = notificationName, NotificationId = personalSetting.NotificationId == null ? 0 : Convert.ToInt32(personalSetting.NotificationId), Subscribed = true });
                    }

                    
                }
            }
            catch
            {
                response.StatusCode = ErrorCodes.SERVER_ERROR_CODE;
                response.Status = Status.DEFAULT_FAILURE;
                response.Message = ErrorMessages.NOTIFICATIONS_SETTINGS_FETCH_ERROR;
                return response;
            }
            return await Task.FromResult(response);
        }

        public async  Task<MyNotificationsResponse> GetMyNotifications(NotificationQueryFilter filter, CancellationToken cancellationToken)
        {
            var response = new MyNotificationsResponse();
            try
            {
                var dataResponse = await GetMyNotificationsV3(filter, cancellationToken);
                response.Data = dataResponse;
                response.StatusCode = SuccessCodes.DEFAULT_SUCCESS_CODE;
                response.Status = ResponseStatusEnum.Successful.ToDescription();
                if(dataResponse.UserNotifications.Any())
                {
                    response.Message = SuccessMessages.SUCCESSFUL_USER_NOTIFICATIONS_LIST_RETRIEVAL;

                }
                else
                {
                    response.Message = SuccessMessages.FAILED_USER_NOTIFICATIONS_LIST_RETRIEVAL;
                }


                // return ResponseHandler.SuccessResponse(SuccessMessages.SUCCESSFUL_REQUEST_LIST_RETRIEVAL, response);

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                var dataResponse = new NotificationItemsResponse();
                response.Data = dataResponse;
                response.StatusCode = ErrorCodes.SERVER_ERROR_CODE;
                response.Status = ResponseStatusEnum.Failed.ToDescription();
                response.Message = ErrorMessages.FAILED_USER_NOTIFICATIONS_RETRIEVAL;

                //return ResponseHandler.FailureResponse(ErrorCodes.SERVER_ERROR_CODE, ErrorMessages.FAILED_REQUEST_CATEGORY_LIST_RETRIEVAL);


            }
            return response;
        }

        private async Task<NotificationItemsResponse> GetMyNotificationsV3(NotificationQueryFilter filter, CancellationToken cancellationToken)
        {
            var response = new NotificationItemsResponse();
            BasePageFilter pageFilter = new(filter.PageSize, filter.PageIndex);



#pragma warning disable CS8601 // Possible null reference assignment.
            NotificationFilterDto notificationFilter = new()
            {
                //UserId = filter.UserId,
                ReadStatus = filter.ReadStatus
            };

#pragma warning restore CS8601 // Possible null reference assignment.

            //var expression = new NotificationQueryObject(notificationFilter).Expression;
            //var orderExpression = ProcessOrderFunc(sorting);
            // var userId=Convert.ToInt32(filter.UserId);
            //var query = _requestRepository.Table.Where(p=>p.IsDeleted==false && p.CreatedByUserId==filter.UserId).AsNoTrackingWithIdentityResolution()
            // .OrderByWhere(expression, orderExpression);
            IQueryable<UserNotification> query;
            int totalCount = 0;
            int totalPages = 0;
            if (_authenticatedUserService.UserId > 0)
            {
                query = _userNotificationRepository.Table.Where(p => p.UserId == _authenticatedUserService.UserId).AsNoTrackingWithIdentityResolution()
               ;
                if(filter.ReadStatus!=null)
                {
                    query = _userNotificationRepository.Table.Where(p => p.UserId == _authenticatedUserService.UserId && p.ReadStatus==filter.ReadStatus).AsNoTrackingWithIdentityResolution()
                ;
                }
                totalCount = await query.CountAsync(cancellationToken);

                query = query.Select(x => new Shared.Data.Models.UserNotification
                {

                    Id = x.Id,
                    DateCreated = x.DateCreated,
                    NotificationId= x.NotificationId,
                    UserId= x.UserId,
                    Notification = _repository.Table.First(p => p.Id == x.NotificationId),
                    User = _userRepository.Table.First(p => p.UserId == x.UserId),
                    ReadStatus = x.ReadStatus,
                    DateRead=x.DateRead


                }).Paginate(pageFilter.PageNumber, pageFilter.PageSize);


            }
            else
            {
                var emptyRequestList = new List<UserNotification>();
                query = emptyRequestList.AsQueryable();
            }

            // var requests = await query.ToListAsync(cancellationToken);
            var notifications = query;
            totalPages = NumberManipulator.PageCountConverter(totalCount, pageFilter.PageSize);

            response.Pagination.PageSize = pageFilter.PageSize;
            response.Pagination.TotalRecords = totalCount;
            response.Pagination.TotalPages = totalPages;
            response.Pagination.PageIndex = pageFilter.PageNumber;
            if (notifications.Any())
            {
                foreach (var notification in notifications)
                {
                    response.UserNotifications.Add(new UserNotificationItem { DateCreated=notification.DateCreated, NotificationMessage=notification.NotificationMessage, ReadStatus=notification.ReadStatus, UserNotificationId=notification.NotificationId});
                }
            }


            return response;
        }

        public async Task<AcknowledgeNotificationResponse> AcknowledgeNotification(AcknowledgeNotificationInput input, CancellationToken cancellationToken)
        {
            var response = new AcknowledgeNotificationResponse();

            try
            {
                var userNotification = _userNotificationRepository.Table.FirstOrDefault(p => p.UserId == _authenticatedUserService.UserId && p.Id == input.UserNotificationId);

                if(userNotification == null)
                {
                    var dataResponse = new NotificationResponseData();
                    response.Data = dataResponse;
                    response.StatusCode = ErrorCodes.ERROR_N_01;
                    response.Status = ResponseStatusEnum.Failed.ToDescription();
                    response.Message = ErrorMessages.NON_EXISTENT_NOTIFICATION;
                }
                else
                {
                    userNotification.ReadStatus=true;
                    userNotification.DateRead = DateTime.Now;
                    _userNotificationRepository.Update(userNotification);
                }
            }
            catch
            {
                var dataResponse = new NotificationResponseData();
                response.Data = dataResponse;
                response.StatusCode = ErrorCodes.SERVER_ERROR_CODE;
                response.Status = ResponseStatusEnum.Failed.ToDescription();
                response.Message = ErrorMessages.FAILED_USER_NOTIFICATIONS_ACKNOWLEDGEMENT;
            }


            return await Task.FromResult(response);
        }
    }
}
