using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Security.Claims;
using UxCarrier.Helper;
using UxCarrier.Models;
using UxCarrier.Models.AppSetting;
using UxCarrier.Models.Dto;
using UxCarrier.Models.Entities;
using UxCarrier.Models.Request;
using UxCarrier.Services;
using UxCarrier.Services.IService;
using static UxCarrier.Models.Dto.LoginRequestDto;
using static UxCarrier.Models.Dto.OtpRequestDto;

namespace UxCarrier.Controllers
{
    [Route("api/v1/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        protected ApiResponse _response;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IAppSetting _appSetting;
        public UsersController(IMapper mapper, IUserService userRepo, IWebHostEnvironment hostingEnvironment,
            IOptions<AppSetting> appSetting)
        {
            _mapper = mapper;
            _userService = userRepo;
            _response = new();
            _hostingEnvironment = hostingEnvironment;
            _appSetting = appSetting.Value;
        }

        //[HttpPost("XnxLJx5Vrd")]
        //[Authorize]
        //public async Task<ActionResult<ApiResponse>> XnxLJx5VrdAsync()
        //{
        //    if (_hostingEnvironment.IsDevelopment())
        //    {
        //        var cardNo = HttpContext.User.GetClaimName();
        //        var user = _userService.GetUserByCardNo(cardNo);

        //        if (!string.IsNullOrEmpty(_appSetting.DevOpList?.FirstOrDefault())&&_appSetting.DevOpList.Contains(user.Email))
        //        {
        //            UxCardEmail uxCardEmail = new UxCardEmail();
        //            uxCardEmail.CardNo = _appSetting.DevTestCardNo;
        //            uxCardEmail.Email = _appSetting.DevTestEmail;
        //            var userDto = _mapper.Map<UserDTO>(uxCardEmail);
        //            _response.Result = _userService.Login(userDto);
        //            return Ok(_response);
        //        }
        //        else
        //        {
        //            return Ok(_response.ErrorMessage("非DevOpList設定人員"));
        //        }
        //    }
        //    return Ok(_response.ErrorMessage("非開發環境"));
        //}

        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("captchaValidate")]
        public async Task<ActionResult> CaptchaValidateAsync([FromBody] CaptchaRequest model)
        {
            if (!ModelState.IsValid)
            {
                _response.ErrorMessage(ModelState.ErrorMessage());
                return Ok(_response);
            }
            return Ok(_response);
        }

        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("captchaCode")]
        public IActionResult CaptchaCodeAsync()
        {
            _response.Result = 6.CreateRandomStringCode().EncryptData();
            return Ok(_response);
        }
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("captchaCodeDecode")]
        public IActionResult CaptchaCodeDecodeAsync(string code)
        {
            return Ok(code.DecryptData());
        }

        [HttpPost("captchaImg")]
        public async Task<IActionResult> CaptchaImgAsync(string code)
        {
            string captcha = code.DecryptData();

            //Response.Clear();
            //Response.ContentType = "image/Png";
            using (Bitmap bmp = new Bitmap(120, 30))
            {
                int x1 = 0;
                int y1 = 0;
                int x2 = 0;
                int y2 = 0;
                int x3 = 0;
                int y3 = 0;
                int intNoiseWidth = 25;
                int intNoiseHeight = 15;
                Random rdn = new Random();
                using (Graphics g = Graphics.FromImage(bmp))
                {

                    //設定字型
                    using (System.Drawing.Font font = new System.Drawing.Font("Courier New", 16, FontStyle.Bold))
                    {

                        //設定圖片背景
                        g.Clear(Color.CadetBlue);

                        //產生雜點
                        for (int i = 0; i < 100; i++)
                        {
                            x1 = rdn.Next(0, bmp.Width);
                            y1 = rdn.Next(0, bmp.Height);
                            bmp.SetPixel(x1, y1, Color.DarkGreen);
                        }

                        using (Pen pen = new Pen(Brushes.Gray))
                        {
                            //產生擾亂弧線
                            for (int i = 0; i < 15; i++)
                            {
                                x1 = rdn.Next(bmp.Width - intNoiseWidth);
                                y1 = rdn.Next(bmp.Height - intNoiseHeight);
                                x2 = rdn.Next(1, intNoiseWidth);
                                y2 = rdn.Next(1, intNoiseHeight);
                                x3 = rdn.Next(0, 45);
                                y3 = rdn.Next(-270, 270);
                                g.DrawArc(pen, x1, y1, x2, y2, x3, y3);
                            }
                        }

                        //把GenPassword()方法換成你自己的密碼產生器，記得把產生出來的密碼存起來日後才能與user的輸入做比較。

                        g.DrawString(captcha, font, Brushes.Black, 3, 3);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            bmp.Save(ms, ImageFormat.Png);
                            byte[] bmpBytes = ms.GetBuffer();
                            //await Response.Body.WriteAsync(bmpBytes);
                            _response.Result = Convert.ToBase64String(bmpBytes);
                            return Ok(_response);
                        }
                    }
                }
            }

            return new EmptyResult();
        }

        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("otp")]
        public async Task<IActionResult> OtpAsync([FromBody] OtpRequestDto model)
        {
            var validationResult = new OtpRequestDtoValidator().Validate(model);
            if (!validationResult.IsValid)
            {
                _response.ErrorMessage(BaseService.GetFluentValidationResult(validationResult));
                return Ok(_response);
            }

            var user = _userService.GetUser(model.Email!);
            //wait to do...model.IsJoinMember需求待確認
            if (user == null)
            {
                user = _userService.CreateUser(model.Email!);
            }
            var data = await _userService.OtpCodeSend(user);
            if (string.IsNullOrEmpty(data.CardNo))
            {
                return Ok(_response.ErrorMessage());
            }
            _response.Result = new { CardNo = data.CardNo, Email = data.Email };
            return Ok(_response);
        }

        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto model)
        {
            var validationResult = new LoginRequestDtoValidator().Validate(model);
            if (!validationResult.IsValid)
            {
                _response.ErrorMessage(new List<string>() { BaseService.GetFluentValidationResult(validationResult) });
                return Ok(_response);
            }

            (bool result, string? msg, UxCardEmail? cardEmail) = 
                _userService.LoginValidate(email: model.Email!, otpCode: model.OtpCode!);

            if (!result)
            {
                _response.ErrorMessage(msg);
                return Ok(_response);
            }

            var userDto = _mapper.Map<UserDTO>(cardEmail);
            var loginResponse = _userService.Login(userDto);
            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _response.ErrorMessage("登入失敗");
                return Ok(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = loginResponse;
            return Ok(_response);
        }
    }
}
