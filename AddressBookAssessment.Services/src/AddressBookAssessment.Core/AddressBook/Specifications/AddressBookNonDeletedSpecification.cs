using AddressBookAssessment.Core.Shared;

namespace AddressBookAssessment.Core.AddressBook.Specifications;

public class AddressBookNonDeletedSpecification : ISpecification<AddressBookEntry>
{
	public IQueryable<AddressBookEntry> Apply(IQueryable<AddressBookEntry> queryable)
	{
		return queryable.Where(entry => entry.DeletedAt == null);
	}
}
