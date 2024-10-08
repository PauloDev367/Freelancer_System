using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Entities;

public class Professional : User
{
    public bool ShowServicesOnList { get; set; }
    public List<ServiceCategory> ServiceCategories { get; set; }

    public Professional()
    {
        Role = UserRole.PROFESSIONAL;
        ServiceCategories = new List<ServiceCategory>();
    }

    public async Task HandleClientProposalAsync(Proposal proposal, ProposalAction action)
    {
        VerifyProfileIsCompleteAsync();
        if (!proposal.Type.Equals(ProposalType.CLIENT_PROPOSAL))
            throw new InvalidOperationException("You only can handle client proposal types");

        if (proposal.ProfessionalId.Equals(Id))
            throw new InvalidProposalException("You can't handle a proposal that you send to someone");

        await proposal.SetProposalStatusAsync(action);
    }
}