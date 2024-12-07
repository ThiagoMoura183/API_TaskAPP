using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using AutoMapper;
using Domain.Abstractions;
using Domain.Entity;
using Infra.Persistency;
using MediatR;

namespace Application.UserCQ.Handlers {
    public class CreateUserCommandHandler(TasksDbContext context, IMapper mapper, IAuthService authService) : IRequestHandler<CreateUserCommand, ResponseBase<UserInfoViewModel?>> {
        private readonly TasksDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IAuthService _authService = authService;

        public async Task<ResponseBase<UserInfoViewModel>> Handle(CreateUserCommand request, CancellationToken cancellationToken) {

            // Mapeia para um objeto USER a partir do source "request", que é o CreateUserCommand
            var user = _mapper.Map<User>(request);
            user.RefreshToken = _authService.GenerateRefreshToken();
            user.PasswordHash = _authService.HashingPassword(request.Password!);

            _context.Users.Add(user);
            _context.SaveChanges();

            var userInfoVM = _mapper.Map<UserInfoViewModel>(user);
            // Aqui o TokenJWT é null, conforme mapping acima. Logo, vamos atualizar chamando o método da dependência de authService
            userInfoVM.TokenJWT = _authService.GenerateJWT(user.Email!, user.Username!);

            return new ResponseBase<UserInfoViewModel>() {
                ResponseInfo = null,
                Value = userInfoVM
            };
        }
    }
}
