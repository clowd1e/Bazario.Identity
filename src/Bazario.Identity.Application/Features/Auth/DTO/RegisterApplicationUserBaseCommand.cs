namespace Bazario.Identity.Application.Features.Auth.DTO
{
    public interface IRegisterApplicationUserBaseCommand
    {
        string Email { get; }
        
        string FirstName { get; }
        
        string LastName { get; }
        
        DateOnly BirthDate { get; }

        string PhoneNumber { get; }
    }
}
