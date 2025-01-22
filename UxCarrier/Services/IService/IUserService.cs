using UxCarrier.Models.Dto;
using UxCarrier.Models.Entities;

namespace UxCarrier.Services.IService
{
    public interface IUserService
    {
        bool IsUniqueUser(string email);
        Task<UxCardEmail> OtpCodeSend(UxCardEmail uxCardEmail);
        UxCardEmail GetUser(string email);
        UxCardEmail GetUserByCardNo(string cardNo);
        UxCardEmail CreateUser(string email);
        bool IsOtpCodeValidate(UxCardEmail uxCardEmail, string otpCode);
        bool IsOtpCodeExpired(UxCardEmail uxCardEmail);
        (bool result, string msg, UxCardEmail? cardEmail) LoginValidate(string email, string otpCode);
        LoginResponseDTO Login(UserDTO userDTO);
    }
}
