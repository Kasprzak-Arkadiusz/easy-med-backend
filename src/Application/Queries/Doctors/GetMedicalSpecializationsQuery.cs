using EasyMed.Domain.Enums;
using MediatR;

namespace EasyMed.Application.Queries.Doctors;

public class GetMedicalSpecializationsQuery : IRequest<IEnumerable<MedicalSpecialization>> { }

public class
    GetMedicalSpecializationsQueryHandler : IRequestHandler<GetMedicalSpecializationsQuery,
        IEnumerable<MedicalSpecialization>>
{
    public async Task<IEnumerable<MedicalSpecialization>> Handle(GetMedicalSpecializationsQuery request,
        CancellationToken cancellationToken)
    {
        var medicalSpecializations = Enum.GetValues(typeof(MedicalSpecialization)).Cast<MedicalSpecialization>();

        return medicalSpecializations;
    }
}