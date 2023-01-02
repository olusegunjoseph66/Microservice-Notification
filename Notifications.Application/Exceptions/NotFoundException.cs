using Notifications.Application.CodeFactories;
using Notifications.Application.Constants;
using Notifications.Application.DTOs.APIDataFormatters;
using System.Globalization;

namespace Notifications.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base()
        {
            Response = ResponseHandler.FailureResponse(ErrorCodes.NOTFOUND_ERROR_CODE, ErrorMessages.NOT_FOUND_ERROR);
        }

        public NotFoundException(string message) : base(message)
        {
            Response = ResponseHandler.FailureResponse(ErrorCodes.NOTFOUND_ERROR_CODE, message);
        }

        public ApiResponse Response { get; private set; }

        public NotFoundException(string message, string code, params object[] args) : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
            Response = ResponseHandler.FailureResponse(code, message);
        }
    }
}
