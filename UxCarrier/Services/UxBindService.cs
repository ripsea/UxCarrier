using Microsoft.Extensions.Options;
using System.Text;
using System;
using UxCarrier.Models;
using UxCarrier.Models.Entities;
using System.Collections.Specialized;
using UxCarrier.Helper;
using UxCarrier.Models.AppSetting;
using UxCarrier.Repository.IRepository;
using UxCarrier.Models.Dto;
using System.Security.Policy;
using UxCarrier.Services.IService;
using UxCarrier.Controllers;

namespace UxCarrier.Services
{
    public class UxBindService: BaseService
    {
        private readonly IAppSetting _appSettingUtility;
        private readonly IRepositoryWrapper _repo;
        private readonly IUserService _userService;
        private readonly ILogger<UxBindService> _logger;
        public UxBindService(IOptions<AppSetting> appSettingUtility,
            IHttpClientFactory httpClient,
            ILogger<UxBindService> logger,
            IRepositoryWrapper repo, IUserService userRepo
            ) : base(httpClient, logger)
        {
            _appSettingUtility = appSettingUtility.Value;
            _userService = userRepo;
            _repo = repo;
            _logger = logger;
        }

        public string GetUxBindUrl()
        {
            return _appSettingUtility.UxBindUrl!;
        }

        public StringBuilder UxBindSendFormPostString(NameValueCollection dto)
        {
            return Utilities.GetFormPostString(_appSettingUtility.UxBindUrl!, dto);
        }

        public async Task<TResponse> HttpClientPostAsync<TRequest, TResponse>(TRequest dto, string url)
        {
            return await PostAsync<TRequest, TResponse>(url, dto);
        }

        public enum StepEnum : ushort
        {
            Step0 = 0,
            Step1 = 1,
            Step2 = 2,
            Step3 = 3,
            Step4 = 4,
            None = 99
        }

        //public void DoStep1(EInvoiceBindDto dto)
        //{
        //    var aaa = EInvoiceBindDtoGenerator(dto.ban, dto.token);
        //    _repo.EInvoiceBindCard.Create(aaa);
        //}

        public EInvoiceBindCard? GetEInvoiceBindCardByNonce(string nonce)
        {
            return _repo.EInvoiceBindCard.FindByCondition(x => x.nonce == nonce).FirstOrDefault();
        }

        public EInvoiceBindCard? CreateEInvoiceBindCardOrNull(EInvoiceBindDto dto)
        {
            var eInvoiceBindCard = _repo.EInvoiceBindCard.FindAll()
                .Where(x => x.ban == dto.ban)
                .Where(x => x.token == dto.token).FirstOrDefault();
            if (Utilities.IsNull(eInvoiceBindCard))
            {
                var randomString = Utilities.GetRandomCharacters(n: 16);
                //var aaa = EInvoiceBindDtoGenerator(dto.ban, dto.token, "1234567890ABCDEF");
                var eInvoiceBindDto = EInvoiceBindDtoGenerator(dto.ban, dto.token, randomString);
                _repo.EInvoiceBindCard.Create(eInvoiceBindDto);
                _repo.Save();
                return eInvoiceBindDto;
            }
            return null;
        }

        public void DelStep2(EInvoiceBindDto dto)
        {
            var ttt = _repo.EInvoiceBindCard.FindAll()
                .Where(x => x.ban == dto.ban)
                .Where(x => x.token == dto.token).FirstOrDefault();
            if (ttt!=null)
            { 
                _repo.EInvoiceBindCard.Delete(ttt);
                _repo.Save();
        
            }
        }

        public UxCardEmail? CreateOrGetUser(EInvoiceBindDto dto)
        {
            if (!string.IsNullOrEmpty(dto.email))
            {
                var cardEmail = _repo.CardEmail.FindByCondition(x=>x.Email== dto.email).FirstOrDefault();
                if (Utilities.IsNull(cardEmail))
                {
                    var user = _userService.CreateUser(dto.email);
                    return user;
                }
                return cardEmail;
            }
            return null;
            //create signature
            //send send request to ienvoice
        }

        public EInvoiceBindCard? UpdateTokenFlag(EInvoiceBindStep3Dto dto, EInvoiceBindCard card)
        {
            if (!Utilities.IsNull(card))
            {
                card.err_msg = dto.err_msg??string.Empty;
                card.token_flag = (dto.token_flag??string.Empty).Equals("Y");
                card.UpdateTokenFlagDateTime = DateTime.Now;
                _repo.EInvoiceBindCard.Update(card);
                _repo.Save();
                return card;
            }
            return null;
        }

        public EInvoiceBindCard? UpdateStep4Data(EInvoiceBindStep4Dto dto, EInvoiceBindCard card)
        {
            if (!Utilities.IsNull(card))
            {
                card.card_ban = dto.card_ban ?? string.Empty;
                card.card_no1 = dto.card_no1 ?? string.Empty;
                card.card_no2 = dto.card_no2 ?? string.Empty;
                card.card_type = dto.card_type ?? string.Empty;
                card.token = dto.token ?? string.Empty;
                card.signature = dto.signature ?? string.Empty;
                card.BindSendDateTime = DateTime.Now;
                _repo.EInvoiceBindCard.Update(card);
                _repo.Save();
                return card;
            }
            return null;
        }

        public StepEnum GetEInoviceBindStep(EInvoiceBindDto dto)
        {
            if (new Step0Validator().Validate(dto).IsValid)
                return StepEnum.Step0;
            if (new Step1Validator().Validate(dto).IsValid)
                return StepEnum.Step1;
            //if (new Step2Validator().Validate(dto).IsValid)
            //    return StepEnum.Step2;
            //if (new Step3Validator().Validate(dto).IsValid)
            //    return StepEnum.Step3;
            if (new Step4Validator().Validate(dto).IsValid)
                return StepEnum.Step4;

            return StepEnum.None;
        }

        public EInvoiceBindCard EInvoiceBindDtoGenerator(string ban, string token, string nonce="")
        {
            EInvoiceBindCard eInvoiceBindDto = new();
            eInvoiceBindDto.ban = ban;
            eInvoiceBindDto.token = token;
            if (!string.IsNullOrEmpty(token))
            {
                eInvoiceBindDto.nonce = nonce;
            }
            return eInvoiceBindDto;
        }

        public EInvoiceBindStep4Dto EInvoiceBindStep4DtoGenerator(string email, string token)
        {
            EInvoiceBindStep4Dto eInvoiceBindStep4Dto = new();
            eInvoiceBindStep4Dto.card_ban = _appSettingUtility.UxBindCardBan;
            eInvoiceBindStep4Dto.card_no1 = email;
            eInvoiceBindStep4Dto.card_no2 = email;
            eInvoiceBindStep4Dto.card_type = _appSettingUtility.UxBindCardType;
            eInvoiceBindStep4Dto.token = token;
            //eInvoiceBindStep4Dto.url = _appSettingUtility.EInvoiceBindStep4Url;
            var queryString = Utilities.ConvertToQueryString(eInvoiceBindStep4Dto,false);
            eInvoiceBindStep4Dto.signature = Utilities.HmacSHA256(queryString, _appSettingUtility.EInvoiceBindSecretKey);

            _logger.LogInformation("EInvoiceBindStep4Dto=" + eInvoiceBindStep4Dto.ToString());

            //加密要用明文, form post時才需做base64
            eInvoiceBindStep4Dto.card_no1 = Utilities.EncodeBase64(email);
            eInvoiceBindStep4Dto.card_no2 = Utilities.EncodeBase64(email);
            eInvoiceBindStep4Dto.card_type = Utilities.EncodeBase64(_appSettingUtility.UxBindCardType);

            return eInvoiceBindStep4Dto;
        }

        public UxBindCard UxBindCardGenerator(string cardno)
        {
            UxBindCard uxBindCard = new();
            uxBindCard.card_ban = _appSettingUtility.UxBindCardBan;
            uxBindCard.card_no1 = cardno;
            uxBindCard.card_no2 = cardno;
            uxBindCard.card_type = _appSettingUtility.UxBindCardType;
            uxBindCard.back_url = _appSettingUtility.UxBindCallBack;
            uxBindCard.token = Guid.NewGuid().ToString();
            return uxBindCard;
        }
    }
}
