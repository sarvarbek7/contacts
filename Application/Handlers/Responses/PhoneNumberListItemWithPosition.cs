using Contacts.Application.ProcessingServices.Models.Responses.HrmPro;

namespace Contacts.Application.Handlers.Responses;

public record PhoneNumberListItemWithPosition(Guid Id,
                                              string Number,
                                              string Type,
                                              WorkerResponse? AssignedUser,
                                              Position? Position);