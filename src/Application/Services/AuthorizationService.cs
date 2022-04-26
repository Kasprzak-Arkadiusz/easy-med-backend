using EasyMed.Application.Common.Exceptions;

namespace EasyMed.Application.Services;

public static class AuthorizationService
{
    public static void VerifyIfSameUser(int id, int currentUserId, string failMessage = "You are not authorized")
    {
        if (id != currentUserId)
        {
            throw new ForbiddenAccessException(failMessage);
        }
    }
}