using TicketGo.Application.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TicketGo.Application.DTOs;
using TicketGo.Application.Interfaces;

namespace TicketGo.Application.Services
{
    public class VNPayService : IVNPayService
    {
        private readonly IConfiguration _config;

        public VNPayService(IConfiguration config)
        {
            _config = config;
        }
        public string CreatePaymentUrl(HttpContext httpContext, VnPayRequestDto model)
        {
            string tick;
            try
            {
                var now = DateTime.Now;
                Console.WriteLine($"Current DateTime: {now}");

                tick = now.Ticks.ToString();
                Console.WriteLine($"Ticks: {tick}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating ticks: {ex.Message}");
                throw; // hoặc xử lý ngoại lệ phù hợp
            }

            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", _config["VnPay:Version"]);
            vnpay.AddRequestData("vnp_Command", _config["VnPay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString());
            //Số tiền thanh toán. Số tiền không 
            //mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND
            //(một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần(khử phần thập phân), sau đó gửi sang VNPAY
            //là: 10000000

            vnpay.AddRequestData("vnp_CreateDate", model.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"]);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(httpContext));
            vnpay.AddRequestData("vnp_Locale", _config["VnPay:Locale"]);
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng:" + model.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", _config["VnPay:PaymentBackReturnUrl"]);
            vnpay.AddRequestData("vnp_TxnRef", tick);
            // Mã tham chiếu của giao dịch tại hệ thống của merchant.Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY.Không đượctrùng lặp trong ngày
            var paymentUrl = vnpay.CreateRequestUrl(_config["VnPay:BaseUrl"], _config["VnPay:HashSecret"]);

            return paymentUrl;
        }


        public VnPaymentResponseDto PaymentExecute(IQueryCollection collections)
        {
            var vnpay = new VnPayLibrary();

            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            string vnp_orderId = vnpay.GetResponseData("vnp_TxnRef");
            string vnp_Amount = vnpay.GetResponseData("vnp_Amount") ;
            string vnp_TransactionId = vnpay.GetResponseData("vnp_TranactionNo");
            string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnp_orderInfo = vnpay.GetResponseData("vnp_OrderInfo");
            Console.WriteLine($"vnp_TxnRef: {vnp_orderId}, vnp_Amount: {vnp_Amount}");

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config["VnPay:HashSecret"]);

        
            if (!checkSignature)
            {
                return new VnPaymentResponseDto
                {
                    Success = false
                };
            }

            return new VnPaymentResponseDto
            {
                Success = true,
                PaymentMethod = "VnPay",
                OrderDescription = vnp_orderInfo,
                OrderId = vnp_orderId.ToString(),
                TransactionId = vnp_TransactionId.ToString(),
                Token = vnp_SecureHash,
                VnPayResponseCode = vnp_ResponseCode.ToString()
            };
        }
    }
}