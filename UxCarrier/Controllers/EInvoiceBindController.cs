using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using UxCarrier.Helper;
using UxCarrier.Models;
using UxCarrier.Models.AppSetting;
using UxCarrier.Models.Dto;
using UxCarrier.Models.Entities;
using UxCarrier.Services;
using UxCarrier.Services.IService;
using static UxCarrier.Services.UxBindService;

namespace UxCarrier.Controllers
{
    [Route("api/v1/einvoicebind")]
    public class EInvoiceBindController : Controller
    {
        private readonly ILogger<UxBindController> _logger;
        private readonly ApiResponse _response;
        private readonly IMapper _mapper;
        private readonly UxBindService _uxBindService;
        private readonly IAppSetting _appSettingUtility;
        private readonly IUserService _userService;
        private static IHttpContextAccessor m_httpContextAccessor;
        public static HttpContext Current => m_httpContextAccessor.HttpContext;
        public static string AppBaseUrl => $"{Current.Request.Scheme}://{Current.Request.Host}{Current.Request.PathBase}";


        public EInvoiceBindController(IOptions<AppSetting> appSettingUtility,
            IMapper mapper, ILogger<UxBindController> logger,
            UxBindService uxBindService, IUserService userRepo, IHttpContextAccessor contextAccessor)
        {
            _appSettingUtility = appSettingUtility.Value; 
            _mapper = mapper;
            _response = new();
            _logger = logger;
            _uxBindService = uxBindService;
            _userService = userRepo;
            m_httpContextAccessor = contextAccessor;
        }

        [HttpGet("trystart")]
        //[Authorize]
        public IActionResult Step0()
        {
            ViewData["url"] = $"{Current.Request.PathBase}/api/v1/einvoicebind/validate";
            return View();
        }


        [HttpPost("getHMACSHA256")]
        [Authorize]
        public IActionResult GetHMACSHA256([FromBody] HMACSHA256Dto dto)
        {
            //var ttt = "card_ban=97162640&card_no1=1234&card_no2=987654321&card_type=BG0001&token=eyJhbGciOiJkaXIiLCJlbmMiOiJBMjU2R0NNIiwiY3R5IjoiSldUIiwia2lkIjoiMDEifQ..wNSlKWvyuPo20rYz.9AH2ig6DWLTn1CEGDiSx72SNQVpsYKeNQ4SmI4xqcAcHuriic_2XokWfDBqJNi1uN1pO1iJk3WlANXVRG6W4MYFp_bKiuYRo1wITDo_xCy26WrkjSOQhtZfbltrzdFHPnKvMzBoiqu6njirr9uBPJFSlI7Qu8Er56NWnzWJNRtOUrkEcB4JQYdWZ2pfFvqKMDtmI_4iFWwNgCjHX3P1WWTxcnDKq5R_oX8xx_u9a3NhsKgEwOwCl3hyPawJmq9vWswbmCM5BSTYSVk5PHeWRKik9LTBd1KZuw3005RXlVRAYqJKrkvdOUMpmdmFFbfopZK4t4UjtfhXpvSXhAJbipgDL_EJSlbB09xrly3DCK5B2WMmUZ6TJ07ryOuhQ9ZhtxfFIzin6VeKu_YNT0D8nugegMVqWjXHJrw4BMBxchy5MCUZB_CMLyBGZvemCHDZrPOjckFEiORLt6D7TXJOOBQN5kCXE43zYxcv__bAqHtcOjR3q6Yy7i51caI2zmlgkS_G.oIjs4WBcUsTJkXmuw4_Z9g";
            //string key = "XQcpGwtz5esvvdqTTsQ0bA=="
            return Ok(Utilities.HmacSHA256(dto.msg, dto.key));
        }

        [HttpPost("validate")]
        public async Task<ActionResult> Step1([FromForm] EInvoiceBindDto dto)
        {
            //_logger.LogInformation(dto.ToString());

            //測試用, 刪除ban=12345678資料
            //_uxBindService.DelStep2(dto);

            var step = _uxBindService.GetEInoviceBindStep(dto);
            if (step.Equals(StepEnum.Step0)) { return Ok(); }

            if (step.Equals(StepEnum.Step1)) 
            {
                EInvoiceBindCard? eInvoiceBindCard = _uxBindService.CreateEInvoiceBindCardOrNull(dto);
                if (Utilities.IsNull(eInvoiceBindCard))
                {
                    ViewData["message"] = "Step1token已建立.";
                    return View("~/Views/Home/ModalMessage.cshtml");
                }

                EInvoiceBindStep2Dto eInvoiceBindStep2PostDto = _mapper.Map<EInvoiceBindStep2Dto>(eInvoiceBindCard);
                EInvoiceBindStep3Dto recv = await _uxBindService
                    .HttpClientPostAsync<EInvoiceBindStep2Dto, EInvoiceBindStep3Dto>(
                        eInvoiceBindStep2PostDto,
                        _appSettingUtility.EInvoiceBindStep2Url!);

                if (Utilities.IsNull(recv))
                {
                    ViewData["message"] = "Step2傳送大平台失敗.";
                    return View("~/Views/Home/ModalMessage.cshtml");
                }

                eInvoiceBindCard = _uxBindService.UpdateTokenFlag(recv, eInvoiceBindCard);       

                if (recv.token_flag.Equals("N"))
                {
                    ViewData["message"] = recv.err_msg;
                    return View("~/Views/Home/ModalMessage.cshtml");
                }

                ViewData["nonce"] = eInvoiceBindCard.nonce;
                ViewData["token"] = eInvoiceBindCard.token;
                ViewData["url"] = $"{Current.Request.PathBase}/api/v1/einvoicebind/validate";
                ViewData["otpUrl"] = $"{Current.Request.PathBase}/api/v1/users/otp";
                ViewData["loginUrl"] = $"{Current.Request.PathBase}/api/v1/users/login";
                return View("~/Views/EInvoiceBind/EmailInputStep3.cshtml");
            }

            if (step.Equals(StepEnum.Step4))
            {
                var user = _uxBindService.CreateOrGetUser(dto);
                if (Utilities.IsNotNull(user)&&!string.IsNullOrEmpty(dto.nonce)) 
                {
                    var eInvoiceBindCard = _uxBindService.GetEInvoiceBindCardByNonce(dto.nonce);
                    if (!Utilities.IsNull(eInvoiceBindCard))
                    {
                        EInvoiceBindStep4Dto eInvoiceBindStep4Dto =
                            _uxBindService.EInvoiceBindStep4DtoGenerator(user.Email!, dto.token!);

                        _uxBindService.UpdateStep4Data(eInvoiceBindStep4Dto, eInvoiceBindCard);
                        _logger.LogInformation("EInvoiceBindStep4DtoForm=" + eInvoiceBindStep4Dto.ToString());
                        ViewData["url"] = _appSettingUtility.EInvoiceBindStep4Url;
                        return View("~/Views/EInvoiceBind/SendEInvoiceStep4.cshtml", eInvoiceBindStep4Dto);
                    }
                }
            }

            ViewData["message"] = "StepEnum.None";
            return View("~/Views/Home/ModalMessage.cshtml");
        }

    }
}
