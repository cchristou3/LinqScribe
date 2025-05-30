LinqScribe is a lightweight C# library that enables expressive, dynamic, and type-safe filtering of IQueryable<T> collections based on structured filter objects. 
It automatically maps filter properties to entity fields and constructs the corresponding LINQ expressions at runtime; eliminating repetitive boilerplate and simplifying API query logic.

### Backlog
- Filter using primitive types (DONE)
- Support complex types (nested objects)
- Support multiple value filters
- Support date ranges (Convention TransactionDateFrom -> GreaterThanOrEquals && TransactionDateTo -> LessThanOrEqual)
- Support filtering with different naming schemes (database models may have different names)