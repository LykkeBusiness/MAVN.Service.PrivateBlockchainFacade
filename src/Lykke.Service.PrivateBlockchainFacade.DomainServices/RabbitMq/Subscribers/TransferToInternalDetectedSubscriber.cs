﻿using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Job.EthereumBridge.Contract;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.PrivateBlockchainFacade.Domain.RabbitMq;

namespace Lykke.Service.PrivateBlockchainFacade.DomainServices.RabbitMq.Subscribers
{
    public class TransferToInternalDetectedSubscriber : JsonRabbitSubscriber<TransferToInternalDetectedEvent>
    {
        private readonly ITransferToInternalDetectedHandler _handler;
        private readonly ILog _log;

        public TransferToInternalDetectedSubscriber(
            ITransferToInternalDetectedHandler handler,
            string connectionString,
            string exchangeName,
            string queueName,
            ILogFactory logFactory)
            : base(connectionString, exchangeName, queueName, logFactory)
        {
            _handler = handler;
            _log = logFactory.CreateLog(this);
        }

        protected override async Task ProcessMessageAsync(TransferToInternalDetectedEvent message)
        {
            await _handler.HandleAsync(message.PublicTransferId, message.PrivateAddress, message.PublicAddress,
                message.Amount);

            _log.Info("Processed TransferToInternalDetectedEvent", message);
        }
    }
}