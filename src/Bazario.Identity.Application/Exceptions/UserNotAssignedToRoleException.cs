namespace Bazario.Identity.Application.Exceptions
{
    public sealed class UserNotAssignedToRoleException
        : Exception
    {
        public UserNotAssignedToRoleException(string userId)
            : base($"User with ID '{userId}' is not assigned to any role.")
        { }
    }
}
