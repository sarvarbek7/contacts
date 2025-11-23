using Contacts.Application.ProcessingServices.Models.Responses.HrmPro;

namespace Contacts.Application.Handlers.Responses;

public record PhoneNumberListItemWithPosition(Guid Id,
                                              string Number,
                                              string Type,
                                              List<PositionWrapper> Positions);

public record PositionWrapper(Guid PositionAssignmentId, Position Position, IEnumerable<WorkerResponse> Workers);