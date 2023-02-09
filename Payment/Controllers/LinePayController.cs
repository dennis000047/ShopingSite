using System;
using Payment.Dtos;
using Payment.LinePayService;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Payment.Providers;
using System.Net.Mime;
using Microsoft.AspNetCore.Cors;

namespace LineBotMessage.Controllers
{
    [EnableCors("AllowAny")]
    [ApiController]
    [Route("api/[Controller]")]
    public class LinePayController : ControllerBase
    {
        private readonly LinePayService _linePayService;
        public LinePayController()
        {
            _linePayService = new LinePayService();
        }

        [HttpPost("Create")]
        public async Task<PaymentResponseDto> CreatePayment(PaymentRequestDto dto)
        {
            return await _linePayService.SendPaymentRequest(dto);
        }

        [HttpPost("Confirm")]
        public async Task<PaymentConfirmResponseDto> ConfirmPayment([FromQuery] string transactionId, [FromQuery] string orderId, PaymentConfirmDto dto )
        {
            return await _linePayService.ConfirmPayment(transactionId, orderId,dto);
        }

        [HttpGet("Cancel")]
        public async void CancelTransaction([FromQuery] string transactionId)
        {
            _linePayService.TransactionCancel(transactionId);
        }
    }
}

