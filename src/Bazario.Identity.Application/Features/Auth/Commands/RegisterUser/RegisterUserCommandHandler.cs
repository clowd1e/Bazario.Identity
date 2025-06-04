using Bazario.AspNetCore.Shared.Abstractions.Data;
using Bazario.AspNetCore.Shared.Abstractions.Messaging;
using Bazario.AspNetCore.Shared.Application.Mappers;
using Bazario.AspNetCore.Shared.Domain.Common.Users.Roles;
using Bazario.AspNetCore.Shared.Results;
using Bazario.Identity.Application.Abstractions.Identity;
using Bazario.Identity.Application.Abstractions.Security;
using Bazario.Identity.Application.Features.Auth.DTO;
using Bazario.Identity.Application.Features.Auth.DTO.RequestModels;
using Bazario.Identity.Application.Identity;
using Bazario.Identity.Domain.ConfirmEmailTokens;
using Bazario.Identity.Domain.Users;
using Bazario.Identity.Domain.Users.Errors;

namespace Bazario.Identity.Application.Features.Auth.Commands.RegisterUser
{
    internal sealed class RegisterUserCommandHandler
        : ICommandHandler<RegisterUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentityService _identityService;
        private readonly IConfirmEmailTokenRepository _confirmEmailTokenRepository;
        private readonly ITokenService _tokenService;
        private readonly IHasher _hasher;
        private readonly Mapper<IRegisterApplicationUserBaseCommand, ApplicationUser> _commandMapper;
        private readonly Mapper<ApplicationUser, Result<User>> _userMapper;
        private readonly Mapper<CreateConfirmEmailTokenRequestModel, Result<ConfirmEmailToken>> _tokenMapper;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            IIdentityService identityService,
            IConfirmEmailTokenRepository confirmEmailTokenRepository,
            ITokenService tokenService,
            IHasher hasher,
            Mapper<IRegisterApplicationUserBaseCommand, ApplicationUser> commandMapper,
            Mapper<ApplicationUser, Result<User>> userMapper,
            Mapper<CreateConfirmEmailTokenRequestModel, Result<ConfirmEmailToken>> tokenMapper,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _identityService = identityService;
            _confirmEmailTokenRepository = confirmEmailTokenRepository;
            _tokenService = tokenService;
            _hasher = hasher;
            _commandMapper = commandMapper;
            _userMapper = userMapper;
            _tokenMapper = tokenMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            RegisterUserCommand request,
            CancellationToken cancellationToken)
        {
            #region User creation

            // Map the command to a User

            var applicationUser = _commandMapper.Map(request);

            // Check if the user already exists

            var existingUser = await _identityService.GetByEmailAsync(request.Email);

            if (existingUser is not null)
            {
                return Result.Failure(UserErrors.AlreadyRegistered);
            }

            // Map application User to domain User

            var userMappingResult = _userMapper.Map(applicationUser);

            if (userMappingResult.IsFailure)
            {
                return userMappingResult.Error;
            }

            var user = userMappingResult.Value;

            #endregion

            #region Token generation

            // Generate a confirmation token

            var token = _tokenService.GenerateEmailConfirmationToken();

            var tokenHash = _hasher.Hash(token);

            var confirmEmailTokenModel = new CreateConfirmEmailTokenRequestModel(
                tokenHash,
                user);

            var tokenMappingResult = _tokenMapper.Map(confirmEmailTokenModel);

            if (tokenMappingResult.IsFailure)
            {
                return tokenMappingResult.Error;
            }

            var confirmEmailToken = tokenMappingResult.Value;

            // Insert the token into the database

            await _confirmEmailTokenRepository.InsertAsync(
                confirmEmailToken,
                cancellationToken);

            #endregion

            #region User insertion

            // Insert user into the database

            await _userRepository.InsertAsync(
                user,
                cancellationToken);

            await _identityService.CreateAsync(
                applicationUser,
                password: request.Password);

            user.Register(
                confirmEmailToken.Id.Value,
                token,
                user.Id.Value,
                user.Email.Value,
                request.FirstName,
                request.LastName,
                request.BirthDate,
                request.PhoneNumber);

            try
            {
                await _identityService.AssignUserToRoleAsync(
                    applicationUser,
                    roleName: Role.User.ToString());

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception)
            {
                await _identityService.DeleteAsync(applicationUser);

                throw;
            }

            #endregion

            return Result.Success();
        }
    }
}
