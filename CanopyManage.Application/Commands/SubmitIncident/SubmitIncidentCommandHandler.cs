using CanopyManage.Application.Services;
using CanopyManage.Application.Services.Requests;
using CanopyManage.Application.Services.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CanopyManage.Application.Commands.SubmitIncident
{
    public class SubmitIncidentCommandHandler : IRequestHandler<SubmitIncidentCommand>
    {
        private readonly IServiceNowService serviceNowService;

        public SubmitIncidentCommandHandler(IServiceNowService serviceNowService)
        {
            this.serviceNowService = serviceNowService ?? throw new ArgumentNullException(nameof(serviceNowService));
        }

        public async Task<Unit> Handle(SubmitIncidentCommand request, CancellationToken cancellationToken)
        {
            var addNewIncidentRequest = new AddNewIncidentRequest()
            {
                Title = request.Title,
                Message = request.Message
            };

            AddNewIncidentResponse result = await serviceNowService.AddNewIncidentAsync(addNewIncidentRequest, cancellationToken);
            
            //Publish message to queue
            return Unit.Value;
        }
    }
}
