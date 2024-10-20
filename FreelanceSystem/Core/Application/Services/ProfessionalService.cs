using System;
using Application.Exceptions;
using Application.InfraPorts;
using Application.Requests.Client;
using Application.Requests.Professional;
using Application.ServicesPorts;
using Domain.Entities;
using Domain.Enums;
using Domain.Ports;

namespace Application.Services;

public class ProfessionalService : IProfessionalService
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAuthUserAdapter _authUserAdapter;
    private readonly IServiceCategoryRepository _serviceCategoryRepository;
    private readonly IProposalRepository _proposalRepository;

    public ProfessionalService(IServiceRepository serviceRepository, IUserRepository userRepository, IAuthUserAdapter authUserAdapter, IServiceCategoryRepository serviceCategoryRepository, IProposalRepository proposalRepository)
    {
        _serviceRepository = serviceRepository;
        _userRepository = userRepository;
        _authUserAdapter = authUserAdapter;
        _serviceCategoryRepository = serviceCategoryRepository;
        _proposalRepository = proposalRepository;
    }

    public async Task<List<Service>> GetNewServicesAsync(Application.Requests.Professional.GetServicesRequest request)
    {
        var data = await _serviceRepository.GetAllWaitingProposalAsync(request.PerPage, request.Page);
        return data;
    }

    public async Task<Proposal> SendProfessionalProposalAsync(SendProposalToServiceRequest request, Guid serviceId, string userId)
    {
        var authUser = await _authUserAdapter.GetOneByIdAsync(new Guid(userId));
        var user = await _userRepository.GetOneByEmailAsync(authUser.Email);
        var service = await _serviceRepository.GetOneById(serviceId);
        if (service == null)
            throw new EntityNotFoundException("Service was not founded");

        var proposal = new Proposal(service.Id, request.Comment, request.Price, ProposalType.PROFESSIONAL_PROPOSAL, user.Id, service.ClientId);
        await _proposalRepository.CreateAsync(proposal);

        return proposal;
    }
}
