using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UxCarrier.Models.AppSetting;
using UxCarrier.Models.Dto;
using UxCarrier.Models.Entities;
using UxCarrier.Repository.IRepository;
using UxCarrier.Services.Email;
using UxCarrier.Services.IService;

namespace UxCarrier.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryWrapper _repo;
        private readonly IAppSetting _appSetting;
        private readonly EmailFactory _emailFactory;
        private readonly string[] otpCode = ["8206", "8924", "8582", "9852", "6688", "5213", "8686", "6942", "9737", "9472"];

        public UserService(IConfiguration configuration,
            EmailFactory emailFactory,
            IOptions<AppSetting> appSetting, IRepositoryWrapper db
            )

        {
            _repo = db;
            _appSetting = appSetting.Value;
            _emailFactory = emailFactory;
        }

        public bool IsUniqueUser(string email)
        {
            var user = _repo.CardEmail.FindByCondition(x => x.Email == email).FirstOrDefault();
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public UxCardEmail GetUser(string email)
        {
            return _repo.CardEmail.FindByCondition(x => x.Email == email).FirstOrDefault()!;
        }

        private string GetOtpCode(UxCardEmail uxCardEmail)
        {
            var mm = String.Format("{0:mm}", uxCardEmail.LastOtpCodeRequestDateTime);
            var otpCodeIndex = Int32.Parse(mm.Substring(1, 1));
            return this.otpCode[otpCodeIndex];
        }

        public bool IsOtpCodeValidate(UxCardEmail uxCardEmail, string otpCode)
        {
            string otpCodeString = GetOtpCode(uxCardEmail);
            if (otpCode.Equals(otpCodeString))
                return true;
            else return false;
        }

        public bool IsOtpCodeExpired(UxCardEmail uxCardEmail)
        {
            var tt = DateTime.Now;
            var aa = uxCardEmail.LastOtpCodeRequestDateTime.AddMinutes(5);
            int result = DateTime.Compare(DateTime.Now,
                uxCardEmail.LastOtpCodeRequestDateTime.AddMinutes(5));
            if (result<0)
                return true;
            else
                return false;
        }

        public (bool result, string msg, UxCardEmail? cardEmail) 
            LoginValidate(string email, string otpCode)
        {
            bool result = false;
            if (string.IsNullOrEmpty(email)||string.IsNullOrEmpty(otpCode)) return (result, "email或otpCode為空白", null);

            var cardEmail = GetUser(email);
            if (cardEmail == null)
            {
                return (result, "無此會員", null);
            }

            var otpValidate = IsOtpCodeValidate(cardEmail, otpCode);
            if (!otpValidate) return (result, "驗證碼有誤", null);

            var otpExpired = IsOtpCodeExpired(cardEmail);
            if (!otpExpired) return (result, "驗證碼過期", null);

            return (true, "", cardEmail);
        }

        public LoginResponseDTO Login(UserDTO userDTO)
        {

            if (string.IsNullOrEmpty(userDTO.CardNo) || string.IsNullOrEmpty(_appSetting.JwtSecret!))
                return null!;

            string token = GetToken(userDTO);

            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = token,
                User = userDTO,

            };
            return loginResponseDTO;
        }

        public string GetToken(UserDTO userDTO)
        {
            //if user was found generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSetting.JwtSecret!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userDTO.CardNo),
                    new Claim(ClaimTypes.Email, userDTO.Email),
                    new Claim(ClaimTypes.Role, "user")
                }),
                Expires = DateTime.UtcNow.AddMinutes(20),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(securityToken);
            return token;
        }

        public async Task<UxCardEmail> OtpCodeSend(UxCardEmail uxCardEmail)
        {
            //wait to do...若有LastOtpCodeRequestDateTime存在, 判斷是否3分鐘, 防重覆寄送
            uxCardEmail.LastOtpCodeRequestDateTime = DateTime.Now;
            uxCardEmail.UpdatedDate = DateTime.Now;
            _repo.CardEmail.Update(uxCardEmail);
            _repo.Save();
            var code = GetOtpCode(uxCardEmail);
            _emailFactory.SendEmailToCustomer(emailTo: uxCardEmail.Email,
                subject: "網際優勢UxCarrier安全性驗證碼",
                body: await _emailFactory.CreateVerifyCodeEmailBody(code));
            return uxCardEmail;
        }

        public UxCardEmail CreateUser(string email)
        {
            UxCard uxMember = new();
            uxMember.CardNo = DateTime.Now.ToString("yyMMddhhmmss");
            _repo.Card.Create(uxMember);
            UxCardEmail uxMemberEmail = new();
            uxMemberEmail.CardNo = uxMember.CardNo;
            uxMemberEmail.Email = email;
            _repo.CardEmail.Create(uxMemberEmail);
            _repo.Save();
            return uxMemberEmail;
        }

        public UxCardEmail GetUserByCardNo(string cardNo)
        {
            return _repo.CardEmail.FindByCondition(x => x.CardNo == cardNo).FirstOrDefault()!;
        }
    }
}
