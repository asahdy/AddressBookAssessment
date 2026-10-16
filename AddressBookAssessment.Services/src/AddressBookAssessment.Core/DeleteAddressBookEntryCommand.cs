using AddressBookAssessment.Core.Shared;
using Mediator;
using System.Security.Claims;

namespace AddressBookAssessment.Core.AddressBook;

public record DeleteAddressBookEntryCommand : ICommand<AddressBookEntry>
{
	public required ClaimsPrincipal User { get; set; }
	public Guid Id { get; set; }
}

public class DeleteAddressBookEntryCommandHandler(IWorkspace workspace, TimeProvider timeProvider)
	: ICommandHandler<DeleteAddressBookEntryCommand, AddressBookEntry>
{
	public async ValueTask<AddressBookEntry> Handle(DeleteAddressBookEntryCommand command, CancellationToken cancellationToken)
	{
		command.User.AssertIsAdmin();

		var entries = await workspace.Load(new EntityByIdSpecification<AddressBookEntry, Guid>(command.Id), cancellationToken);

		var entry = entries.FirstOrDefault();

		if (entry is null)
		{
			throw new NotFoundException<AddressBookEntry>(command.Id.ToString());
		}

		var now = timeProvider.GetUtcNow();
		entry.Delete(now);

		return entry;
	}
}