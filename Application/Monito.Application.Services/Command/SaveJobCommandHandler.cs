using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Monito.Application.Model.Command;
using Monito.Domain.Entity;
using Monito.Persistence.Model;
using Monito.Persistence.Repository.Interface;

namespace Monito.Application.Services.Command
{
    public class SaveJobCommandHandler : IRequestHandler<SaveJobCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<RequestPersistenceModel> _requestRepository;

        public SaveJobCommandHandler(
            IMapper mapper,
            IRepository<RequestPersistenceModel> requestRepository
        )
        {
            _mapper = mapper;
            _requestRepository = requestRepository;
        }

        public Task<Guid> Handle(SaveJobCommand command, CancellationToken cancellationToken)
        {

            #region Building request
            // Evaluating enum
            var httpMethod = JobHttpMethod.HEAD;
            switch (command.Method) {
                case SaveJobCommandHttpMethod.GET:
                    httpMethod = JobHttpMethod.GET;
                    break;
                default:
                    break;
            }

            // Building the request options
            var requestOptions = RequestOptionsDomainEntity.Build(
                httpMethod,
                command.Redirects,
                command.Threads,
                command.Timeout,
                command.UserAgent
            );

            // Building links
            var links = BuildRequestLinksFromRawText(command.Links);

            // Building the request itself
            var requestDomainEntity = RequestDomainEntity.Build(
                command.RequestingIP,
                links,
                RequestType.Simple,
                requestOptions,
                command.UserID
            );
            #endregion
            
            #region Conversion to persistence model
            var requestPersistenceModel = _mapper.Map<RequestPersistenceModel>(requestDomainEntity);
            #endregion

            #region Store
            _requestRepository.Insert(requestPersistenceModel);
            #endregion

            return Task.FromResult(requestPersistenceModel.UUID);
            
        }

        private ICollection<LinkDomainEntity> BuildRequestLinksFromRawText(string links) {

        	var rawLinks = (links ?? "")
        		.Split('\n')
        		.Select(x => new IntermediateLink() {
        			URL = x.Trim(),
        			URI = ParseURI(x.Trim())
        		})
        		.Where(x => !string.IsNullOrWhiteSpace(x.URL) && x.URI != null);

        	var linksGroupedByHost = rawLinks
        		.GroupBy(x => x.URI.Host);

        	var hosts = linksGroupedByHost
        		.ToDictionary(x => x.Key, x => x.ToList());

        	var hostsEmpty = false;
        	var index = 0;

        	var roundedLinks = new List<IntermediateLink>();

        	while (!hostsEmpty) {
        		hostsEmpty = true;
        		foreach(var keyValue in hosts) {
        			if (keyValue.Value.Count > index) {
        				hostsEmpty = false;
        				roundedLinks.Add(keyValue.Value.ElementAt(index));
        			}
        		}
        		index++;
        	}

            return roundedLinks
                .Select(x => LinkDomainEntity.Build(x.URL))
                .ToList();
        }

        private Uri ParseURI(string url) {
        	if (!url.StartsWith("http"))
        		url = "http://" + url;
        	try {
        		return new Uri(url);
        	}  catch (Exception) {
        		return null;
        	}
        }

        private class IntermediateLink {
            public Uri URI { get; set; }
            public string URL { get; set; }
        }
    }
}
