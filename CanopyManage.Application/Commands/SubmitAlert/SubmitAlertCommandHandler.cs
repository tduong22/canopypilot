using CanopyManage.Application.IntegrationEvents.Events;
using CanopyManage.Common.EventBus.Abstractions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CanopyManage.Application.Commands.SubmitAlert
{
    public class SubmitAlertCommandHandler : IRequestHandler<SubmitAlertCommand>
    {
        private readonly IEventBusPublisher publisher;

        public SubmitAlertCommandHandler(IEventBusPublisher publisher)
        {
            this.publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        }

        public async Task<Unit> Handle(SubmitAlertCommand request, CancellationToken cancellationToken)
        {
            var evt = new IncidentSubmitIntegrationEvent()
            {
                TenantId = request.TenantId,
                AlertId = request.AlertId,
                ServiceNowSettingID = request.ServiceNowSettingID,
                AlertType = request.AlertType,
                Title = request.Title,
                Message = request.Message
            };

            await publisher.PublishAsync(evt, evt.AlertType);

            return Unit.Value;
        }
    }
}
