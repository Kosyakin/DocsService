using DocsService.Interfaces;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocsService.Models;
using DocsService.Repositories;
using DocsService.Services;
using DocsService.Interfaces;
using System.Globalization;
using DocsService.Contracts;

namespace DocsService.Services
{
    public class UserService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUsersRepository _usersRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IEmailService _emailService;
        public UserService(IUsersRepository usersRepository, IPasswordHasher passwordHasher,
            IJwtProvider jwtProvider, IEmailService emailService) 
        {
            _passwordHasher = passwordHasher;
            _usersRepository = usersRepository;
            _jwtProvider = jwtProvider;
            _emailService = emailService;
        }
        public async Task Register(string userName, string email, string password,
            string firstName, string lastName, string middleName,
            string position, string documentNumber)
        {
            var hashedPassword = _passwordHasher.Generate(password);
            var user = User.Create(Guid.NewGuid(),  userName, hashedPassword, email,
                firstName, lastName, middleName,
                position, documentNumber);
            await _usersRepository.Add(user);

            var message = string.Join("\r\n", $"Добрый день, {firstName}!", "Вы успешно зарегистрировались в системе DocsService.", $"Ваш логин: {email}");

            await _emailService.SendEmailAsync(email, "Добро пожаловать в DocsService", message);
        }

        public async Task<string> Login(string email, string password)
        {
            
            var user = await _usersRepository.GetByEmail(email);
            var result = _passwordHasher.Verify(password, user.PasswordHash);

            if (result == false)
            {
                throw new Exception("Failed to login");
            }

            var token = _jwtProvider.GenerateToken(user);

            return token;
        }

        public async Task<UserInfoResponse> GetUserById(Guid userId)
        {
            var userEntity = await _usersRepository.GetById(userId);

            if (userEntity == null)
            {
                throw new Exception("User not found");
            }

            return new UserInfoResponse
            {
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                MiddleName = userEntity.MiddleName,
                Position = userEntity.Position,
                DocumentNumber = userEntity.DocumentNumber,
                Email = userEntity.Email,
                ReminderDateOTseptember = userEntity.ReminderDateOTseptember,
                ReminderDatePBseptember = userEntity.ReminderDatePBseptember,
                ReminderDateOTmarch = userEntity.ReminderDateOTmarch,
            };
        }

       
    }
}
