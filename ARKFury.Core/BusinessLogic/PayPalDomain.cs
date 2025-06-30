using ArkFury.common.Services;
using ArkFury.Common;
using ArkFury.Common.Models;
using ArkFury.Common.Services;
using ArkFury.Entities.DTOs;
using ArkFury.Entities.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ArkFury.Core.BusinessLogic
{
    public interface IPayPalDomain
    {
        bool AuthorizePayment(PayPalTransactionDTO transaction);
        Task<int> CompletePayment(PayPalTransactionDTO transaction);
        Task<AdjustableDTO<PayPalTransactionDTO>> Get(long steamId, bool onlyUnpaid);
    }

    public class PayPalDomain : BaseDomain, IPayPalDomain
    {
        private readonly IElasticSearchService _elasticSearchService;
        private readonly IPlayerDomain _playerDomain;
        public PayPalDomain(ArkShopContext context, 
            IMapper mapper, 
            IPlayerDomain playerDomain,
           IElasticSearchService elastic,
           ILogger logger,
           IHttpContextAccessor httpContextAccessor,
           IOptions<AppSettings> settings,
           ICacheService cache) : base(context, mapper, elastic, logger, httpContextAccessor, settings, cache)
        {
            _elasticSearchService = elastic;
            _playerDomain = playerDomain;
        }

        public bool AuthorizePayment(PayPalTransactionDTO transaction)
        {
            var result = _elastic.Index(transaction, "transaction");
            return result > 0;
        }

        public async Task<AdjustableDTO<PayPalTransactionDTO>> Get(long steamId, bool onlyUnpaid)
        {
            var transactions = await All();
            var relevantRecords = transactions.Data.Where(t => t.SteamId == steamId.ToString()).ToList();

            if (onlyUnpaid)
                relevantRecords.RemoveAll(r => r.State != "Delivered");

            return new AdjustableDTO<PayPalTransactionDTO>(new PagingRequest(), relevantRecords);
        }

        public async Task<AdjustableDTO<PayPalTransactionDTO>> All()
        {
            return await _elasticSearchService.Search<PayPalTransactionDTO>(new PagingRequest(true), "transaction");
        }

        public async Task<int> CompletePayment(PayPalTransactionDTO transaction)
        {
            var result = _elastic.Index(transaction, "transaction");
            var mailClient = new SmtpClient();
            mailClient.Host = "email-smtp.us-west-2.amazonaws.com";
            mailClient.Port = 25;
            mailClient.EnableSsl = true;
            mailClient.Credentials = new NetworkCredential("AKIATPVZIXDUSIFDUME4", "BPa6O8ZhfQsaqXphJhyhkZR8WSyjCQkz4s8mPnXAOxIt");
            mailClient.Send("admin@shorelinegames.com",
                            _settings.Value.PaymentNotificationEmailList,
                            $"New PayPal Purchase Complete",
$@"

OrderId:           {transaction.OrderID}
CreatedOn:     {transaction.Create_time}
Intent:             {transaction.Intent}
Description:  {transaction.ProductDescription}
Server:           {transaction.Server}
State:             {transaction.State}
SteamId:       {transaction.SteamId}

MoreDetails: https://arkfury.com/#/transaction/{transaction.OrderID}");

            return await _playerDomain.ChangePoints(Convert.ToInt64(transaction.SteamId), Convert.ToInt32(Convert.ToDecimal(transaction.Transactions.FirstOrDefault().Amount.Total)) * 1000);
        }
    }
}
