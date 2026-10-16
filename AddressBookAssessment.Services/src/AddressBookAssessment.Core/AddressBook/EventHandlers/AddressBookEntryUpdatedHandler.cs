using Mediator;
using Microsoft.Extensions.Logging;

namespace AddressBookAssessment.Core.AddressBook.EventHandlers;

public class AddressBookEntryUpdatedHandler(ILogger<AddressBookEntryUpdatedHandler> logger) : INotificationHandler<AddressBookEntryUpdated>
{
	public ValueTask Handle(AddressBookEntryUpdated notification, CancellationToken cancellationToken)
	{
		logger.LogInformation($"AddressBook entry# {notification.Id}, with name: {notification.Name} updated.");

		return ValueTask.CompletedTask;
	}
}
