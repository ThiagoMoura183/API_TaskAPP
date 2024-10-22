using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using Domain.Entity;
using Infra.Persistency;
using MediatR;

namespace Application.UserCQ.Handlers {
    public class CreateUserCommandHandler(TasksDbContext context) : IRequestHandler<CreateUserCommand, ResponseBase<UserInfoViewModel?>> {
        private readonly TasksDbContext _context = context;
        public async Task<ResponseBase<UserInfoViewModel?>> Handle(CreateUserCommand request, CancellationToken cancellationToken) {
            var user = new User() {
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                PasswordHash = request.Password,
                Username = request.Username,
                RefreshToken = Guid.NewGuid().ToString(),
                RefreshTokenExpirationTime = DateTime.Now.AddDays(5)
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            var userInfo = new ResponseBase<UserInfoViewModel>() {
                ResponseInfo = null,
                Value = new() {
                    Name = user.Name,
                    Surname = user.Surname,
                    Email = user.Email,
                    Username = user.Username,
                    RefreshToken = user.RefreshToken,
                    RefreshTokenExpirationTime = user.RefreshTokenExpirationTime,
                    TokenJWT = Guid.NewGuid().ToString()
                }
            };

            return userInfo;
        }
    }
}
