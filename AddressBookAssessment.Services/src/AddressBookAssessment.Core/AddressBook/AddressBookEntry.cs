using AddressBookAssessment.Core.Shared;

namespace AddressBookAssessment.Core.AddressBook;

public class AddressBookEntry : Entity<Guid>
{
	private readonly List<AddressBookContact> _contacts = [];
	public Address Address { get; private set; }
	public TimezoneId TimezoneId { get; private set; }
	public string Name { get; private set; }
	public IReadOnlyList<AddressBookContact> Contacts => _contacts;
	public required DateTimeOffset CreatedAt { get; init; }
	public DateTimeOffset UpdatedAt { get; private set; }
	public DateTimeOffset? DeletedAt { get; private set; }

	private AddressBookEntry()
	{
		Name = default!;
		Address = default!;
		TimezoneId = default!;
	}

	public static AddressBookEntry Create(AddressBookEntryRequest addressBookEntryRequest, DateTimeOffset createdAt)
	{
		var entry = new AddressBookEntry
		{
			Id = Guid.NewGuid(),
			Address = addressBookEntryRequest.Address,
			Name = addressBookEntryRequest.Name,
			TimezoneId = addressBookEntryRequest.TimezoneId,
			CreatedAt = createdAt,
			UpdatedAt = createdAt
		};

		entry.UpdateContacts(addressBookEntryRequest.Contacts);

		entry.RecordEvent(new AddressBookEntryCreated(entry.Id, entry.Name, entry.Address, entry.Contacts));

		return entry;
	}

	public void Update(AddressBookEntryRequest addressBookUpdateRequest, DateTimeOffset updatedAt)
	{

		Name = addressBookUpdateRequest.Name;
		Address = addressBookUpdateRequest.Address;
		TimezoneId = addressBookUpdateRequest.TimezoneId;
		UpdateContacts(addressBookUpdateRequest.Contacts);

		UpdatedAt = updatedAt;

		RecordEvent(new AddressBookEntryUpdated(Id, Name, Address, Contacts));
	}

	public void Delete(DateTimeOffset deletedAt)
	{
		DeletedAt = deletedAt;
		UpdatedAt = deletedAt;

		RecordEvent(new AddressBookEntryDeleted(Id, Name, Address, Contacts));
	}

	public void UpdateContacts(IReadOnlyList<AddressBookContact> contacts)
	{
		_contacts.Clear();
		_contacts.AddRange(contacts);
	}
}

public abstract record AddressBookEvent(Guid Id) : IEvent;
public record AddressBookEntryCreated(Guid Id, string Name, Address Address, IReadOnlyList<AddressBookContact> Contacts) : AddressBookEvent(Id);
public record AddressBookEntryUpdated(Guid Id, string Name, Address Address, IReadOnlyList<AddressBookContact> Contacts) : AddressBookEvent(Id);
public record AddressBookEntryDeleted(Guid Id, string Name, Address Address, IReadOnlyList<AddressBookContact> Contacts) : AddressBookEvent(Id);