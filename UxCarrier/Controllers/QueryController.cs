using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UxCarrier.Helper;
using UxCarrier.Models;
using UxCarrier.Models.AppSetting;
using UxCarrier.Models.Dto;
using UxCarrier.Models.Request;
using UxCarrier.Models.Response;
using UxCarrier.Repository.IRepository;
using UxCarrier.Services;
using UxCarrier.Services.IService;

namespace UxCarrier.Controllers
{
    [Route("api/v1/query")]
    [ApiController]
    [Authorize]
    public class QueryController : ControllerBase
    {
        private readonly ILogger<QueryController> _logger;
        private readonly ApiResponse _response;
        private readonly IMapper _mapper;
        //private readonly UxBindService _uxBindService;
        private readonly IRepositoryWrapper _repo;
        private readonly IUserService _userService;
        private readonly IAppSetting _appSetting;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public QueryController(
            IMapper mapper, ILogger<QueryController> logger,
            //UxBindService uxBindService,
            IRepositoryWrapper repo,
            IUserService userService,
            IOptions<AppSetting> appSetting, IWebHostEnvironment hostingEnvironment)
        {
            _mapper = mapper;
            this._response = new();
            _logger = logger;
            //_uxBindService = uxBindService;
            _repo = repo;
            _userService = userService;
            _appSetting = appSetting.Value;
            _hostingEnvironment = hostingEnvironment;
        }

        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("send")]
        public async Task<ActionResult<ApiResponse>> InvoiceAsync([FromBody] InvoiceQueryRequest dto)
        {
            var validator = new InvoiceQueryRequestValidator().Validate(dto);
            if (!validator.IsValid)
            {
                _response.ErrorMessage(BaseService.GetFluentValidationResult(validator));
                return Ok(_response);
            }

            var pageInfoDto = new PageInfoDto(
                        pageIndex: dto.PageIndex,
                        pageSize: dto.PageSize,
                        sortText: dto.Sort);

            var email = HttpContext.User.GetClaimEmail();
            if (string.IsNullOrEmpty(email))
            {
                return Ok(_response.ErrorMessage($"Email:{email}¦³»~."));
            }

            if (_hostingEnvironment.IsDevelopment())
            {
                if (!string.IsNullOrEmpty(_appSetting.DevOpList?.FirstOrDefault()) && _appSetting.DevOpList.Contains(email))
                {
                    email = dto.Email;
                }
            }

            InvoicesQueryDto invoicesQueryDto = new InvoicesQueryDto();
            invoicesQueryDto.PageInfo = new PageInfo(pageInfoDto);
            invoicesQueryDto.CarrierNo = email;
            var (data,page) = _repo.InvoiceItem.GetInvoices(invoicesQueryDto); //wait to do ... ©ñ¦bservice
            var queryInvoiceDto = _mapper.Map<IEnumerable<QueryInvoiceDto>>(data);
            PagedQueryResponse resp = new PagedQueryResponse() { PagedList = queryInvoiceDto, PagedResult = page };
            _response.Result = resp;
            return Ok(_response);
        }
    }
}
