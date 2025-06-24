using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DiyawannaSupBackend.Api.Exceptions
{
    public static class GlobalExceptionHandler
    {
        public static ProblemDetails HandleException(Exception exception)
        {
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string title = "An error occurred";
            string detail = exception.Message;
            string type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"; // Default for 500

            switch (exception )
            {
                case UserNotFoundException:
                // case UniversityNotFoundException:
                // case FacultyNotFoundException:
                // case CartNotFoundException:
                // case QueryNotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    title = "Resource Not Found";
                    type = "https://tools.ietf.org/html/rfc7231#section-6.5.4";
                    break;
                case UserAlreadyExistsException:
                // case UniversityAlreadyExistsException:
                // case QueryAlreadyExistsException:
                    statusCode = (int )HttpStatusCode.Conflict;
                    title = "Resource Conflict";
                    type = "https://tools.ietf.org/html/rfc7231#section-6.5.8";
                    break;
                case AuthenticationException:
                    statusCode = (int )HttpStatusCode.Unauthorized;
                    title = "Authentication Failed";
                    type = "https://tools.ietf.org/html/rfc7235#section-3.1";
                    break;
                case ArgumentException: // For invalid input arguments
                case System.ComponentModel.DataAnnotations.ValidationException:
                    statusCode = (int )HttpStatusCode.BadRequest;
                    title = "Invalid Request";
                    type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                    break;
                // Add more custom exceptions here
                default:
                    // Log the unexpected exception for debugging
                    Console.WriteLine($"Unhandled exception: {exception.GetType( ).Name} - {exception.Message}");
                    break;
            }

            return new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Type = type,
                Instance = "Your API endpoint path here" // Can be dynamically set
            };
        }
    }
}
